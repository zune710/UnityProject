using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarController : MonoBehaviour
{
    public GameObject sidebar;
    private Animator Anim;
    public bool check;

    public float test;

    private void Awake()
    {
        Anim = sidebar.GetComponent<Animator>();
    }

    void Start()
    {
        check = false;
        //test = 0.0f;
    }


    private void Update()
    {
        /*
        if(check)
        {
            sidebar.transform.position = Vector3.Lerp(
                new Vector3(Screen.width + 300.0f, Screen.height * 0.5f, 0.0f),
                new Vector3(Screen.width - 220.0f, Screen.height * 0.5f, 0.0f),
                test);
        }
        else
        {
            sidebar.transform.position = Vector3.Lerp(
                sidebar.transform.position,
                new Vector3(Screen.width + 300.0f, Screen.height * 0.5f, 0.0f),
                test);
        }
        */
    }

    public void ClickButton()
    {
        check = !check;
        Anim.SetBool("Move", check);

        //test = 0.0f;
    }
}
