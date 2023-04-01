using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public enum EnemyType
    {
        Chameleon = 1,
        Plant = 2,
        Rock1 = 3
    };

    private EnemyManager() { }
        
    private static EnemyManager instance = null;

    public static EnemyManager GetInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    // ** 생성되는 Enemy를 담아둘 상위 객체
    private GameObject Parent;

    // ** Enemy로 사용할 원형 객체
    private GameObject Prefab;
    //private GameObject HPPrefab;

    // ** 플레이어의 누적 이동 거리
    public float Distance;

    public EnemyType enemyType;

    private float SpawnTime;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            Distance = 0.0f;

            // ** 생성되는 Enemy를 담아둘 상위 객체
            Parent = new GameObject("EnemyList");
        }
    }


    // ** 시작하자마자 Start 함수를 코루틴 함수로 실행
    private IEnumerator Start()
    {
        GetEnemyInfo();

        // ** Enemy로 사용할 원형 객체
        Prefab = Resources.Load("Prefabs/Enemies/" + enemyType.ToString()) as GameObject;
        //HPPrefab = Resources.Load("Prefabs/EnemyHPSlider") as GameObject;

        while (true)
        {
            if (ControllerManager.GetInstance().onEnemy) //  && ControllerManager.GetInstance().DirRight
            {
                // ** Enemy 원형 객체를 복제한다.
                GameObject Obj = Instantiate(Prefab);

                // ** Enemy HP UI 복제
                //GameObject Bar = Instantiate(HPPrefab);

                // ** 복제된 UI를 캔버스에 위치시킨다.
                //Bar.transform.SetParent(GameObject.Find("EnemyHPCanvas").transform);

                // ** 클론의 위치를 초기화
                Obj.transform.position = new Vector3(
                    18.0f, Random.Range(-5.9f, -3.1f), 0.0f);  // Random.Range(-8.2f, -5.5f)

                // ** 클론의 이름 초기화
                Obj.transform.name = enemyType.ToString();  // "Enemy"

                // ** 클론의 계층구조 설정
                Obj.transform.SetParent(Parent.transform);

                // ** UI 객체가 들고 있는 스크립트에 접근
                //EnemyHPBar enemyHPBar = Bar.GetComponent<EnemyHPBar>();

                // ** 스크립트의 Target을 지금 생성된 Enemy로 세팅
                //enemyHPBar.Target = Obj;
            }
            // ** 1.5초 휴식
            yield return new WaitForSeconds(SpawnTime);
        }
    }

    private void Update()
    {
        if (ControllerManager.GetInstance().DirRight)
        {
            Distance += Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        }

        Prefab = Resources.Load("Prefabs/Enemies/" + enemyType.ToString()) as GameObject;

        GetEnemyInfo();
    }

    private void GetEnemyInfo()
    {
        switch (RoundManager.GetInstance.EnemyId)
        {
            case 1:
                enemyType = EnemyType.Chameleon;
                SpawnTime = 1.5f;
                break;

            case 2:
                enemyType = EnemyType.Plant;
                SpawnTime = 3.0f;
                break;

            case 3:
                enemyType = EnemyType.Rock1;
                SpawnTime = 3.0f;
                break;
        }
    }
}
