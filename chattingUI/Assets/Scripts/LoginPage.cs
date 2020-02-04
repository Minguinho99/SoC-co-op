using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPage : MonoBehaviour {

    public InputField idField;
    public InputField passwordField;

    public Button loginBtn;

    public void SigninBtn() {
        SceneManager.LoadScene("SigninScene");
    }

    public void FindPasswordBtn() {
        SceneManager.LoadScene("FindPasswordScene");
    }

    public void LoginBtn() {
        StartCoroutine(LoginCo());
    }

    IEnumerator LoginCo() {
        WWWForm form = new WWWForm();
        form.AddField("name", idField.text);
        form.AddField("password", passwordField.text);
        Debug.Log(idField.text);
        Debug.Log(passwordField.text);
        WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
        yield return www;
        if (www.text[0] == '0') {
            DBManager.username = idField.text;
            DBManager.nickname = www.text.Split('\t')[1];
            UnityEngine.SceneManagement.SceneManager.LoadScene("ChattingScene");
        }
        else {
            Debug.Log("User login failed. Error #" + www.text);
        }
        
    }

    // public void VerifyInputs() {
    //     loginBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    // }
}
