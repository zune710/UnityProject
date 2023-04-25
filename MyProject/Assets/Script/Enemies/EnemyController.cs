using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    private Animator Anim;
    private Vector3 Movement;

    public GameObject Player;
    PlayerController playerController;

    private GameObject EnemyBullet;
    private GameObject fxPrefab;
    private GameObject CoinPrefab;

    // ** 복제된 총알의 저장공간
    private List<GameObject> Bullets = new List<GameObject>();

    private bool onAttack;

    private string EnemyType;

    // Inspector
    public bool hasBullet;
    public int HP;
    public float Speed;
    public float AttackRange;

    private float playerSpeedOffset;

    private IObjectPool<EnemyController> EnemyPool;

    private GameObject CoinParent;


    private void Awake()
    {
        Anim = GetComponent<Animator>();

        Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();

        EnemyType = EnemyManager.GetInstance.enemyType.ToString();

        if (hasBullet)
            EnemyBullet = Resources.Load("Prefabs/Enemies/" + EnemyType + "Bullet") as GameObject;

        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
        CoinPrefab = Resources.Load("Prefabs/Coin") as GameObject;

        CoinParent = GameObject.Find("CoinList") ? GameObject.Find("CoinList").gameObject : new GameObject("CoinList");
    }

    void Start()
    {
        Movement = new Vector3(1.0f, 0.0f, 0.0f);

        onAttack = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(
                    Player.transform.position,
                    transform.position);

        playerSpeedOffset = playerController.Speed * 0.2f;  // 1(속도 증가 스킬 사용하면 2)


        if (distance < AttackRange)
        {
            Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(1.0f * playerSpeedOffset, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 0.0f); // 1.0f: Background 2 속도

            transform.position -= Movement * Time.deltaTime;

            OnAttack();
        }
        else
        {
            Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(Speed + 1.0f * playerSpeedOffset, 0.0f, 0.0f) : new Vector3(Speed, 0.0f, 0.0f); // 1.0f: Background 2 속도

            transform.position -= Movement * Time.deltaTime;
            Anim.SetFloat("Speed", Movement.x);
        }

        if (ControllerManager.GetInstance().onBoss)
        {
            HP = 0;

            Anim.SetTrigger("Die");
            GetComponent<CapsuleCollider2D>().enabled = false;
        }

        if (transform.position.x < -20.0f)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (collision.transform.name == "BigBullet")
                HP = 0;
            else
            {
                --HP;
                Anim.SetTrigger("Hit");
            }

            if (HP <= 0)
            {
                Anim.SetTrigger("Die");
                GetComponent<CapsuleCollider2D>().enabled = false;  // 죽고 있는 Enemy의 Collider 끄기

                ++ControllerManager.GetInstance().EnemyCount;

                if (ControllerManager.GetInstance().EnemyCount == ControllerManager.GetInstance().Goal)
                    ControllerManager.GetInstance().GoalClear = true;

                if (Random.value <= 0.3f)
                    CreateCoin();
            }
        }
    }


    private void OnAttack()
    {
        // 공통
        if (onAttack)
            return;

        onAttack = true;

        Anim.SetTrigger("Attack");

        // 개별 - Attack Animation에 event로 함수 호출
    }

    private void SetAttack()
    {
        onAttack = false;
    }

    private IEnumerator CreatePlantBullet()  // Plant Attack Anim Event
    {
        GameObject Obj = Instantiate(EnemyBullet);

        Obj.transform.position = new Vector3(
                    transform.position.x - 0.5f,
                    transform.position.y + 0.05f,
                    transform.position.z);

        // ** 총알의 BulletController 스크립트를 받아온다.
        EnemyBulletController Controller = Obj.GetComponent<EnemyBulletController>();

        // ** 총알 스크립트 내부의 방향 변수를 설정한다. (왼쪽으로만 발사)
        Controller.Direction = new Vector3(-1.0f, 0.0f, 0.0f);

        Controller.BasicAttack = true;

        // ** 총알 스크립트 내부의 FX Prefab을 설정한다.
        Controller.fxPrefab = fxPrefab;

        // ** 모든 설정이 종료되었다면 저장소에 보관한다.
        Bullets.Add(Obj);

        yield return new WaitForSeconds(3.0f);

        SetAttack();
    }

    private void RockSplit()
    {
        GetComponent<AudioSource>().Play();

        GameObject RockPrefab;
        List<GameObject> SplitRock = new List<GameObject>();
        Vector3 pos;

        GameObject Parent = GameObject.Find("EnemyList");

        if (transform.name == "Rock1")
        {
            RockPrefab = Resources.Load("Prefabs/Enemies/Rock2") as GameObject;

            SplitRock.Add(Instantiate(RockPrefab));
            SplitRock.Add(Instantiate(RockPrefab));

            int i = 0;

            while (i < 2)
            {
                pos = new Vector3(
                    Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f),
                    Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f),
                    0.0f);

                if (pos.y > -3.1f || pos.y < -5.9f)
                    continue;

                // ** 클론의 위치 초기화
                SplitRock[i].transform.position = pos;

                // ** 클론의 이름 초기화
                SplitRock[i].transform.name = "Rock2";

                // ** 클론의 계층구조 설정
                SplitRock[i].transform.SetParent(Parent.transform);

                ++i;
            }
        }

        if (transform.name == "Rock2")
        {
            RockPrefab = Resources.Load("Prefabs/Enemies/Rock3") as GameObject;

            SplitRock.Add(Instantiate(RockPrefab));
            SplitRock.Add(Instantiate(RockPrefab));
            SplitRock.Add(Instantiate(RockPrefab));

            int i = 0;

            while (i < 3)
            {
                pos = new Vector3(
                    Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f),
                    Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f),
                    0.0f);

                if (pos.y > -3.1f || pos.y < -5.9f)
                    continue;

                // ** 클론의 위치 초기화
                SplitRock[i].transform.position = pos;

                // ** 클론의 이름 초기화
                SplitRock[i].transform.name = "Rock3";

                // ** 클론의 계층구조 설정
                SplitRock[i].transform.SetParent(Parent.transform);

                ++i;
            }
        }
    }

    private void CreateCoin()
    {
        GameObject coin = Instantiate(CoinPrefab, transform.position, Quaternion.identity);

        coin.transform.SetParent(CoinParent.transform);
    }

    public void SetPool(IObjectPool<EnemyController> pool)
    {
        EnemyPool = pool;
    }

    private void ReleaseEnemy()   // Die Anim Event
    {
        EnemyPool.Release(this);
    }

    private void DestroyEnemy()  // Die Anim Event(Rock2, Rock3)
    {
        Destroy(gameObject, 0.016f);
    }
}
