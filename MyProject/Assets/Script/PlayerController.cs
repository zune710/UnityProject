using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public�� ����Ƽ Inspector â�� ������ �ȴ�.

    // ** �����̴� �ӵ�
    private float Speed;

    // ** �������� �����ϴ� ����
    private Vector3 Movement;

    // �÷��̾� ü��
    private int Health;

    // ** �÷��̾��� Animator ������Ҹ� �޾ƿ��� ����
    private Animator animator;

    // ** �÷��̾��� SpriteRenderer ������Ҹ� �޾ƿ��� ����
    private SpriteRenderer spriteRenderer;

    // ** [���� üũ]
    private bool onAttack;  // ���ݻ���
    private bool onHit;     // �ǰݻ���
    private bool onJump;    // ����
    private bool onRoll;    // ������
    private bool onDead;    // ���
    private bool onDive;    // ����

    // ** ������ �Ѿ� ����
    private GameObject BulletPrefab;

    // ** ������ FX ����
    private GameObject fxPrefab;

    public GameObject[] stageBack = new GameObject[7];

    // ** ������ �Ѿ��� �������
    private List<GameObject> Bullets = new List<GameObject>();

    // ** �÷��̾ ���������� �ٶ� ����
    private float Direction;

    // ** �÷��̾ �ٶ󺸴� ����
    public bool DirLeft;
    public bool DirRight;


    private void Awake()  // Start���� ���� ����ǰ� �� �����
    {
        // ** Player�� Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();  // 'this.' ���� ����

        // ** Player�� SpriteRenderer�� �޾ƿ´�.
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        // ** [Resources] �������� ����� ���ҽ��� ���´�.
        BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        fxPrefab = Resources.Load("Prefabs/FX/Smoke") as GameObject;
    }


    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �ʱⰪ�� ������ �� ���
    void Start()
    {
        // ** �ӵ��� �ʱ�ȭ
        Speed = 5.0f;

        // �÷��̾� ü���� �ʱ�ȭ
        Health = 10;

        // ** �ʱⰪ ����
        onAttack = false;
        onHit = false;
        onJump = false;
        onRoll = false;
        onDead = false;
        onDive = false;
        Direction = 1.0f;

        DirLeft = false;
        DirRight = false;

        for (int i = 0; i < 7; ++i)
            stageBack[i] = GameObject.Find(i.ToString());
    }


    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �����Ӹ��� �ݺ������� ����Ǵ� �Լ�
    // �ʴ� 60��(����) ~ ��õ�� ������Ʈ
    void Update()
    {
        // ** [�Ǽ� ���� IEEE754]

        // ** Input.GetAxis�� -1�� 1 ������ ���� �Ǽ��� ��ȯ(�Ҽ��� ����) / PC, �ܼ� / ������ ������ ���� �� ��� / �Ǽ� ���� - ���ϡ�
        // ** Input.GetAxisRaw�� -1, 0, 1 �� �� �ϳ��� ��ȯ / �����, 2D / ���� ���� �� �ַ� ���(����ȭ)
        float Hor = Input.GetAxisRaw("Horizontal");
        // float Ver = Input.GetAxisRaw("Vertical");

        // ** �Է¹��� ������ �÷��̾ �����δ�.
        // Time.deltaTime: �����Ӱ� ������ ������ ������ �̿��ؼ� ��ǻ�� ���ɰ� ������� ��� ��ǻ�Ϳ��� �����ϰ� �۵��ǵ��� �ϱ� ���� ��
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed,
            0.0f,
            0.0f);

        // ** Hor�� 0�̶�� ���� �ִ� �����̹Ƿ� ����ó���� ���ش�.
        if (Hor != 0)
            Direction = Hor;

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // ** �÷��̾��� ��ǥ�� 0 ���� ���� �� �÷��̾ �����δ�.
            if (transform.position.x < 0)
                // ** ���� �÷��̾ �����δ�.
                transform.position += Movement;
            else
            {
                ControllerManager.GetInstance().DirRight = true;
                ControllerManager.GetInstance().DirLeft = false;
                // DirRight, DirLeft: �������� ǥ���� ���� ��� / Hor ���� �ϳ������ε� �ۼ� ����
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = true;
            
            // ** �÷��̾��� ��ǥ�� -15.0 ���� Ŭ �� �÷��̾ �����δ�.
            if(transform.position.x > -15.0f)
                // ** ���� �÷��̾ �����δ�.
                transform.position += Movement;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = false;
        }


        // ** �÷��̾ �ٶ󺸰� �ִ� ���⿡ ���� �̹��� ����(�ø�) ����
        if (Direction < 0)
        {
            spriteRenderer.flipX = DirLeft = true;
        }
            
        else if (Direction > 0)
        {
            spriteRenderer.flipX = false;
            DirRight = true;
        }

        // ** ���� ��Ʈ��Ű�� �Է��Ѵٸ�
        if (Input.GetKey(KeyCode.LeftControl))
            // ** ����
            OnAttack();

        // ** ���� ����ƮŰ�� �Է��Ѵٸ�
        if (Input.GetKey(KeyCode.LeftShift))
           // ** �ǰ�
           OnHit();
        
        //if (Input.GetButtonDown("Jump"))      
        //    OnJump();

        //if (transform.position.y > 0)
        //    OnDive();

        if (Input.GetKey(KeyCode.Q))
            OnRoll();

        if (Health <= 0)
            OnDead();

        // ** �����̽��ٸ� �Է��Ѵٸ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ** �Ѿ� ������ �����Ѵ�.
            GameObject Obj = Instantiate(BulletPrefab);
            // Obj.transform.name = "";

            //** ������ �Ѿ��� ��ġ�� ���� �÷��̾��� ��ġ�� �ʱ�ȭ�Ѵ�.
            Obj.transform.position = transform.position;  // �÷��̾� position ��ġ�� ����

            // ** �Ѿ��� BulletController ��ũ��Ʈ�� �޾ƿ´�.
            BulletController Controller = Obj.AddComponent<BulletController>();

            // ** �Ѿ� ��ũ��Ʈ ������ ���� ������ ���� �÷��̾��� ���� ������ �����Ѵ�.
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            // ** �Ѿ� ��ũ��Ʈ ������ FX Prefab�� �����Ѵ�.
            Controller.fxPrefab = fxPrefab;

            // ** �Ѿ��� SpriteRenderer�� �޾ƿ´�.
            SpriteRenderer bulletRenderer = Obj.GetComponent<SpriteRenderer>();

            // ** �Ѿ��� �̹��� ���� ���¸� �÷��̾��� �̹��� ���� ���·� �����Ѵ�.
            bulletRenderer.flipY = spriteRenderer.flipX;

            // ** ��� ������ ����Ǿ��ٸ� ����ҿ� �����Ѵ�.
            Bullets.Add(Obj);
        }

        // ** �÷��̾��� �����ӿ� ���� �̵� ����� �����Ѵ�.
        animator.SetFloat("Speed", Hor);

        // ** offset box
        // transform.position += Movement;  // ���� FixedUpdate�� ��� ��
    }


    private void OnAttack()
    {
        // ** �̹� ���� ����� ���� ���̶��
        if (onAttack)
            // ** �Լ��� �����Ų��.
            return;

        // ** �Լ��� ������� �ʾҴٸ�
        // ** ���� ���¸� Ȱ��ȭ�ϰ�
        onAttack = true;

        // ** ���� ����� �����Ų��.
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        // ** �Լ��� ����Ǹ� ���� ����� ��Ȱ��ȭ�ȴ�.
        // ** �Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Եȴ�.
        onAttack = false;
    }

    private void OnHit()
    {
        // ** �̹� �ǰ� ����� ���� ���̶��
        if (onHit)
            // ** �Լ��� �����Ų��.
            return;

        // ** �Լ��� ������� �ʾҴٸ�
        // ** �ǰ� ���¸� Ȱ��ȭ�ϰ�
        onHit = true;

        // ** �ǰ� ����� �����Ų��.
        animator.SetTrigger("Hit");

        Health -= 5;
    }

    private void SetHit()
    {
        // ** �Լ��� ����Ǹ� �ǰ� ����� ��Ȱ��ȭ�ȴ�.
        // ** �Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Եȴ�.
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

    private void ThrowBullet()
    {
        float Hor = Input.GetAxisRaw("Horizontal");

        GameObject Obj = Instantiate(BulletPrefab);
        // Obj.transform.name = "";
        Obj.transform.position = transform.position;  // �÷��̾� position ��ġ�� ����
        BulletController Controller = Obj.AddComponent<BulletController>();
        SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

        // ** �Ѿ� ��ũ��Ʈ ������ FX Prefab�� �����Ѵ�.
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
}

