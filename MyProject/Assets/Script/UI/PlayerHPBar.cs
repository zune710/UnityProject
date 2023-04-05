using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    private Slider HPBar;

    private Text HPText;

    public GameObject Parent;
    private GameObject HeartPrefab;

    private void Awake()
    {
        HPBar = GetComponent<Slider>();
        HPText = transform.Find("HPText").GetComponent<Text>();

        HeartPrefab = Resources.Load("Prefabs/Heart") as GameObject;
    }

    private void Start()
    {
        HPBar.maxValue = 100;
        HPBar.value = ControllerManager.GetInstance().Player_HP;

        int count = ControllerManager.GetInstance().Heart;  // 1 ~ 3

        for (int i = 0; i < count; ++i)
        {
            GameObject Obj = Instantiate(HeartPrefab);
            Obj.name = "Heart";
            Obj.transform.SetParent(Parent.transform);
        }
    }

    private void Update()
    {
        //if(Input.GetMouseButton(0))
        //{
        //    if (ControllerManager.GetInstance().Player_HP < 100)
        //        ControllerManager.GetInstance().Player_HP += 1;
        //}

        //if (Input.GetMouseButton(1))
        //{
        //    if (ControllerManager.GetInstance().Player_HP > 0)
        //        ControllerManager.GetInstance().Player_HP -= 1;
        //}

        HPBar.value = ControllerManager.GetInstance().Player_HP;

        HPText.text = ControllerManager.GetInstance().Player_HP.ToString();
    }
}
