using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    // ** 총알이 날아가는 속도
    private float Speed;

    // ** 이펙트 효과 원본
    public GameObject fxPrefab;

    // ** 총알이 날아가야 할 방향
    public Vector3 Direction { get; set; }


    private void Awake()
    {
        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
    }

    private void Start()
    {
        // ** 속도 초기값
        Speed = 10.0f;
    }

    void Update()
    {
        // ** 방향으로 속도만큼 위치를 변경
        transform.position += Direction * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ** 이펙트 효과 복제
        GameObject Obj = Instantiate(fxPrefab);

        // ** 이펙트 효과의 위치를 지정
        Obj.transform.position = transform.position;


        // ** 진동 효과를 생성할 관리자 생성
        GameObject camera = new GameObject("Camera Test");

        // ** 진동 효과 컨트롤러 생성
        camera.AddComponent<CameraController>();

        // 총알 삭제
        Destroy(this.gameObject, 0.016f);
    }


    private void DestroyEnemyBullet()
    {
        Destroy(gameObject, 0.016f);
    }
}
