using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Text text;
    private RectTransform rectTransform;

    private Color OldColor;


    private void Awake()
    {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        text.text = transform.name;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print(eventData.position.x);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print(eventData.position.x);
        
        text.color = OldColor;

        // Down, Up 위치 같아야(버튼 위에 마우스 있는 상태로 뗴야) LoadScene하도록 하기(예외처리)
        //if(eventData.position.x)
        if (transform.name == "Game Start")
            SceneManager.LoadScene(text.text);
        //else if (transform.name == "Quit")
            
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print(eventData.position.x);

        OldColor = text.color;
        text.color = Color.white;
    }
}
