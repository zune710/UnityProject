using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // private SpriteRenderer spriteRenderer;

    // ** 총알이 날아가는 속도
    private float Speed;

    // ** 총알이 충돌한 횟수
    private int hp;

    // ** 이펙트 효과 원본
    public GameObject fxPrefab;


    // private float DefaultX;  // 생성될 때 position.x 값

    // ** 총알이 날아가야 할 방향
    public Vector3 Direction { get; set; } // 과 동일
    //public Vector3 Direction
    //{
    //    get
    //    {
    //        return Direction;
    //    }
    //    set
    //    {
    //        Direction = value;
    //    }
    //}  // stack overflow 발생(왜?)

    private void Start()
    {
        // spriteRenderer = this.GetComponent<SpriteRenderer>();

        // ** 속도 초기값
        Speed = 10.0f;

        // ** 충돌 횟수를 3으로 지정한다.
        hp = 3;

        // DefaultX = transform.position.x;
    }

    void Update()
    {
        //if (Direction.x == -1)
        //    spriteRenderer.flipY = true;
        //else
        //    spriteRenderer.flipY = false;

        // ** 방향으로 속도만큼 위치를 변경
        transform.position += Direction * Speed * Time.deltaTime;

        //if(Mathf.Abs(DefaultX - transform.position.x) > 5)  // 처음 위치보다 일정 거리만큼 멀어지면 사라진다.
        //    Destroy(this.gameObject);
    }

    // ** 충돌체(Collider2D)와 물리엔진(Rigidbody2D)이 포함된 오브젝트가 다른 충돌체와 충돌한다면 실행되는 함수
    // Trigger: 충돌하지만 통과됨 / Collider: 충돌하면 움직임이 막힘
    // Enter: 처음 부딪힌 순간 / Stay: 충돌체 안에 있을 때 / Exit: 충돌체에서 빠져나오기 직전의 순간
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ** 충돌 횟수 차감
        --hp;

        // ** 이펙트 효과 복제
        GameObject Obj = Instantiate(fxPrefab);

        // ** 이펙트 효과의 위치를 지정
        Obj.transform.position = transform.position;

        // ** collision = 충돌한 대상
        // ** 충돌한 대상을 삭제한다. ??
        // (총알이 카메라 밖을 벗어나 보이지 않는 벽에 충돌하면 삭제됨)
        if (collision.transform.tag == "Wall")
            Destroy(this.gameObject);
        else
        {
            // ** 진동 효과를 생성할 관리자 생성
            GameObject camera = new GameObject("Camera Test");

            // ** 진동 효과 컨트롤러 생성
            camera.AddComponent<CameraController>();
        }

        // ** 총알의 충돌 횟수가 0이 되면(충돌 가능 횟수를 모두 소진하면) 총알 삭제
        if (hp == 0)
            Destroy(this.gameObject, 0.016f);
    }
}

// 유니티에서는 안 쓰는 함수도 실행되면서 스택이 쌓이기 때문에 주석 처리하지 않고 지우는 것이 좋다.
