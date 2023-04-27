using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/** Object Pooling 사용 **/
public class EnemyBullet : MonoBehaviour
{
    // ** 총알이 날아가는 속도
    private float Speed;
    public GameObject Target = null;

    public bool Option;
    public bool BasicAttack = false;

    // ** 이펙트 효과 원본
    [HideInInspector]
    public GameObject fxPrefab;

    // ** 총알이 날아가야 할 방향
    [HideInInspector]
    public Vector3 Direction { get; set; }

    private IObjectPool<EnemyBullet> TreeBulletPool;

    private void Start()
    {
        // 기본 공격일 때 총알 속도
        if (BasicAttack)
        {
            Speed = 10.0f;
            return;
        }
        
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
        if (Target)
        {
            if (Option)
            {
                StartCoroutine(TimeLimit(2.0f));
                Option = false;
            }

            Direction = ((Target.transform.position + new Vector3(0.0f, -0.3f, 0.0f)) - transform.position).normalized;

            float fAngle = getAngle(Vector3.down, Direction);

            transform.eulerAngles = new Vector3(0.0f, 0.0f, fAngle + 120f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ** collision = 충돌한 대상
        // ** Wall과 충돌하면 Bullet을 삭제한다.
        if (collision.transform.tag == "Wall")
            TreeBulletPool.Release(this);
        else
        {
            // ** 이펙트 효과 복제
            GameObject Obj = Instantiate(fxPrefab);

            // ** 이펙트 효과의 위치를 지정
            Obj.transform.position = transform.position;

            // ** 진동 효과를 생성할 관리자 생성
            GameObject camera = new GameObject("CameraFX");

            // ** 진동 효과 컨트롤러 생성
            camera.AddComponent<CameraController>();

            // 총알 Release
            TreeBulletPool.Release(this);

        }
    }

    public void SetPool(IObjectPool<EnemyBullet> pool)
    {
        TreeBulletPool = pool;
    }

    public float getAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.down, to - from).eulerAngles.z;
    }
}
