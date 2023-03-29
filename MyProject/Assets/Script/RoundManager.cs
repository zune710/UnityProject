using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Slider GoalBar;
    private Text RoundText;

    //private List<GameObject> Enemies;

    public int Round;
    public bool Next;

    // Enemy 처치 수(목표)
    private int Goal;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            RoundInfoCanvas = GameObject.Find("RoundInfoCanvas");
            GoalBar = RoundInfoCanvas.transform.Find("Slider").GetComponent<Slider>();
            RoundText = GameObject.Find("RoundText").GetComponent<Text>();

            //Enemies = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Enemies"));
        }
    }
   private void Start()
    {
        Round = 1;
        Goal = 3;
        Next = false;

        RoundText.text = "Round" + Round.ToString();

        GoalBar.maxValue = Goal;
        GoalBar.value = 0;
    }

    private void Update()
    {
        if (ControllerManager.GetInstance().onEnemy)
        { 
            GoalBar.value = ControllerManager.GetInstance().EnemyCount;
        }


        if(ControllerManager.GetInstance().EnemyCount == Goal)
        {
            // 목표 도달 -> 라운드 종료 -> 보스전 -> 다음 라운드 -> ...
            ControllerManager.GetInstance().onEnemy = false;
            ControllerManager.GetInstance().onBoss = true;
            BossManager.GetInstance.active = true;

            RoundInfoCanvas.SetActive(false);

            ControllerManager.GetInstance().EnemyCount = 0;

            // + Boss 체력 바 SetActive(true) - Round Goal 위치에 만들기


            //StartCoroutine(PrepareBossRound());
        }

        if (Next)
        {
            ++Round;
            Goal *= 2;

            // Enemy Type 변경
            EnemyManager.GetInstance.enemyType = EnemyManager.EnemyType.Plant;
            // + 다음 Boss 변경

            ControllerManager.GetInstance().onBoss = false;
            ControllerManager.GetInstance().onEnemy = true;

            RoundInfoCanvas.SetActive(true);
        }
    }

    private IEnumerator PrepareBossRound()
    {
        // 목표 도달 -> 라운드 종료 -> 보스전 -> 다음 라운드 -> ...
        ControllerManager.GetInstance().onEnemy = false;
        ControllerManager.GetInstance().onBoss = true;
        BossManager.GetInstance.active = true;

        yield return new WaitForSeconds(1.5f);

        RoundInfoCanvas.SetActive(false);
        ControllerManager.GetInstance().EnemyCount = 0;

        // + Boss 체력 바 SetActive(true) - Round Goal 위치에 만들기

    }
}
