using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{ 
    // public은 유니티 Inspector 창에 노출이 된다.

    // ** 움직이는 속도
    private float Speed;

    // ** 움직임을 저장하는 벡터
    private Vector3 Movement;

    // 플레이어 체력
    //private int HP;

    // ** 플레이어의 Animator 구성요소를 받아오기 위해
    private Animator animator;

    // ** 플레이어의 SpriteRenderer 구성요소를 받아오기 위해
    private SpriteRenderer spriteRenderer;

    // ** [상태 체크]
    private bool onAttack;  // 공격상태
    private bool onHit;     // 피격상태
    private bool onJump;    // 점프
    private bool onRoll;    // 구르기
    private bool onDead;    // 사망
    private bool onDive;    // 착지

    // ** 복제할 총알 원본
    private GameObject BulletPrefab;

    // ** 복제할 FX 원본
    private GameObject fxPrefab;

    // ** 추후에 list로 변경해야 함
    // public GameObject[] stageBack = new GameObject[7];
    public List<GameObject> stageBack;

    /*
     Dictionary<string, Object> dic1;
     Dictionary<string, GameObject> dic2;
    */

    // ** 복제된 총알의 저장공간
    private List<GameObject> Bullets = new List<GameObject>();

    // ** 플레이어가 마지막으로 바라본 방향
    private float Direction;

    [Header("방향")]
    // ** 플레이어가 바라보는 방향
    [Tooltip("왼쪽")]
    public bool DirLeft;
    [Tooltip("오른쪽")]
    public bool DirRight;


    private void Awake()  // Start보다 먼저 실행되고 덜 실행됨
    {
        // ** Player의 Animator를 받아온다.
        animator = this.GetComponent<Animator>();  // 'this.' 생략 가능

        // ** Player의 SpriteRenderer를 받아온다.
        spriteRenderer = this.GetComponent<SpriteRenderer>();

// 테스트할 때는 이렇게! 에디터 모드에서만 뜨기 때문이다.
//#if UNITY_EDITOR
//        print("test");
//#else
//    print("???");
//#endif

        // ** [Resources] 폴더에서, 사용할 리소스를 들고온다.
        BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        //fxPrefab = Resources.Load("Prefabs/FX/Smoke") as GameObject;
        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;

        stageBack = new List<GameObject>(Resources.LoadAll<GameObject>("Backgrounds"));
    }

        // ** 유니티 기본 제공 함수
        // ** 초기값을 설정할 때 사용
        void Start()
    {
        // ** 속도를 초기화
        Speed = 5.0f;

        // 플레이어 체력을 초기화
        //HP = ControllerManager.GetInstance().PlayerHP;

        // ** 초기값 세팅
        onAttack = false;
        onHit = false;
        onJump = false;
        onRoll = false;
        onDead = false;
        onDive = false;
        Direction = 1.0f;

        DirLeft = false;
        DirRight = false;

         // 이게 효율적이지만 Resource를 Load하는 방법(82줄)을 권장한다.
         //for (int i = 0; i < 7; ++i)
         //    stageBack[i] = GameObject.Find(i.ToString());
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

        // ** 입력받은 값으로 플레이어를 움직인다.
        // Time.deltaTime: 프레임과 프레임 사이의 간격을 이용해서 컴퓨터 성능과 상관없이 모든 컴퓨터에서 동일하게 작동되도록 하기 위한 것
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed,
            0.0f,
            0.0f);

        transform.position += new Vector3(0.0f, Ver * Time.deltaTime * Speed, 0.0f);

        // ** Hor이 0이라면 멈춰 있는 상태이므로 예외처리를 해준다.
        if (Hor != 0)
            Direction = Hor;

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // ** 플레이어의 좌표가 0.0 보다 작을 때 플레이어만 움직인다.
            if (transform.position.x < 0)
                // ** 실제 플레이어를 움직인다.
                transform.position += Movement;
            else
            {
                ControllerManager.GetInstance().DirRight = true;
                ControllerManager.GetInstance().DirLeft = false;
                // DirRight, DirLeft: 직관적인 표현을 위해 사용 / Hor 변수 하나만으로도 작성 가능
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = true;
            
            // ** 플레이어의 좌표가 -15.0 보다 클 때 플레이어만 움직인다.
            if(transform.position.x > -15.0f)
                // ** 실제 플레이어를 움직인다.
                transform.position += Movement;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || 
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = false;
        }


        // ** 플레이어가 바라보고 있는 방향에 따라 이미지 반전(플립) 설정
        if (Direction < 0)
        {
            spriteRenderer.flipX = DirLeft = true;
        }
            
        else if (Direction > 0)
        {
            spriteRenderer.flipX = false;
            DirRight = true;
        }

        // ** 좌측 컨트롤키를 입력한다면
        if (Input.GetKey(KeyCode.LeftControl))
            // ** 공격
            OnAttack();

        // ** 좌측 쉬프트키를 입력한다면
        if (Input.GetKey(KeyCode.LeftShift))
           // ** 피격
           OnHit();
        
        //if (Input.GetButtonDown("Jump"))      
        //    OnJump();

        //if (transform.position.y > 0)
        //    OnDive();

        if (Input.GetKey(KeyCode.Q))
            OnRoll();

        if (ControllerManager.GetInstance().PlayerHP <= 0)
            OnDead();

        // ** 스페이스바를 입력한다면
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ** 총알 원본을 복제한다.
            GameObject Obj = Instantiate(BulletPrefab);
            // Obj.transform.name = "";

            //** 복제된 총알의 위치를 현재 플레이어의 위치로 초기화한다.
            Obj.transform.position = transform.position;  // 플레이어 position 위치에 놓음

            // ** 총알의 BulletController 스크립트를 받아온다.
            BulletController Controller = Obj.AddComponent<BulletController>();

            // ** 총알 스크립트 내부의 방향 변수를 현재 플레이어의 방향 변수로 설정한다.
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            // ** 총알 스크립트 내부의 FX Prefab을 설정한다.
            Controller.fxPrefab = fxPrefab;

            // ** 총알의 SpriteRenderer를 받아온다.
            SpriteRenderer bulletRenderer = Obj.GetComponent<SpriteRenderer>();

            // ** 총알의 이미지 반전 상태를 플레이어의 이미지 반전 상태로 설정한다.
            bulletRenderer.flipY = spriteRenderer.flipX;

            // ** 모든 설정이 종료되었다면 저장소에 보관한다.
            Bullets.Add(Obj);
        }

        // ** 플레이어의 움직임에 따라 이동 모션을 실행한다.
        animator.SetFloat("Speed", Hor);

        // ** offset box
        // transform.position += Movement;  // 원래 FixedUpdate에 써야 함
    }


    private void OnAttack()
    {
        // ** 이미 공격 모션이 진행 중이라면
        if (onAttack)
            // ** 함수를 종료시킨다.
            return;

        // ** 함수가 종료되지 않았다면
        // ** 공격 상태를 활성화하고
        onAttack = true;

        // ** 공격 모션을 실행시킨다.
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        // ** 함수가 실행되면 공격 모션이 비활성화된다.
        // ** 함수는 애니메이션 클립의 이벤트 프레임으로 삽입된다.
        onAttack = false;
    }

    private void OnHit()
    {
        // ** 이미 피격 모션이 진행 중이라면
        if (onHit)
            // ** 함수를 종료시킨다.
            return;

        // ** 함수가 종료되지 않았다면
        // ** 피격 상태를 활성화하고
        onHit = true;

        // ** 피격 모션을 실행시킨다.
        animator.SetTrigger("Hit");

        ControllerManager.GetInstance().PlayerHP -= 20;
    }

    private void SetHit()
    {
        // ** 함수가 실행되면 피격 모션이 비활성화된다.
        // ** 함수는 애니메이션 클립의 이벤트 프레임으로 삽입된다.
        onHit = false;
    }

    private void OnJump()
    {
        if (onJump)
            return;

        onJump = true;
        animator.SetTrigger("Jump");
        while(transform.position.y < 2)
        transform.position += new Vector3(0.0f, 0.3f, 0.0f);
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
        transform.position -= new Vector3(0.0f, 0.3f, 0.0f);
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
            ControllerManager.GetInstance().PlayerHP = 100;
            return;
        }

        onDead = true;
        animator.SetTrigger("Dead");
        ControllerManager.GetInstance().PlayerHP = 100;
    }

    private void SetDead()
    {
        onDead = false;
    }

    private void ThrowBullet()
    {
        float Hor = Input.GetAxisRaw("Horizontal");

        GameObject Obj = Instantiate(BulletPrefab);
        // Obj.transform.name = "";
        Obj.transform.position = transform.position;  // 플레이어 position 위치에 놓음
        BulletController Controller = Obj.AddComponent<BulletController>();
        SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

        // ** 총알 스크립트 내부의 FX Prefab을 설정한다.
        Controller.fxPrefab = fxPrefab;

        renderer.flipY = spriteRenderer.flipX;

        if (Hor == 0)
        {
            if (spriteRenderer.flipX)
                Controller.Direction = new Vector3(-1.0f, 0.0f, 0.0f);
            else
                Controller.Direction = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else
            Controller.Direction = new Vector3(Hor, 0.0f, 0.0f);

        Bullets.Add(Obj);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            print("collision");
    }
}

