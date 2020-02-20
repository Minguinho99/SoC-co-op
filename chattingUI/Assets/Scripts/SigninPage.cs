using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SigninPage : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;
    public InputField passwordField;
    public InputField passwordCheckField;

    public Button signinBtn;
    public Text wrongText;

    [System.Serializable]
	public class UserData
	{
		public string state;
		public int userid;
	}
    // public string serverUrl = "http://localhost:8080";
    // Socket socket;


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
            StartCoroutine(SigninCo());
        }
    }

    IEnumerator SigninCo() {
        WWWForm form = new WWWForm();
        form.AddField("username", idField.text);
        form.AddField("nickname", nameField.text);
        form.AddField("password", passwordField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/signin", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
			Debug.Log(www.error);
            Debug.Log("Error");
		}
		else {
			Debug.Log("text : " + www.downloadHandler.text);
			UserData data = JsonUtility.FromJson<UserData> (www.downloadHandler.text);
			
            if (data.state == "Success") {
                Debug.Log("User created successfully.");
                UnityEngine.SceneManagement.SceneManager.LoadScene("SuccessSigninScene");
            }
            else {
                Debug.Log("User Creation failed.");
                wrongText.GetComponent<Text>().text = "Name already exists";
            }
		}
        
    }

    public void VerifyInputs() {
        signinBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
