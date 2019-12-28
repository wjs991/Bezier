using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_Curve : MonoBehaviour
{
    [SerializeField]
    private Color curveColor = Color.green;

    [SerializeField]
    private Color startPointColor = Color.red;

    [SerializeField]
    private Color endPointColor = Color.blue;

    [SerializeField]
    private int sampling = 25;

    [SerializeField]
    [HideInInspector]
    public List<Bezier_point> point_list = new List<Bezier_point>();

    [SerializeField]
    [Range(0f, 1f)]
    float normalizedTime = 0.5f;


    public int Point_count
    {
        get{
            return this.point_list.Count;
        }
    }


    public Bezier_point AddKeyPoint()
    {
        return this.AddKeyPointEnd();
    }

    public Bezier_point AddKeyPointEnd()
    {
        Bezier_point newPoint = new GameObject("Point " + Point_count, typeof(Bezier_point)).GetComponent<Bezier_point>();
        
        newPoint.transform.parent = this.transform;
        newPoint.transform.localRotation = Quaternion.identity;
        newPoint.bezier_Curve = this.GetComponent<Bezier_Curve>();

        if(Point_count == 0 || Point_count ==1)
        {
            newPoint.transform.localPosition = Vector3.zero;
        }else
        {
            newPoint.transform.position = (point_list[Point_count - 1].transform.position - point_list[Point_count - 2].transform.position).normalized + point_list[Point_count - 1].transform.position;
        }
        
            
        point_list.Add(newPoint);

        return newPoint;
    }

    public Vector3 GetPoint(float time)
    {   
        if(time<=1f){
            Bezier_point startPoint;
            Bezier_point endPoint;
            float timeRelativeToSegment;

            this.GetCubicSegment(time, out startPoint, out endPoint, out timeRelativeToSegment);

            return Bezier_Curve.GetPointOnCubicCurve(timeRelativeToSegment, startPoint, endPoint);
        }else{
            return point_list[Point_count-1].transform.position;
        }
        
    }

    public void GetCubicSegment(float time, out Bezier_point startPoint, out Bezier_point endPoint, out float timeRelativeToSegment)
    {
        startPoint = null;
        endPoint = null;
        timeRelativeToSegment = 0f;

        float subCurvePercent = 0f;
        float totalPercent = 0f;
        float approximateLength = this.GetApproximateLength();
        int subCurveSampling = (sampling/ (Point_count - 1)) + 1;

        for (int i = 0; i < Point_count - 1; i++)
        {
            subCurvePercent = Bezier_Curve.GetApproximateLengthOfCubicCurve(point_list[i], point_list[i + 1], subCurveSampling) / approximateLength;
            if (subCurvePercent + totalPercent > time)
            {
                startPoint = point_list[i];
                endPoint = point_list[i + 1];

                break;
            }

            totalPercent += subCurvePercent;
        }

        if (endPoint == null)
        {
            // If the evaluated point is very near to the end of the curve we are in the last segment
            startPoint = point_list[Point_count - 2];
            endPoint = point_list[Point_count - 1];

            // We remove the percentage of the last sub-curve
            totalPercent -= subCurvePercent;
        }

        timeRelativeToSegment = (time - totalPercent) / subCurvePercent;
    }


    public float GetApproximateLength()
    {
        float length = 0;
        int subCurveSampling = (sampling / (Point_count - 1)) + 1;
        for (int i = 0; i < Point_count - 1; i++)
        {
            length += Bezier_Curve.GetApproximateLengthOfCubicCurve(point_list[i], point_list[i + 1], subCurveSampling);
        }

        return length;
    }
    public static float GetApproximateLengthOfCubicCurve(Bezier_point startPoint, Bezier_point endPoint, int sampling)
    {
        return GetApproximateLengthOfCubicCurve(startPoint.transform.position, endPoint.transform.position, startPoint.sub_handle_1, endPoint.sub_handle_2, sampling);
    }    
    public static float GetApproximateLengthOfCubicCurve(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int sampling)
    {
        float length = 0f;
        Vector3 fromPoint = GetPointOnCubicCurve(0f, startPosition, endPosition, startTangent, endTangent);

        for (int i = 0; i < sampling; i++)
        {
            float time = (i + 1) / (float)sampling;
            Vector3 toPoint = GetPointOnCubicCurve(time, startPosition, endPosition, startTangent, endTangent);
            length += Vector3.Distance(fromPoint, toPoint);
            fromPoint = toPoint;
        }

        return length;
    }
    public static Vector3 GetPointOnCubicCurve(float time, Bezier_point startPoint, Bezier_point endPoint)
    {
        return GetPointOnCubicCurve(time, startPoint.transform.position, endPoint.transform.position, startPoint.sub_handle_1, endPoint.sub_handle_2);
    }
    public static Vector3 GetPointOnCubicCurve(float time, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent)
    {
        float t = time;
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            (u3) * startPosition +
            (3f * u2 * t) * startTangent +
            (3f * u * t2) * endTangent +
            (t3) * endPosition;

        return result;
    }
    
     protected virtual void OnDrawGizmos()
        {
            if (this.point_list.Count > 1)
            {
                // Draw the curve
                Vector3 fromPoint = this.GetPoint(0f);

                for (int i = 0; i < sampling; i++)
                {
                    float time = (i + 1) / (float)sampling;
                    Vector3 toPoint = this.GetPoint(time);

                    // Draw segment
                    Gizmos.color = this.curveColor;
                    Gizmos.DrawLine(fromPoint, toPoint);

                    fromPoint = toPoint;
                }

                // Draw the start and the end of the curve indicators
                Gizmos.color = this.startPointColor;
                Gizmos.DrawSphere(point_list[0].transform.position, 0.05f);

                Gizmos.color = this.endPointColor;
                Gizmos.DrawSphere(point_list[Point_count - 1].transform.position, 0.05f);

                // Draw the point at the normalized time
                Vector3 point = this.GetPoint(this.normalizedTime);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(point, 0.025f);

                
            }
        }
        
}
