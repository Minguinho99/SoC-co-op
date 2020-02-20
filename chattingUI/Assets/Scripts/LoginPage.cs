using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginPage : MonoBehaviour {

    public InputField idField;
    public InputField passwordField;
    public Text wrongText;

    [System.Serializable]
	public class UserData
	{
		public string state;
        public bool success;
	}

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

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/login", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
			Debug.Log(www.error);
            Debug.Log("Error");
		}
		else {
			Debug.Log("text : " + www.downloadHandler.text);
			UserData data = JsonUtility.FromJson<UserData> (www.downloadHandler.text);
			
            if (data.success) {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ChattingScene");
                DBManager.username = idField.text;
            }
            else {
                wrongText.GetComponent<Text>().text= data.state;
            }
		}        
    }
    // public void VerifyInputs() {
    //     loginBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    // }
}
