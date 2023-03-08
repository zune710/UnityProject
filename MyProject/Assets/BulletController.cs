using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // private SpriteRenderer spriteRenderer;

    // ** 총알이 날아가는 속도
    private float Speed;

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
    // Trigger: 지나치는 상태 / Collider:
    // Enter: 처음 부딪힌 순간 / Stay: 충돌체 안에 있을 때 / Exit: 충돌체에서 빠져나오기 직전의 순간
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyObject(this.gameObject);
    }
}
