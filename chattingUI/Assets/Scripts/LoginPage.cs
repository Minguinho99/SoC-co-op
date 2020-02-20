using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using socket.io;

public class LoginPage : MonoBehaviour {

    public InputField idField;
    public InputField passwordField;
    public Text wrongText;

    public string serverUrl = "http://localhost:8080";
    Socket socket;

    public void SigninBtn() {
        SceneManager.LoadScene("SigninScene");

        // socket.On("login", (string data) => {
        //     socket.Emit("disconnect", "");
        // });
    }

    public void FindPasswordBtn() {
        SceneManager.LoadScene("FindPasswordScene");

        // socket.On("login", (string data) => {
        //     socket.Emit("disconnect", "");
        // });
    }

    public void LoginBtn() {
        socket = Socket.Connect(serverUrl);

        string loginInfo = idField.text + "|" + passwordField.text;

        socket.On("login", (string data) => {
            socket.Emit("loginCheck", loginInfo);
        });

        socket.On("loginIdWrong", (string data) => {
            wrongText.GetComponent<Text>().text= "There is no such ID";
        });

        socket.On("loginPasswordWrong", (string data) => {
            wrongText.GetComponent<Text>().text= "Password is wrong";
        });

        socket.On("loginSuccess", (string data) => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ChattingScene");
            DBManager.username = idField.text;
            DBManager.nickname = data;
            // socket.Emit("disconnect", "");
        });
        // StartCoroutine(LoginCo());
    }

    // IEnumerator LoginCo() {
    //     WWWForm form = new WWWForm();
    //     form.AddField("name", idField.text);
    //     form.AddField("password", passwordField.text);
    //     Debug.Log(idField.text);
    //     Debug.Log(passwordField.text);
    //     WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
    //     yield return www;
    //     if (www.text[0] == '0') {
    //         DBManager.username = idField.text;
    //         DBManager.nickname = www.text.Split('\t')[1];
    //         UnityEngine.SceneManagement.SceneManager.LoadScene("ChattingScene");
    //     }
    //     else {
    //         Debug.Log("User login failed. Error #" + www.text);
    //     }
        
    // }

    // public void VerifyInputs() {
    //     loginBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    // }
}
