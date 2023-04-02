using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Text text;

    //private Color OldColor;

    private bool onHover;

    public Button LoadGame;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();

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
        //text.color = OldColor;

        if(onHover)
        {
            if (transform.name == "New Game")
            {
                Time.timeScale = 1.0f;
                ResetValue();
                ControllerManager.GetInstance().LoadGame = true;
                SceneManager.LoadScene("Game Start");
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
            else if (transform.name == "Quit")
            {
                Application.Quit();  // 에디터에서는 무시됨
                UnityEditor.EditorApplication.ExitPlaymode();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //OldColor = text.color;
        //text.color = new Color32(164, 160, 160, 154);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHover = false;
    }

    private void ResetValue()
    {
        ControllerManager.GetInstance().Player_HP = 100;
        ControllerManager.GetInstance().BulletSpeed = 10.0f;
        ControllerManager.GetInstance().EnemyCount = 0;
        ControllerManager.GetInstance().Goal = 3;

        ControllerManager.GetInstance().BossId = 1;
        ControllerManager.GetInstance().EnemyId = 1;
        ControllerManager.GetInstance().Round = 1;


        ControllerManager.GetInstance().onEnemy = true;
        ControllerManager.GetInstance().onBoss = false;

        ControllerManager.GetInstance().GameClear = false;
        ControllerManager.GetInstance().GameOver = false;

    }
}
