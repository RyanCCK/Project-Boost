using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMan : MonoBehaviour
{
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidBody.AddRelativeForce(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidBody.AddRelativeForce(Vector3.left);
        }
    }
}
