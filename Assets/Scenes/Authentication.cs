using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Authentication : MonoBehaviour
{
    public Button apiButton;
    void Start()
    {
        //apiButton.onClick.AddListener(getRequest);
       
    }

     IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Process the result
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }
    public void getRequest() {

       StartCoroutine( GetRequest("https://www.boredapi.com/api/activity"));
    }
}
