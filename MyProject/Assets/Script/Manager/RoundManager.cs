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
    private GameObject BossHPBar;
    private Slider GoalSlider;
    private Slider BossHPSlider;
    private Text RoundText;
    private Text BossText;


    //private List<GameObject> Enemies;

    // Enemy 처치 수(목표)
    public int Goal;

    public int Round;
    public bool GoalClear;
    public bool BossClear;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            RoundInfoCanvas = GameObject.Find("RoundInfoCanvas");
            GoalBar = RoundInfoCanvas.transform.Find("GoalSlider").gameObject;
            BossHPBar = RoundInfoCanvas.transform.Find("BossHPSlider").gameObject;
            GoalSlider = GoalBar.GetComponent<Slider>();
            BossHPSlider = BossHPBar.GetComponent<Slider>();
            RoundText = GoalSlider.transform.Find("RoundText").gameObject.GetComponent<Text>();
            BossText = BossHPSlider.transform.Find("BossText").gameObject.GetComponent<Text>();

            // ** 씬이 변경되어도 계속 유지될 수 있게 해준다.
            //DontDestroyOnLoad(this.gameObject);  // this 생략 가능(색이 어두우면 생략해도 된다는 뜻!)

            //Enemies = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Enemies"));
        }
    }
   private void Start()
    {
        Round = 1;
        Goal = 3;
        GoalClear = false;
        BossClear = false;

        RoundText.text = "Round " + Round.ToString();
        BossText.text = BossManager.GetInstance.bossType.ToString();

        GoalSlider.maxValue = Goal;
        GoalSlider.value = 0;

        BossHPSlider.maxValue = BossManager.GetInstance.bossHP;
        BossHPSlider.value = BossHPSlider.maxValue;
    }

    private void Update()
    {
        if (ControllerManager.GetInstance().onEnemy)
        {
            GoalSlider.value = ControllerManager.GetInstance().EnemyCount;
        }

        if (GameObject.Find("Boss"))
        {
            BossHPSlider.value = GameObject.Find("Boss").GetComponent<BossController>().HP;
        }


        if (GoalClear || BossClear)
        {
            // 목표 도달 ->  다음 라운드 -> ...

            // 변수 리셋(if문 한 번만 실행되게)
            GoalClear = false;
            BossClear = false;

            // Round Setting
            if (ControllerManager.GetInstance().onEnemy)
            {
                // Enemy Off
                ControllerManager.GetInstance().onEnemy = false;  // 78줄 멈추게
                StartCoroutine(BossRoundSetting());
            }
            else if(ControllerManager.GetInstance().onBoss)
                StartCoroutine(EnemyRoundSetting());

            // Reload Scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        
        yield return new WaitForSeconds(1.5f);

        // Boss Type 변경
        BossManager.GetInstance.bossType = BossManager.BossType.PENGUIN;
        BossText.text = BossManager.GetInstance.bossType.ToString();
        
        BossHPSlider.maxValue = BossManager.GetInstance.bossHP;
        BossHPSlider.value = BossHPSlider.maxValue;

        // Boss On(2/2)
        BossManager.GetInstance.active = true;

        //BossHPBar On
        BossHPBar.SetActive(true);
    }

    private IEnumerator EnemyRoundSetting()
    {
        // Boss Off
        ControllerManager.GetInstance().onBoss = false;

        yield return new WaitForSeconds(1.5f);

        //BossHPBar Off
        BossHPBar.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        // 라운드 증가
        ++Round;
        RoundText.text = "Round" + Round.ToString();

        // Enemy 처치 목표 증가
        Goal *= 2;
        GoalSlider.maxValue = Goal;
        GoalSlider.value = 0;

        // Enemy Type 변경
        EnemyManager.GetInstance.enemyType = EnemyManager.EnemyType.Plant;

        // Enemy On
        ControllerManager.GetInstance().onEnemy = true;

        // GoalBar On
        GoalBar.SetActive(true);
    }
}
