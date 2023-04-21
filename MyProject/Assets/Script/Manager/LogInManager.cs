using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


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
    private LogInManager() { }

    private static LogInManager instance = null;

    public static LogInManager GetInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }


    string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";

    private EventSystem eventSystem;

    public GameObject SignUpFrame;
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

    private string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    private string id;
    private string password;
    private string userName;
    private string age;
    private string gender;

    private bool isChanged;


    private void Awake()
    {
        if (instance == null)
            instance = this;

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

    private bool SetLogInData()  // LoginCheck
    {
        id = ID.text;

        if (Regex.IsMatch(id, emailPattern))
        {
            // ** true
            password = Security(Password.text);
            
            if (password == null)
                return false;
            else
                return true;
        }
        else
        {
            // ** false
            Notice.text = "email 형식을 다시 확인하세요.";
            Notice.enabled = true;
            return false;
        }
    }
    
    private bool SetSignUpData()
    {
        id = NewID.text;

        if (Regex.IsMatch(id, emailPattern))
        {
            password = Security(NewPassword.text);

            userName = UserName.text;
            age = Age.text;
            gender = GenderM.isOn ? "1" : "2";

            if (password == null)
                return false;
            else if (name == "" || age == "" || (!GenderM.isOn && !GenderF.isOn))
            {
                Notice.text = "모두 입력하세요.";
                Notice.enabled = true;
                return false;
            }
            else
                return true;
        }
        else
        {
            Notice.text = "email 형식을 다시 확인하세요.";
            Notice.enabled = true;
            return false;
        }
    }

    string Security(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            // ** true
            Notice.text = "password는 필수 입력값입니다.";
            Notice.enabled = true;
            return null;
        }
        else
        {
            // ** false
            // ** 보안처리: 암호화 & 복호화
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            
            return stringBuilder.ToString();
        }
    }


    public void SignUp()
    {
        ButtonSFX.Play();

        Interactable(false);

        if (!SetSignUpData())
        {
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
        #region ...
        // request 사용 후 해제되도록 using 사용(안전한 사용을 위한 것)
        #endregion
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

        #region memo
        /** 요청을 하기 위한 작업 **/
        /* Get 방식 */
        //UnityWebRequest request = UnityWebRequest.Get(URL);

        /* Post 방식 */
        //UnityWebRequest request = UnityWebRequest.Get(URL, form);

        /** 요청을 주고받는 작업 **/
        //yield return request.SendWebRequest();

        /** 응답에 대한 작업 **/
        //print(request.downloadHandler.text);
        #endregion
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
        foreach (Selectable selectableUI in Selectable.allSelectablesArray)
        {
            selectableUI.interactable = value;
        }

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
