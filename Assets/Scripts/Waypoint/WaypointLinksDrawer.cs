using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

[ExecuteInEditMode]
public class WaypointLinksDrawer : MonoBehaviour
{
    [SerializeField] private Waypoint waypoint;

    [SerializeField] private float arrowHeadLength = 10;
    [SerializeField] private float arrowHeadAngle = 25;
    [SerializeField] private float lineThickness = 45;

    [SerializeField] Color color = Color.red;

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (waypoint.Neighbours == null || waypoint.Neighbours.Length == 0)
        {
            return;
        }
        foreach (var neighbour in waypoint.Neighbours)
        {
            if (neighbour == null)
                continue;
            Handles.color = color;
            Vector3 direction = neighbour.transform.position - transform.position;
            Handles.DrawAAPolyLine(lineThickness, transform.position, neighbour.transform.position);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
            Handles.DrawAAPolyLine(lineThickness, neighbour.transform.position, neighbour.transform.position + right * arrowHeadLength);
            Handles.DrawAAPolyLine(lineThickness, neighbour.transform.position, neighbour.transform.position + left * arrowHeadLength);
        }
#endif

    }
}
