using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBot : MonoBehaviour
{
    [SerializeField] private float duration = 10;

    private float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }
}
