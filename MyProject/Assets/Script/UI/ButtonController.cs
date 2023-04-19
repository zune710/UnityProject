using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Text PlayerId;
    private Text text;

    private bool onHover;

    private Animator OptionAnim;
    private GameObject SubMenuCanvas;
    private GameObject AlertMenu;

    private AudioSource ButtonSFX;

    private void Awake()
    {
        PlayerId = GameObject.Find("PlayerID").GetComponent<Text>();
        text = transform.Find("Text").GetComponent<Text>();

        SubMenuCanvas = GameObject.Find("SubMenuCanvas");
        OptionAnim = SubMenuCanvas.transform.Find("OptionMenu").GetComponent<Animator>();
        AlertMenu = SubMenuCanvas.transform.Find("AlertMenu").gameObject;
        #region memo
        // 꺼져 있으면 GameObject.Find는 못 찾지만
        // transform.Find는 찾을 수 있음
        #endregion

        ButtonSFX = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayerId.text = ControllerManager.GetInstance().PlayerId;

        text.text = transform.name;

        AlertMenu.gameObject.SetActive(false);

        onHover = false;

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

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onHover)
        {
            ButtonSFX.Play();

            if (transform.name == "New Game")
            {
                if (ControllerManager.GetInstance().LoadGame)
                {
                    // 저장된 데이터 덮어쓰기 경고 UI
                    AlertMenu.gameObject.SetActive(true);
                }
                else
                {
                    ResetValue();
                    ControllerManager.GetInstance().LoadGame = true;
                    SceneManager.LoadScene("GameStart");
                }

            }
            else if (transform.name == "Load Game" && ControllerManager.GetInstance().LoadGame)
            {
                if (ControllerManager.GetInstance().GameOver)
                {
                    ControllerManager.GetInstance().GameOver = false;
                    ControllerManager.GetInstance().Player_HP = 100;
                    ControllerManager.GetInstance().EnemyCount = 0;
                }

                if (ControllerManager.GetInstance().onBoss)
                {
                    ControllerManager.GetInstance().Player_HP = 100;
                    ControllerManager.GetInstance().EnemyCount = 0;
                }
                #region ...
                // 라운드 넘어갈 때 저장되어 onBoss, onEnemy 모두 false인 경우
                #endregion
                else if (!ControllerManager.GetInstance().onEnemy)
                {
                    ControllerManager.GetInstance().onEnemy = true;
                    ControllerManager.GetInstance().Player_HP = 100;
                }

                SceneManager.LoadScene("GameStart");
            }
            else if (transform.name == "How To Play")
            {
                OptionAnim.SetBool("Move", true);
            }
            else if (transform.name == "Quit")
            {
                Application.Quit();  // 에디터에서는 무시됨
                UnityEditor.EditorApplication.ExitPlaymode();  // 에디터 플레이모드 나가기
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
        ControllerManager.GetInstance().Round = 1;
        ControllerManager.GetInstance().Goal = 20;
        ControllerManager.GetInstance().onEnemy = true;
        ControllerManager.GetInstance().onBoss = false;
        ControllerManager.GetInstance().BossActive = false;

        ControllerManager.GetInstance().Player_HP = 100;
        ControllerManager.GetInstance().BulletSpeed = 10.0f;
        ControllerManager.GetInstance().Heart = 3;
        ControllerManager.GetInstance().EnemyCount = 0;

        ControllerManager.GetInstance().EnemyId = 1;
        ControllerManager.GetInstance().BossId = 0;

        ControllerManager.GetInstance().GameOver = false;
        ControllerManager.GetInstance().GameClear = false;
    }

    
    public void LogOutInGame()  // LogOut Button OnClick
    {
        // all button interactble false
        foreach (Selectable selectableUI in Selectable.allSelectablesArray)
        {
            selectableUI.interactable = false;
        }

        #region ...
        // 불러올 데이터가 없는 플레이어가 로그인했을 때를 위해 이전 플레이어 값들을 리셋
        #endregion
        ResetValue();
        ControllerManager.GetInstance().LoadGame = false;
        ControllerManager.GetInstance().GoalClear = false;
        ControllerManager.GetInstance().BossClear = false;

        DataManager.GetInstance.LogOut();

        StartCoroutine(LoadLogInScene());
    }

    private IEnumerator LoadLogInScene()
    {
        while (true)
        {
            if (DataManager.GetInstance.isDone)
            {
                DataManager.GetInstance.isDone = false;

                SceneManager.LoadScene("LogInScene");

                yield break;
            }

            yield return null;
        }
    }


    public void AlertYES()  // Button onClick
    {
        ButtonSFX.Play();

        // New Game 실행
        ResetValue();
        ControllerManager.GetInstance().LoadGame = true;
        SceneManager.LoadScene("GameStart");
    }

    public void AlertNO()  // Button onClick
    {
        ButtonSFX.Play();

        // 경고창 끄기
        AlertMenu.gameObject.SetActive(false);
    }
}
