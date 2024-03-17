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

public class CoursesManagment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FetchDataFromAPI()
    {
        // Create a UnityWebRequest object to send GET request to the API
        UnityWebRequest request = UnityWebRequest.Get("https://elearning.avaxia-dev.avaxia-group.com/api/courses?search=&take=11&page=1");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check if there was an error with the request
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
        else
        {
            // Data retrieval successful, parse and process the response
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Received data: " + jsonResponse);

            // Here you can parse the JSON response and handle the data as needed
        }
    }
}
