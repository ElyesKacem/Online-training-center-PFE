using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{
    public GameObject BigCanvas;
    public float mouvementSpeed = 20f;
    public GameObject parentListOfCourses;
    public GameObject cardPrefab;
    private GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.Find("Menu Desktop");
        StartCoroutine(GetRequest("https://elearning.avaxia-dev.avaxia-group.com/api/courses?search=&take=11&page=1"));

    }

    // Update is called once per frame
    
    public void closeToPlayer()
    {
        BigCanvas.transform.Translate(-Vector3.forward *mouvementSpeed/100);
    }

    public void farFromPlayer()
    {
        BigCanvas.transform.Translate(Vector3.forward  * mouvementSpeed / 100);
    }

    public void rotateRight()
    {
        BigCanvas.transform.RotateAround(spawn.transform.position,Vector3.up,mouvementSpeed/5);
    } 
    public void rotateLeft()
    {
        BigCanvas.transform.RotateAround(spawn.transform.position,Vector3.down,mouvementSpeed/5);
    }

    public void setCourse(GameObject o,string title, string image)
    {
        StartCoroutine(GetImage(image,o,title));

    }

    IEnumerator GetImage(string imageUrl,GameObject card,string title)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://elearning.avaxia-dev.avaxia-group.com/api/courses/images/"+imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request) as Texture2D;
            card.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0, 0, downloadedTexture.width, downloadedTexture.height), new Vector2(0, 0));

        }
            card.transform.GetChild(3).GetComponent<TMP_Text>().text = title;
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
                //Debug.Log(webRequest.downloadHandler.text);
                var info = JObject.Parse(webRequest.downloadHandler.text);
                
                

                // Access the "data" array
                JArray dataArray = (JArray)info["data"];

                // Iterate over each item in the array
                foreach (JObject item in dataArray)
                {
                    
                    GameObject childGameObject = Instantiate(cardPrefab);
                    childGameObject.transform.SetParent(parentListOfCourses.transform, false);
                    setCourse(childGameObject, item["title"].ToString(), item["imagePath"].ToString());
                    if (item["hasUpcomingTraining"].ToString() != "True")
                    {
                        childGameObject.transform.GetChild(4).gameObject.SetActive(false);
                    }



                }


            }
        }
    }

}
