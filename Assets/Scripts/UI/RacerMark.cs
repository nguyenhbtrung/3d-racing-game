using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerMark : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private Transform target;

    public Transform Target { get => target; set => target = value; }

    private void Update()
    {
        if (Target == null)
            return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(Target.position + offset);
        transform.position = screenPos;
        if (transform.position.z  < 0)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

}
