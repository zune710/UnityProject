using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Text text;

    private bool onHover;

    private Animator OptionAnim;
    private GameObject SubMenuCanvas;
    private GameObject AlertMenu;

    private AudioSource ButtonSFX;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();

        SubMenuCanvas = GameObject.Find("SubMenuCanvas");
        OptionAnim = SubMenuCanvas.transform.Find("OptionMenu").GetComponent<Animator>();
        AlertMenu = SubMenuCanvas.transform.Find("AlertMenu").gameObject;
        // 꺼져 있으면 GameObject.Find는 못 찾지만
        // transform.Find는 찾을 수 있음

        ButtonSFX = GetComponent<AudioSource>();

        if (ControllerManager.GetInstance().LoadGame)
        {
            if (transform.name == "Load Game")
            {
                transform.Find("Inactive").gameObject.SetActive(false);
            }
        }
        else
            GameObject.Find("Inactive").gameObject.SetActive(true);
    }

    private void Start()
    {
        text.text = transform.name;

        AlertMenu.gameObject.SetActive(false);

        onHover = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(onHover)
        {
            ButtonSFX.Play();
            
            if (transform.name == "New Game")
            {
                if(ControllerManager.GetInstance().LoadGame)
                {
                    // 저장된 데이터 덮어쓰기 경고 UI
                    AlertMenu.gameObject.SetActive(true);
                }
                else
                {
                    ResetValue();
                    ControllerManager.GetInstance().LoadGame = true;
                    SceneManager.LoadScene("Game Start");
                }
                
            }
            else if(transform.name == "Load Game" && ControllerManager.GetInstance().LoadGame)
            {
                if(ControllerManager.GetInstance().GameOver)
                {
                    ControllerManager.GetInstance().GameOver = false;
                    ControllerManager.GetInstance().Player_HP = 100;
                    ControllerManager.GetInstance().EnemyCount = 0;
                }

                if(ControllerManager.GetInstance().onBoss)
                {
                    ControllerManager.GetInstance().Player_HP = 100;
                }

                SceneManager.LoadScene("Game Start");
            }
            else if(transform.name == "How To Play")
            {
                OptionAnim.SetBool("Move", true);
            }
            else if (transform.name == "Quit")
            {
                Application.Quit();  // 에디터에서는 무시됨
                //UnityEditor.EditorApplication.ExitPlaymode();  // 에디터 플레이모드 나가기
            }
        }
    }

    public void CloseOption()
    {
        ButtonSFX.Play();

        OptionAnim.SetBool("Move", false);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    // 마우스가 버튼 위에 있을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover = true;
    }

    // 마우스가 버튼을 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        onHover = false;
    }

    private void ResetValue()
    {
        ControllerManager.GetInstance().Player_HP = 100;
        ControllerManager.GetInstance().BulletSpeed = 10.0f;
        ControllerManager.GetInstance().EnemyCount = 0;
        ControllerManager.GetInstance().Heart = 3;
        ControllerManager.GetInstance().Goal = 20;

        ControllerManager.GetInstance().BossId = 1;
        ControllerManager.GetInstance().EnemyId = 1;
        ControllerManager.GetInstance().Round = 1;

        ControllerManager.GetInstance().onEnemy = true;
        ControllerManager.GetInstance().onBoss = false;
        ControllerManager.GetInstance().BossActive = false;

        ControllerManager.GetInstance().GameClear = false;
        ControllerManager.GetInstance().GameOver = false;
    }


    // 버튼 onClick
    public void AlertYES()
    {
        ButtonSFX.Play();

        // New Game 실행
        ResetValue();
        ControllerManager.GetInstance().LoadGame = true;
        SceneManager.LoadScene("Game Start");
    }

    public void AlertNO()
    {
        ButtonSFX.Play();
        
        // 경고창 끄기
        AlertMenu.gameObject.SetActive(false);
    }
}
