using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    public GameObject ui;
    public RectTransform uiTranspos;

    public float sizeX;
    public float sizeY;

    //private bool active;

    private void Awake()
    {
        uiTranspos = ui.GetComponent<RectTransform>();
    }

    private void Start()
    {
        sizeX = 350.0f;
        sizeY = 350.0f;

        //active = true;

        StartCoroutine(EffectUi());
    }
    
    //private void Update()
    //{
    //    if(active)
    //    {
    //        active = false;
    //        StartCoroutine(EffectUi());
    //    }
    //}

    private void OnEnable()
    {
        StartCoroutine(EffectUi());
    }

    private void OnDisable()
    {
        uiTranspos.sizeDelta = new Vector2(5.0f, 5.0f);
    }

    IEnumerator EffectUi()
    {
        float fTime = 0.0f;

        while(uiTranspos.sizeDelta.y < sizeY)
        {
            fTime += Time.unscaledDeltaTime * 10.0f;

            uiTranspos.sizeDelta = Vector2.Lerp(
                new Vector2(5.0f, 5.0f),
                new Vector2(5.0f, sizeY),
                fTime);

            yield return null;
        }

        fTime = 0.0f;

        while (uiTranspos.sizeDelta.x < sizeX)
        {
            fTime += Time.unscaledDeltaTime * 7.0f;

            uiTranspos.sizeDelta = Vector2.Lerp(
                new Vector2(5.0f, sizeY),
                new Vector2(sizeX, sizeY),
                fTime);

            yield return null;
        }
    }
}
