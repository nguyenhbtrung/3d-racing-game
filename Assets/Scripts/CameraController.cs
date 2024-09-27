using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject child;
    public Controller playerController;
    public float speed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        child = player.transform.Find("Camera Constraint").gameObject;
        playerController = player.GetComponent<Controller>();
    }

    private void FixedUpdate()
    {
        Follow();
        float kph = playerController.kph;
        speed = (kph >= 50) ? 20 : kph / 4;
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, child.transform.position, Time.deltaTime * speed);
        transform.LookAt(player.transform.position);
    }
}
