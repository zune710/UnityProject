using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTest : MonoBehaviour
{
    private List<GameObject> Images = new List<GameObject>();
    private List<GameObject> Buttons = new List<GameObject>();
    private List<Image> ButtonImages = new List<Image>();
    private float cooldown;
    private int slot;

    private void Start()
    {
        GameObject SkillsObj = GameObject.Find("Skills");

        for(int i = 0; i < SkillsObj.transform.childCount; ++i)
            Images.Add(SkillsObj.transform.GetChild(i).gameObject);

        for(int i = 0; i < Images.Count; ++i)
            Buttons.Add(Images[i].transform.GetChild(0).gameObject);

        for (int i = 0; i < Buttons.Count; ++i)
            ButtonImages.Add(Buttons[i].transform.GetComponent<Image>());

        cooldown = 0.0f;
    }

    public void PushButton()
    {
        ButtonImages[slot - 1].fillAmount = 0;
        Buttons[slot - 1].GetComponent<Button>().enabled = false;

        StartCoroutine(Testcase_Coroutine());
    }

    IEnumerator Testcase_Coroutine()
    {
        float cool = cooldown;

        while(ButtonImages[slot - 1].fillAmount != 1)
        {
            ButtonImages[slot - 1].fillAmount += Time.deltaTime * cool;
            yield return null;
        }

        Buttons[slot - 1].GetComponent<Button>().enabled = true;
    }

    public void Testcase1()
    {
        slot = 1;
        cooldown = 0.5f;
        ControllerManager.GetInstance().BulletSpeed += 1.0f;

    }

    public void Testcase2()
    {
        slot = 2;
        cooldown = 0.5f;
    }

    public void Testcase3()
    {
        slot = 3;
        cooldown = 0.5f;

    }

    public void Testcase4()
    {
        slot = 4;
        cooldown = 0.5f;

    }

    public void Testcase5()
    {
        slot = 5;
        cooldown = 0.5f;
        if (ControllerManager.GetInstance().PlayerHP + 10 > 100)
            ControllerManager.GetInstance().PlayerHP = 100;
        else
            ControllerManager.GetInstance().PlayerHP += 10;
    }
}
