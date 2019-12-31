using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public Text now_mode;
    public Button next_button;
    public Button prev_button;
    public List<GameObject> target_list;
    public List<Button> content_list;
    public ship_move ship;
    public GameObject view_container;

    public GameObject target
    {
        get
        {
            return ship.target;
        }
        set
        {
            ship.target = value;
        }
    }


    public Look look_mod
    {
        get
        {
            return ship.look;
        }
        set
        {
            ship.look = value;
        }
    }
    public int now_mod_index;
    // Start is called before the first frame update
    void Start()
    {
        look_mod = ship.look;
        now_mode.text = get_look_mod_string(look_mod);
        now_mod_index = 0;
        init_target_list();
        if(!look_mod.Equals(Look.forward))
        {
            set_off_target_button();
        }
    }


    public void nextbtn()
    {
        now_mod_index++;
        if(now_mod_index >= 6)
        {
            now_mod_index = 0;
        }
        look_mod = (Look)now_mod_index;
        Debug.Log(look_mod);
        now_mode.text = get_look_mod_string(look_mod);
    }

    public void prevbtn()
    {
        now_mod_index--;
        if(now_mod_index < 0)
        {
            now_mod_index = 5;
        }
        look_mod = (Look)now_mod_index;
        now_mode.text = get_look_mod_string(look_mod);
        Debug.Log(look_mod);
    }

    public string get_look_mod_string(Look look)
    {
        string mod = string.Empty;
        switch(look)
        {
           case Look.target :
                mod = "target";
                set_on_target_button();
                break;
            case Look.forward :
                mod = "forward";
                set_off_target_button();
                break;
            case Look.up :
                mod = "up";
                break;
            case Look.bottom :
                mod = "bottom";
                break;
            case Look.right :
                mod = "right";
                break;
            case Look.left :
                mod = "left";
                set_off_target_button();
                break;
        }

        return mod;
    }

    public void set_on_target_button()
    {
        for(int i=0;i<target_list.Count;i++)
        {
            content_list[i].interactable = true;
        }
    }

    public void set_off_target_button()
    {
        for(int i=0;i<target_list.Count;i++)
        {
            content_list[i].interactable = false;
        }
    }
    
    public void init_target_list()
    {
        for(int i=0;i<target_list.Count;i++)
        {
            GameObject content = Instantiate(Resources.Load("Content")as GameObject);
            content.transform.SetParent(view_container.transform);
            content.transform.localPosition = new Vector3(0,0,0);
            content.transform.localScale = new Vector3(1,1,1);
            content.GetComponent<content_info>().target = target_list[i];
            content.GetComponent<content_info>().name_.text = target_list[i].name;
            content.GetComponent<Button>().onClick.AddListener(()=>{
                target = content.GetComponent<content_info>().target;
            });
            content_list.Add(content.GetComponent<Button>());
        }
    }
}
