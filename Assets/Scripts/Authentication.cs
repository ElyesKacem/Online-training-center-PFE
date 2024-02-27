using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine.Networking;

public class Authentication : MonoBehaviour
{
    //public Button apiButton;
    public SocketIOUnity socket;
public string serverUrlLink = "http://localhost:3000";
    void Start()
    {
       Debug.Log(Application.platform);
       Debug.Log(Application.absoluteURL);
    }
    //void OnDestroy()
    //{
    //    socket.Dispose();
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            socket.EmitAsync("message", "Hello, server!"); // replace with your message
        }
    }
     IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Process the result
                Debug.Log(webRequest.downloadHandler.text);
                string clientID = "e3e26337-c4c5-4137-8aa8-9671eec42fb6";
                string roomID = webRequest.downloadHandler.text;
                string state = clientID+";"+roomID;


                Application.OpenURL("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id="+clientID+"&response_type=code&redirect_uri=http://localhost:3000/getcode&response_mode=query&scope=.default&state="+ state);
                var uri = new Uri(serverUrlLink);
                socket = new SocketIOUnity(uri);

                //here
                socket.OnConnected += (sender, e) =>
                {
                    Debug.Log("Connected to the socket of url : " + url);
                    //socket.Emit("Conntected");
                };


                //socket.Emit("hello","world");
                socket.On(webRequest.downloadHandler.text, (response) =>
                {
                    
                    Debug.Log(getToken(response.ToString()));
                    //here
                   

                    socket.Disconnect();
                });
                //socket.Emit("message", "worlddddd");


                socket.Connect();
            }
        }
    }
    private string getToken(string token)
    {
        return token.Substring(2,token.Length - 4);
    }
    public void getRequest() {

       //StartCoroutine( GetRequest("https://www.boredapi.com/api/activity"));
       StartCoroutine( GetRequest("http://localhost:3000/requestid"));
    }
}
