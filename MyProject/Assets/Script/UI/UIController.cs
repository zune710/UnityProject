using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject SkillCanvas;
    public bool SkillCanvasActive;

    void Start()
    {
        //true 상태로만 두는 것은 좋지 않음
        SkillCanvasActive = true;
        SkillCanvas.SetActive(SkillCanvasActive);
    }

    public void onSkillCanvasActive()
    {
        SkillCanvasActive = !SkillCanvasActive;
        SkillCanvas.SetActive(SkillCanvasActive);
    }

    public void onMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
}
