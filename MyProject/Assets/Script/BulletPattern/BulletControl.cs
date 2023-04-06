using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    // ** 총알이 날아가는 속도
    public float Speed;
    public GameObject Target = null;

    public bool Option;

    // ** 이펙트 효과 원본
    public GameObject fxPrefab;

    // ** 총알이 날아가야 할 방향
    public Vector3 Direction { get; set; }


    private void Awake()
    {
        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
    }

    private void Start()
    {
        // ** 속도 초기값
        Speed = Option ? 10.0f : 1.0f;

        // ** 벡터의 정규화
        if (Option)
            Direction = (Target.transform.position + new Vector3(0.0f, -0.3f, 0.0f)) - transform.position;  // Player Collider 방향으로
        Direction.Normalize();

        float fAngle = getAngle(Vector3.down, Direction);

        transform.eulerAngles = new Vector3(
            0.0f, 0.0f, fAngle + 90f);
    }

    void Update()
    {
        // ** 실시간으로 타깃의 위치를 확인하고 방향을 갱신한다.
        if(Target)
        {
            if(Option)
            {
                StartCoroutine(TimeLimit(2.0f));
                Option = false;
            }
            
            Direction = ((Target.transform.position + new Vector3(0.0f, -0.3f, 0.0f)) - transform.position).normalized;

            float fAngle = getAngle(Vector3.down, Direction);

            transform.eulerAngles =  new Vector3(0.0f, 0.0f, fAngle + 120f);
        }

        // ** 방향으로 속도만큼 위치를 변경
        transform.position += Direction * Speed * Time.deltaTime;
    }

    private IEnumerator TimeLimit(float _time)
    {
        float time = _time;

        while (true)
        {
            if (time <= 0)
                break;

            time -= Time.deltaTime;

            yield return null;
        }

        // ** 이펙트 효과 복제
        GameObject Obj = Instantiate(fxPrefab);

        // ** 이펙트 효과의 위치를 지정
        Obj.transform.position = transform.position;

        Destroy(gameObject);
    }

    //private IEnumerator FollowTarget(float _time)
    //{
    //    float time = _time;

    //    while (time > 0)
    //    {
    //        Direction = (Target.transform.position + new Vector3(0.0f, -0.3f, 0.0f) - transform.position);
    //        Direction.Normalize();

    //        float fAngle = getAngle(Vector3.down, Direction);

    //        transform.eulerAngles = new Vector3(
    //            0.0f, 0.0f, fAngle + 90f);

    //        time -= Time.deltaTime;

    //        yield return null;
    //    }

    //    // ** 이펙트 효과 복제
    //    GameObject Obj = Instantiate(fxPrefab);

    //    // ** 이펙트 효과의 위치를 지정
    //    Obj.transform.position = transform.position;

    //    Destroy(gameObject);
    //}

    // ** 충돌체와 물리엔진이 포함된 오브젝트가 다른 충돌체와 충돌한다면 실행되는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ** 이펙트 효과 복제
        GameObject Obj = Instantiate(fxPrefab);

        // ** 이펙트 효과의 위치를 지정
        Obj.transform.position = transform.position;

        // ** collision = 충돌한 대상
        // ** Wall과 충돌하면 Bullet을 삭제한다.
        if (collision.transform.tag == "Wall")
            Destroy(this.gameObject);
        else
        {
            // ** 진동 효과를 생성할 관리자 생성
            GameObject camera = new GameObject("Camera Test");

            // ** 진동 효과 컨트롤러 생성
            camera.AddComponent<CameraController>();

            Destroy(this.gameObject, 0.016f);
        }
    }

    public float getAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.down, to - from).eulerAngles.z;
        // 오일러 각(eulerAngles)의 짐벌락 현상 때문에 Quaternion 사용
    }

    //void getAngle()
    //{
    //    Vector3 bullet = transform.position;
    //    Vector3 player =  GameObject.Find("Player").transform.position;
    //    Vector3 Direction = (bullet - player).normalized;
    //    float angle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;

    //    transform.rotation = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
    //}
}