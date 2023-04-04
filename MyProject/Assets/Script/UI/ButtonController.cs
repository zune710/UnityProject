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

    public Button LoadGame;
    private Animator OptionAnim;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();

        OptionAnim = GameObject.Find("OptionMenu").GetComponent<Animator>();

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

        onHover = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(onHover)
        {
            if (transform.name == "New Game")
            {
                if(ControllerManager.GetInstance().LoadGame)
                {
                    // 기존의 Load 파일 사라짐 경고 UI SetActive true
                    
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
                    ControllerManager.GetInstance().BossActive = true;
                    ControllerManager.GetInstance().Player_HP = 100;
                }

                SceneManager.LoadScene("Game Start");
            }
            else if(transform.name == "Option")
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
        ControllerManager.GetInstance().Goal = 10;

        ControllerManager.GetInstance().BossId = 1;
        ControllerManager.GetInstance().EnemyId = 1;
        ControllerManager.GetInstance().Round = 1;

        ControllerManager.GetInstance().onEnemy = true;
        ControllerManager.GetInstance().onBoss = false;

        ControllerManager.GetInstance().GameClear = false;
        ControllerManager.GetInstance().GameOver = false;
    }


    // 버튼 onClick
    public void WarningYES()
    {
        ResetValue();
        ControllerManager.GetInstance().LoadGame = true;
        SceneManager.LoadScene("Game Start");
    }

    public void WarningNO()
    {
        // 경고창 끄기
    }
}
