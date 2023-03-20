using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Text ui;

    public GameObject Player;
    public GameObject test;

    void Start()
    {
        ui = GetComponent<Text>();
    }

    void Update()
    {
        /*
         * 거리 구하는 공식
        {
            float x = Player.transform.position.x - test.transform.position.x;
            float y = Player.transform.position.y - test.transform.position.y;

            float distance = Mathf.Sqrt((x * x) + (y * y));
        }
        */

        /*
         * 방향 구하는 공식1
        {
            Vector3 Direction = new Vector3(
                Player.transform.position.x - test.transform.position.x,
                Player.transform.position.y - test.transform.position.y,
                0.0f);

        Direction.Normalize(); // 1보다 작은 값으로 변경하여 방향만 남김
        }
        */

        /*
         * 방향 구하는 공식2
        {
            Vector3 Direction = Player.transform.position - test.transform.position;
            Direction.Normalize();
        }
        */

        /*
         * 방향 구하는 공식3
        {
            Vector3 Direction = (Player.transform.position - test.transform.position).normalized;
        }
        */

        Vector3 Direction = (Player.transform.position - test.transform.position).normalized;

        test.transform.position += Direction * Time.deltaTime * 2.0f;

        // 유니티 제공 함수(거리 구하기)
        //float distance = Vector3.Distance(
        //    Player.transform.position,
        //    test.transform.position);

        //ui.text = distance.ToString();

        //if (distance > 5.0f)
        //    test.GetComponent<MyGizmo>().color = Color.green;
        //else
        //    test.GetComponent<MyGizmo>().color = Color.red;

        ui.text = "HP: " + ControllerManager.GetInstance().PlayerHP.ToString();
    }
}
