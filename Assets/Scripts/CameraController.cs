using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private VehicleController controller;
    private Transform player;
    private Transform cameraConstraint;
    private float speed;
    private float defaultFOV;

    [SerializeField] private float boostedFOV;
    [SerializeField] [Range(0, 5)] private float smoothTime;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraConstraint = player.Find("Camera Constraint");
        controller = player.GetComponent<VehicleController>();
        defaultFOV = Camera.main.fieldOfView;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        BoostFOV();
    }

    private void FollowPlayer()
    {
        float kph = controller.Kph;
        speed = Mathf.Lerp(speed, kph / 4, Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, cameraConstraint.position, Time.deltaTime * speed);
        transform.LookAt(player.position);
    }

    private void BoostFOV()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, boostedFOV, Time.deltaTime * smoothTime);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, Time.deltaTime * smoothTime);
        }
    }
}
