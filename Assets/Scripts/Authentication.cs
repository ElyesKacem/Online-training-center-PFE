using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime;
using Fusion;

public class Authentication : MonoBehaviour
{
    //public Button apiButton;
    public SocketIOUnity socket;
    public string serverUrlLink;
    public GameObject Windows, android;
    void Start()
    {
       //Debug.Log(Application.platform);
       // if(Application.platform == RuntimePlatform.WindowsEditor)
       // {
       //     Windows.SetActive(true);
       // }
       // else
       // {
       //     android.SetActive(true);
       // }
       //Debug.Log(Application.absoluteURL);
    }
    void OnDestroy()
    {
        socket.Dispose();
    }
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
                var formLink = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=" + clientID + "&response_type=code&redirect_uri=" + serverUrlLink + "/getcode&response_mode=query&scope=.default&state=" + roomID;
                Application.OpenURL(formLink);
                Debug.Log("connected to this link : " + formLink);
                var uri = new Uri(serverUrlLink);
                socket = new SocketIOUnity(uri);
                Debug.Log("hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");

                //here
                socket.OnConnected += (sender, e) =>
                {
                    Debug.Log("Connected to the socket of url : " + url);
                    //socket.Emit("Conntected");
                };


                //socket.Emit("hello","world");
                socket.On(webRequest.downloadHandler.text, (response) =>
                {

                    //Debug.Log(getToken(response.ToString()));
                    string data = getToken(response.ToString());
                    string accessToken = data.Split(";")[0];
                    string refreshToken = data.Split(';')[1];
                    //Debug.Log(data);
                    

                    var parts = accessToken.Split('.');
                    if (parts.Length > 2)
                    {
                        var decode = parts[1];
                        var padLength = 4 - decode.Length % 4;
                        if (padLength < 4)
                        {
                            decode += new string('=', padLength);
                        }
                        var bytes = System.Convert.FromBase64String(decode);
                        var userInfo = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        Debug.Log(userInfo);
                        JObject jsonUser = JObject.Parse(userInfo);
                        PlayerPrefs.SetString("email", jsonUser["upn"].ToString());
                        PlayerPrefs.SetString("name", jsonUser["given_name"].ToString());
                        PlayerPrefs.SetString("surname", jsonUser["family_name"].ToString());
                        PlayerPrefs.SetString("access_token", accessToken);
                        
                        
                    }
                    ;
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
       StartCoroutine( GetRequest(serverUrlLink+"/requestid"));
    }
}
