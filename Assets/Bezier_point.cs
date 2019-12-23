using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_point : MonoBehaviour
{

    public Vector3 sub_handle_1_localposition;
    public Vector3 sub_handle_2_localposition;
    public Bezier_Curve bezier_Curve;



    public Vector3 sub_handle_1{
        get{
            return this.transform.TransformPoint(this.sub_handle_1);
        }
        set{
            this.sub_handle_1_localposition = this.transform.InverseTransformPoint(value);
        }
    }

    public Vector3 sub_handle_2{
         get{
            return this.transform.TransformPoint(this.sub_handle_2);
        }
        set{
            this.sub_handle_2_localposition = this.transform.InverseTransformPoint(value);
        }
    }

    

}
