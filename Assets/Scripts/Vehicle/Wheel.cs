using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wheel
{
    private MeshRenderer mesh;
    private WheelCollider collider;
    private WheelType wheelType;
    private TrailRenderer skidMark;
    private ParticleSystem smokeParticle;
    private string name;


    public MeshRenderer Mesh { get => mesh; set => mesh = value; }
    public WheelCollider Collider { get => collider; set => collider = value; }
    public WheelType WheelType { get => wheelType; set => wheelType = value; }
    public TrailRenderer SkidMark { get => skidMark; set => skidMark = value; }
    public ParticleSystem SmokeParticle { get => smokeParticle; set => smokeParticle = value; }
    public string Name { get => name; set => name = value; }

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
