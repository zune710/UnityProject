using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    // ** 따라다닐 객체
    public GameObject Target;

    // ** 세부위치 조정
    private Vector3 offset;

    private Slider HPBar;

    private BossController controller;


    private void Awake()
    {
        HPBar = GetComponent<Slider>();
    }

    private void Start()
    {
        // ** 위치 세팅
        offset = new Vector3(0.0f, 2.7f, 0.0f);  // Boss 머리 위

        controller = Target.GetComponent<BossController>();
        
        HPBar.maxValue = controller.HP;
        HPBar.value = HPBar.maxValue;
    }

    private void Update()
    {
        // ** WorldToScreenPoint = 월드 좌표를 카메라 좌표로 변환
        // ** 월드상에 있는 타깃의 좌표를 카메라 좌표로 변환하여 UI에 세팅한다.
        transform.position = Camera.main.WorldToScreenPoint(Target.transform.position + offset);

        HPBar.value = controller.HP;
        
        if (controller.HP <= 0)  // !Target
        {
            Destroy(gameObject, 0.016f);
        }
    }
}