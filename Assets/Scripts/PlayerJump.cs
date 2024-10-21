using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody rb;

    [SerializeField] private List<JumpEffect> jumpEffects;

    private bool _isPressing;
    private float _normalizedPressTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var effect in jumpEffects)
            {
                effect.ChargingJump(this);
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var effect in jumpEffects)
            {
                effect.Jump(this);
            }
        }
    }
}
