using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public GameObject Target;

    private Slider HPBar;
    private Text BossName;

    private RectTransform rectTransform;

    private BossController controller;


    private void Awake()
    {
        HPBar = GetComponent<Slider>();

        BossName = transform.Find("BossText").gameObject.GetComponent<Text>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.anchoredPosition = new Vector3(0.0f, -120.0f, 0.0f);
        
        BossName.text = BossManager.GetInstance.bossType.ToString();

        controller = Target.GetComponent<BossController>();
        
        HPBar.maxValue = controller.HP;
        HPBar.value = HPBar.maxValue;
    }

    private void Update()
    {
        HPBar.value = controller.HP;
        
        if (controller.HP <= 0)
        {
            Destroy(gameObject, 3.0f);
        }
    }
}
