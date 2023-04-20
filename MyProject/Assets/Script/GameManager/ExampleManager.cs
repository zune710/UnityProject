using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Security.Cryptography;


[System.Serializable]
public class MemberForm
{
    public int index;
    public string id;
    public string password;
    public int gender;

    public MemberForm(int index, string id, string password, int gender)
    {
        this.index = index;
        this.id = id;
        this.password = password;
        this.gender = gender;
    }
}
// 회원가입
// 로그인

public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";


    IEnumerator Start()
    {
        // ** 요청을 하기 위한 작업
        // Get 방식
        //UnityWebRequest request = UnityWebRequest.Get(URL);

        /*
        // Post 방식
        MemberForm member = new MemberForm("변사또", 45);

        WWWForm form = new WWWForm();
        form.AddField("Name", member.Name);
        form.AddField("Age", member.Age);

        // request 사용 후 해제되도록 using 사용(안전한 사용을 위한 것)
        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
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
            print(json.id);
            print(json.password);
            print(json.gender);
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("progressScene");
    }

    IEnumerator Request(int index, string id, string password, int gender)
    {
        //MemberForm form = new MemberForm(index, id, (password), gender);

        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            MemberForm json = JsonUtility.FromJson<MemberForm>(request.downloadHandler.text);

            // ** 응답에 대한 작업
            
        }


    }
}
