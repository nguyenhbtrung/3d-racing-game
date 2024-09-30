using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private Rigidbody rb;
    public float kph;

    public float Kph { get => kph; set => kph = value; }
    protected Rigidbody Rb { get => rb; set => rb = value; }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        CalculateKph();
    }

    private void CalculateKph()
    {
        kph = Rb.velocity.magnitude * 3.6f;
    }
}
