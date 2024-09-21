using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    const float waypointGizmoRadius = 0.3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            Gizmos.color = Color.yellow;
        }
    }

    public int GetNearestIndex(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        int nearestIndex = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            float distance = Vector3.Distance(position, GetWaypoint(i));
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }
        return nearestIndex;
    }


    public int GetNextIndex(int i)
    {
        if (i + 1 == transform.childCount)
        {
            return 0;
        }
        return i + 1;
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }

    public PatrolPoint GetPatrolPoint(int i)
    {
        return transform.GetChild(i).GetComponent<PatrolPoint>();
    }
}

