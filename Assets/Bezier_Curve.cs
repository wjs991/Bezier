using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_Curve : MonoBehaviour
{

    public List<Bezier_point> point_list;

    public void Add_point(){
        Bezier_point point = new GameObject("point",typeof(Bezier_point)).GetComponent<Bezier_point>();
        point.transform.SetParent(this.transform);
        point.transform.localRotation = Quaternion.identity;

        point.transform.localPosition = Vector3.zero;

        point_list.Add(point);
    }


    /*
     protected virtual void OnDrawGizmos()
        {
            if (this.point_list.Count > 1)
            {
                // Draw the curve
                Vector3 fromPoint = this.GetPoint(0f);

                for (int i = 0; i < this.Sampling; i++)
                {
                    float time = (i + 1) / (float)this.Sampling;
                    Vector3 toPoint = this.GetPoint(time);

                    // Draw segment
                    Gizmos.color = this.curveColor;
                    Gizmos.DrawLine(fromPoint, toPoint);

                    fromPoint = toPoint;
                }

                // Draw the start and the end of the curve indicators
                Gizmos.color = this.startPointColor;
                Gizmos.DrawSphere(this.KeyPoints[0].Position, 0.05f);

                Gizmos.color = this.endPointColor;
                Gizmos.DrawSphere(this.KeyPoints[this.KeyPointsCount - 1].Position, 0.05f);

                // Draw the point at the normalized time
                Vector3 point = this.GetPoint(this.normalizedTime);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(point, 0.025f);

                Vector3 tangent = this.GetTangent(this.normalizedTime);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(point, point + tangent / 2f);

                Vector3 binormal = this.GetBinormal(this.normalizedTime, Vector3.up);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(point, point + binormal / 2f);

                Vector3 normal = this.GetNormal(this.normalizedTime, Vector3.up);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(point, point + normal / 2f);
            }
        }
        */
}
