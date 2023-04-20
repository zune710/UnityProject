 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

//[System.Serializable]
//class ItemForm
//{
//    public string A;
//    public string B;

//    ItemForm(string _A, string _B)
//    {
//        A = _A;
//        B = _B;
//    }
//}


[System.Serializable]
public class DataForm
{
    public bool LoadGame;

    public bool GoalClear;
    public bool BossClear;

    public bool GameOver;
    public bool GameClear;

    public int Round;
    public int Goal;
    public bool onEnemy;
    public bool onBoss;
    public bool BossActive;

    public int Player_HP;
    public int Heart;
    public int EnemyCount;

    public int EnemyId;
    public int BossId;

    public DataForm() { }

    //ItemForm item;  // json 안에 json 형태로 저장
}


public class DataManager : MonoBehaviour
{
    // (호출되려면 싱글톤화 필요)
    private DataManager() { }

    private static DataManager instance = null;

    public static DataManager GetInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    public bool isDone = false;
    public bool isPaused = false;

    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData()
    {
        WWWForm www = new WWWForm();
        www.AddField("order", "load data");
        
        StartCoroutine(Post(www, nameof(LoadData)));
    }

    public void SaveData()
    {
        WWWForm form = new WWWForm();

        form.AddField("order", "save data");

        form.AddField("Player_HP", ControllerManager.GetInstance().Player_HP);
        form.AddField("Heart", ControllerManager.GetInstance().Heart);
        form.AddField("EnemyCount", ControllerManager.GetInstance().EnemyCount);

        form.AddField("EnemyId", ControllerManager.GetInstance().EnemyId);
        form.AddField("BossId", ControllerManager.GetInstance().BossId);

        form.AddField("Round", ControllerManager.GetInstance().Round);
        form.AddField("Goal", ControllerManager.GetInstance().Goal);
        form.AddField("onEnemy", ControllerManager.GetInstance().onEnemy.ToString());
        form.AddField("onBoss", ControllerManager.GetInstance().onBoss.ToString());
        form.AddField("BossActive", ControllerManager.GetInstance().BossActive.ToString());

        form.AddField("LoadGame", ControllerManager.GetInstance().LoadGame.ToString());
        form.AddField("GoalClear", ControllerManager.GetInstance().GoalClear.ToString());
        form.AddField("BossClear", ControllerManager.GetInstance().BossClear.ToString());
        form.AddField("GameOver", ControllerManager.GetInstance().GameOver.ToString());
        form.AddField("GameClear", ControllerManager.GetInstance().GameClear.ToString());

        StartCoroutine(Post(form, nameof(SaveData)));
    }

    public void LogOut()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "sign out");

        ControllerManager.GetInstance().PlayerId = null;

        StartCoroutine(Post(form, nameof(LogOut)));
    }

    private IEnumerator Post(WWWForm www, string order)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, www))
        {
            yield return request.SendWebRequest();
            
            if (request.isDone)
            {
                Debug.Log(request.downloadHandler.text);
                
                if(order == nameof(LoadData))
                {
                    if (request.downloadHandler.text == "데이터 없음")
                    {
                        isDone = true;
                        yield break;
                    }

                    DataForm form = JsonUtility.FromJson<DataForm>(request.downloadHandler.text);

                    InsertData(form);
                }
            }
            else
                Debug.Log("응답없음");
        }

        isDone = true;
    }

    private void InsertData(DataForm form)
    {
        ControllerManager.GetInstance().Player_HP = form.Player_HP;
        ControllerManager.GetInstance().Heart = form.Heart;
        ControllerManager.GetInstance().EnemyCount = form.EnemyCount;

        ControllerManager.GetInstance().EnemyId = form.EnemyId;
        ControllerManager.GetInstance().BossId = form.BossId;

        ControllerManager.GetInstance().Round = form.Round;
        ControllerManager.GetInstance().Goal = form.Goal;
        ControllerManager.GetInstance().onEnemy = form.onEnemy;
        ControllerManager.GetInstance().onBoss = form.onBoss;
        ControllerManager.GetInstance().BossActive = form.BossActive;

        ControllerManager.GetInstance().LoadGame = form.LoadGame;
        ControllerManager.GetInstance().GoalClear = form.GoalClear;
        ControllerManager.GetInstance().BossClear = form.BossClear;
        ControllerManager.GetInstance().GameOver = form.GameOver;
        ControllerManager.GetInstance().GameClear = form.GameClear;
    }


    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (SceneManager.GetActiveScene().name == "GameStart")
            {
                isPaused = true;  // RoundManager PlayFadeIn 코루틴 함수에서 사용
                GameObject obj = GameObject.Find("SideMenuCanvas").transform.Find("DarkBackground").gameObject;

                if (!obj.activeSelf)  
                    GameObject.Find("SideMenuCanvas").GetComponent<SidebarController>().ClickButton();
            }
        }
        else
        {
            if(isPaused)
                isPaused = false;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            if (SceneManager.GetActiveScene().name == "GameStart")
            {
                GameObject obj = GameObject.Find("SideMenuCanvas").transform.Find("DarkBackground").gameObject;
                
                if (!obj.activeSelf)
                    GameObject.Find("SideMenuCanvas").GetComponent<SidebarController>().ClickButton();
            }
        }
    }

    private void OnApplicationQuit()
    {
        LogOut();
    }

    #region memo
    /** 1. 로컬 저장 **/
    /* Start -- Load Data */
    ////var JsonData = Resources.Load<TextAsset>("saveFile/Data");
    //var JsonData = Resources.Load<TextAsset>("saveFile/SaveData");

    //if(JsonData)
    //{
    //    DataForm form = JsonUtility.FromJson<DataForm>(JsonData.ToString());

    //    value = int.Parse(form.age);
    //    userName = form.name;

    //    print(userName + " : " + value);
    //}

    /* custom method -- Save Data */
    //DataForm form = new DataForm();
    //string JsonData = JsonUtility.ToJson(form);

    //// 없으면 경로에 파일 생성  // using System.IO; 필요
    //FileStream fileStream = new FileStream(
    //    Application.dataPath + "/Resources/saveFile/SaveData.json", FileMode.Create);

    //byte[] data = Encoding.UTF8.GetBytes(JsonData);  // using System.Text; 필요

    //// 파일에 내용 입력
    //fileStream.Write(data, 0, data.Length);
    //fileStream.Close();  // 중요!


    /** 2. 서버 저장 **/
    //WWWForm wform = new WWWForm();  // WWWForm 사용
    #endregion
}
