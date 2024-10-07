using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class VehiclePicker : MonoBehaviour
{
    [Serializable]
    internal struct VehicleTemplate
    {
        public GameObject prefab;
        public Sprite sprite;
    }

    [SerializeField] private VehicleTemplate[] vehicleTemplates;
    [SerializeField] private GameObject vehicleObj;
    [SerializeField] private MenuUIHandler menuUIHandler;

    internal VehicleTemplate[] VehicleTemplates { get => vehicleTemplates; set => vehicleTemplates = value; }

    private void Start()
    {
        PickVehicle(0);
    }
    public void PickVehicle(int index)
    {
        GameData.Instance.PlayerVehicle = vehicleTemplates[index].prefab;
        menuUIHandler.ClosePickVehicle();
        if(vehicleObj != null)
        {
            Destroy(vehicleObj);
        }
        vehicleObj = Instantiate(vehicleTemplates[index].prefab, Vector3.up * 1.7f, Quaternion.identity);
        vehicleObj.GetComponent<VehicleController>().enabled = false;
        vehicleObj.transform.localScale = Vector3.one;
        Debug.Log("scale: " + vehicleObj.transform.lossyScale);
    }
}
