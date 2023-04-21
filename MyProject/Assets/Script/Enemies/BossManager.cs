using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public enum BossType
    {
        PENGUIN = 1,
        RHINO = 2,
        TREE = 3
    };

    private BossManager() { }
        
    private static BossManager instance = null;

    public static BossManager GetInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    // ** 생성되는 Boss를 담아둘 상위 객체
    public GameObject Parent;

    // ** Boss로 사용할 원형 객체
    private GameObject Prefab;
    private GameObject HPPrefab;

    public BossType bossType;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // ** 생성되는 Boss를 담아둘 상위 객체
            Parent = new GameObject("BossList");
        }
    }

    private void Start()
    {
        GetBossInfo();

        // ** Enemy로 사용할 원형 객체
        Prefab = Resources.Load("Prefabs/Boss/" + bossType.ToString()) as GameObject;
        HPPrefab = Resources.Load("Prefabs/BossHPSlider") as GameObject;
    }

    private void Update()
    {
        GetBossInfo();
        Prefab = Resources.Load("Prefabs/Boss/" + bossType.ToString()) as GameObject;

        if (ControllerManager.GetInstance().onBoss && ControllerManager.GetInstance().BossActive)
        {
            // ** Enemy 원형 객체를 복제한다.
            GameObject Obj = Instantiate(Prefab);

            // ** Enemy HP UI 복제
            GameObject Bar = Instantiate(HPPrefab);

            // ** 복제된 UI를 캔버스에 위치시킨다.
            Bar.transform.SetParent(GameObject.Find("RoundInfoCanvas").transform);

            // ** 클론의 위치를 초기화
            Obj.transform.position = new Vector3(
                18.0f, -4.5f, 0.0f);   // 18.0f, -7.5f, 0.0f

            // ** 클론의 이름 초기화
            Obj.transform.name = "Boss";

            // ** 클론의 계층구조 설정
            Obj.transform.SetParent(Parent.transform);

            // ** UI 객체가 들고 있는 스크립트에 접근
            BossHPBar bossHPBar = Bar.GetComponent<BossHPBar>();

            // ** 스크립트의 Target을 지금 생성된 Enemy로 세팅
            bossHPBar.Target = Obj;

            // 하나만 생성되게 함
            ControllerManager.GetInstance().BossActive = false;
        }
    }

    private void GetBossInfo()
    {
        switch (ControllerManager.GetInstance().BossId)
        {
            case 1:
                bossType = BossType.PENGUIN;
                break;

            case 2:
                bossType = BossType.RHINO;
                break;

            case 3:
                bossType = BossType.TREE;
                break;
        }
    }
}