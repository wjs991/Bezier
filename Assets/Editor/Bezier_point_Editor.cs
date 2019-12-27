using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bezier_point),true)]
[CanEditMultipleObjects]
public class Bezier_point_Editor : Editor
{
    public const float CircleSize = 0.075f;
    public const float RecSize = 0.1f;
    public const float SphereSize = 0.15f;

    public static float point_circle_size = RecSize;
    public static float sub_handle_size = CircleSize;
    [SerializeField]
    public Bezier_Curve curve_;

    private Bezier_point point;
    private SerializedProperty sub_1;
    private SerializedProperty sub_2;
    protected virtual void OnEnable()
    {
        this.point = (Bezier_point)this.target;
        point.bezier_Curve = curve_;
        sub_1 = this.serializedObject.FindProperty("sub_handle_1_localposition");
        sub_2 = this.serializedObject.FindProperty("sub_handle_2_localposition");
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sub_1);
        if (EditorGUI.EndChangeCheck())
        {
            this.sub_2.vector3Value = -this.sub_1.vector3Value;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sub_2);
        if (EditorGUI.EndChangeCheck())
        {
            this.sub_1.vector3Value = -this.sub_2.vector3Value;
        }

        this.serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnSceneGUI()
    {
        //Bezier_point_Editor.sub_handle_size = Bezier_point_Editor.CircleSize;
        //Bezier_curve_Editor.DrawPointsSceneGUI(this.point.bezier_Curve, this.point);

        Bezier_point_Editor.sub_handle_size = Bezier_point_Editor.SphereSize;
        Bezier_point_Editor.DrawPointSceneGUI(this.point, Handles.DotHandleCap, Handles.SphereHandleCap);
    }

    public static void DrawPointSceneGUI(Bezier_point point)
    {
        DrawPointSceneGUI(point, Handles.RectangleHandleCap, Handles.CircleHandleCap);
    }

    public static void DrawPointSceneGUI(Bezier_point point, Handles.CapFunction drawPointFunc, Handles.CapFunction drawHandleFunc)
    {
        // Draw a label for the point
        Handles.color = Color.black;
        Handles.Label(point.transform.position + new Vector3(0f, HandleUtility.GetHandleSize(point.transform.position) * 0.4f, 0f), point.gameObject.name);

        // Draw the center of the control point
        Handles.color = Color.yellow;
        Vector3 newPointPosition = Handles.FreeMoveHandle(point.transform.position, point.transform.rotation,
            HandleUtility.GetHandleSize(point.transform.position) * Bezier_point_Editor.point_circle_size, Vector3.one * 0.5f, drawPointFunc);

        if (point.transform.position != newPointPosition)
        {
            Undo.RegisterCompleteObjectUndo(point.transform, "Move Point");
            point.transform.position = newPointPosition;
        }

        // Draw the left and right handles
        Handles.color = Color.white;
        Handles.DrawLine(point.transform.position, point.sub_handle_1);
        Handles.DrawLine(point.transform.position, point.sub_handle_2);

        Handles.color = Color.cyan;
        Vector3 newLeftHandlePosition = Handles.FreeMoveHandle(point.sub_handle_1, point.transform.rotation,
            HandleUtility.GetHandleSize(point.sub_handle_1) * Bezier_point_Editor.sub_handle_size, Vector3.zero, drawHandleFunc);

        if (point.sub_handle_1 != newLeftHandlePosition)
        {
            Undo.RegisterCompleteObjectUndo(point, "Move Left Handle");
            point.sub_handle_1 = newLeftHandlePosition;
        }

        Vector3 newRightHandlePosition = Handles.FreeMoveHandle(point.sub_handle_2, point.transform.rotation,
            HandleUtility.GetHandleSize(point.sub_handle_2) * Bezier_point_Editor.sub_handle_size, Vector3.zero, drawHandleFunc);

        if (point.sub_handle_2 != newRightHandlePosition)
        {
            Undo.RegisterCompleteObjectUndo(point, "Move Right Handle");
            point.sub_handle_2 = newRightHandlePosition;
        }
    }

        private static bool MouseButtonDown(int button)
        {
            return Event.current.type == EventType.MouseDown && Event.current.button == button;
        }

        private static bool MouseButtonUp(int button)
        {
            return Event.current.type == EventType.MouseUp && Event.current.button == button;
        }
}
