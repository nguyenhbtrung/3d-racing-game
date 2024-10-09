using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBotInputManager : MonoBehaviour, IInputManager
{
    [SerializeField] private float minSpeed = 1;

    private WaypointFollower waypointFollower;
    private VehicleController vehicleController;

    public float horizontal;
    public float vertical;

    private float stuckCount;
    private bool isGoBack = false;

    private void Awake()
    {
        waypointFollower = GetComponent<WaypointFollower>();
        vehicleController = GetComponent<VehicleController>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
        {
            return;
        }
        CaculateStuckCount();
    }

    private void CaculateStuckCount()
    {
        if (vehicleController.Kph <= 5.0f)
        {
            stuckCount += Time.deltaTime;

        }
        else if (stuckCount >= 0)
        {
            stuckCount -= Time.deltaTime * 2;
        }
        if (stuckCount >= 3)
        {
            isGoBack = true;
        }
        else if (stuckCount <= 0)
        {
            isGoBack = false;
        }
    }

    public float GetHorizontalInput()
    {
        if (waypointFollower.TargetWaypoint == null || isGoBack)
        {
            return 0;
        }
        Vector3 target = waypointFollower.GetTargetPosition();
        Debug.DrawLine(transform.position, target);
        Vector3 direction = target - transform.position;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        float forwardValue = Vector3.Dot(transform.forward, direction);
        float sideValue = Vector3.Dot(transform.right, direction);
        sideValue = (forwardValue > 0) ? sideValue : Mathf.Sign(sideValue);

        horizontal = sideValue;
        vertical = forwardValue;

        return sideValue;
    }

    public float GetVerticalInput()
    {
        if (isGoBack)
        {
            return -1;
        }
        if (vehicleController.Kph <= minSpeed)
        {
            return 1;
        }
        return waypointFollower.GetThrottleStatus();
    }

    public bool IsActivatedBoost()
    {
        return false;
    }

    public bool IsHandBraking()
    {
        if (waypointFollower.IsHandBraking())
        {
            return true;
        }
        return (vehicleController.Kph >= 40 && (Mathf.Abs(horizontal) > 0.9 || vertical < 0));
        // 40, 0.9
    }
}
