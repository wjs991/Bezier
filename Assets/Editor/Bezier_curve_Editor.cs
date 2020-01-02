using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Bezier_Curve))]
[CanEditMultipleObjects]
public class Bezier_curve_Editor : Editor
{
    private const float AddButtonWidth =80f;
    private const float RemoveButtonWidth = 19f;
    private Bezier_Curve curve;

    private ReorderableList point_list;

    private bool show_point = true;

    private bool length_view = false;
    private bool color_group = false;

    [MenuItem("GameObject/Create Other/Bezier Curve")]
    private static void Create_Bezier_Curve(){
        Bezier_Curve curve = new GameObject("Bezier_Curve",typeof(Bezier_Curve)).GetComponent<Bezier_Curve>();
        Vector3 pos = Vector3.zero;
        if(Camera.current != null){
            pos = Camera.current.transform.position + Camera.current.transform.forward * 10f;
        }
        curve.transform.position = pos;

        Bezier_curve_Editor.AddDefaultPoints(curve);

        Undo.RegisterCreatedObjectUndo(curve.gameObject, "Create Curve");

        Selection.activeGameObject = curve.gameObject;
    }

    private static void AddDefaultPoints(Bezier_Curve curve)
    {
        Bezier_point startPoint = curve.AddKeyPoint();
        startPoint.transform.localPosition = new Vector3(-1f, 0f, 0f);
        startPoint.sub_handle_1_Local_pos = new Vector3(-0.35f, -0.35f, 0f);

        Bezier_point endPoint = curve.AddKeyPoint();
        endPoint.transform.localPosition = new Vector3(1f, 0f, 0f);
        endPoint.sub_handle_1_Local_pos = new Vector3(-0.35f, 0.35f, 0f);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.serializedObject.Update();



        this.length_view = EditorGUILayout.Foldout(length_view,"show length");
        
        if(length_view)
        {
            EditorGUILayout.LabelField("Total Length : ", string.Format("{0}",curve.GetApproximateLength()));
            EditorGUILayout.LabelField("Now Length : ", string.Format("{0}",curve.GetApproximateLength() * curve.normalizedTime));
        }
        
        this.show_point = EditorGUILayout.Foldout(this.show_point, "Key Points");
        if (this.show_point)
        {
            if (GUILayout.Button("Add Point"))
            {
                AddKeyPointAt(this.curve,this.curve.Point_count);
            }
            this.point_list.DoLayoutList();
        }

        this.serializedObject.ApplyModifiedProperties();
    }

    private static Bezier_point AddKeyPointAt(Bezier_Curve curve, int index)
    {
        Bezier_point newPoint = new GameObject("Point " + curve.Point_count, typeof(Bezier_point)).GetComponent<Bezier_point>();
        newPoint.transform.parent = curve.transform;
        newPoint.transform.localRotation = Quaternion.identity;
        newPoint.bezier_Curve = curve;

        if (curve.Point_count == 0 || curve.Point_count == 1)
        {
            newPoint.transform.localPosition = Vector3.zero;
        }
        else
        {
            if (index == 0)
            {
                newPoint.transform.position = (curve.point_list[0].transform.position - curve.point_list[1].transform.position).normalized + curve.point_list[0].transform.position;
            }
            else if (index == curve.Point_count)
            {
                newPoint.transform.position = (curve.point_list[index - 1].transform.position - curve.point_list[index - 2].transform.position).normalized + curve.point_list[index - 1].transform.position;
            }
            else
            {
                newPoint.transform.position = Bezier_Curve.GetPointOnCubicCurve(0.5f, curve.point_list[index - 1], curve.point_list[index]);
            }
        }

        Undo.IncrementCurrentGroup();
        Undo.RegisterCreatedObjectUndo(newPoint.gameObject, "Create Point");
        Undo.RegisterCompleteObjectUndo(curve, "Save Curve");

        curve.point_list.Insert(index, newPoint);
        RenamePoints(curve);

        //Undo.RegisterCompleteObjectUndo(curve, "Save Curve");

        return newPoint;
    }

    private static void RenamePoints(Bezier_Curve curve)
    {
        for (int i = 0; i < curve.Point_count; i++)
        {
            curve.point_list[i].name = "Point " + i;
        }
    }
    protected virtual void OnEnable()
    {
        this.curve = (Bezier_Curve)this.target;
        
        this.point_list = new ReorderableList(this.serializedObject, serializedObject.FindProperty("point_list"), true, true, false, false);
        this.point_list.drawElementCallback = this.DrawElementCallback;
        this.point_list.drawHeaderCallback =
            (Rect rect) =>
            {
                EditorGUI.LabelField(rect, string.Format("Points: {0}", this.point_list.serializedProperty.arraySize), EditorStyles.boldLabel);
            };
    }

    private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = this.point_list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        // Draw "Add Before" button
        if (GUI.Button(new Rect(rect.x, rect.y, AddButtonWidth, EditorGUIUtility.singleLineHeight), new GUIContent("Add Before")))
        {
            AddKeyPointAt(this.curve, index);
        }

        // Draw point name
        EditorGUI.PropertyField(
            new Rect(rect.x + AddButtonWidth + 5f, rect.y, rect.width - AddButtonWidth * 2f - 35f, EditorGUIUtility.singleLineHeight), element, GUIContent.none);

        // Draw "Add After" button
        if (GUI.Button(new Rect(rect.width - AddButtonWidth + 8f, rect.y, AddButtonWidth, EditorGUIUtility.singleLineHeight), new GUIContent("Add After")))
        {
            AddKeyPointAt(this.curve, index + 1);
        }

        // Draw remove button
        if (this.curve.Point_count > 2)
        {
            if (GUI.Button(new Rect(rect.width + 14f, rect.y, RemoveButtonWidth, EditorGUIUtility.singleLineHeight), new GUIContent("x")))
            {
                RemoveKeyPointAt(this.curve, index);
            }
        }
    }
    private static bool RemoveKeyPointAt(Bezier_Curve curve, int index)
        {
            if (curve.Point_count < 2)
            {
                return false;
            }

            var point = curve.point_list[index];

            Undo.IncrementCurrentGroup();
            Undo.RegisterCompleteObjectUndo(curve, "Save Curve");

            curve.point_list.RemoveAt(index);
            RenamePoints(curve);

            //Undo.RegisterCompleteObjectUndo(curve, "Save Curve");
            Undo.DestroyObjectImmediate(point.gameObject);

            return true;
        }

    public static void DrawPointsSceneGUI(Bezier_Curve curve, Bezier_point exclude = null)
    {
        for (int i = 0; i < curve.Point_count; i++)
        {
            if (curve.point_list[i] == exclude)
            {
                continue;
            }

            Bezier_point_Editor.sub_handle_size = Bezier_point_Editor.CircleSize;
            Bezier_point_Editor.DrawPointSceneGUI(curve.point_list[i]);
        }
    }
}
