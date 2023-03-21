using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    // ** 따라다닐 객체
    public GameObject Target;

    // ** 세부위치 조정
    private Vector3 offset;

    private Slider HPBar;

    private void Awake()
    {
        HPBar = GetComponent<Slider>();
    }

    private void Start()
    {
        // ** 위치 세팅
        offset = new Vector3(0.6f, 0.4f, 0.0f);  // Enemy 머리 위

        HPBar.maxValue = ControllerManager.GetInstance().Enemy_HP;
        HPBar.value = HPBar.maxValue;
    }

    private void Update()
    {
        // ** WorldToScreenPoint = 월드 좌표를 카메라 좌표로 변환
        // ** 월드상에 있는 타깃의 좌표를 카메라 좌표로 변환하여 UI에 세팅한다.
        transform.position = Camera.main.WorldToScreenPoint(Target.transform.position + offset);

        HPBar.value = ControllerManager.GetInstance().Enemy_HP;
    }
}
