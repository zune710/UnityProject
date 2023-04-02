using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    private Slider HPBar;

    private Text HPText;


    private void Awake()
    {
        HPBar = GetComponent<Slider>();
        HPText = transform.Find("HPText").GetComponent<Text>();
    }

    private void Start()
    {
        HPBar.maxValue = 100;
        HPBar.value = ControllerManager.GetInstance().Player_HP;
    }

    private void Update()
    {
        //if(Input.GetMouseButton(0))
        //{
        //    if (ControllerManager.GetInstance().Player_HP < 100)
        //        ControllerManager.GetInstance().Player_HP += 1;
        //}

        if (Input.GetMouseButton(1))
        {
            if (ControllerManager.GetInstance().Player_HP > 0)
                ControllerManager.GetInstance().Player_HP -= 1;
            
        }

        HPBar.value = ControllerManager.GetInstance().Player_HP;

        if(ControllerManager.GetInstance().Player_HP <= 0)
        {
            // Die
        }

        HPText.text = ControllerManager.GetInstance().Player_HP.ToString();
    }
}
