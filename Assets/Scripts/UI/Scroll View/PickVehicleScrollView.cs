using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickVehicleScrollView : MonoBehaviour
{
    [SerializeField] private VehiclePicker vehiclePicker;
    [SerializeField] private GameObject vehicleSlotPrefab;

    [SerializeField] private Transform content;


    private void Start()
    {

        for (int i = 0; i < vehiclePicker.VehicleTemplates.Length; i++)
        {
            GameObject vehicleSlotObj =  Instantiate(vehicleSlotPrefab, content);
            VehicleSlot vehicleSlot = vehicleSlotObj.GetComponent<VehicleSlot>();
            vehicleSlot.Index = i;

            Image image = vehicleSlotObj.transform.Find("Image Vehicle").GetComponent<Image>();
            image.sprite = vehiclePicker.VehicleTemplates[i].sprite;
        }
    }
}
