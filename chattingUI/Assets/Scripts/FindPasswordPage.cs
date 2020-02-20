using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FindPasswordPage : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;

    public Text passwordText;

    public class UserData
	{
		public string state;
        public bool success;
        public string password;
	}

    public void SigninBtn() {
        SceneManager.LoadScene("SigninScene");
    }

    public void FindPasswordBtn() {
        StartCoroutine(PasswordCo());
        GetComponent<Animator>().SetTrigger("findPassword");
    }

    IEnumerator PasswordCo() {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nameField.text);
        form.AddField("username", idField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/password", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
			Debug.Log(www.error);
            Debug.Log("Error");
		}
		else {
			Debug.Log("text : " + www.downloadHandler.text);
			UserData data = JsonUtility.FromJson<UserData> (www.downloadHandler.text);

            if (data.success) {
                Debug.Log("User created successfully.");
                passwordText.GetComponent<Text>().text= data.password;
                Debug.Log(data.password);
                GameObject.Find("Canvas/FindAfter/title/Password").SetActive(true);
                GameObject.Find("Canvas/FindAfter/title/wrongText").SetActive(false);
            }
            else {
                GameObject.Find("Canvas/FinadAfter").SetActive(true);
                GameObject.Find("Canvas/FindAfter/title/Password").SetActive(false);
                GameObject.Find("Canvas/FindAfter/title/wrongText").SetActive(true);
            }

        }        
    }

    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");
    }
}
