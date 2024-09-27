using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    internal enum DriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        AllWheelDrive
    }

    [SerializeField] private DriveType driveType;
    private InputManager inputManager;
    private Transform centerOfMass;
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelMesh = new GameObject[4];
    public int motorTorque = 200;
    public float radius = 6f;
    public float steeringMax = 4;
    public float centreOfGravityOffset = -1f;
    public float wheelbase;
    public float trackWidth;
    public float kph;
    public float downForceValue = 50;
    public float brakeTorque;
    public float[] slip = new float[4];
    Rigidbody rigidBody;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();
        centerOfMass = transform.Find("Center Of Mass");
        //Debug.Log("old: " + rigidBody.centerOfMass);
        rigidBody.centerOfMass = centerOfMass.localPosition;
        //Debug.Log("new: " + rigidBody.centerOfMass);
    }

    private void Start()
    {
        //rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        CalculateSpecifications();
    }

    

    private void FixedUpdate()
    {
        AddDownForce();
        animateWheels();
        MoveVehicle();
        steerVehicle();
        GetFriction();
    }


    private void CalculateSpecifications()
    {
        wheelbase = Vector3.Distance(wheels[0].transform.position, wheels[2].transform.position);
        trackWidth = Vector3.Distance(wheels[0].transform.position, wheels[1].transform.position);
    }

    private void steerVehicle()
    {
        if (inputManager.horizontal > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (radius + (trackWidth / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (radius - (trackWidth / 2))) * inputManager.horizontal;
        }
        else if (inputManager.horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (radius - (trackWidth / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (radius + (trackWidth / 2))) * inputManager.horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }


        for (int i = 0; i < wheels.Length - 2; i++)
        {
            wheels[i].steerAngle = inputManager.horizontal * steeringMax;
        }
    }

    private void MoveVehicle()
    {
        if (driveType == DriveType.AllWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (motorTorque / 4);
            }
        }
        else if (driveType == DriveType.FrontWheelDrive)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (motorTorque / 2);
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (motorTorque / 2);
            }
        }
        kph = rigidBody.velocity.magnitude * 3.6f;

        if (inputManager.handBrake)
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = brakeTorque;
        }
        else
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = 0;
        }
    }

    private void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < wheelMesh.Length; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
    private void AddDownForce()
    {
        rigidBody.AddForce(-Vector3.up * downForceValue * rigidBody.velocity.magnitude);
    }
    private void GetFriction()
    {
        //for (int i = 0; i < wheels.Length; i++)
        //{
        //    WheelHit wheelHit;
        //    wheels[i].GetGroundHit(out wheelHit);
        //    slip[i] = wheelHit.forwardSlip;
        //}
        
    }
}
