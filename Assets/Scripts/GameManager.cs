using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform speedometerUIParent;
    private VehicleController vehicleController;

    private void Awake()
    {
        vehicleController = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleController>();
        Instantiate(vehicleController.SpeedometerUI, speedometerUIParent.position, Quaternion.identity, speedometerUIParent);
    }
}
