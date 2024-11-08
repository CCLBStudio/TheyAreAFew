using UnityEngine;

public static class RigidBody2DEx
{
    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionOrigin, float explosionRadius, float upwardsModifier = 0.0F) 
    {
        var explosionDir = rb.position - explosionOrigin;
        var explosionDistance = explosionDir.magnitude / explosionRadius;

        if (upwardsModifier <= Mathf.Epsilon)
        {
            explosionDir /= explosionDistance;
        }
        else 
        {
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, 1f - explosionDistance) * explosionDir, ForceMode2D.Impulse);
    }
}
