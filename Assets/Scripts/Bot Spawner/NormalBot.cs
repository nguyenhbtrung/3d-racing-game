using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBot : MonoBehaviour
{
    [SerializeField] private float duration = 10;
    private Transform player;
    private float playerDistance = 500;
    private float timer = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
}
