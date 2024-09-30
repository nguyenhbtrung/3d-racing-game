using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wheel
{
    private MeshRenderer mesh;
    private WheelCollider collider;
    private string name;
    private WheelType wheelType;


    public MeshRenderer Mesh { get => mesh; set => mesh = value; }
    public WheelCollider Collider { get => collider; set => collider = value; }
    public string Name { get => name; set => name = value; }
    public WheelType WheelType { get => wheelType; set => wheelType = value; }

    public Wheel(string name)
    {
        Name = name;
    }

    public void AnimateWheel()
    {
        if (Collider == null)
        {
            return;
        }
        Collider.GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);
        Mesh.transform.position = wheelPosition;
        Mesh.transform.rotation = wheelRotation;
    }
}

public enum WheelType
{
    Front,
    Back
}
