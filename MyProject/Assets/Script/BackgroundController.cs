using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // ** Background가 모여있는 계층구조의 최상위 객체(부모)
    private Transform parent;
    
    // ** Sprite를 포함하고 있는 구성요소 
    private SpriteRenderer spriteRenderer;
    
    // ** 이미지
    private Sprite sprite;

    // ** 생성 지점
    private float endPoint;

    // ** 삭제 지점
    private float exitPoint;

    // ** 이미지 이동 속도
    public float Speed;

    private float playerSpeed;

    // ** 플레이어 정보
    private GameObject player;
    private PlayerController playerController;

    // ** 움직임 정보
    private Vector3 movement;
    
    // ** 이미지가 중앙 위치에 정상적으로 노출될 수 있도록 하기 위한 완충 역할(버퍼) 
    private Vector3 offset = new Vector3(0.0f, 4.5f, 0.0f);  // 7.5f


    private void Awake()
    {
        // ** 플레이어의 기본 정보를 받아온다.
        player = GameObject.Find("Player").gameObject;  // .gameObject 안 써도 들어가지나 가독성 위해 씀

        // ** 부모객체를 받아온다.
        parent = GameObject.Find("BackgroundList").transform;

        // ** 현재 이미지를 담고 있는 구성요소를 받아온다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ** 플레이어 구성요소를 받아온다. ??
        playerController = player.GetComponent<PlayerController>();
    }


    void Start()
    {
        // ** 구성요소에 포함된 이미지를 받아온다.
        sprite = spriteRenderer.sprite;

        // ** 시작 지점을 설정
        endPoint = sprite.bounds.size.x * 0.5f + transform.position.x;
        
        // ** 종료 지점을 설정
        exitPoint = -(sprite.bounds.size.x * 0.5f) + player.transform.position.x;
    }


    void Update()
    {
        offset.y = player.transform.position.y * -1;
        playerSpeed = playerController.Speed * 0.2f;

        // ** 이동 정보 세팅
        movement = new Vector3(
            Input.GetAxisRaw("Horizontal") * Time.deltaTime * Speed * playerSpeed + offset.x, // ** 나중에 singleton으로 변경해야 함
            player.transform.position.y + offset.y,
            0.0f + offset.z);

        // ** singleton
        // ** 플레이어가 바라보고 있는 방향에 따라 분기됨(플레이어가 오른쪽으로 갈 때만 맵이 움직임)
        if (ControllerManager.GetInstance().DirRight)
        {
            // ** 우측 이동
            // ** 이동 정보 적용
            transform.position -= movement;
            endPoint -= movement.x;
        }

        // ** 동일한 이미지 복사
        if (player.transform.position.x + (sprite.bounds.size.x * 0.5f) + 1 > endPoint)
        {
            // ** 이미지를 복제한다.
            GameObject Obj = Instantiate(this.gameObject);
            
            // ** 복제된 이미지의 부모를 설정한다.
            Obj.transform.SetParent(parent.transform);
            
            // ** 복제된 이미지의 이름을 설정한다.
            Obj.transform.name = transform.name;

            // ** 복제된 이미지의 위치를 설정한다.
            Obj.transform.position = new Vector3(
                endPoint + 25.0f,
                0.0f, 0.0f);

            // ** 시작 지점을 변경한다.
            endPoint += endPoint + 25.0f;
        }

        // ** 종료 지점에 도달하면 삭제한다.
        if (transform.position.x + (sprite.bounds.size.x * 0.5f) -2 < exitPoint)
            Destroy(this.gameObject);
    }
}
