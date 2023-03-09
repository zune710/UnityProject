using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // private SpriteRenderer spriteRenderer;

    // ** �Ѿ��� ���ư��� �ӵ�
    private float Speed;
    private int hp;

    public GameObject fxPrefab;


    // private float DefaultX;  // ������ �� position.x ��

    // ** �Ѿ��� ���ư��� �� ����
    public Vector3 Direction { get; set; } // �� ����
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
    //}  // stack overflow �߻�(��?)

    private void Start()
    {
        // spriteRenderer = this.GetComponent<SpriteRenderer>();

        // ** �ӵ� �ʱⰪ
        Speed = 10.0f;

        hp = 3;

        // DefaultX = transform.position.x;
    }

    void Update()
    {
        //if (Direction.x == -1)
        //    spriteRenderer.flipY = true;
        //else
        //    spriteRenderer.flipY = false;
        
        // ** �������� �ӵ���ŭ ��ġ�� ����
        transform.position += Direction * Speed * Time.deltaTime;
        
        //if(Mathf.Abs(DefaultX - transform.position.x) > 5)  // ó�� ��ġ���� ���� �Ÿ���ŭ �־����� �������.
        //    Destroy(this.gameObject);
    }

    // ** �浹ü(Collider2D)�� ��������(Rigidbody2D)�� ���Ե� ������Ʈ�� �ٸ� �浹ü�� �浹�Ѵٸ� ����Ǵ� �Լ�
    // Trigger: �浹������ ����� / Collider: �浹�ϸ� �������� ����
    // Enter: ó�� �ε��� ���� / Stay: �浹ü �ȿ� ���� �� / Exit: �浹ü���� ���������� ������ ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            DestroyObject(this.gameObject);
            return;
        }

        --hp;

        GameObject Obj = Instantiate(fxPrefab);

        GameObject camera = new GameObject("Camera Test");
        camera.AddComponent<CameraController>();

        Obj.transform.position = transform.position;

        DestroyObject(collision.transform.gameObject);
        
        if(hp == 0)
            DestroyObject(this.gameObject);
    }

    // ����Ƽ������ �� ���� �Լ��� ����Ǹ鼭 ������ ���̱� ������ �ּ� ó������ �ʰ� ����� ���� ����.
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        print("Stay");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Exit");
    }
    */
}
