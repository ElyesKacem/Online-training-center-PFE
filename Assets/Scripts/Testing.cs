using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    Quaternion defaultRotation;

    // Start is called before the first frame update
    void Awake()
    {
        defaultRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Get the current rotation
        Quaternion currentRotation = transform.rotation;

        // Create a new rotation where only the Z axis is locked (frozen)
        Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, defaultRotation.eulerAngles.z);

        // Apply the new rotation
        transform.rotation = newRotation;
    }
}
