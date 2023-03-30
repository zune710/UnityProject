using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SidebarController : MonoBehaviour
{
    
    public GameObject Menu;
    public GameObject Dark;

    //public GameObject sidebar;
    //public GameObject Xbutton;
    //public GameObject Skillbutton;

    private Animator Anim;
    public bool check;

    //public float test;

    //private Navigation xNavNone;
    //private Navigation xNavExplicit;

    private void Awake()
    {
        Menu = transform.Find("MenuFrame").gameObject;

        Dark = transform.Find("DarkBackground").gameObject;

        Anim = Menu.GetComponent<Animator>();

        //Anim = sidebar.GetComponent<Animator>();
    }

    void Start()
    {
        check = false;
        Dark.SetActive(false);

        Time.timeScale = 1.0f;

        //xNavNone = new Navigation();
        //xNavNone.mode = Navigation.Mode.None;

        //xNavExplicit = new Navigation();
        //xNavExplicit.mode = Navigation.Mode.Explicit;

        //xNavExplicit.selectOnDown = Skillbutton.GetComponent<Button>();
        //xNavExplicit.selectOnLeft = Skillbutton.GetComponent<Button>();

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
        Dark.SetActive(check);
        Anim.SetBool("Move", check);

        if (check)
        {
            Time.timeScale = 0.0f;

            //Xbutton.GetComponent<Button>().navigation = xNavExplicit;

            //// sidebar 화면 안으로 들어간 후 X 버튼 선택 상태로 만들기
            //EventSystem.current.SetSelectedGameObject(Xbutton);
        }
        else
        {
            Time.timeScale = 1.0f;

            //Xbutton.GetComponent<Button>().navigation = xNavNone;
            
            //// sidebar 화면 밖으로 나간 후 X 버튼 선택 상태 해제하기
            //EventSystem.current.SetSelectedGameObject(null);
        }
        

        //test = 0.0f;
    }
}
