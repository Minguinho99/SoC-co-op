﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPage : MonoBehaviour {

    public void SigninBtn() {
        SceneManager.LoadScene("SigninScene");
    }

    public void LoginBtn() {
        SceneManager.LoadScene("LoginScene");
    }
}
