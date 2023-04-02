using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SidebarController : MonoBehaviour
{
    
    public GameObject Menu;
    public GameObject End;
    public GameObject Dark;

    private Animator Anim;
    public bool check;


    private void Awake()
    {
        Menu = transform.Find("MenuFrame").gameObject;

        End = transform.Find("GameEnd").gameObject;

        Dark = transform.Find("DarkBackground").gameObject;

        Anim = Menu.GetComponent<Animator>();
    }

    void Start()
    {
        check = false;
        End.SetActive(false);
        Dark.SetActive(false);

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (ControllerManager.GetInstance().GameOver)
        {
            Time.timeScale = 0.0f;
            Dark.SetActive(true);
            End.GetComponent<Text>().text = "GAME OVER";
            End.SetActive(true);
            End.GetComponent<Animator>().SetTrigger("Move");

        }
        else if(ControllerManager.GetInstance().GameClear)
        {
            Time.timeScale = 0.0f;
            Dark.SetActive(true);
            End.GetComponent<Text>().text = "GAME CLEAR";
            End.SetActive(true);
            End.GetComponent<Animator>().SetTrigger("Move");
        }
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
