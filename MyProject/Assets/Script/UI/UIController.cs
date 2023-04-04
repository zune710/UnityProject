using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject SettingMenu;
    public bool SettingMenuActive;


    void Start()
    {
        SettingMenuActive = false;
        SettingMenu.SetActive(SettingMenuActive);

    }


    public void onMainMenu()
    {
        if(ControllerManager.GetInstance().GameOver)
        {
            // Heart 사라지는 UI(Heart 이미지 바꾸기)


        }
        
        SceneManager.LoadScene("MainMenu");
    }

    public void onRestart()  // 해당 라운드 처음부터 다시
    {
        // 변수 초기화
        if (ControllerManager.GetInstance().onBoss)
        {
            ControllerManager.GetInstance().BossActive = true;
            ControllerManager.GetInstance().Player_HP = 100;
        }
        else
        {
            ControllerManager.GetInstance().EnemyCount = 0;
            ControllerManager.GetInstance().Player_HP = 100;
        }

        // Reload Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void onSettingMenuActive()  // ScrollView
    {
        SettingMenuActive = !SettingMenuActive;
        SettingMenu.SetActive(SettingMenuActive);
    }

    public void onQuit()
    {
        Application.Quit();  // 에디터에서는 무시됨
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}
