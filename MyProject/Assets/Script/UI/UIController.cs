using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject SkillCanvas;
    public GameObject ScrollViewCanvas;
    public bool SkillCanvasActive;
    public bool ScrollViewCanvasActive;

    private void Awake()
    {
        // 아래처럼 해 주는 것이 좋으나 Canvas가 비활성화된 상태에서는
        // Find로 Canvas를 찾을 수가 없기 때문에 에디터에서 변수에 바로 넣어 준다.

        // SkillCanvas = GameObject.Find("SkillCanvas");
        // ScrollViewCanvas = GameObject.Find("ScrollViewCanvas");
        
    }
    void Start()
    {
        //true 상태로만 두는 것은 좋지 않음
        //Scene이 넘어가도 UI가 그대로 떠 있는 등 문제가 발생할 수 있기 때문
        SkillCanvasActive = true;
        SkillCanvas.SetActive(SkillCanvasActive);

        ScrollViewCanvasActive = false;
        ScrollViewCanvas.SetActive(ScrollViewCanvasActive);

    }

    public void onSkillCanvasActive()
    {
        SkillCanvasActive = !SkillCanvasActive;
        SkillCanvas.SetActive(SkillCanvasActive);
    }

    public void onMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }

    public void onScrollViewCanvasActive()
    {
        ScrollViewCanvasActive = !ScrollViewCanvasActive;
        ScrollViewCanvas.SetActive(ScrollViewCanvasActive);
    }

    public void onQuit()
    {
        Application.Quit();  // 에디터에서는 무시됨
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}
