using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    
    public float Speed;
    public float jumpPower;

    public bool isJump = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(hor, ver, 0.0f) * Speed * Time.deltaTime;

        transform.position += movement;
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJump)
            {
                isJump = true;

                rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name.Equals("Tilemap"))
        {
            isJump = false;
        }
    }
}
