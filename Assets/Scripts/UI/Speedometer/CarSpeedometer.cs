using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSpeedometer : MonoBehaviour
{
    [SerializeField] private Text kphText;
    [SerializeField] private Text currentGearText;
    [SerializeField] private Transform needle;
    [SerializeField] private Slider nitrousSlider;

    private float startRotNeedle = 32;
    private float endRotNeedle = -210;
    private float maxRPM = 10000;

    private CarController carController;

    private void Awake()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
    }

    private void Update()
    {
        kphText.text = carController.Kph.ToString("F0");
        UpdateGearNum();
        UpdateNeedle();
        UpdateNitrous();
    }

    private void UpdateGearNum()
    {
        string gearNum = (carController.GearNum + 1).ToString();
        if (carController.IsReverse())
        {
            gearNum = "R";
        }
        if (carController.IsHandBraking())
        {
            gearNum = "N";
        }
        if (carController.EngineRPM < 1100)
        {
            gearNum = "P";
        }

        currentGearText.text = gearNum;
    }

    private void UpdateNeedle()
    {
        float rpm = carController.EngineRPM;
        float ratio = rpm / maxRPM;
        float currentRot = startRotNeedle + (endRotNeedle - startRotNeedle) * ratio;
        needle.eulerAngles = new Vector3(0, 0, currentRot);
    }

    private void UpdateNitrous()
    {
        nitrousSlider.value = 0.64f * carController.Nitrous / carController.MaxNitrous;
    }
}
