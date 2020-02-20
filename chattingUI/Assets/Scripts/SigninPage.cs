using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using socket.io;

public class SigninPage : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;
    public InputField passwordField;
    public InputField passwordCheckField;

    public Button signinBtn;
    public Text wrongText;


    public string serverUrl = "http://localhost:8080";
    Socket socket;


    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");

        // socket.On("signin", (string data) => {
        //     socket.Emit("disconnect", "");
        // });
    }
    public void SigninBtn() {
        if (nameField.text == "") {
            wrongText.GetComponent<Text>().text= "Please input your name field";
        }
        else if (idField.text == "") {
            wrongText.GetComponent<Text>().text= "Please input your id field";
        }
        else if (passwordField.text == "") {
            wrongText.GetComponent<Text>().text= "Please input your password field";
        }
        else if (passwordField.text != passwordCheckField.text) {
            wrongText.GetComponent<Text>().text= "Password does not same";
        }
        else {
            socket = Socket.Connect(serverUrl);
            
            string sendStr = nameField.text + "|" + idField.text + "|" + passwordField.text;

            socket.On("signin", (string data) => {
                socket.Emit("idCheck", sendStr);
            });

            socket.On("signinSuc", (string data) => {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SuccessSigninScene");
                // socket.Emit("disconnect", "");
            });

            socket.On("idExist", (string data) => {
                wrongText.GetComponent<Text>().text= "Name already exists";
            });
        }
        // StartCoroutine(SigninCo());
    }

    public void VerifyInputs() {
        signinBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
