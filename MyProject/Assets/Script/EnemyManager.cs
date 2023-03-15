using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // ** 씬이 변경되어도 계속 유지될 수 있게 해준다.
            DontDestroyOnLoad(this.gameObject);  // this 생략 가능(색이 어두우면 생략해도 된다는 뜻!)

            // ** 생성되는 Enemy를 담아둘 상위 객체
            Parent = new GameObject("EnemyList");

            // ** Enemy로 사용할 원형 객체
            Prefab = Resources.Load("Prefabs/Enemy/Enemy") as GameObject;
        }
    }


    // ** 시작하자마자 Start 함수를 코루틴 함수로 실행
    private IEnumerator Start()
    {
        while(true)
        {
            // ** Enemy 원형 객체를 복제한다.
            GameObject Obj = Instantiate(Prefab);

            // ** Enemy 작동 스크립트 포함
            //Obj.AddComponent<EnemyController>();  // 애니메이션 이벤트 함수가 안 떠서 이렇게 안 하고 Enemy에 script 바로 넣어줌

            // ** 클론의 위치를 초기화
            Obj.transform.position = new Vector3(
                18.0f, Random.Range(-8.2f, -5.5f), 0.0f);

            // ** 클론의 이름 초기화
            Obj.transform.name = "Enemy";

            // ** 클론의 계층구조 설정
            Obj.transform.parent = Parent.transform;

            // ** 1.5초 휴식
            yield return new WaitForSeconds(1.5f);
        }
    }
}
