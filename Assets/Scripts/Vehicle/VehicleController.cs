using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private GameObject speedometerUI;
    private Rigidbody rb;
    public float kph;
    [SerializeField] private int startLane;

    public float Kph { get => kph; set => kph = value; }
    protected Rigidbody Rb { get => rb; set => rb = value; }
    public GameObject SpeedometerUI { get => speedometerUI; set => speedometerUI = value; }
    public int StartLane { get => startLane; set => startLane = value; }

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
