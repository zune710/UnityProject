using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
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
public class RoundDataForm
{
    public int Round;
    public int Goal;
    public bool onEnemy;
    public bool onBoss;
    public bool BossActive;

    public RoundDataForm() { }

    public RoundDataForm(int round, int goal, bool onEnemy, bool onBoss, bool bossActive)
    {
        Round = round;
        Goal = goal;
        this.onEnemy = onEnemy;
        this.onBoss = onBoss;
        BossActive = bossActive;
    }
}

[System.Serializable]
public class GameDataForm
{
    public bool LoadGame;

    public bool GoalClear;
    public bool BossClear;

    public bool GameOver;
    public bool GameClear;

    public GameDataForm() { }

    public GameDataForm(bool loadGame, bool goalClear, bool bossClear, bool gameOver, bool gameClear)
    {
        LoadGame = loadGame;
        GoalClear = goalClear;
        BossClear = bossClear;
        GameOver = gameOver;
        GameClear = gameClear;
    }
}

[System.Serializable]
public class PlayerDataForm
{
    public int Player_HP;
    public int Heart;
    public int EnemyCount;

    public PlayerDataForm(int player_HP, int heart, int enemyCount)
    {
        Player_HP = player_HP;
        Heart = heart;
        EnemyCount = enemyCount;
    }
}

[System.Serializable]
public class EnemyDataForm
{
    public int EnemyId;
    public int BossId;

    public EnemyDataForm(int enemyId, int bossId)
    {
        EnemyId = enemyId;
        BossId = bossId;
    }
}

[System.Serializable]
public class DataForm
{
    public RoundDataForm RoundData;
    public GameDataForm GameData;
    public PlayerDataForm PlayerData;
    public EnemyDataForm EnemyData;

    public DataForm() { }

    public DataForm(RoundDataForm roundData, GameDataForm gameData, PlayerDataForm playerData, EnemyDataForm enemyData)
    {
        RoundData = roundData;
        GameData = gameData;
        PlayerData = playerData;
        EnemyData = enemyData;
    }

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

    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";

    //private string userName;
    //private int value;

    void Start()
    {
        //var JsonData = Resources.Load<TextAsset>("saveFile/Data");
        var JsonData = Resources.Load<TextAsset>("saveFile/SaveData");

        if(JsonData)
        {
            DataForm form = JsonUtility.FromJson<DataForm>(JsonData.ToString());  // 로컬 저장

            ControllerManager.GetInstance().Player_HP = form.PlayerData.Player_HP;
            ControllerManager.GetInstance().Heart = form.PlayerData.Heart;
            ControllerManager.GetInstance().EnemyCount = form.PlayerData.EnemyCount;

            ControllerManager.GetInstance().EnemyId = form.EnemyData.EnemyId;
            ControllerManager.GetInstance().BossId = form.EnemyData.BossId;

            ControllerManager.GetInstance().Round = form.RoundData.Round;
            ControllerManager.GetInstance().Goal = form.RoundData.Goal;
            ControllerManager.GetInstance().onEnemy = form.RoundData.onEnemy;
            ControllerManager.GetInstance().onBoss = form.RoundData.onBoss;
            ControllerManager.GetInstance().BossActive = form.RoundData.BossActive;

            ControllerManager.GetInstance().LoadGame = form.GameData.LoadGame;
            ControllerManager.GetInstance().GoalClear = form.GameData.GoalClear;
            ControllerManager.GetInstance().BossClear = form.GameData.BossClear;
            ControllerManager.GetInstance().GameOver = form.GameData.GameOver;
            ControllerManager.GetInstance().GameClear = form.GameData.GameClear;

        }

        DontDestroyOnLoad(gameObject);

        //value = int.Parse(form.age);
        //userName = form.name;

        //print(userName + " : " + value);



        //WWWForm wform = new WWWForm(); // 서버 저장
    }

    //private void Update()
    //{
    //    if(Input.GetKeyUp(KeyCode.UpArrow))
    //    {
    //        ++value;
    //        print(value);
    //    }

    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        --value;
    //        print(value);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        SaveData(userName, value.ToString());
    //    }
    //}

    //public void ExitButton()
    //{
    //    SaveData();
    //}




