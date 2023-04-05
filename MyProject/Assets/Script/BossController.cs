using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossController : MonoBehaviour
{
    // BASE
    const int STATE_WALK = 1;
    const int STATE_ATTACK = 2;  // 기본 공격
    
    // PENGUIN
    const int STATE_SLIDE = 3;

    // RHINO
    const int STATE_ATTACKRUN1 = 2;
    const int STATE_ATTACKRUN2 = 3;


    // TREE
    const int STATE_BULLET = 3;


    private GameObject Target;
    
    private Animator Anim;

    // ** Boss의 SpriteRenderer 구성요소를 받아오기 위해
    private SpriteRenderer spriteRenderer;

    private Vector3 Movement;
    private Vector3 EndPoint;

    private float CoolDown;

    public int HP;
    public float Speed;

    private bool onStart;

    public bool SkillAttack;
    public bool Attack;
    private bool Walk;
    private bool Hit;
    private bool active;
    private bool cool;

    private int choice;

    private GameObject ApearUI;
    private Animator ApearAnim;

    private List<GameObject> BulletList = new List<GameObject>();
    private GameObject TreeBulletPrefab;
    private GameObject fxPrefab;


    private void Awake()
    {
        Target = GameObject.Find("Player");

        Anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        ApearUI = GameObject.Find("RoundInfoCanvas").transform.Find("BossInfo").gameObject;
        
        ApearAnim = ApearUI.GetComponent<Animator>();

        TreeBulletPrefab = Resources.Load("Prefabs/Boss/TreeBullet") as GameObject;

        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
    }

    void Start()
    {
        onStart = false;

        CoolDown = 1.5f;
 
        active = false;
        cool = true;

        SkillAttack = false;
        Attack = false;
        Walk = false;
        Hit = false;


        switch (ControllerManager.GetInstance().BossId)
        {
            case 1:
                ApearAnim.SetTrigger("PengMove");
                break;
            case 2:
                ApearAnim.SetTrigger("RinoMove");
                break;
            case 3:
                ApearAnim.SetTrigger("TreeMove");
                break;
        }

        StartCoroutine(BossStart());  // 화면에 보일 때까지 걸어온다.

        // StartCoroutine(OnCoolDown());  // 1. 코루틴 실행 후 코루틴 종료 상관없이 Start는 종료(코루틴 종료를 기다리지 않는다)
    }

    void Update()
    {
        if(onStart)
        {
            switch (ControllerManager.GetInstance().BossId)
            {
                case 1:
                    Penguin();
                    break;

                case 2:
                    Rhino();
                    break;

                case 3:
                    Tree();
                    break;
            }
        }
    }

    private IEnumerator BossStart()
    {
        float time = 5.0f;

        while (true)
        {
            if (time <= 0)
            {
                onStart = true;
                break;
            }

            Vector3 pos = new Vector3(14.0f, transform.position.y, 0.0f);

            Movement = new Vector3(-Speed, 0.0f, 0.0f);

            transform.position += Movement * Time.deltaTime;

            Anim.SetFloat("Speed", Mathf.Abs(Movement.x) + Mathf.Abs(Movement.y));

            time -= Time.deltaTime;

            yield return null;
        }
    }


    private void Penguin()
    {
        float result = Target.transform.position.x - transform.position.x;

        if (result < 0.0f)
            spriteRenderer.flipX = true;
        else if (result > 0.0f)
            spriteRenderer.flipX = false;

        if (ControllerManager.GetInstance().DirRight)
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
        else if (cool)
        {
            //active = true;
            cool = false;
            choice = OnController();
            StartCoroutine(OnCoolDown());
        }
    }

    private void Rhino()
    {
        float result = Target.transform.position.x - transform.position.x;

        if (result < 0.0f)
            spriteRenderer.flipX = false;
        else if (result > 0.0f)
            spriteRenderer.flipX = true;

        if (ControllerManager.GetInstance().DirRight)
            transform.position -= new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

        if (active)
        {
            switch (choice)
            {
                case STATE_WALK:
                    OnWalk();
                    break;

                case STATE_ATTACKRUN1:
                case STATE_ATTACKRUN2:  // 확률 올리려고
                    OnAttackRun();
                    break;
            }
        }
        else if (cool)
        {
            cool = false;
            choice = OnController();
            StartCoroutine(OnCoolDown());
        }
    }

    private void Tree()
    {
        float result = Target.transform.position.x - transform.position.x;

        if (result < 0.0f)
            spriteRenderer.flipX = false;
        else if (result > 0.0f)
            spriteRenderer.flipX = true;

        if (ControllerManager.GetInstance().DirRight)
            transform.position -= new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;
        
        if (active)
        {
            switch (choice)
            {
                case STATE_WALK:
                    OnWalk();
                    break;

                case STATE_ATTACK:
                    OnAttackBullet();
                    break;

                case STATE_BULLET:
                    OnBulletPattern();
                    break;
            }
        }
        else if (cool)
        {
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
                if(ControllerManager.GetInstance().BossId == 1)
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

        switch (ControllerManager.GetInstance().BossId)
        {
            case 1:
                return Random.Range(STATE_WALK, STATE_SLIDE + 1);

            case 2:
                return Random.Range(STATE_WALK, STATE_ATTACKRUN2 + 1);

            case 3:
                return Random.Range(STATE_WALK, STATE_BULLET + 1);

            default:
                return 1;
        }
        
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
        if (!Walk)
            StartCoroutine(TimeLimit(Random.Range(3.0f, 5.0f)));
        
        Walk = true;

        // ** Player 따라가기
        float Distance = Vector3.Distance(Target.transform.position, transform.position);

        if (Distance > 0.1f)
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
    }

    private IEnumerator TimeLimit(float _time)
    {
        float time = _time;

        while(true)
        {
            if (time <= 0)
            {
                active = false;
                break;
            }

            time -= Time.deltaTime;

            yield return null;
        }
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

    private void OnAttackRun()
    {
        Attack = true;

        float Distance = Vector3.Distance(EndPoint, transform.position);

        if (Distance > 0.1f)  // 0.05f
        {
            Vector3 Direction = (EndPoint - transform.position).normalized;

            Movement = new Vector3(
                Speed * 4 * 8.0f * Direction.x,     // Speed 4배 증가
                Speed * 4 * 8.0f * Direction.y,
                0.0f);

            transform.position += Movement * Time.deltaTime;

            // AttackRun 실행 중일 때 반복 실행하지 않기 위해
            if (!Anim.GetBool("AttackRun"))
            {
                Anim.SetBool("AttackRun", true);
            }
        }
        else
        {
            Anim.SetBool("AttackRun", false);
            Hit = false;
            active = false;
        }

        if (Hit)
        {
            Anim.SetBool("AttackRun", false);
            Hit = false;
            active = false;
        }
    }

    private void OnAttackBullet()
    {
        if (Attack)
            return;

        Attack = true;

        Movement = new Vector3(0.0f, 0.0f, 0.0f);
        Anim.SetFloat("Speed", Movement.x);
        Anim.SetTrigger("Attack");
    }

    private IEnumerator ShootBullet()
    {
        for(int i = 0; i < 3; ++i)
        {

            GameObject Obj = Instantiate(TreeBulletPrefab);

            // ** 총알의 EnemyBulletController 스크립트를 받아온다.
            EnemyBulletController Controller = Obj.AddComponent<EnemyBulletController>();

            float Direction;
            if (spriteRenderer.flipX)
            {
                Obj.transform.position = new Vector3(
                transform.position.x + 1.45f,
                transform.position.y - 0.3f,
                transform.position.z);

                Direction = 1.0f;
            }
            else
            {
                Obj.transform.position = new Vector3(
                transform.position.x - 1.45f,
                transform.position.y - 0.3f,
                transform.position.z);

                Direction = -1.0f;
            }

            // ** 총알 스크립트 내부의 방향 변수를 현재 플레이어의 방향 변수로 설정한다.
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            // ** 총알의 SpriteRenderer를 받아온다.
            SpriteRenderer bulletRenderer = Obj.GetComponent<SpriteRenderer>();

            // ** 총알의 이미지 반전 상태를 플레이어의 이미지 반전 상태로 설정한다.
            bulletRenderer.flipX = spriteRenderer.flipX;

            // ** 모든 설정이 종료되었다면 저장소에 보관한다.
            BulletList.Add(Obj);

            yield return new WaitForSeconds(0.2f);
        }

        active = false;
    }

    private void OnBulletPattern()
    {
        if (SkillAttack)
            return;

        SkillAttack = true;
        
        float choice = Random.Range(1, 5);

        switch(choice)  // Bullet Pattern
        {
            case 1:
                GetScrewPattern(5.0f, (int)(360 / 5.0f));
                break;

            case 2:
                StartCoroutine(GetDelayScrewPattern());
                break;

            case 3:
                StartCoroutine(ExplosionPattern(10.0f, (int)(360 / 10.0f)));
                break;

            case 4:
                GuideBulletPattern();
                break;
        }
    }

    private void GetScrewPattern(float _angle, int _count, bool _option = false)
    {
        for (int i = 0; i < _count; ++i)
        {
            GameObject Obj = Instantiate(TreeBulletPrefab);
            BulletControl controller = Obj.AddComponent<BulletControl>();

            controller.Option = _option;

            _angle += 5.0f;

            controller.Direction = new Vector3(
                Mathf.Cos(_angle * 3.141592f / 180),
                Mathf.Sin(_angle * 3.141592f / 180),
                0.0f) * 5;

            Obj.transform.position = transform.position;

            BulletList.Add(Obj);
        }

        active = false;
    }

    private IEnumerator GetDelayScrewPattern()
    {
        float fAngle = 30.0f;

        int iCount = (int)(360 / fAngle);

        int i = 0;

        float value = (i + 12) / iCount;

        while (i < (iCount) * 7)
        {
            GameObject Obj = Instantiate(TreeBulletPrefab);
            BulletControl controller = Obj.AddComponent<BulletControl>();

            controller.Option = false;

            if(value % 2 == 0)
                fAngle += 30.0f;
            else
                fAngle += 25.0f;

            controller.Direction = new Vector3(
                Mathf.Cos(fAngle * Mathf.Deg2Rad),
                Mathf.Sin(fAngle * Mathf.Deg2Rad),
                0.0f) * 5;

            Obj.transform.position = transform.position;

            BulletList.Add(Obj);

            ++i;

            yield return new WaitForSeconds(0.025f);
        }

        active = false;
    }

    public IEnumerator ExplosionPattern(float _angle, int _count, bool _option = false)
    {
        GameObject ParentObj = Instantiate(TreeBulletPrefab);

         BulletControl control = ParentObj.AddComponent<BulletControl>();

        control.Option = _option;

        control.Direction = GameObject.Find("Player").transform.position - transform.position;

        ParentObj.transform.position = transform.position;

        yield return new WaitForSeconds(1.0f);

        GameObject fxObj = Instantiate(fxPrefab);
        fxObj.transform.position = transform.position;

        if (!ParentObj)
            yield break;
        
        Vector3 pos = ParentObj.transform.position;
        Destroy(ParentObj);

        for (int i = 0; i < _count; ++i)
        {
            GameObject Obj = Instantiate(TreeBulletPrefab);

            BulletControl controller = Obj.AddComponent<BulletControl>();

            controller.Option = _option;

            _angle += 10.0f;

            controller.Direction = new Vector3(
                Mathf.Cos(_angle * 3.141592f / 180),
                Mathf.Sin(_angle * 3.141592f / 180),
                0.0f) * 5;

            Obj.transform.position = pos;

            BulletList.Add(Obj);
        }

        active = false;
    }

    public void GuideBulletPattern()
    {
        GameObject Obj = Instantiate(TreeBulletPrefab);
        BulletControl controller = Obj.AddComponent<BulletControl>();

        controller.Target = GameObject.Find("Player");
        controller.Option = true;

        Obj.transform.position = transform.position;

        active = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(onStart)
        {
            if (collision.tag == "Player")
            {
                // Rhino 공격으로 플레이어와 충돌
                if (ControllerManager.GetInstance().BossId == 2 && Attack)
                {
                    // 플레이어와 충돌하면 AttackHit 애니메이션 실행
                    Hit = true;
                    Anim.SetTrigger("AttackHit");
                }
            }
            if (collision.tag == "Bullet")
            {
                if(ControllerManager.GetInstance().BossId == 2 && Attack)  // Rino 공격 중 무적상태
                {
                    if(Anim.GetBool("AttackRun"))
                        return;
                }

                if (collision.transform.name == "BigBullet")
                    HP -= 10;
                else
                {
                    --HP;
                    //Anim.SetTrigger("Hit");
                }

                if (HP <= 0)
                {
                    Anim.SetTrigger("Die");
                    // 죽고 있는 Enemy의 Collider 끄기
                    GetComponent<CapsuleCollider2D>().enabled = false;

                    if (ControllerManager.GetInstance().BossId == 3)
                        ControllerManager.GetInstance().GameClear = true;
                    else
                        // 다음 라운드로 넘어가기
                        RoundManager.GetInstance.BossClear = true;
                }
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject, 0.016f);
    }
}
