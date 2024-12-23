using PathCreation;
using UnityEngine;
using UnityEngine.Events;

public class PathFollower : MonoBehaviour
{
    public PathCreator path;
    public float speed = 5f;
    public UnityEvent onPathCompleted;
    
    private float _distanceTravelled;
    private bool _isMoving;
    
    private void Update()
    {
        if (!_isMoving)
        {
            return;
        }

        _distanceTravelled += speed * Time.deltaTime;
        if (_distanceTravelled >= path.path.length)
        {
            _isMoving = false;
            onPathCompleted?.Invoke();
            return;
        }
        
        transform.position = path.path.GetPointAtDistance(_distanceTravelled);
    }

    public void LaunchMovement()
    {
        _isMoving = true;
        _distanceTravelled = 0f;
    }

    public void DebugLog()
    {
        Debug.Log("Path completed !");
    }
}
