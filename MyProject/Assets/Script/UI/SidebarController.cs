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
    public GameObject DeleteDataText;  // Inspector
    public GameObject Bunny;  // Inspector

    private GameObject gameManager;

    private Animator Anim;
    private Animator HeartAnim;
    private Animator DeleteDataTextAnim;
    private Animator BunnyAnim;

    private AudioSource ButtonSFX;
    private AudioSource BGM;

    private AudioClip[] BGMs;

    public bool check;
    public bool active;


    private void Awake()
    {
        Menu = transform.Find("MenuFrame").gameObject;
        End = transform.Find("GameEnd").gameObject;
        Dark = transform.Find("DarkBackground").gameObject;
        brokenHeart = transform.Find("BrokenHeart").gameObject;

        gameManager = GameObject.Find("GameManager").gameObject;

        Anim = Menu.GetComponent<Animator>();
        HeartAnim = brokenHeart.GetComponent<Animator>();
        DeleteDataTextAnim = DeleteDataText.GetComponent<Animator>();
        BunnyAnim = Bunny.GetComponent<Animator>();

        ButtonSFX = GetComponent<AudioSource>();
        BGM = gameManager.GetComponent<AudioSource>();

        BGMs = gameManager.GetComponent<RoundManager>().BGMs;
    }

    void Start()
    {
        check = false;
        active = true;
        End.SetActive(false);
        Dark.SetActive(false);
        brokenHeart.SetActive(false);
        DeleteDataText.SetActive(false);

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

                BGM.clip = BGMs[2];  // GameClear
                BGM.Play();

                Time.timeScale = 0.0f;
                Dark.SetActive(true);

                BunnyAnim.SetTrigger("Move");

                End.GetComponent<Text>().text = "GAME CLEAR";
                End.SetActive(true);

                StartCoroutine(GameClearAnim());

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

        // Player HP Bar 위 Heart 하나 삭제
        Destroy(HeartCount.transform.GetChild(0).gameObject);

        // End(Game Over) UI Animation
        End.GetComponent<Animator>().SetTrigger("Move");
        
        // Heart가 0일 때 저장된 데이터 삭제 알림
        if(ControllerManager.GetInstance().Heart == 0)
        {
            DeleteDataText.SetActive(true);
            DeleteDataTextAnim.SetTrigger("Move");
        }
    }

    private IEnumerator GameClearAnim()
    {
        // Time.timeScale = 0.0f일 때는 Realtime 사용
        yield return new WaitForSecondsRealtime(4.0f);  // Bunny Anim 재생시간(5초)

        // End(Game Clear) UI Animation
        End.GetComponent<Animator>().SetTrigger("Move");
    }
}
