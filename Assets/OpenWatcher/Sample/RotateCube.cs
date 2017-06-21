using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [Watch] public float speed;
    [Watch] public bool stop;
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) speed += 0.1f;
        else if (Input.GetKey(KeyCode.W)) speed -= 0.1f;

        if (!stop)
            transform.Rotate(new Vector3(speed, speed, speed));
    }
}
