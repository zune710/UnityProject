using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollView : MonoBehaviour
{
    public GameObject ui;
    public RectTransform uiTranspos;

    public GameObject SoundButton;
    public Sprite SoundOn;
    public Sprite SoundOff;

    public GameObject SoundSlider;
    private Slider Volume;

    private bool active;

    public float sizeX;
    public float sizeY;


    private void Awake()
    {
        uiTranspos = ui.GetComponent<RectTransform>();
        Volume = SoundSlider.GetComponent<Slider>();
    }

    private void Start()
    {
        sizeX = 350.0f;
        sizeY = 350.0f;

        active = true;

        Volume.maxValue = 100;
        Volume.value = 50;

        StartCoroutine(EffectUi());
    }

    private void Update()
    {
        // VolumeControl
        if (Volume.value == 0)
        {
            SoundButton.GetComponent<Image>().sprite = SoundOff;
            active = false;
        }
        else
        {
            SoundButton.GetComponent<Image>().sprite = SoundOn;
            active = true;
        }
    }

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

    public void SoundOnOff()
    {
        if(active)
        {
            SoundButton.GetComponent<Image>().sprite = SoundOff;
            Volume.value = 0;
            
            // Sound Off
        }
        else
        {
            SoundButton.GetComponent<Image>().sprite = SoundOn;
            Volume.value = 50;
            
            // Sound On
        }

        active = !active;
    }
}
