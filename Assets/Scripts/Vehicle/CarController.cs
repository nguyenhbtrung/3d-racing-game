using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : VehicleController
{
    [SerializeField] private AnimationCurve enginePower;
    [SerializeField] private float[] gears;
    [SerializeField] private float[] gearChangeSpeeds;
    [SerializeField] private float shiftGearMaxRPM;
    [SerializeField] private float shiftGearMinRPM;
    [SerializeField] private float steerMinRadius = 6;


    private Wheel[] wheels;
    private Transform centerOfMass;
    private IInputManager inputManager;

    private float motorTorque;
    private float brakeTorque;
    private float wheelsRPM;
    private float engineRPM;
    private float wheelbase;
    private float trackWidth;
    private float steerCurrentRadius;
    private float vertical;
    private float horizontal;
    private int gearNum = 0;

    internal enum DriveType
    {
        AllWheels,
        FrontWheels,
        BackWheels
    }
    [SerializeField] private DriveType driveType;


    protected override void Awake()
    {
        base.Awake();
        InitWheels();
        inputManager = GetComponent<IInputManager>();
        centerOfMass = transform.Find("Center Of Mass");
        Rb.centerOfMass = centerOfMass.localPosition;
    }

    private void Start()
    {
        CalculateSpecifications();
    }

    protected override void Update()
    {
        base.Update();

        vertical = inputManager.GetVerticalInput();
        horizontal = inputManager.GetHorizontalInput();

        AdjustDrag();

        CalculateEnginePower();
        AutoShiftGear();
        AnimateWheels();

        Steer();
        Move();
        Brake();
    }

    private void AdjustDrag()
    {
        if (vertical != 0)
        {
            Rb.drag = 0.005f;
        }
        if (vertical == 0)
        {
            Rb.drag = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void Steer()
    {
        WheelCollider frontLeft = null, frontRight = null;
        steerCurrentRadius = steerMinRadius + Kph / 20;
        foreach (var wheel in wheels)
        {
            if (wheel.Name == "Front Left") frontLeft = wheel.Collider;
            if (wheel.Name == "Front Right") frontRight = wheel.Collider;
        }
        if (horizontal > 0)
        {
            frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (steerCurrentRadius + (trackWidth / 2))) * horizontal;
            frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (steerCurrentRadius - (trackWidth / 2))) * horizontal;
        }
        else if (horizontal < 0)
        {
            frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (steerCurrentRadius - (trackWidth / 2))) * horizontal;
            frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (steerCurrentRadius + (trackWidth / 2))) * horizontal;
        }
        else
        {
            frontLeft.steerAngle = 0;
            frontRight.steerAngle = 0;
        }
    }

    private void CalculateEnginePower()
    {
        float motorTorqueMultiplier = 3.6f;
        float engineRPMMultiplier = 3.6f;
        float velocity = 0.0f;
        float smoothTime = 0.09f;
        float minEngineRPM = 1000.0f;

        CalculateWheelsRPM();
        engineRPM = Mathf.SmoothDamp(engineRPM, minEngineRPM + Mathf.Abs(wheelsRPM) * engineRPMMultiplier * gears[gearNum], ref velocity, smoothTime);
        engineRPM = Mathf.Clamp(engineRPM, engineRPM, shiftGearMaxRPM + 1000);
        motorTorque = motorTorqueMultiplier * vertical * enginePower.Evaluate(engineRPM);
    }

    private void CalculateWheelsRPM()
    {
        float sum = 0;
        foreach (var wheel in wheels)
        {
            sum += wheel.Collider.rpm;
        }
        wheelsRPM = (wheels.Length != 0) ? sum / wheels.Length : 0;

        
    }


    private bool CheckGears()
    {
        return (Kph >= gearChangeSpeeds[gearNum]);
    }
    private void AutoShiftGear()
    {
        if (!IsGrounded())
        {
            return;
        }
        if (engineRPM > shiftGearMaxRPM &&
            gearNum < gears.Length - 1 &&
            !IsReverse() && CheckGears())
        {
            gearNum++;
        }
        if (engineRPM < shiftGearMinRPM && gearNum > 0)
        {
            gearNum--;
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.AnimateWheel();
        }
    }

    private void Brake()
    {
        brakeTorque = 10;
        if (vertical < 0)
        {
            brakeTorque = (Kph >= 10) ? 1000 : 10;
        }
        

        foreach (var wheel in wheels)
        {
            if (inputManager.IsBraking())
            {
                wheel.Collider.brakeTorque = brakeTorque;
            }
            else
            {
                wheel.Collider.brakeTorque = 0;
            }
        }
    }

    private void CalculateSpecifications()
    {
        Vector3 frontLeft = Vector3.zero, 
            frontRight = Vector3.zero,
            backLeft = Vector3.zero;
        foreach (var wheel in wheels)
        {
            if (wheel.Name == "Front Left") frontLeft = wheel.Collider.transform.position;
            if (wheel.Name == "Front Right") frontRight = wheel.Collider.transform.position;
            if (wheel.Name == "Back Left") backLeft = wheel.Collider.transform.position;
        }

        wheelbase = Vector3.Distance(frontLeft, backLeft);
        trackWidth = Vector3.Distance(frontLeft, frontRight);
    }

    private bool IsGrounded()
    {
        return wheels[0].Collider.isGrounded 
            && wheels[1].Collider.isGrounded 
            && wheels[2].Collider.isGrounded 
            && wheels[3].Collider.isGrounded;
    }

    private bool IsReverse()
    {
        return wheelsRPM < 0;
    }
    private void Move()
    {
        if (driveType == DriveType.AllWheels)
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.Collider.motorTorque = motorTorque / 4;
            }
        }
        else if (driveType == DriveType.FrontWheels)
        {
            foreach (Wheel wheel in wheels)
            {
                if (wheel.WheelType != WheelType.Front)
                    continue;
                wheel.Collider.motorTorque = motorTorque / 2;
            }
        }
        else if (driveType == DriveType.BackWheels)
        {
            foreach (Wheel wheel in wheels)
            {
                if (wheel.WheelType != WheelType.Back)
                    continue;
                wheel.Collider.motorTorque = motorTorque / 2;
            }
        }
    }

    private void InitWheels()
    {
        wheels = new Wheel[]
        {
            new Wheel("Front Left"),
            new Wheel("Front Right"),
            new Wheel("Back Left"),
            new Wheel("Back Right")
        };

        Transform wheelMeshesParent = transform.Find("Wheel Meshes");
        Transform wheelCollidersParent = transform.Find("Wheel Colliders");
        foreach (var wheel in wheels)
        {
            wheel.Mesh = wheelMeshesParent.Find(wheel.Name).GetComponent<MeshRenderer>();
            wheel.Collider = wheelCollidersParent.Find(wheel.Name).GetComponent<WheelCollider>();
            if (wheel.Name.StartsWith("Front"))
            {
                wheel.WheelType = WheelType.Front;
            }
            else if (wheel.Name.StartsWith("Back"))
            {
                wheel.WheelType = WheelType.Back;
            }
        }
    }

}
