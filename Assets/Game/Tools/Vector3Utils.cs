using System.Collections.Generic;
using UnityEngine;

public static class Vector3Utils
{
    public static (Vector3, Vector3) GetFarthestPoints(List<Vector3> points)
    {
        if (points == null || points.Count < 2)
        {
            throw new System.ArgumentException("Not enough points in list.");
        }

        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;
        float maxDistance = 0f;

        for (int i = 0; i < points.Count - 1; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                float distance = Vector3.Distance(points[i], points[j]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    pointA = points[i];
                    pointB = points[j];
                }
            }
        }

        return (pointA, pointB);
    }
}
