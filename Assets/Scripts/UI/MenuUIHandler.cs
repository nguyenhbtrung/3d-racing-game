using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] GameObject panelPickVehicle;
    public void OpenPickVehicle()
    {
        panelPickVehicle.SetActive(true);
    }

    public void ClosePickVehicle()
    {
        panelPickVehicle.SetActive(false);
    }
}
