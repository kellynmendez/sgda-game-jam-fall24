using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotateForce = new Vector3(0, 100, 0);

    void Update()
    {
        // rotate this object each frame
        this.transform.Rotate(rotateForce * Time.deltaTime);
    }
}