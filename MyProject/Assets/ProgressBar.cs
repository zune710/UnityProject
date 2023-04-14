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
    public Image image;

    IEnumerator Start()
    {
        //EditorApplication.isPaused = true;  // playmode 일시정지

        // MainMenu로 이동하기 전에 LoadData 실행
        DataManager dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        dataManager.LoadData();

        asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        asyncOperation.allowSceneActivation = false;

        image.fillAmount = 0;

        while (!asyncOperation.isDone)
        {
            //float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            //text.text = (progress * 100f).ToString() + "%";

            // ** asyncOperation.progress: 0.0 ~ 0.9
            float progress = asyncOperation.progress / 0.9f * 100f;  // 0.0 ~ 100.0
            text.text = progress.ToString() + "%";

            image.fillAmount = asyncOperation.progress / 0.9f;  // 0.0 ~ 1.0

            yield return null;

            if (asyncOperation.progress > 0.7f)
            {
                //yield return new WaitForSeconds(2.5f);
                //asyncOperation.allowSceneActivation = true;
                
                // LoadData 완료되면 다음 Scene으로 이동
                if(dataManager.isDone)
                {
                    messageText.gameObject.SetActive(true);
                    
                    if (Input.GetMouseButtonDown(0))  // or Input.anyKeyDown
                        asyncOperation.allowSceneActivation = true;

                }

            }
        }
    }
}
