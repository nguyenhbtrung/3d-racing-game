
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 20f; 
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            Destroy(gameObject); 
        }
    }
}