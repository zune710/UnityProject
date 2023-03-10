using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // private SpriteRenderer spriteRenderer;

    // ** �Ѿ��� ���ư��� �ӵ�
    private float Speed;

    // ** �Ѿ��� �浹�� Ƚ��
    private int hp;

    // ** ����Ʈ ȿ�� ����
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

        // ** �浹 Ƚ���� 3���� �����Ѵ�.
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
        // ����ó�� (�Ѿ��� ī�޶� ���� ��� ������ �ʴ� ���� �浹�ϸ� ������)
        if (collision.tag == "Wall")
        {
            Destroy(this.gameObject);
            return;
        }

        // ** �浹 Ƚ�� ����
        --hp;

        // ** ����Ʈ ȿ�� ����
        GameObject Obj = Instantiate(fxPrefab);

        // ** ���� ȿ���� ������ ������ ����
        GameObject camera = new GameObject("Camera Test");

        // ** ���� ȿ�� ��Ʈ�ѷ� ����
        camera.AddComponent<CameraController>();

        // ** ����Ʈ ȿ���� ��ġ�� ����
        Obj.transform.position = transform.position;

        // ** collision = �浹�� ���
        // ** �浹�� ����� �����Ѵ�.
        Destroy(collision.transform.gameObject);

        // ** �Ѿ��� �浹 Ƚ���� 0�� �Ǹ�(�浹 ���� Ƚ���� ��� �����ϸ�) �Ѿ� ����
        if (hp == 0)
            Destroy(this.gameObject);
    }
}

// ����Ƽ������ �� ���� �Լ��� ����Ǹ鼭 ������ ���̱� ������ �ּ� ó������ �ʰ� ����� ���� ����.
