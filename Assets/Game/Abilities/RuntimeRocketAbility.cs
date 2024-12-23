using UnityEngine;

public class RuntimeRocketAbility : RuntimeAbility
{
    private float _cooldown;
    private PlayerAbilities _playerAbilities;
    private Vector2 _aimDir = Vector2.down;
    private ScriptableRocketAbility _rocketAbility;

    private void Update()
    {
        if (_cooldown > 0f)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    public override void Initialize(PlayerAbilities playerAbilities, ScriptableAbility rocketAbility)
    {
        _cooldown = 0f;
        _playerAbilities = playerAbilities;
        _rocketAbility = (ScriptableRocketAbility)rocketAbility;
    }

    public override void OnAim(Vector2 direction)
    {
        _aimDir = direction == Vector2.zero ? Vector2.down : direction.normalized;
    }

    private void LaunchRocket()
    {
        var rocket = _rocketAbility.VisualPool.RequestObjectAs<PooledRocket>();
        rocket.SetPositionAndRotation(_playerAbilities.AbilityHolder.position, ComputeRotationTowardAxis(_aimDir));
        rocket.Initialize(_rocketAbility);
        _cooldown = _rocketAbility.Cooldown;
    }

    public override void OnInputPressed()
    {
        if (_cooldown <= 0f)
        {
            LaunchRocket();
        }
    }

    public override void OnInputReleased()
    {
    }
}
