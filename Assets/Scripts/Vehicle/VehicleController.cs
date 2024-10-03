using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private GameObject speedometerUI;
    [SerializeField] private int startLane;

    private Rigidbody rb;
    private float kph;
    private float nitrous;
    private readonly float maxNitrous = 100;
    private bool isBoosting = false;


    public float Kph { get => kph; set => kph = value; }
    protected Rigidbody Rb { get => rb; set => rb = value; }
    public GameObject SpeedometerUI { get => speedometerUI; set => speedometerUI = value; }
    public int StartLane { get => startLane; set => startLane = value; }
    public float Nitrous
    {
        get => nitrous; protected set => nitrous = Mathf.Clamp(value, 0, MaxNitrous);
    }

    public float MaxNitrous => maxNitrous;

    public bool IsBoosting { get => isBoosting; protected set => isBoosting = value; }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Nitrous = 20;
    }

    protected virtual void Update()
    {
        CalculateKph();
        GainNitrous();
    }

    private void CalculateKph()
    {
        kph = Rb.velocity.magnitude * 3.6f;
    }

    protected virtual void GainNitrous()
    {
        float gainAmount = 1;
        Nitrous += gainAmount * Time.deltaTime;
    }
}
