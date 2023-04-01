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
        SceneManager.LoadScene("MainMenu");
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
