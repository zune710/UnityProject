using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public�� ����Ƽ Inspector â�� ������ �ȴ�.

    // ** �����̴� �ӵ�
    private float Speed;
    private Vector3 Movement;

    private int Health;

    private Animator animator;

    private bool onAttack; 
    private bool onHit;
    private bool onJump;
    private bool onRoll;
    private bool onDead;
    private bool onDive;

    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �ʱⰪ�� ������ �� ���
    void Start()
    {
        // ** �ӵ��� �ʱ�ȭ
        Speed = 5.0f;

        Health = 10;

        // ** Player�� Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();  // 'this.' ���� ����
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
        float Ver = Input.GetAxisRaw("Vertical");

        // Time.deltaTime: �����Ӱ� ������ ������ ������ �̿��ؼ� ��ǻ�� ���ɰ� ������� ��� ��ǻ�Ϳ��� �����ϰ� �۵��ǵ��� �ϱ� ���� ��
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed,
            Ver * Time.deltaTime * Speed, 
            0.0f);


        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        if (Input.GetKey(KeyCode.Space))
        {
            OnJump();
          //  transform.position += new Vector3(0.0f, Speed, 0.0f);
          //  OnDive();
          //  transform.position -= new Vector3(0.0f, Speed, 0.0f);
        }
            

        if (Input.GetKey(KeyCode.Q))
            OnRoll();

        if (Health <= 0)
            OnDead();

        animator.SetFloat("Speed", Hor);
        transform.position += Movement;  // ���� FixedUpdate�� ��� ��
    }

    private void OnAttack()
    {
        if (onAttack)
            return;

        onAttack = true;
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        onAttack = false;
    }

    private void OnHit()
    {
        if (onHit)
            return;

        onHit = true;
        animator.SetTrigger("Hit");
        Health -= 5;
    }

    private void SetHit()
    {
        onHit = false;
    }

    private void OnJump()
    {
        if (onJump)
            return;

        onJump = true;
        animator.SetTrigger("Jump");
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

    /*
    private void FixedUpdate()
    {
        transform.position += Movement;
    }
    */
}

