using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


[System.Serializable]
public class MemberForm
{
    public string id;
    public string password;
    public string userName;
    public int age;
    public int gender;

    public MemberForm(string id, string password, string userName, int age, int gender)
    {
        this.id = id;
        this.password = password;
        this.userName = userName;
        this.age = age;
        this.gender = gender;
    }
}

[System.Serializable]
public class InfoForm
{
    public string order;
    public string date;
    public string message;

    public InfoForm(string order, string date, string message)
    {
        this.order = order;
        this.date = date;
        this.message = message;
    }
}


public class LogInManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";


    private EventSystem system;

    public GameObject SignUpFrame;
    public InputField ID;
    public InputField Password;
    public InputField NewID;
    public InputField NewPassword;
    public InputField UserName;
    public InputField Age;
    public Toggle GenderM;
    public Toggle GenderF;
    public Text Notice;

    private string id;
    private string password;
    private string userName;
    private string age;
    private string gender;

    private bool isChanged;


    private void Start()
    {
        system = EventSystem.current;
    }

    private void Update()
    {
        // InputField, Toggle 선택하면 알림 off
        if (ID.isFocused || Password.isFocused || NewID.isFocused ||
            NewPassword.isFocused || UserName.isFocused || Age.isFocused || isChanged)
        {
            Notice.enabled = false;
            isChanged = false;
        }

        // Tab키로 이동
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (system.currentSelectedGameObject != null)
            {
                Selectable next;

                // 오른쪽으로 이동해야 하는 경우
                if (system.currentSelectedGameObject.transform.name == "PasswordInput" ||
                    system.currentSelectedGameObject.transform.name == "Gender(M)")
                    next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
                // 아래쪽으로 이동해야 하는 경우
                else
                    next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();


                if (next != null)
                {
                    InputField inputfield = next.GetComponent<InputField>();

                    if (inputfield != null)
                        inputfield.OnPointerClick(new PointerEventData(system));

                    system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
                }
            }
        }
    }

    bool SetLogInInfo()
    {
        id = ID.text;
        password = Password.text;

        if (id == "" || password == "")
            return false;
        else
            return true;
    }

    bool SetSignUpInfo()
    {
        id = NewID.text;
        password = NewPassword.text;

        userName = UserName.text;
        age = Age.text;
        gender = GenderM.isOn ? "1" : "2";

        if (id == "" || password == "" || name == "" ||
            age == "" || (!GenderM.isOn && !GenderF.isOn))
            return false;
        else
            return true;
    }

    public void SignUp()
    {
        if (!SetSignUpInfo())
        {
            Debug.Log("비어있음");
            Notice.text = "모두 입력하세요.";
            Notice.enabled = true;
            return;
        }

        //nameof
        WWWForm form = new WWWForm();
        form.AddField("order", "sign up");
        form.AddField("id", id);
        form.AddField("password", password);
        form.AddField("userName", userName);
        form.AddField("age", age);
        form.AddField("gender", gender);

        StartCoroutine(Post(form));
    }

    public void LogIn()
    {
        if (!SetLogInInfo())
        {
            Debug.Log("비어있음");
            Notice.text = "모두 입력하세요.";
            Notice.enabled = true;
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
            {
                Debug.Log(request.downloadHandler.text);
                InfoForm info = JsonUtility.FromJson<InfoForm>(request.downloadHandler.text);

                if (info.message == "로그인 완료")
                    NextScene();
                else if (info.message == "로그인 실패")
                {
                    Notice.text = "아이디 또는 비밀번호를 잘못 입력했습니다.";
                    Notice.enabled = true;
                }
                else if (info.message == "회원가입 완료")
                    SetActiveSignUpFrame();
                else if (info.message == "이미 존재하는 id입니다.")
                {
                    Notice.text = "이미 존재하는 ID입니다.";
                    Notice.enabled = true;
                }
            }
            else
                Debug.Log("응답없음");
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("progressScene");
    }

    public void SetActiveSignUpFrame()
    {
        Notice.enabled = false;
        SignUpFrame.SetActive(!SignUpFrame.activeSelf);
    }

    public void SelectMale()
    {
        isChanged = true;

        if (GenderM.isOn)
            GenderF.isOn = false;
    }

    public void SelectFemale()
    {
        isChanged = true;

        if (GenderF.isOn)
            GenderM.isOn = false;
    }
}
