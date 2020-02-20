using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using socket.io;

public class FindPasswordPage : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;

    public Text passwordText;

    public string serverUrl = "http://localhost:8080";
    Socket socket;

    public void SigninBtn() {
        SceneManager.LoadScene("SigninScene");
    }

    public void FindPasswordBtn() {
        socket = Socket.Connect(serverUrl);

        string passwordInfo = idField.text + "|" + nameField.text;

        socket.On("findPassword", (string data) => {
            socket.Emit("passwordCheck", passwordInfo);
        });

        GetComponent<Animator>().SetTrigger("findPassword");

        socket.On("passwordWrongInfo", (string data) => {
            GameObject.Find("Canvas/FinadAfter").SetActive(true);
            GameObject.Find("Canvas/FindAfter/title/Password").SetActive(false);
            GameObject.Find("Canvas/FindAfter/title/wrongText").SetActive(true);
            // socket.Emit("disconnect", "");
        });

        socket.On("successFindPassword", (string data) => {
            Debug.Log("User created successfully.");
            passwordText.GetComponent<Text>().text= data;
            Debug.Log(data);
            GameObject.Find("Canvas/FindAfter/title/Password").SetActive(true);
            GameObject.Find("Canvas/FindAfter/title/wrongText").SetActive(false);
            // socket.Emit("disconnect", "");
        });
    }

    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");
    }
}
