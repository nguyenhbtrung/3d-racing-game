using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour, IInputManager
{
    public float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVerticalInput()
    {
        return Input.GetAxis("Vertical");
    }

    public bool IsActivatedBoost()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public bool IsHandBraking()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
