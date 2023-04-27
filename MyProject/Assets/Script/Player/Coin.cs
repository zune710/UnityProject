using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : MonoBehaviour
{
    private Vector3 Movement;
    private float playerSpeedOffset;
    PlayerController playerController;

    private Animator Anim;
    private AudioSource sfx;

    private Animator CoinUpAnim;

    private IObjectPool<Coin> CoinPool;


    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        Anim = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();  // Coin

        CoinUpAnim = GameObject.Find("IncreaseText").GetComponent<Animator>();
    }

    private void Update()
    {
        playerSpeedOffset = playerController.Speed * 0.2f;  // 1.0 또는 2.0

        Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(1.0f * playerSpeedOffset, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 0.0f); // 1.0f: Background 2 속도

        transform.position -= Movement * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ControllerManager.GetInstance().Coin += 10;

            sfx.Play();

            Anim.SetTrigger("Collect");
            CoinUpAnim.SetTrigger("Collect");

            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    public void SetPool(IObjectPool<Coin> pool)
    {
        CoinPool = pool;
    }

    private void ReleaseCoin()
    {
        CoinPool.Release(this);
    }
}
