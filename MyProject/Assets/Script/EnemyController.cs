using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed;
    public int HP;
    private Animator Anim;
    private Vector3 Movement;

    public GameObject Player;
    private GameObject EnemyBullet;
    private GameObject Obj;
    private bool onAttack;
    private bool onMagic;
    private float CoolTime;
    private float MagicCool;

    private void Awake()
    {
        Anim = GetComponent<Animator>();

        Player = GameObject.Find("Player");

        EnemyBullet = Resources.Load("Prefabs/Enemy/EnemyBullet") as GameObject;
    }

    void Start()
    {
        Speed = 0.2f;
        Movement = new Vector3(1.0f, 0.0f, 0.0f);
        HP = 3;

        onAttack = false;
        onMagic = false;
        CoolTime = 10.0f;
        MagicCool = CoolTime;
    }

    void Update()
    {
        float distance = Vector3.Distance(
                Player.transform.position,
                transform.position);

        if (MagicCool < CoolTime)
            MagicCool += Time.deltaTime;

        if (distance < 2.0f)
        {
            Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 0.0f);
            
            transform.position -= Movement * Time.deltaTime;

            OnAttack();
        }
        else if (distance < 5.0f && MagicCool >= CoolTime)
        {
            Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 0.0f);

            transform.position -= Movement * Time.deltaTime;

            OnMagic();
        }
        else
        {
            Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(Speed + 1.0f, 0.0f, 0.0f) : new Vector3(Speed, 0.0f, 0.0f); // 1.0f: Background 2 속도

            transform.position -= Movement * Time.deltaTime;
            Anim.SetFloat("Speed", Movement.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            --HP;

            if (HP <= 0)
            {
                Anim.SetTrigger("Die");
                GetComponent<CapsuleCollider2D>().enabled = false;  // 죽고 있는 Enemy의 Collider 끄기
            }
        }
    }

    private void OnAttack()
    {
        if (onAttack || onMagic)
            return;

        onAttack = true;

        Anim.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        onAttack = false;
    }

    private void OnMagic()
    {
        if (onMagic || onAttack)
            return;

        onMagic = true;
        MagicCool = 0.0f;

        Anim.SetTrigger("Magic");

        CreateEnemyBullet();
    }

    private void SetMagic()
    {
        onMagic = false;
    }

    private void CreateEnemyBullet()
    {
        Obj = Instantiate(EnemyBullet);
        Obj.transform.position = new Vector3(
            Player.transform.position.x,
            Player.transform.position.y + 0.5f,
            0.0f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject, 0.016f);
    }
}
