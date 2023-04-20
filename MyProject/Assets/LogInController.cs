using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;  // Regex 함수

public class LogInController : MonoBehaviour
{
    public InputField emailInputField;
    private string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    public InputField passwordInputField;
    public Text message;

    //string URL = "https://script.google.com/macros/s/AKfycbxn_T-N-5X8mkNQK4UhKlC0zHl3ENXuuCtnAXShkm_Z1Iqd7NhOQzPI-qe47WAjUW9ZwA/exec";

    void Start()
    {
        message.text = "";
    }

    public void LoginCheck()
    {
        string email = emailInputField.text;

        if(Regex.IsMatch(email, emailPattern))
        {
            // ** true
            string password = Security(passwordInputField.text);

            print(password);

            // ** login (1. 쓰레드 사용 / 2.싱글톤? Manager는 다 singlton 이어야 함)
            
        }
        else
        {
            // ** false
            message.text = "email 형식을 다시 확인하세요.";
        }
    }

    string Security(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            // ** true
            message.text = "password는 필수 입력값입니다.";
            return "null";
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
}
