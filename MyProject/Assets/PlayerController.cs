using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public은 유니티 Inspector 창에 노출이 된다.

    // ** 움직이는 속도
    private float Speed;
    private Vector3 Movement;

    private int Health;

    private Animator animator;

    private bool onAttack; 
    private bool onHit;
    private bool onJump;
    private bool onRoll;
    private bool onDead;
    private bool onDive;

    // ** 유니티 기본 제공 함수
    // ** 초기값을 설정할 때 사용
    void Start()
    {
        // ** 속도를 초기화
        Speed = 5.0f;

        Health = 10;

        // ** Player의 Animator를 받아온다.
        animator = this.GetComponent<Animator>();  // 'this.' 생략 가능
    }

    // ** 유니티 기본 제공 함수
    // ** 프레임마다 반복적으로 실행되는 함수
    // 초당 60번(보통) ~ 수천번 업데이트
    void Update()
    {
        // ** [실수 연산 IEEE754]

        // ** Input.GetAxis는 -1과 1 사이의 값을 실수로 반환(소수점 단위) / PC, 콘솔 / 움직임 서서히 멈출 때 사용 / 실수 연산 - 부하↑
        // ** Input.GetAxisRaw는 -1, 0, 1 셋 중 하나를 반환 / 모바일, 2D / 게임 만들 때 주로 사용(최적화)
        float Hor = Input.GetAxisRaw("Horizontal");
        float Ver = Input.GetAxisRaw("Vertical");

        // Time.deltaTime: 프레임과 프레임 사이의 간격을 이용해서 컴퓨터 성능과 상관없이 모든 컴퓨터에서 동일하게 작동되도록 하기 위한 것
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed,
            Ver * Time.deltaTime * Speed, 
            0.0f);


        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        if (Input.GetKey(KeyCode.Space))
        {
            OnJump();
          //  transform.position += new Vector3(0.0f, Speed, 0.0f);
          //  OnDive();
          //  transform.position -= new Vector3(0.0f, Speed, 0.0f);
        }
            

        if (Input.GetKey(KeyCode.Q))
            OnRoll();

        if (Health <= 0)
            OnDead();

        animator.SetFloat("Speed", Hor);
        transform.position += Movement;  // 원래 FixedUpdate에 써야 함
    }

    private void OnAttack()
    {
        if (onAttack)
            return;

        onAttack = true;
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        onAttack = false;
    }

    private void OnHit()
    {
        if (onHit)
            return;

        onHit = true;
        animator.SetTrigger("Hit");
        Health -= 5;
    }

    private void SetHit()
    {
        onHit = false;
    }

    private void OnJump()
    {
        if (onJump)
            return;

        onJump = true;
        animator.SetTrigger("Jump");
    }

    private void SetJump()
    {
        onJump = false;
    }

    private void OnDive()
    {
        if (onDive)
            return;

        onDive = true;
        animator.SetTrigger("Dive");
    }

    private void SetDive()
    {
        onDive = false;
    }

    private void OnRoll()
    {
        if (onRoll)
            return;

        onRoll = true;
        animator.SetTrigger("Roll");
    }

    private void SetRoll()
    {
        onRoll = false;
    }

    private void OnDead()
    {
        if (onDead)
        {
            Health = 10;
            return;
        }

        onDead = true;
        animator.SetTrigger("Dead");
        Health = 10;
    }

    private void SetDead()
    {
        onDead = false;
    }

    /*
    private void FixedUpdate()
    {
        transform.position += Movement;
    }
    */
}

