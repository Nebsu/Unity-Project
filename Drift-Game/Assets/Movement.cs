using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            body.MovePosition(body.position + Vector3.forward * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.MovePosition(body.position + Vector3.back * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.MovePosition(body.position + Vector3.left * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            body.MovePosition(body.position + Vector3.right * speed); 
        }
    }
}
