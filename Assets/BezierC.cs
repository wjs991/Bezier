using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
https://denisrizov.com/2016/06/02/bezier-curves-unity-package-included/
https://en.wikipedia.org/wiki/Linear_interpolation
*/
public class BezierC : MonoBehaviour
{
    public GameObject start_pos;
    public GameObject end_pos;
    public GameObject second_pos;
    public GameObject third_pos;
    // Start is called before the first frame update

    public GameObject s_s;
    public GameObject s_t;
    public GameObject t_e;

    public GameObject A_;
    public GameObject B_;

    public GameObject Goal;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 Lerp(Vector3 A, Vector3 B, float t){
        return (1-t)*A + t*B;
    }

    public void Onsliderchanged(float value){
        Vector3 a = Lerp(start_pos.transform.position,second_pos.transform.position,value);
        Vector3 b = Lerp(second_pos.transform.position,third_pos.transform.position,value);
        Vector3 c = Lerp(third_pos.transform.position,end_pos.transform.position,value);

        Vector3 a_ = Lerp(a,b,value);
        Vector3 b_ = Lerp(b,c,value);

        Vector3 g = Lerp(a_,b_,value);

        s_s.transform.position = a;
        s_t.transform.position = b;
        t_e.transform.position = c;

        A_.transform.position = a_;
        B_.transform.position = b_;
        A_.GetComponent<LineRenderer>().SetPosition(0,A_.transform.position);
        A_.GetComponent<LineRenderer>().SetPosition(1,B_.transform.position);

        Goal.transform.position = g;
    }
}
