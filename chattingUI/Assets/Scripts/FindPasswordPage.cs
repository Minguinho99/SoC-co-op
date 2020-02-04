using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FindPasswordPage : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;

    public Text passwordText;

    public void SigninBtn() {
        SceneManager.LoadScene("SigninScene");
    }

    public void FindPasswordBtn() {
        GetComponent<Animator>().SetTrigger("findPassword");
        StartCoroutine(SigninCo());
    }

    IEnumerator SigninCo() {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nameField.text);
        form.AddField("username", idField.text);
        WWW www = new WWW("http://localhost/sqlconnect/findpassword.php", form);
        yield return www;
        if (www.text != "0") {
            Debug.Log("User created successfully.");
            passwordText.GetComponent<Text>().text= www.text;
            Debug.Log(www.text);
            GameObject.Find("Canvas/FindAfter/title/Password").SetActive(true);
            GameObject.Find("Canvas/FindAfter/title/wrongText").SetActive(false);
            
        }
        else {
            Debug.Log("User creation failed. Err " + www.text);
            GameObject.Find("Canvas/FindAfter/title/Password").SetActive(false);
            GameObject.Find("Canvas/FindAfter/title/wrongText").SetActive(true);
        }
        
    }

    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");
    }
}
