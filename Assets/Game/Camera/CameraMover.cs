using ReaaliStudio.Systems.ScriptableValue;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerFacadeListValue players;
    [SerializeField] private float moveSpeed, distance;

    private Vector3 _centroid;
    private float _minY;

    private void Start()
    {
        _minY = cameraTransform.position.y;
    }

    private void Update()
    {
        _centroid = ComputeCentroid(players.Value.Select(x => x.transform.position).ToList());
        _centroid.z = cameraTransform.position.z;
        _centroid.y = Mathf.Max(_minY, _centroid.y);

        cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, _centroid, moveSpeed * Vector3.Distance(cameraTransform.position, _centroid) * Time.deltaTime);
    }

    Vector3 ComputeCentroid(List<Vector3> points)
    {
        if (points == null || points.Count == 0)
            return Vector3.zero;

        Vector3 pointMoyen = points.Aggregate(Vector3.zero, (acc, point) => acc + point) / points.Count;
        return pointMoyen;
    }
}
