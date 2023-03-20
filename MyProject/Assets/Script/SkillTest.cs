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

    private GameObject BulletPrefab;
    private GameObject fxPrefab;
    private GameObject Player;


    private void Awake()
    {
        BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
    }

    private void Start()
    {
        GameObject SkillsObj = GameObject.Find("Skills");
        Player = GameObject.Find("Player");

        for (int i = 0; i < SkillsObj.transform.childCount; ++i)
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

    public void Testcase1()  // Bullet Speed 증가
    {
        slot = 1;
        cooldown = 0.5f;

        ControllerManager.GetInstance().BulletSpeed += 1.0f;
    }

    public void Testcase2()  // 방어(보호막)
    {
        slot = 2;
        cooldown = 0.5f;
    }

    public void Testcase3()  // 이속 증가
    {
        slot = 3;
        cooldown = 0.5f;

    }

    public void Testcase4()  // 광역 공격
    {
        slot = 4;
        cooldown = 0.5f;

    }

    public void Testcase5()  // 회복
    {
        slot = 5;
        cooldown = 0.5f;

        int value = 10;

        if (ControllerManager.GetInstance().PlayerHP + value > ControllerManager.GetInstance().PlayerMaxHP)
            ControllerManager.GetInstance().PlayerHP = ControllerManager.GetInstance().PlayerMaxHP;
        else
            ControllerManager.GetInstance().PlayerHP += value;
    }

    private void ThrowBullet()
    {
        SpriteRenderer spriteRenderer = Player.GetComponent<SpriteRenderer>();

        float Hor = Input.GetAxisRaw("Horizontal");

        GameObject Obj = Instantiate(BulletPrefab);
        // Obj.transform.name = "";
        Obj.transform.position = Player.transform.position;  // 플레이어 position 위치에 놓음
        BulletController Controller = Obj.AddComponent<BulletController>();
        SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

        Controller.fxPrefab = fxPrefab;

        renderer.flipY = spriteRenderer.flipX;

        if (Hor == 0)
        {
            if (spriteRenderer.flipX)
                Controller.Direction = new Vector3(-1.0f, 0.0f, 0.0f);
            else
                Controller.Direction = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else
            Controller.Direction = new Vector3(Hor, 0.0f, 0.0f);
    }
}
