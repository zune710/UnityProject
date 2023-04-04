using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SidebarController : MonoBehaviour
{
    
    private GameObject Menu;
    private GameObject End;
    private GameObject Dark;
    private GameObject brokenHeart;

    public GameObject HeartCount;  // Inspector

    private Animator Anim;
    private Animator HeartAnim;

    private AudioSource ButtonSFX;

    public bool check;
    public bool active;


    private void Awake()
    {
        Menu = transform.Find("MenuFrame").gameObject;
        End = transform.Find("GameEnd").gameObject;
        Dark = transform.Find("DarkBackground").gameObject;
        brokenHeart = transform.Find("BrokenHeart").gameObject;

        Anim = Menu.GetComponent<Animator>();
        HeartAnim = brokenHeart.GetComponent<Animator>();

        ButtonSFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        check = false;
        active = true;
        End.SetActive(false);
        Dark.SetActive(false);
        brokenHeart.SetActive(false);

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if(active)
        {
            if (ControllerManager.GetInstance().GameOver)
            {
                active = false;
                
                Time.timeScale = 0.0f;
                Dark.SetActive(true);

                brokenHeart.SetActive(true);
                HeartAnim.SetTrigger("Broken");

                End.GetComponent<Text>().text = "GAME OVER";
                End.SetActive(true);

                StartCoroutine(GameOverAnim());

            }
            else if (ControllerManager.GetInstance().GameClear)
            {
                active = false;

                Time.timeScale = 0.0f;
                Dark.SetActive(true);

                End.GetComponent<Text>().text = "GAME CLEAR";
                End.SetActive(true);
                End.GetComponent<Animator>().SetTrigger("Move");

                ControllerManager.GetInstance().LoadGame = false;
            }
        }
    }

    public void ClickButton()
    {
        ButtonSFX.Play();

        check = !check;
        Dark.SetActive(check);
        Anim.SetBool("Move", check);

        if (check)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    private IEnumerator GameOverAnim()
    {
        // Time.timeScale = 0.0f일 때는 Realtime 사용
        yield return new WaitForSecondsRealtime(2.0f);  // BrokenHeart Anim 재생시간만큼

        Destroy(HeartCount.transform.GetChild(0).gameObject);

        End.GetComponent<Animator>().SetTrigger("Move");
    }
}
