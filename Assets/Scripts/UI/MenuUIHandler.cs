using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] GameObject panelPickVehicle;
    [SerializeField] GameObject panelSelectMap;
    public void OpenPickVehicle()
    {
        panelPickVehicle.SetActive(true);
    }

    public void ClosePickVehicle()
    {
        panelPickVehicle.SetActive(false);
    }

    public void OpenSelectMap()
    {
        panelSelectMap.SetActive(true);
    }

    public void CloseSelectMap()
    {
        panelSelectMap.SetActive(false);
    }
}
