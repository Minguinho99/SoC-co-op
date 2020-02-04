using UnityEngine;
using socket.io;
using Newtonsoft.Json;


namespace Sample
{

    public class join_user
    {
        public int cmd;
        public string message;
    }

    public class emit_join_user
    {
        public int cmd;
        public string userName;
        public emit_join_user(int cmd, string userName)
        {
            this.cmd = cmd;
            this.userName = userName;
        }
    }

    public class other_user_join
    {
        public int cmd;
        public string userName;
        public other_user_join(int cmd, string userName)
        {
            this.cmd = cmd;
            this.userName = userName;
        }
    }


    /// <summary>
    /// The basic sample to show how to send and receive messages.
    /// </summary>
    public class Events : MonoBehaviour
    {

        public string userName;

        void Start()
        {
            var serverUrl = "http://localhost:3000";
            var socket = Socket.Connect(serverUrl);

            socket.On("join_user", (string data) => {
                var a = JsonConvert.DeserializeObject<join_user>(data);
                Debug.LogFormat("{0}, {1}", a.cmd, a.message);

                socket.Emit("emit_join_user", JsonConvert.SerializeObject(new emit_join_user(201, userName)));

            });

            socket.On("other_user_join", (string data) => {
                var a = JsonConvert.DeserializeObject<other_user_join>(data);
                Debug.LogFormat("{0}, {1}", a.cmd, a.userName);
            });

            // receive "news" event
            socket.On("news", (string data) => {
                Debug.Log(data);

                // Emit raw string data
                socket.Emit("my other event", "{ my: data }");

                // Emit json-formatted string data
                socket.EmitJson("my other event", @"{ ""my"": ""data"" }");
            });
        }

    }

}