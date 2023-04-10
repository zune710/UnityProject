using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class MemberForm
{
    public string Name;
    public int Age;

    public MemberForm(string name, int age)
    {
        this.Name = name;
        this.Age = age;
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
        //UnityWebRequest request = UnityWebRequest.Get(URL);

        MemberForm member = new MemberForm("변사또", 45);

        WWWForm form = new WWWForm();

        form.AddField("Name", member.Name);
        form.AddField("Age", member.Age);

        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))  // 안전한 사용을 위해 request 사용 후 해제
        {
            // 요청을 주고받는 작업
            yield return request.SendWebRequest();


            // ** 응답에 대한 작업
            print(request.downloadHandler.text);
        }
    }
}
