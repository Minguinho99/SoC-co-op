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
            });

            socket.On("idExist", (string data) => {
                wrongText.GetComponent<Text>().text= "Name already exists";
            });
        }
        // StartCoroutine(SigninCo());
    }

    // IEnumerator SigninCo() {
    //     //check value
    //     WWWForm form = new WWWForm();
    //     form.AddField("nickname", nameField.text);
    //     form.AddField("username", idField.text);
    //     form.AddField("password", passwordField.text);
        
    //     // UnityWebRequest www = UnityWebRequest.Post("http://localhost:3306/s", form);
    //     UnityWebRequest www = UnityWebRequest.Get("http://localhost:3306/signinCheck");
    //     yield return www.SendWebRequest();

    //     if(www.isNetworkError || www.isHttpError) {
    //         Debug.Log(www.error);
    //     }
    //     else {
    //         Debug.Log(www.downloadHandler.text);
    //         // UserData data = JsonUtility.FromJson<UserData> (www.downloadHandler.text);
    //         // Debug.Log("name:"+data.userid);
    //     }
    //     //end


    //         // WWW www = new WWW("http://localhost/sqlconnect/signin.php", form);
    //         // yield return www;

    //         // if (www.text == "0") {
    //         //     Debug.Log("User created successfully.");
    //         //     UnityEngine.SceneManagement.SceneManager.LoadScene("SuccessSigninScene");
    //         // }
    //         // else {
    //         //     Debug.Log("User creation failed. Err " + www.text);
    //         //     wrongText.GetComponent<Text>().text= www.text;
    //         // }

    //     // if(www.isNetworkError) {
    //     //     Debug.Log(www.error);
    //     // }
    //     // else {
    //     //     Debug.Log(www.downloadHandler.text);
    //     //     // PacketData data = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
            
    //     // }        
    // }

    public void VerifyInputs() {
        signinBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
