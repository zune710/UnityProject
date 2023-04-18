using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject SettingMenu;
    public bool SettingMenuActive;

    public GameObject SavingData;
    public GameObject MenuFrame;
    public Button GameEndButton;

    private AudioSource ButtonSFX;

    private void Awake()
    {
        ButtonSFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        SettingMenuActive = false;
        SettingMenu.SetActive(SettingMenuActive);

        SavingData.SetActive(false);
    }


    public void onMainMenu()  // Save & Exit
    {
        ButtonSFX.Play();  // 소리 나기 전에 넘어가는 듯

        if(ControllerManager.GetInstance().Heart == 0 || ControllerManager.GetInstance().GameClear)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        MenuFrame.SetActive(false);
        GameEndButton.interactable = false;
        SavingData.SetActive(true);
        DataManager.GetInstance.SaveData();

        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        while (true)
        {
            if (DataManager.GetInstance.isDone)
            {
                SavingData.SetActive(false);
                DataManager.GetInstance.isDone = false;

                SceneManager.LoadScene("MainMenu");

                yield break;
            }

            yield return null;
        }
    }

    public void onRestart()  // 해당 라운드 처음부터 다시
    {
        ButtonSFX.Play();

        // 변수 초기화
        if (ControllerManager.GetInstance().onBoss)
        {
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
        ButtonSFX.Play();

        SettingMenuActive = !SettingMenuActive;
        SettingMenu.SetActive(SettingMenuActive);
    }

    public void onQuit()  // 사용 안 하는 듯
    {
        ButtonSFX.Play();

        DataManager.GetInstance.SaveData();

        Application.Quit();  // 에디터에서는 무시됨
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}
