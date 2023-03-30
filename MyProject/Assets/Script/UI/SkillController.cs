using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    private List<GameObject> SlotButtons = new List<GameObject>();
    private List<Image> ButtonImages = new List<Image>();
    private List<Image> FillImages = new List<Image>();
    private float cooldown;
    private int slot;
    private int maxHP;

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
            SlotButtons.Add(SkillsObj.transform.GetChild(i).gameObject);

        for (int i = 0; i < SlotButtons.Count; ++i)
            ButtonImages.Add(SlotButtons[i].transform.GetComponent<Image>());

        for (int i = 0; i < SlotButtons.Count; ++i)
            FillImages.Add(SlotButtons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>());


        cooldown = 0.0f;
        maxHP = ControllerManager.GetInstance().Player_HP;
    }

    public void PushButton()
    {
        if (Time.timeScale > 0)
        {
            FillImages[slot - 1].fillAmount = 1;
            SlotButtons[slot - 1].GetComponent<Button>().enabled = false;
        }

        StartCoroutine(Cooldown_Coroutine());
    }

    IEnumerator Cooldown_Coroutine()
    {
        float cool = cooldown;
        int value = slot - 1;

        while(FillImages[value].fillAmount != 0)
        {
            FillImages[value].fillAmount -= Time.deltaTime * cool;
            yield return null;
        }

        SlotButtons[value].GetComponent<Button>().enabled = true;
    }


    public void Slot1_PowerUp()  // Big Bullet
    {
        if (Time.timeScale > 0)
        {
            slot = 1;
            cooldown = 0.1f;  // 0.5f
            BulletTest();
        }
    }

    public void Slot2_SpeedUp()  // Bullet Speed 증가
    {
        if (Time.timeScale > 0)
        {
            slot = 2;
            cooldown = 0.05f;  // 0.5f

            FillImages[slot - 1].fillAmount = 1;
            SlotButtons[slot - 1].GetComponent<Button>().enabled = false;

            StartCoroutine(UsingSpeedUp());
        }
    }

    private IEnumerator UsingSpeedUp()
    {

        float speed = ControllerManager.GetInstance().BulletSpeed;

        ControllerManager.GetInstance().BulletSpeed += 5.0f;  // 1.0f

        yield return new WaitForSeconds(5.0f);

        ControllerManager.GetInstance().BulletSpeed = speed;  // 원래대로

        PushButton();
    }

    public void Slot3_Heal()  // 회복
    {
        if (Time.timeScale > 0)
        {
            slot = 3;
            cooldown = 0.01f;  // 0.5f


            int value = 10;
            int goal = ControllerManager.GetInstance().Player_HP + value;

            if (goal > maxHP)
                StartCoroutine(Healing(maxHP));
            else
                StartCoroutine(Healing(goal));
        }
    }

    private IEnumerator Healing(int goal)
    {
        while (ControllerManager.GetInstance().Player_HP < goal)
        {
            ++ControllerManager.GetInstance().Player_HP;
            yield return null;
        }
    }

    private void BulletTest()
    {
        SpriteRenderer spriteRenderer = Player.GetComponent<SpriteRenderer>();

        float Hor = Input.GetAxisRaw("Horizontal");

        GameObject Obj = Instantiate(BulletPrefab);

        Obj.transform.position = Player.transform.position;
        // Bullet 크기 조정
        Obj.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        // 그림자 위치 조정
        Obj.transform.GetChild(0).transform.position += new Vector3(0.0f, 1.0f, 0.0f);
        // Collider 위치 조정
        Obj.GetComponent<CapsuleCollider2D>().offset = new Vector2(0.42f, -0.03f);

        BulletController Controller = Obj.AddComponent<BulletController>();
        SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

        Controller.fxPrefab = fxPrefab;
        Controller.hp = 50;

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