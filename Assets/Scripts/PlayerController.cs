using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Variables
    //[SerializeField] private float speed = 20.0f;
    [SerializeField] private float horsePower = 20000f;
    private const float turnSpeed = 25.0f;
    private float horizontalInput;
    private float forwardInput;
    public Camera mainCamera;
    public Camera driverCamera;
    public KeyCode switchKey;
    public string inputID;
    private Rigidbody playerRb;
    public GameObject centerOfMass;
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] float rpm;

    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] int wheelsOnGround;

    public void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }
    private void Update()
    {
        speed = Mathf.Round(playerRb.velocity.magnitude * 3.6f);
        speedometerText.SetText("Speed: " + speed + "km/h");

        rpm = (speed % 30) * 110;
        rpmText.SetText("RPM: " + rpm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This is where we get player imput
        horizontalInput = Input.GetAxis("Horizontal" + inputID);
        forwardInput = Input.GetAxis("Vertical" + inputID);

        if (IsOnGround())
        {


            //We move the vehicule forward
            playerRb.AddRelativeForce(Vector3.forward * horsePower * forwardInput);
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

            // Switch between 1st and 3rd person view
            if (Input.GetKeyDown(switchKey))
            {
                mainCamera.enabled = !mainCamera.enabled;
                driverCamera.enabled = !driverCamera.enabled;
            }
            if (Input.GetButtonDown("Fire3" + inputID))
            {
                mainCamera.enabled = !mainCamera.enabled;
                driverCamera.enabled = !driverCamera.enabled;
            }
        }
    }
    bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }
        if (wheelsOnGround == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
