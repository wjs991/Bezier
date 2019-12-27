using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bezier_Curve))]
[CanEditMultipleObjects]
public class Bezier_curve_Editor : Editor
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
