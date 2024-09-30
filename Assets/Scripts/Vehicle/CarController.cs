using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : VehicleController
{
    [SerializeField] private AnimationCurve enginePower;
    [SerializeField] private float[] gears;
    [SerializeField] private float shiftGearMaxRPM;
    [SerializeField] private float shiftGearMinRPM;


    private Wheel[] wheels;
    private IInputManager inputManager;

    private float motorTorque;
    private float wheelsRPM;
    private float engineRPM;
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
        
    }

    private void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        CalculateEnginePower();
        AutoShiftGear();
        AnimateWheels();
    }

    private void FixedUpdate()
    {
        Move();
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
        motorTorque = motorTorqueMultiplier * inputManager.GetVerticalInput() * enginePower.Evaluate(engineRPM) * gears[gearNum];
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

    private void AutoShiftGear()
    {
        if (!IsGrounded())
        {
            return;
        }
        if (engineRPM > shiftGearMaxRPM && gearNum < gears.Length - 1 && !IsReverse())
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
