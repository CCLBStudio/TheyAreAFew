using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float speed = 10f;

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
