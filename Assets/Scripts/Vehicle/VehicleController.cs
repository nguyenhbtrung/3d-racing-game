using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private GameObject speedometerUI;
    [SerializeField] private ParticleSystem[] nitrousFlameParticles;
    [SerializeField] private Color[] nitrousFlameColors;
    [SerializeField] private int startLane;
    [SerializeField] protected float maxSpeed = 250;

    protected IInputManager inputManager;
    private Rigidbody rb;
    public float kph;
    private float nitrous;
    private readonly float maxNitrous = 100;
    private float vertical;
    private float horizontal;
    private float thrust = 2500;
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
    public float Vertical { get => vertical; set => vertical = value; }
    public float Horizontal { get => horizontal; set => horizontal = value; }

    public bool IsBoosting { get => isBoosting; protected set => isBoosting = value; }

    protected virtual void Awake()
    {
        inputManager = GetComponent<IInputManager>();
        Rb = GetComponent<Rigidbody>();
        Nitrous = 100;
    }

    protected virtual void Update()
    {
        Vertical = inputManager.GetVerticalInput();
        Horizontal = inputManager.GetHorizontalInput();
        CalculateKph();
        GainNitrous();
        Boost();
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

    private void Boost()
    {
        if (inputManager.IsActivatedBoost())
        {
            IsBoosting = true;
        }
        if (Nitrous <= 1 || inputManager.IsHandBraking() || Vertical < 0)
        {
            IsBoosting = false;
        }

        float nitrousConsumption = 7 * Time.deltaTime;
        if (IsBoosting)
        {
            Rb.AddForce(transform.forward * thrust);
            UpdateFlameColor();
            foreach (var particle in nitrousFlameParticles)
            {
                particle.Play();
            }
            Nitrous -= nitrousConsumption;
        }
    }

    private void UpdateFlameColor()
    {
        int index = (int)Nitrous / (int)(MaxNitrous / 4);
        float k = ((int)Nitrous % (int)(MaxNitrous / 4)) / (MaxNitrous / 4);
        if (index >= 4) return;
        foreach (var particle in nitrousFlameParticles)
        {
            var main = particle.main;
            main.startColor = Color.Lerp(nitrousFlameColors[index], nitrousFlameColors[index + 1], k);
        }

    }
}
