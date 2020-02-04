using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using socket.io;

public class ChattingPage : MonoBehaviour {

    public GameObject contents;

    public InputField chatInputField;

    public InputField userNameField;

    public string serverUrl = "http://localhost:8080";
    Socket socket;
    

    private void Awake() {
        if (DBManager.username == null) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
        userNameField.text = DBManager.username;
    }

    void Start() {
        if (DBManager.LoggedIn) {
            Debug.Log("Done!");
        }

        socket = Socket.Connect(serverUrl);

        socket.On("new", (string data) => {
            socket.Emit("userName", DBManager.username);
        });
    }

    void Update() {
        socket.On("disUser", (string data) => {
            OutUser(data);
        });
        socket.On("newUser", (string name) => {
            NewUser(name);
        });
        socket.On("Rmessage", (string data) => {
            Debug.Log("data : "+ data);
            var dataR = data.Substring(1, data.Length - 1);
            var splitText = dataR.Split('|');
            Debug.Log("splite : " + splitText[1]);
            if (splitText[0] != DBManager.username) {
                string Msg = splitText[0] + " : " + splitText[1];
                ReceiveMsg(Msg);
            }
        });
    }

    public void OutUser(string data) {
        GameObject prefabOutUser = Resources.Load ("outUser") as GameObject;
        GameObject outUser = MonoBehaviour.Instantiate (prefabOutUser) as GameObject;

        outUser.name = "outUser";
        outUser.GetComponent<Text>().text = data + "님이 나가셨습니다.";
        outUser.transform.parent = contents.transform;
        outUser.transform.localScale = Vector3.one;
    }

    public void NewUser(string name) {
        GameObject prefabEnterUser = Resources.Load ("enterUser") as GameObject;
        GameObject enterUser = MonoBehaviour.Instantiate (prefabEnterUser) as GameObject;

        enterUser.name = "outUser";
        enterUser.GetComponent<Text>().text = name + "님이 들어오셨습니다.";
        enterUser.transform.parent = contents.transform;
        enterUser.transform.localScale = Vector3.one;
    }

    public void HomeBtn() {
        DBManager.LogOut();
        SceneManager.LoadScene("MainScene");
        Debug.Log(DBManager.username);
        socket.On("new", (string data) => {
            socket.Emit("disconnect", DBManager.username);
        });
    }

    public void SendBtn() {
        PlayerPrefs.SetString("chatText", chatInputField.text);
        Debug.Log(PlayerPrefs.GetString("chatText", "null"));
        chatInputField.text = "";

        GameObject prefabMyText = Resources.Load ("talkMe") as GameObject;
        GameObject myText = MonoBehaviour.Instantiate (prefabMyText) as GameObject;

        myText.name = "myText";
        myText.GetComponent<Text>().text = PlayerPrefs.GetString("chatText", "null");
        myText.transform.SetParent(contents.transform);
        myText.transform.localScale = Vector3.one;

        // socket.On("new", (string data) => {
        //     socket.Emit("message", PlayerPrefs.GetString("chatText", "null"));
        // });
        socket.Emit("message", PlayerPrefs.GetString("chatText", "null"));
        Debug.Log(PlayerPrefs.GetString("chatText", "null"));
    }
    
    public void ReceiveMsg(string data) {
        GameObject prefabOtherText = Resources.Load ("talkOther") as GameObject;
        GameObject otherText = MonoBehaviour.Instantiate (prefabOtherText) as GameObject;

        otherText.name = "otherText";
        otherText.GetComponent<Text>().text = data;
        otherText.transform.SetParent(contents.transform);
        otherText.transform.localScale = Vector3.one;
    }

    public void SaveBtn() {
        StartCoroutine(SaveUserData());
    }

    IEnumerator SaveUserData() {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("nickname", DBManager.nickname);

        WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        yield return www;
        if (www.text == "0") {
            Debug.Log("Game Saved.");
        }
        else {
            Debug.Log("Save failed. Error #" + www.text);
        }
    }
    
}
