using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float Speed;

    private GameObject player;
    private Vector3 movement;
    private Vector3 offset = new Vector3(0.0f, 7.5f, 0.0f);

    void Start()
    {
        player = GameObject.Find("Player").gameObject;  // gameObject 안 써도 들어가짐
    }

    void Update()
    {
        movement = new Vector3(
            Input.GetAxisRaw("Horizontal") + offset.x,
            player.transform.position.y + offset.y,
            0.0f + offset.z);
        // Input을 양쪽에서 받으면 좋지 않다. 한쪽에서 받아서 가져와 사용해야 한다.

        transform.position -= movement * Time.deltaTime * Speed;
    }
}
