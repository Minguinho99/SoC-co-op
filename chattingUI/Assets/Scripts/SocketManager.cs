using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class SocketManager : MonoBehaviour {
    string url = "http://127.0.0.1:8080/";
    public static Client Socket { get; private set; }

    void Awake() {
        Socket = new Client(url);
        Socket.Opened += SocketOpened;
        Socket.Connect();
    }

    private void SocketOpened(object sender, System.EventArgs e) {
        Debug.Log("Socket Opened");
    }

    void OnDisable() {
        Socket.Close();
    }
}
