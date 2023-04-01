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

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
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
}
