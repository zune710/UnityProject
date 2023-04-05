using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    private RoundManager() { }

    private static RoundManager instance = null;

    public static RoundManager GetInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }
    
    private GameObject RoundInfoCanvas;
    private GameObject GoalBar;
    private Slider GoalSlider;
    private Text RoundText;

    public int Goal; // Enemy 처치 수(목표)

    public bool GoalClear;
    public bool BossClear;

    public GameObject FadeIn;  // Inspector
    private Image FadeInImg;
    private Animator FadeInAnim;

    public GameObject NextInfo;
    private Animator NextInfoAnim;
    public GameObject NextInfoText;
    public GameObject NextInfoBack;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            RoundInfoCanvas = GameObject.Find("RoundInfoCanvas");
            GoalBar = RoundInfoCanvas.transform.Find("GoalSlider").gameObject;
            GoalSlider = GoalBar.GetComponent<Slider>();
            RoundText = GoalSlider.transform.Find("RoundText").gameObject.GetComponent<Text>();

            
            Goal = ControllerManager.GetInstance().Goal;

            GoalClear = ControllerManager.GetInstance().GoalClear;
            BossClear = ControllerManager.GetInstance().BossClear;
            
            if(ControllerManager.GetInstance().onBoss)
                GoalBar.SetActive(false);

            FadeInImg = FadeIn.GetComponent<Image>();
            FadeInAnim = FadeIn.GetComponent<Animator>();

            NextInfoAnim = NextInfo.GetComponent<Animator>();

            // ** 씬이 변경되어도 계속 유지될 수 있게 해준다.
            //DontDestroyOnLoad(this.gameObject);  // this 생략 가능(색이 어두우면 생략해도 된다는 뜻!)
        }
    }

   private void Start()
    {

        RoundText.text = "Round " + ControllerManager.GetInstance().Round.ToString();

        NextInfoText.GetComponent<Text>().text = RoundText.text;

        GoalSlider.maxValue = Goal;
        GoalSlider.value = ControllerManager.GetInstance().EnemyCount;

        // Fade In + Round Info
        StartCoroutine(PlayFadeIn());
    }

    private void Update()
    {
        if (ControllerManager.GetInstance().onEnemy)
        {
            GoalSlider.value = ControllerManager.GetInstance().EnemyCount;
        }

        if (GoalClear || BossClear)
        {
            /* 목표 도달 ->  다음 라운드 -> ... */

            // 변수 리셋(if문 한 번만 실행되게)
            GoalClear = false;
            BossClear = false;

            // Round Setting
            if (ControllerManager.GetInstance().onEnemy)
            {
                // Enemy Off
                ControllerManager.GetInstance().onEnemy = false;  // Update의 첫 번째 조건문 피하게
                StartCoroutine(BossRoundSetting());
            }
            else if(ControllerManager.GetInstance().onBoss)
                StartCoroutine(EnemyRoundSetting());
        }
    }

    private IEnumerator BossRoundSetting()
    {
        // Boss On(1/2)
        ControllerManager.GetInstance().onBoss = true;

        yield return new WaitForSeconds(1.5f);

        // GoalBar Off
        GoalBar.SetActive(false);

        // EnemyCount 리셋
        ControllerManager.GetInstance().EnemyCount = 0;

        // 플레이어 HP 리셋
        ControllerManager.GetInstance().Player_HP = 100;

        // Info UI(Boss)
        NextInfoText.GetComponent<Text>().text = "Boss Round";  // ??
        NextInfoAnim.SetTrigger("Move");

        yield return new WaitForSeconds(2.5f);

        // EnemyId 변경
        ++ControllerManager.GetInstance().EnemyId;

        // Boss On(2/2)
        ControllerManager.GetInstance().BossActive = true;
    }

    private IEnumerator EnemyRoundSetting()
    {
        // Boss Off
        ControllerManager.GetInstance().onBoss = false;

        // 라운드 증가
        ++ControllerManager.GetInstance().Round;
        RoundText.text = "Round" + ControllerManager.GetInstance().Round.ToString();

        yield return new WaitForSeconds(1.5f);

        // Info UI(Round)
        NextInfoText.GetComponent<Text>().text = RoundText.text;
        NextInfoAnim.SetTrigger("Move");

        // 플레이어 HP 리셋
        ControllerManager.GetInstance().Player_HP = 100;

        // 플레이어 위치 초기화
        GameObject Player = GameObject.Find("Player");
        Player.transform.position = new Vector3(0.0f, -4.5f, 0.0f);

        yield return new WaitForSeconds(2.0f);

        // Enemy 처치 목표 증가
        ControllerManager.GetInstance().Goal += 10;
        Goal = ControllerManager.GetInstance().Goal;
        GoalSlider.maxValue = Goal;
        GoalSlider.value = 0;

        // BossId 변경
        ++ControllerManager.GetInstance().BossId;

        // Enemy On
        ControllerManager.GetInstance().onEnemy = true;

        // GoalBar On
        GoalBar.SetActive(true);
    }


    private IEnumerator PlayFadeIn()
    {
        FadeInImg.enabled = true;

        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(1.5f);  // 화면에 뜰 때까지 대기

        FadeInAnim.SetBool("FadeIn", true);

        yield return new WaitForSecondsRealtime(2.0f);  // FadeIn Animation Clip 길이만큼

        FadeInImg.enabled = false;
        FadeInAnim.SetBool("FadeIn", false);

        Time.timeScale = 1.0f;

        // Info UI(Round)
        if(ControllerManager.GetInstance().onBoss)
            NextInfoText.GetComponent<Text>().text = "Boss Round";
        else
            NextInfoText.GetComponent<Text>().text = RoundText.text;
        NextInfoAnim.SetTrigger("Move");

        yield return new WaitForSeconds(2.5f);

        if (ControllerManager.GetInstance().onBoss)
            ControllerManager.GetInstance().BossActive = true;

    }
}
