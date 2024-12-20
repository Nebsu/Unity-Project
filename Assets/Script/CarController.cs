using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nécessaire pour accéder à TextMeshPro et TextMeshProUGUI


public class CarController : MonoBehaviour {

    // Settings
    //public TextMeshProUGUI SpeedText; // Pour les textes UI (dans un Canvas)
    public float MoveSpeed = 20;
    public float MaxSpeed = 50;
    public float Drag = 0.5f;
    public float SteerAngle = 10;
    public float Traction = 1;

    // Variables
    private Vector3 MoveForce;
    private float SteerAngleBreak = 15;

    // Update is called once per frame
    void Update() {

        // Print vitesse
        //SpeedText.text=(GetComponent<Rigidbody>().velocity.magnitude *2.23693629f).ToString("0")+("m/h");
        
        // Moving
        MoveForce += transform.forward * MoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;

        // Steering
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

        // Drag and max speed limit
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // Traction
        Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;

        if (Input.GetKey(KeyCode.Space))
        {
            SteerAngle = SteerAngleBreak;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SteerAngle = 10;
        }
    }
}