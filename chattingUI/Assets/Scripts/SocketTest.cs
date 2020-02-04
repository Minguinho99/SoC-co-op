using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class SocketTest : MonoBehaviour {
    string url = "http://127.0.0.1:8080/";
    public static Client Socket { get; private set; }

    void Awake() {
        Socket = new Client(url);
        Socket.Opened += SocketOpened;
        Socket.Connect();

    }

    private void SocketOpened(object sender, System.EventArgs e) {
        Debug.Log("Socket Opend");
    }

    void OnDisable() {
        Socket.Close();
    }

    // void Start() {
    //     SocketManager.Socket.On("MsgRes", (data) => {
    //         Debug.Log(data.Json.args[0]);
    //     });
    // }

//    void OnGUI() {
//        if (GUI.Button(new Rect(10, 10, 150, 100), "SEND")) {
//            SocketManager.Socket.Emit("Msg", "Hello, World!");
//        }
//    }
}
