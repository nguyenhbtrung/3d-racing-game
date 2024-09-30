using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : VehicleController
{
    private Wheel[] wheels;

    protected override void Awake()
    {
        base.Awake();
        InitWheels();

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
