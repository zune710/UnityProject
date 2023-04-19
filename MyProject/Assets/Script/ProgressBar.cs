using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public Text text;
    public Text messageText;
    public Text TipText;
    public Slider slider;

    IEnumerator Start()
    {
        //EditorApplication.isPaused = true;  // playmode 일시정지

        // MainMenu로 이동하기 전에 LoadData 실행
        DataManager.GetInstance.LoadData();

        asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        asyncOperation.allowSceneActivation = false;

        slider.maxValue = 100;
        slider.value = 0;

        while (!asyncOperation.isDone)
        {
            // ** asyncOperation.progress: 0.0 ~ 0.9
            float progress = asyncOperation.progress / 0.9f * 100f;  // 0.0 ~ 100.0
            text.text = progress.ToString() + "%";

            slider.value = progress;

            yield return null;

            if (asyncOperation.progress > 0.7f)
            {
                //yield return new WaitForSeconds(2.5f);
                //asyncOperation.allowSceneActivation = true;
                
                // LoadData 완료되면 다음 Scene으로 이동
                if(DataManager.GetInstance.isDone)
                {
                    messageText.gameObject.SetActive(true);
                    
                    if (Input.anyKeyDown)  // or Input.GetMouseButtonDown(0)
                    {
                        DataManager.GetInstance.isDone = false;
                        asyncOperation.allowSceneActivation = true;
                    }
                }

            }
        }
    }
}
