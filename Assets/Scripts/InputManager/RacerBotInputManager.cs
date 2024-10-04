using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerBotInputManager : MonoBehaviour, IInputManager
{
    [SerializeField] private Transform targert;
    [SerializeField] private float horizontalSpeed;

    public float horizontal;
    private void Update()
    {
        
    }

    public float GetHorizontalInput()
    {
        Vector3 direction = targert.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        float forwardValue = Vector3.Dot(transform.forward, direction);
        float sideValue = Vector3.Dot(transform.right, direction);
        sideValue = (forwardValue > 0) ? sideValue : Mathf.Sign(sideValue);
        //sideValue = (Mathf.Abs(sideValue) > 0.3 ) ? Mathf.Sign(sideValue) : 0;
        horizontal += sideValue * horizontalSpeed * Time.deltaTime;
        horizontal = Mathf.Clamp(horizontal, -1, 1);
        return sideValue;
    }

    public float GetVerticalInput()
    {
        return 1;
    }

    public bool IsActivatedBoost()
    {
        return false;
    }

    public bool IsHandBraking()
    {
        return false;
    }
}
