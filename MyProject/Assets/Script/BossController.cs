using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    const int STATE_WALK = 1;
    const int STATE_ATTACK = 2;
    const int STATE_SLIDE = 3;

    private GameObject Target;
    
    private Animator Anim;

    // ** Boss의 SpriteRenderer 구성요소를 받아오기 위해
    private SpriteRenderer spriteRenderer;

    private Vector3 Movement;
    private Vector3 EndPoint;

    private float CoolDown;
    private float Speed;
    private int HP;

    private bool SkillAttack;
    private bool Attack;
    private bool Walk;
    private bool active;
    private bool cool;

    private int choice;


    private void Awake()
    {
        Target = GameObject.Find("Player");

        Anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        CoolDown = 1.5f;
        Speed = 0.5f;
        HP = 30000;

        active = false;
        cool = true;

        SkillAttack = false;
        Attack = false;
        Walk = false;

        // StartCoroutine(OnCoolDown());  // 1. 코루틴 실행 후 코루틴 종료 상관없이 Start는 종료(코루틴 종료를 기다리지 않는다)
    }

    void Update()
    {
        float result = Target.transform.position.x - transform.position.x;

        if (result < 0.0f)
            spriteRenderer.flipX = true;
        else if (result > 0.0f)
            spriteRenderer.flipX = false;
        
        if(ControllerManager.GetInstance().DirRight)
            transform.position -= new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

        if (active)
        {
            switch (choice)
            {
                case STATE_WALK:
                    OnWalk();
                    break;

                case STATE_ATTACK:
                    OnAttack();
                    break;

                case STATE_SLIDE:
                    OnSlide();
                    break;
            }
        }
        else if(cool)
        {
            //active = true;
            cool = false;
            choice = OnController();
            StartCoroutine(OnCoolDown());
        }
    }

    private int OnController()
    {
        // ** 행동 패턴에 대한 내용을 추가합니다.

        {
            // ** 초기화
            if(Walk)
            {
                Movement = new Vector3(0.0f, 0.0f, 0.0f);
                Anim.SetFloat("Speed", Movement.x);
                Walk = false;
            }

            if (Attack)
                Attack = false;

            if (SkillAttack)
            {
                Anim.SetBool("Slide", false);
                SkillAttack = false;
            }
        }
        // ** 로직


        // ** 어디로 움직일지 정하는 시점에 플레이어의 위치를 도착지점으로 세팅
        EndPoint = Target.transform.position;

        // * [return]
        // * 1: 이동      STATE_WALK
        // * 2: 공격      STATE_ATTACK
        // * 3: 슬라이딩  STATE_SLIDE


        return Random.Range(STATE_WALK, STATE_SLIDE + 1);
    }

    private IEnumerator OnCoolDown()  // 2. 코루틴 실행
    {
        float fTime = CoolDown;

        while(fTime > 0.0f)
        {
            fTime -= Time.deltaTime;
            yield return null;
        }

        active = true;  // 88줄
        cool = true; 
    }

    private void OnWalk()
    {
        Walk = true;

        // ** Player 따라가기
        float Distance = Vector3.Distance(Target.transform.position, transform.position);

        if (Distance > 3.0f)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;

            Movement = new Vector3(
                Speed * Direction.x, 
                Speed * Direction.y, 
                0.0f);

            transform.position += Movement * Time.deltaTime;

            // 위, 아래로 갈 때도 Walk 애니메이션 나오도록 Movement.y 추가
            Anim.SetFloat("Speed", Mathf.Abs(Movement.x) + Mathf.Abs(Movement.y));
        }
        else
            active = false;
    }

    private void OnAttack()
    {
        if(Attack)
        {
            active = false;
            return;
        }

        float Distance = Vector3.Distance(Target.transform.position, transform.position);

        if (Distance <= 3.0f)
        {
            Attack = true;

            Movement = new Vector3(0.0f, 0.0f, 0.0f);
            Anim.SetFloat("Speed", Movement.x);

            Anim.SetTrigger("Attack");
        }
        
        active = false;

        // StartCoroutine(OnCoolDown());
        // 3. 다른 코루틴 실행(이전에 실행한 코루틴과 일시적으로 잠깐 동시에 존재)
        //    이전 코루틴은 OnAttack 함수 빠져나가고 switch문의 break 실행되면서 종료됨
        // active 사용한 지금 스크립트는 중복(동시 존재) X
    }

    private void OnSlide()
    {
        SkillAttack = true;

        // ** 목적지에 도착할 때까지
        float Distance = Vector3.Distance(EndPoint, transform.position);

        if (Distance > 0.3f)
        {
            Vector3 Direction = (EndPoint - transform.position).normalized;

            Movement = new Vector3(
                Speed * 8.0f * Direction.x,
                Speed * 8.0f * Direction.y,
                0.0f);

            transform.position += Movement * Time.deltaTime;

            // Preslide 실행 중일 때 반복 실행하지 않기 위해
            if(!Anim.GetBool("Slide"))
            {
                Anim.SetBool("Slide", true);
                Anim.SetTrigger("Preslide");
            }
        }

        else
            active = false;
    }


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            --HP;
            //Anim.SetTrigger("Hit");

            if (HP <= 0)
            {
                //Anim.SetTrigger("Die");
                GetComponent<CapsuleCollider2D>().enabled = false;  // 죽고 있는 Enemy의 Collider 끄기
            }
        }
    }
}
