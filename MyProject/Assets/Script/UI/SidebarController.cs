using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SidebarController : MonoBehaviour
{
    
    public GameObject Menu;
    public GameObject Dark;

    private Animator Anim;
    public bool check;


    private void Awake()
    {
        Menu = transform.Find("MenuFrame").gameObject;

        Dark = transform.Find("DarkBackground").gameObject;

        Anim = Menu.GetComponent<Animator>();
    }

    void Start()
    {
        check = false;
        Dark.SetActive(false);

        Time.timeScale = 1.0f;
    }


    public void ClickButton()
    {
        check = !check;
        Dark.SetActive(check);
        Anim.SetBool("Move", check);

        if (check)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
