using UnityEngine;

public static class Collision
{

    // Epsilon for float comparisons
    private const float EPSILON = 0.0001f;

    // Ball vs AABB collision
    public static void resolveCircleAABBCollision(CircleEntity ball, AABB box)
    {
        // Find point on AABB closest to ball center
        Vector2 closestPoint = box.GetClosestPoint(ball.Position);

        // Calculate vector from closest point to ball center
        Vector2 difference = ball.Position - closestPoint;

        float distanceSq = difference.sqrMagnitude;
        float radiusSq = ball.Radius * ball.Radius;

        if (distanceSq < radiusSq && distanceSq > EPSILON)
        {
            float distance = Mathf.Sqrt(distanceSq);
            Vector2 normal = difference / distance;

            // Handle Tunneling
            float overlap = ball.Radius - distance;
            ball.Position += normal * overlap;

            // Reflect velocity
            float velocityAlongNormal = Vector2.Dot(ball.Velocity, normal);
            if (velocityAlongNormal < 0)
            {
                Vector2 reflectedVelocity = ball.Velocity - (1 + ball.Restitution) * velocityAlongNormal * normal;
                ball.Velocity = reflectedVelocity;
            }
        }

    }

    // Player and Ball Collision
    public static void resolveCircleCircleCollision(CircleEntity player, CircleEntity ball)
    {
        Vector2 difference = ball.Position - player.Position;
        float distanceSq = difference.sqrMagnitude;
        float combinedRadius = player.Radius + ball.Radius;

        // Check overlap
        if (distanceSq < combinedRadius * combinedRadius && distanceSq > EPSILON)
        {
            float distance = Mathf.Sqrt(distanceSq);
            Vector2 normal = difference / distance;

            // Anti-tunneling
            float overlap = combinedRadius - distance;
            ball.Position += normal * overlap;

            // Reflect ball velocity
            float playerSpeedInNormal = Vector2.Dot(player.Velocity, normal);

            // ball speed in normal direction
            float ballSpeedInNormal = Vector2.Dot(ball.Velocity, normal);

            float impactVelocity = ballSpeedInNormal - playerSpeedInNormal;

            if (impactVelocity < 0)
            {
                float restitution = Mathf.Max(player.Restitution, ball.Restitution);
                float impulseMagnitude = -(1 + restitution) * impactVelocity;
                impulseMagnitude /= (1 / player.Mass) + (1 / ball.Mass);

                Vector2 impulse = impulseMagnitude * normal;

                ball.Velocity += impulse / ball.Mass;
            }

        }
    }
}