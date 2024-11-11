using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Transform groundRef;
    
    public Vector3 GetGroundPosition()
    {
        return groundRef ? groundRef.position : transform.position;
    }
}
