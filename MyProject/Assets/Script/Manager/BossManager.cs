using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public enum BossType  // 이름 안 쓰고 다음으로 넘어갈 수 있도록 바꾸기
    {
        PENGUIN
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
    private GameObject Parent;

    // ** Boss로 사용할 원형 객체
    private GameObject Prefab;
    private GameObject HPPrefab;

    public bool active;

    public BossType bossType;
    public int bossHP;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            active = false;

            bossType = BossType.PENGUIN;
            bossHP = 10;

            // ** 생성되는 Boss를 담아둘 상위 객체
            Parent = new GameObject("BossList");

            // ** Enemy로 사용할 원형 객체
            Prefab = Resources.Load("Prefabs/Boss/" + bossType.ToString()) as GameObject;
            HPPrefab = Resources.Load("Prefabs/BossHPSlider") as GameObject;
        }
    }


    private void Update()
    {
        if (ControllerManager.GetInstance().onBoss && active)
        {
            // ** Enemy 원형 객체를 복제한다.
            GameObject Obj = Instantiate(Prefab);

            // ** Enemy HP UI 복제
            GameObject Bar = Instantiate(HPPrefab);

            // ** 복제된 UI를 캔버스에 위치시킨다.
            // Bar.transform.parent = GameObject.Find("EnemyHPCanvas").transform;  // 경고 뜸
            Bar.transform.SetParent(GameObject.Find("EnemyHPCanvas").transform);

            // ** Enemy 작동 스크립트 포함
            //Obj.AddComponent<EnemyController>();  // 애니메이션 이벤트 함수가 안 떠서 이렇게 안 하고 Enemy에 script 바로 넣어줌

            // ** 클론의 위치를 초기화
            Obj.transform.position = new Vector3(
                18.0f, -7.5f, 0.0f);

            // ** 클론의 이름 초기화
            Obj.transform.name = "Boss";

            // ** 클론의 계층구조 설정
            Obj.transform.SetParent(Parent.transform);

            // ** UI 객체가 들고 있는 스크립트에 접근
            BossHPBar bossHPBar = Bar.GetComponent<BossHPBar>();

            // ** 스크립트의 Target을 지금 생성된 Enemy로 세팅
            bossHPBar.Target = Obj;

            active = false;  // 하나 생성되면 새로 생성 안 되게 함
        }
    }
}