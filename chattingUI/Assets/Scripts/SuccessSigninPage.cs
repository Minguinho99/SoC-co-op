using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessSigninPage : MonoBehaviour
{
    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");
    }
}
