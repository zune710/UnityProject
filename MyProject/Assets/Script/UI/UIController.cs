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
        ButtonSFX.Play();

        if(ControllerManager.GetInstance().Heart != 0 && !ControllerManager.GetInstance().GameClear)
        {
            SavingData.SetActive(true);
        }

        MenuFrame.SetActive(false);
        GameEndButton.interactable = false;

        DataManager.GetInstance.SaveData();
        StartCoroutine(LoadMainMenuScene());

        #region memo
        /* Heart == 0 또는 GameClear 이후 다시 로그인했을 때 LoadGame = false 되려면 SaveData() 해야 함
           단, '저장 중' 텍스트는 표시하지 않는다. */

        #endregion
    }

    private IEnumerator LoadMainMenuScene()
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
}
