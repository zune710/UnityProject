using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Text text;
    private RectTransform rectTransform;

    private Color OldColor;

    private bool onHover;

    private void Awake()
    {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
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
        text.color = OldColor;

        // 버튼 위에 마우스 있는 상태로 떼야 LoadScene하도록 하기(예외처리)
        //if(eventData.position.x)
        if(onHover)
        {
            if (transform.name == "Game Start")
                SceneManager.LoadScene(text.text);
            else if (transform.name == "Quit")
            {
                Application.Quit();  // 에디터에서는 무시됨
                UnityEditor.EditorApplication.ExitPlaymode();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OldColor = text.color;
        text.color = Color.white;
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
