using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


[System.Serializable]
public class LogInDataForm
{
    public string order;
    public string date;
    public string message;
    public bool login;

    public LogInDataForm(string order, string date, string message, bool login)
    {
        this.order = order;
        this.date = date;
        this.message = message;
        this.login = login;
    }
}


public class LogInManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";


    private EventSystem eventSystem;

    public GameObject SignUpFrame;
    public Button LogInButton;
    public Button OnSignUpButton;
    public Button SignUpButton;
    public InputField ID;
    public InputField Password;
    public InputField NewID;
    public InputField NewPassword;
    public InputField UserName;
    public InputField Age;
    public Image GenderInput;
    public Toggle GenderM;
    public Toggle GenderF;
    public Text Notice;

    private AudioSource ButtonSFX;

    private string id;
    private string password;
    private string userName;
    private string age;
    private string gender;

    private bool isChanged;

    private void Awake()
    {
        ButtonSFX = GetComponent<AudioSource>();

    }

    private void Start()
    {
        eventSystem = EventSystem.current;
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

        // Tab키로 이동(Navigation 설정되어 있어야 작동)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                Selectable next;

                // 오른쪽으로 이동해야 하는 경우
                if (eventSystem.currentSelectedGameObject.transform.name == "PasswordInput" ||
                    eventSystem.currentSelectedGameObject.transform.name == "Gender(M)")
                    next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
                // 아래쪽으로 이동해야 하는 경우
                else
                    next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();


                if (next != null)
                {
                    InputField inputfield = next.GetComponent<InputField>();

                    if (inputfield != null)
                        inputfield.OnPointerClick(new PointerEventData(eventSystem));

                    eventSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(eventSystem));
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
            LogIn();
    }

    private bool SetLogInData()
    {
        id = ID.text;
        password = Password.text;

        if (id == "" || password == "")
            return false;
        else
            return true;
    }

    private bool SetSignUpData()
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
        ButtonSFX.Play();

        Interactable(false);

        if (!SetSignUpData())
        {
            Debug.Log("비어있음");
            Notice.text = "모두 입력하세요.";
            Notice.enabled = true;

            Interactable(true);
            return;
        }
        
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
        ButtonSFX.Play();

        Interactable(false);

        if (!SetLogInData())
        {
            Debug.Log("비어있음");
            Notice.text = "모두 입력하세요.";
            Notice.enabled = true;

            Interactable(true);
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
                LogInDataForm info = JsonUtility.FromJson<LogInDataForm>(request.downloadHandler.text);

                if (info.login)
                {
                    ControllerManager.GetInstance().PlayerId = id;
                    NextScene();
                }
                else
                {
                    if(info.message == "회원가입 완료")
                        SetActiveSignUpFrame();
                    else if (info.message == "중복ID")
                    {
                        Notice.text = "이미 존재하는 ID입니다.";
                        Notice.enabled = true;
                    }
                    else
                    {
                        Notice.text = "아이디 또는 비밀번호를 잘못 입력했습니다.";
                        Notice.enabled = true;
                    }
                }
            }
            else
                Debug.Log("응답없음");

            Interactable(true);
        }
    }

    private void NextScene()
    {
        SceneManager.LoadScene("progressScene");
    }

    public void SetActiveSignUpFrame()
    {
        ButtonSFX.Play();

        Notice.enabled = false;
        SignUpFrame.SetActive(!SignUpFrame.activeSelf);

        if(SignUpFrame.activeSelf)
            eventSystem.SetSelectedGameObject(NewID.gameObject);
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

    private void Interactable(bool value)
    {
        LogInButton.interactable = value;
        OnSignUpButton.interactable = value;
        SignUpButton.interactable = value;

        ID.interactable = value;
        Password.interactable = value;

        NewID.interactable = value;
        NewPassword.interactable = value;
        UserName.interactable = value;
        Age.interactable = value;
        GenderM.interactable = value;
        GenderF.interactable = value;

        GenderInput.color = value ? new Color(1.0f, 1.0f, 1.0f, 1.0f) 
            : new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f);
    }

    public void OnQuit()
    {
        ButtonSFX.Play();

        Application.Quit();
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}
