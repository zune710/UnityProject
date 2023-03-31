using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


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
class DataForm
{
    public string name;
    public string age;

    public int Round;
    public int EnemyId;
    public int BossId;
    public int Goal;
    public bool onEnemy;
    public bool onBoss;
    public bool active;

    //ItemForm item;  // json 안에 json 형태로 저장

    public DataForm() { }

    public DataForm(string _name, string _age)
    {
        name = _name;
        age = _age;
    }
}



//Round = 1;
//EnemyId = 1;
//BossId = 1;
//public bool onEnemy = true;
//public bool onBoss = false;
//       BossManager.GetInstance.active = true;

public class DataManager : MonoBehaviour
{
    // (호출되려면 싱글톤화 필요)

    private string userName;
    private int value;

    void Start()
    {
        //var JsonData = Resources.Load<TextAsset>("saveFile/Data");
        var JsonData = Resources.Load<TextAsset>("saveFile/SaveData");

        if(JsonData)
        {
            DataForm form = JsonUtility.FromJson<DataForm>(JsonData.ToString());  // 로컬 저장

            RoundManager.GetInstance.Round = form.Round;
            RoundManager.GetInstance.EnemyId = form.EnemyId;
            RoundManager.GetInstance.BossId = form.BossId;
            RoundManager.GetInstance.Goal = form.Goal;
            ControllerManager.GetInstance().onEnemy = form.onEnemy;
            ControllerManager.GetInstance().onBoss = form.onBoss;
            BossManager.GetInstance.active = form.active;
        }

        //value = int.Parse(form.age);
        //userName = form.name;

        //print(userName + " : " + value);



        //WWWForm wform = new WWWForm(); // 서버 저장
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            ++value;
            print(value);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            --value;
            print(value);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveData(userName, value.ToString());
        }
    }

    public void ExitButton()
    {
        SaveData();
    }

    // (외부에서 호출되면 외부 데이터 가지고 들어올 수 있어야 함. 지금은 X)
    public void SaveData(string _name, string _age)
    {
        DataForm form = new DataForm(_name, _age);

        string JsonData = JsonUtility.ToJson(form);

        // 없으면 경로에 파일 생성
        FileStream fileStream = new FileStream(
            Application.dataPath + "/Resources/saveFile/Data.json", FileMode.Create);

        byte[] data = Encoding.UTF8.GetBytes(JsonData);

        // 파일에 내용 입력
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();  // 중요!
    }

    public void SaveData()
    {
        DataForm form = new DataForm();

        form.Round = RoundManager.GetInstance.Round;
        form.EnemyId = RoundManager.GetInstance.EnemyId;
        form.BossId = RoundManager.GetInstance.BossId;
        form.Goal = RoundManager.GetInstance.Goal;
        form.onEnemy = ControllerManager.GetInstance().onEnemy;
        form.onBoss = ControllerManager.GetInstance().onBoss;
        form.active = BossManager.GetInstance.active;

        string JsonData = JsonUtility.ToJson(form);

        // 없으면 경로에 파일 생성
        FileStream fileStream = new FileStream(
            Application.dataPath + "/Resources/saveFile/SaveData.json", FileMode.Create);

        byte[] data = Encoding.UTF8.GetBytes(JsonData);

        // 파일에 내용 입력
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();  // 중요!
    }
}
