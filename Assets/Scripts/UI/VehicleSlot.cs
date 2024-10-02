using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSlot : MonoBehaviour
{
    private int index;

    public int Index { get => index; set => index = value; }

    public void PickVehicle()
    {
        VehiclePicker picker = GameObject.Find("Vehicle Picker").GetComponent<VehiclePicker>();
        if (picker != null)
        {
            picker.PickVehicle(Index);
        }
    }

}