    public IEnumerator LoadData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.isDone)
            {
                Debug.Log(request.downloadHandler.text);
                DataForm form = JsonUtility.FromJson<DataForm>(request.downloadHandler.text);

                InsertData(form);
                Debug.Log("LoadData 완료");
            }
            else
                Debug.Log("응답없음");
        }
    }

    public void InsertData(DataForm form)
    {
        ControllerManager.GetInstance().Player_HP = form.PlayerData.Player_HP;
        ControllerManager.GetInstance().Heart = form.PlayerData.Heart;
        ControllerManager.GetInstance().EnemyCount = form.PlayerData.EnemyCount;

        ControllerManager.GetInstance().EnemyId = form.EnemyData.EnemyId;
        ControllerManager.GetInstance().BossId = form.EnemyData.BossId;

        ControllerManager.GetInstance().Round = form.RoundData.Round;
        ControllerManager.GetInstance().Goal = form.RoundData.Goal;
        ControllerManager.GetInstance().onEnemy = form.RoundData.onEnemy;
        ControllerManager.GetInstance().onBoss = form.RoundData.onBoss;
        ControllerManager.GetInstance().BossActive = form.RoundData.BossActive;

        ControllerManager.GetInstance().LoadGame = form.GameData.LoadGame;
        ControllerManager.GetInstance().GoalClear = form.GameData.GoalClear;
        ControllerManager.GetInstance().BossClear = form.GameData.BossClear;
        ControllerManager.GetInstance().GameOver = form.GameData.GameOver;
        ControllerManager.GetInstance().GameClear = form.GameData.GameClear;
    }

    // (외부에서 호출되면 외부 데이터 가지고 들어올 수 있어야 함. 지금은 X)
    public void SaveData()
    {
        DataForm data = new DataForm();
        WWWForm form = new WWWForm();

        form.AddField(nameof(data.PlayerData.Player_HP), ControllerManager.GetInstance().Player_HP);
        form.AddField(nameof(data.PlayerData.Heart), ControllerManager.GetInstance().Heart);
        form.AddField(nameof(data.PlayerData.EnemyCount), ControllerManager.GetInstance().EnemyCount);
        
        form.AddField(nameof(data.EnemyData.EnemyId), ControllerManager.GetInstance().EnemyId);
        form.AddField(nameof(data.EnemyData.BossId), ControllerManager.GetInstance().BossId);
        
        form.AddField(nameof(data.RoundData.Round), ControllerManager.GetInstance().Round);
        form.AddField(nameof(data.RoundData.Goal), ControllerManager.GetInstance().Goal);
        form.AddField(nameof(data.RoundData.onEnemy), ControllerManager.GetInstance().onEnemy.ToString());
        form.AddField(nameof(data.RoundData.onBoss), ControllerManager.GetInstance().onBoss.ToString());
        form.AddField(nameof(data.RoundData.BossActive), ControllerManager.GetInstance().BossActive.ToString());
        
        form.AddField(nameof(data.GameData.LoadGame), ControllerManager.GetInstance().LoadGame.ToString());
        form.AddField(nameof(data.GameData.GoalClear), ControllerManager.GetInstance().GoalClear.ToString());
        form.AddField(nameof(data.GameData.BossClear), ControllerManager.GetInstance().BossClear.ToString());
        form.AddField(nameof(data.GameData.GameOver), ControllerManager.GetInstance().GameOver.ToString());
        form.AddField(nameof(data.GameData.GameClear) , ControllerManager.GetInstance().GameClear.ToString());

        string JsonData = JsonUtility.ToJson(form);

        StartCoroutine(Post(form));

        //// 없으면 경로에 파일 생성
        //FileStream fileStream = new FileStream(
        //    Application.dataPath + "/Resources/saveFile/SaveData.json", FileMode.Create);

        //byte[] data = Encoding.UTF8.GetBytes(JsonData);

        //// 파일에 내용 입력
        //fileStream.Write(data, 0, data.Length);
        //fileStream.Close();  // 중요!
    }

    public IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {
            yield return request.SendWebRequest();

            if (request.isDone)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
                Debug.Log("응답없음");
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            if(SceneManager.GetActiveScene().name == "GameStart")
            {
                GameObject.Find("SideMenuCanvas").GetComponent<SidebarController>().ClickButton();
            }
        }
    }



    //form.AddField(PlayerData.Player_HP = ControllerManager.GetInstance().Player_HP);
    //    form.AddField(PlayerData.Heart = ControllerManager.GetInstance().Heart);
    //    form.AddField(PlayerData.EnemyCount = ControllerManager.GetInstance().EnemyCount);
        
    //    form.AddField(EnemyData.EnemyId = ControllerManager.GetInstance().EnemyId);
    //    form.AddField(EnemyData.BossId = ControllerManager.GetInstance().BossId);
        
    //    form.AddField(RoundData.Round = ControllerManager.GetInstance().Round);
    //    form.AddField(RoundData.Goal = ControllerManager.GetInstance().Goal);
    //    form.AddField(RoundData.onEnemy = ControllerManager.GetInstance().onEnemy);
    //    form.AddField(RoundData.onBoss = ControllerManager.GetInstance().onBoss);
    //    form.AddField(RoundData.BossActive = ControllerManager.GetInstance().BossActive);
        
    //    form.AddField(GameData.LoadGame = ControllerManager.GetInstance().LoadGame);
    //    form.AddField(GameData.GoalClear = ControllerManager.GetInstance().GoalClear);
    //    form.AddField(GameData.BossClear = ControllerManager.GetInstance().BossClear);
    //    form.AddField(GameData.GameOver = ControllerManager.GetInstance().GameOver);
    //    form.AddField(GameData.GameClear = ControllerManager.GetInstance().GameClear);
}
