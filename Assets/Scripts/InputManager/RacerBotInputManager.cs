using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerBotInputManager : MonoBehaviour, IInputManager
{
    private RacerWaypointFollower waypointFollower;
    [SerializeField] private VehicleController vehicleController;
    [SerializeField] private float horizontalSpeed;

    public float horizontal;
    public float vertical;

    private void Awake()
    {
        waypointFollower = GetComponent<RacerWaypointFollower>();
    }

    private void Update()
    {
    }

    public float GetHorizontalInput()
    {
        if (waypointFollower.TargetWaypoint == null)
        {
            return 0;
        }
        Vector3 target = waypointFollower.TargetWaypoint.transform.position;
        Vector3 direction = target - transform.position;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        float forwardValue = Vector3.Dot(transform.forward, direction);
        float sideValue = Vector3.Dot(transform.right, direction);
        sideValue = (forwardValue > 0) ? sideValue : Mathf.Sign(sideValue);
        //sideValue = (Mathf.Abs(sideValue) > 0.3 ) ? Mathf.Sign(sideValue) : 0;

        horizontal = sideValue;
        vertical = forwardValue;

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
        return (vehicleController.Kph >= 40 && (Mathf.Abs(horizontal) > 0.9 || vertical < 0));
    }

}