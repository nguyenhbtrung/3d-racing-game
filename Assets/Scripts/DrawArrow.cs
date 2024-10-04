using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DrawArrow : MonoBehaviour
{
    public Transform target;
    public float arrowHeadLength = 10;
    public float arrowHeadAngle = 25;
    public float lineThickness = 45;

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Handles.color = Color.red;
            Vector3 direction = target.position - transform.position;
            Handles.DrawAAPolyLine(lineThickness, transform.position, target.position);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
            Handles.DrawAAPolyLine(lineThickness, target.position, target.position + right * arrowHeadLength);
            Handles.DrawAAPolyLine(lineThickness, target.position, target.position + left * arrowHeadLength);
        }
    }
}
