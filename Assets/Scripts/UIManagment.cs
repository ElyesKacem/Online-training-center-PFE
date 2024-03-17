using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagment : MonoBehaviour
{
    public GameObject BigCanvas;
    public float mouvementSpeed = 20f;
    private GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.Find("Menu Desktop");
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
        BigCanvas.transform.RotateAround(spawn.transform.position,Vector3.up,mouvementSpeed);
    } 
    public void rotateLeft()
    {
        BigCanvas.transform.RotateAround(spawn.transform.position,Vector3.down,mouvementSpeed);
    }


}
