using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SigninPage : MonoBehaviour
{
    public InputField nameField;
    public InputField idField;
    public InputField passwordField;
    public InputField passwordCheckField;

    public Button signinBtn;
    public Text wrongText;

    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");
    }
    public void SigninBtn() {
        StartCoroutine(SigninCo());
    }

    IEnumerator SigninCo() {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nameField.text);
        form.AddField("username", idField.text);
        form.AddField("password", passwordField.text);
        form.AddField("passwordCheck", passwordCheckField.text);
        WWW www = new WWW("http://localhost/sqlconnect/signin.php", form);
        yield return www;
        if (www.text == "0") {
            Debug.Log("User created successfully.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("SuccessSigninScene");
        }
        else {
            Debug.Log("User creation failed. Err " + www.text);
            wrongText.GetComponent<Text>().text= www.text;
        }
        
    }

    public void VerifyInputs() {
        signinBtn.interactable = (idField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
