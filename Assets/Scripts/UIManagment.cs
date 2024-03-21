using Newtonsoft.Json.Linq;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{
    public GameObject currentTab;
    public GameObject parentListOfCourses;
    public GameObject cardPrefab;
    public GameObject railway;
    public float animationDuration;
    //public LeanTweenType type;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(currentTab.transform.localPosition.);
        StartCoroutine(GetRequest("https://elearning.avaxia-dev.avaxia-group.com/api/courses?search=&take=11&page=1"));

    }

    public void switchingTab(GameObject newTab)
    {
        
        if(newTab != currentTab && !LeanTween.isTweening())
        {
            newTab.SetActive(true);
            float difference =  newTab.transform.localPosition.x- currentTab.transform.localPosition.x;
            LeanTween.moveX(railway, railway.transform.position.x - difference/100, animationDuration).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
            {
                Debug.Log("Animation completed!");
                currentTab.SetActive(false);
                currentTab =newTab;                
                
            });
            

        }
    }
    // Update is called once per frame
    
    //public void closeToPlayer()
    //{
    //    BigCanvas.transform.Translate(-Vector3.forward *mouvementSpeed/100);
    //}

    //public void farFromPlayer()
    //{
    //    BigCanvas.transform.Translate(Vector3.forward  * mouvementSpeed / 100);
    //}



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
