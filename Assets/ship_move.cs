using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Look
{
    target,
    forward,
    left,
    right,
    bottom,
    up
}

public enum Lock_axie{
    x,y,z,no
}

public class ship_move : MonoBehaviour
{
    public Look look;
    public Lock_axie lock_Axie;
    public Bezier_Curve nodes;
    float time=0;

    public bool action;

    public float speed;
    public bool loop;

    public GameObject target;

    void Start()
    {
        action = true;
        loop_start_action();
    }

    IEnumerator move(bool loop_start)
    {
        action = true;
        if(loop_start)
        {
            time = 0;
        }
        else
        {
            time = 1;
        }
        yield return new WaitUntil(()=>{
            if(time>1f || time <0f)
            {
                action = false;
                if(loop && loop_start){
                    loop_back_action();
                }else if(loop && !loop_start){
                    loop_start_action();
                }
                return true;
            }
            else
            {
                if(loop_start){
                    time += Time.deltaTime * speed;
                }else{
                    time -= Time.deltaTime * speed;
                }
                
                this.transform.position = nodes.GetPoint(time);
                return false;
            }
        });
    }

    public void loop_back_action(){
        StartCoroutine("move",false);
    }

    public void loop_start_action(){
        StartCoroutine("move",true);
    }
   
    void Update(){
        switch(look){
            case Look.target :
                this.transform.rotation = look_target( target.transform , lock_Axie);
                break;
            case Look.forward :
                this.transform.forward = look_tangent(nodes,time);
                break;

        }
    }

    public Quaternion look_target(Transform target,Lock_axie lock_Axie)
    {
        Vector3 vec = this.transform.position - target.position;
        switch(lock_Axie){
            case Lock_axie.x:
                vec.x = 0;
                break;

            case Lock_axie.y:
                vec.y = 0;
                break;

            case Lock_axie.z:
                vec.z = 0;
                break;

            case Lock_axie.no:
                
                break;

        }
        
        Vector3 Temp_vec = Vector3.Slerp(vec,this.transform.forward,Time.deltaTime);
        //Temp_vec.Normalize();
        Quaternion quaternion1 = Quaternion.Lerp(this.transform.rotation , Quaternion.LookRotation(vec,Vector3.up),Time.deltaTime * 2f);
        Quaternion quaternion = quaternion1;
        return quaternion;
    }

    public Vector3 look_tangent(Bezier_Curve curve,float timer)
    {
        return curve.GetTangent(timer);
    }
}
