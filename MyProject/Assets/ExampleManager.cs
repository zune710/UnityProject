using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;


[System.Serializable]
public class MemberForm
{
    public int index;
    public string name;
    public int age;
    public int gender;

    public MemberForm(int index, string name, int age, int gender)
    {
        this.index = index;
        this.name = name;
        this.age = age;
        this.gender = gender;
    }
}
// 회원가입
// 로그인

public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";


    public GameObject ID;
    public GameObject Password;
    private InputField id;
    private InputField password;

    private bool login;

    private void Awake()
    {
        id = ID.GetComponent<InputField>();
        password = Password.GetComponent<InputField>();
        login = false;
    }


    IEnumerator Start()
    {
        // ** 요청을 하기 위한 작업
        //UnityWebRequest request = UnityWebRequest.Get(URL);

        /*
        MemberForm member = new MemberForm("변사또", 45);

        WWWForm form = new WWWForm();
        form.AddField("Name", member.Name);
        form.AddField("Age", member.Age);

        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))  // using 사용: 안전한 사용을 위해 request 사용 후 해제
        {
            // 요청을 주고받는 작업
            yield return request.SendWebRequest();

            // ** 응답에 대한 작업
            print(request.downloadHandler.text);
        }
        */

        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            MemberForm json = JsonUtility.FromJson<MemberForm>(request.downloadHandler.text);

            // ** 응답에 대한 작업
            print(json.index);
            print(json.name);
            print(json.age);
            print(json.gender);
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("progressScene");

        //StartCoroutine(LoginTest());
    }

    public IEnumerator LoginTest()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "sign in");
        form.AddField("id", id.text);
        form.AddField("password", password.text);

        form.AddField("userName", id.text);
        form.AddField("age", password.text);

        using (UnityWebRequest request2 = UnityWebRequest.Post(URL, form))
        {
            yield return request2.SendWebRequest();

            if (request2.downloadHandler.text == "login")
                login = true;
        }
        Debug.Log(login);

        if (login)
            SceneManager.LoadScene("progressScene");
        else
        {
            id.text = "";
            password.text = "";
        }
    }
}
