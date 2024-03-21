using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Vector3 f;
    public LeanTweenType type;
    private void Start()
    {

        f = transform.position;
    }
    public void test()
    {
        transform.position = f;
        LeanTween.moveX(this.gameObject, 5, 2).setEase(LeanTweenType.easeInOutExpo);
    }
    private void Update()
    {
        //LeanTween.moveX(this.gameObject, x, 2);
        //x++;

    }
}
