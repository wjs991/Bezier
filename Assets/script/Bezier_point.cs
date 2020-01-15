using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_point : MonoBehaviour
{

    [SerializeField]
    public Vector3 sub_handle_1_localposition = new Vector3(-0.5f, 0f, 0f);
    [SerializeField]
    public Vector3 sub_handle_2_localposition = new Vector3(0.5f, 0f, 0f);
    [SerializeField]
    public Bezier_Curve bezier_Curve;



    public Vector3 sub_handle_1_Local_pos
    {
        get
        {   
            return this.sub_handle_1_localposition;
        }
        set
        {
            this.sub_handle_1_localposition = value;
            this.sub_handle_2_localposition = -value;
        }
    }

    public Vector3 sub_handle_2_Local_pos
    {
        get
        {
            return this.sub_handle_2_localposition;
        }
        set
        {
            this.sub_handle_2_localposition = value;
            this.sub_handle_1_localposition = -value;
        }
    }


    public Vector3 sub_handle_1
    {
        get
        {
            return this.transform.TransformPoint(this.sub_handle_1_Local_pos);
        }
        set
        {
            this.sub_handle_1_Local_pos = this.transform.InverseTransformPoint(value);
        }
    }

    public Vector3 sub_handle_2
    {
        get
        {
            return this.transform.TransformPoint(this.sub_handle_2_Local_pos);
        }
        set
        {
            this.sub_handle_2_Local_pos = this.transform.InverseTransformPoint(value);
        }
    }

    

}
