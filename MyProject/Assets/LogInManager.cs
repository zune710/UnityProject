using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


[System.Serializable]
public class MemberForm
{
    public int index;
    public string id;
    public string password;
    public string name;
    public int age;
    public int gender;

    public MemberForm(int index, string id, string password, string name, int age, int gender)
    {
        this.index = index;
        this.id = id;
        this.password = password;
        this.name = name;
        this.age = age;
        this.gender = gender;
    }
}


public class LogInManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";


    public InputField ID;
    public InputField Password;
    private string id;
    private string password;

    private bool login;

    bool SetIdPassword()
    {
        id = ID.text;
        password = Password.text;

        if (id == "" || password == "")
            return false;
        else
            return true;
    }

    public void SignUp()
    {
        if (!SetIdPassword())
        {
            Debug.Log("비어있음");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "sign up");
        form.AddField("id", id);
        form.AddField("password", password);

        //form.AddField("userName", userName);
        //form.AddField("age", age);

        StartCoroutine(Post(form));
    }

    public void LogIn()
    {
        if (!SetIdPassword())
        {
            Debug.Log("비어있음");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "sign in");
        form.AddField("id", id);
        form.AddField("password", password);

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {
            yield return request.SendWebRequest();

            if (request.isDone)
                Debug.Log(request.downloadHandler.text);
            else
                Debug.Log("응답없음");
        }
    }
}
