using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody rb;
    public float maxJumpForce = 25f;
    public float pressTimeForMaxForce = 1f;
    
    private bool _isPressing;
    private float _normalizedPressTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BeginPress();
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isPressing = false;
            Jump();
        }
        
        if (_isPressing && _normalizedPressTime < 1f)
        {
            _normalizedPressTime = Mathf.Clamp(_normalizedPressTime + Time.deltaTime / pressTimeForMaxForce, 0f, 1f);
        }
    }
    
    private void BeginPress()
    {
        _isPressing = true;
        _normalizedPressTime = 0f;
    }
    
    private void Jump()
    {
        rb.AddForce(Vector3.up * (maxJumpForce * _normalizedPressTime), ForceMode.Impulse);
    }
}
