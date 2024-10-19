using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBot : MonoBehaviour
{
    [SerializeField] private float duration = 10;
    private VehicleController vehicleController;
    private Transform player;
    private float playerDistance = 500;
    private float timer = 0;

    private void Awake()
    {
        vehicleController = GetComponent<VehicleController>();
    }

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Is player found: " + (player != null).ToString());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration && Vector3.Distance(transform.position, player.position) > playerDistance)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Player: ", player);
        if (player == null)
        {
            FindPlayer();
        }
        if (Vector3.Distance(transform.position, player.position) < playerDistance)
        {
            return;
        }
        Transform tf = collision.transform;
        while (tf != null)
        {
            if (tf.CompareTag("Racer"))
            {
                timer = 0;
                gameObject.SetActive(false);
                break;
            }
            tf = tf.parent;
        }
    }
}
