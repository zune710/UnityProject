using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollView : MonoBehaviour
{
    public GameObject ui;
    private RectTransform uiTranspos;

    public GameObject BgmSoundButton;
    public GameObject SfxSoundButton;
    public Sprite SoundOn;
    public Sprite SoundOff;

    public Slider BgmVolume;
    public Slider SfxVolume;

    private bool BgmActive;
    private bool SfxActive;

    public float sizeX;
    public float sizeY;

    private AudioSource ButtonSFX;
    private AudioSource BGM;
    private AudioSource SFX;


    private void Awake()
    {
        uiTranspos = ui.GetComponent<RectTransform>();

        BGM = GameObject.Find("GameManager").GetComponent<AudioSource>();
        SFX = GameObject.Find("CanvasController").GetComponent<AudioSource>();  // MainButton
        ButtonSFX = GetComponent<AudioSource>();  // SubButton
    }

    private void Start()
    {
        BgmActive = true;
        SfxActive = true;

        BgmVolume.maxValue = 1;
        BgmVolume.value = BGM.volume;

        SfxVolume.maxValue = 1;
        SfxVolume.value = SFX.volume;

        StartCoroutine(EffectUi());
    }

    private void Update()
    {
        BGM.volume = BgmVolume.value;
        SFX.volume = SfxVolume.value;
        ButtonSFX.volume = SfxVolume.value;

        // VolumeControl
        if (BgmVolume.value == 0)
        {
            BgmSoundButton.GetComponent<Image>().sprite = SoundOff;
            BgmActive = false;
        }
        else
        {
            BgmSoundButton.GetComponent<Image>().sprite = SoundOn;
            BgmActive = true;
        }

        if (SfxVolume.value == 0)
        {
            SfxSoundButton.GetComponent<Image>().sprite = SoundOff;
            SfxActive = false;
        }
        else
        {
            SfxSoundButton.GetComponent<Image>().sprite = SoundOn;
            SfxActive = true;
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

        while(uiTranspos.sizeDelta.x < sizeX)
        {
            fTime += Time.unscaledDeltaTime * 10.0f;

            uiTranspos.sizeDelta = Vector2.Lerp(
                new Vector2(5.0f, 5.0f),
                new Vector2(sizeX, 5.0f),
                fTime);

            yield return null;
        }

        fTime = 0.0f;

        while (uiTranspos.sizeDelta.y < sizeY)
        {
            fTime += Time.unscaledDeltaTime * 7.0f;

            uiTranspos.sizeDelta = Vector2.Lerp(
                new Vector2(sizeX, 5.0f),
                new Vector2(sizeX, sizeY),
                fTime);

            yield return null;
        }
    }

    public void BgmSoundOnOff()
    {
        ButtonSFX.Play();

        if (BgmActive)
        {
            // Sound Off
            BgmSoundButton.GetComponent<Image>().sprite = SoundOff;
            BgmVolume.value = 0;
        }
        else
        {
            // Sound On
            BgmSoundButton.GetComponent<Image>().sprite = SoundOn;
            BgmVolume.value = 0.7f;
        }

        BgmActive = !BgmActive;
    }

    public void SfxSoundOnOff()
    {
        ButtonSFX.Play();

        if (SfxActive)
        {
            // Sound Off
            SfxSoundButton.GetComponent<Image>().sprite = SoundOff;
            SfxVolume.value = 0;
        }
        else
        {
            // Sound On
            SfxSoundButton.GetComponent<Image>().sprite = SoundOn;
            SfxVolume.value = 0.7f;
        }

        SfxActive = !SfxActive;
    }
}
