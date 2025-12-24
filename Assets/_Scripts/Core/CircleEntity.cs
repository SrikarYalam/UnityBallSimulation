using UnityEngine;

public class CircleEntity
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Radius = 0.5f;
    public float Mass = 1.0f;
    public float Damping = 1.0f;
    public float Restitution = .90f;
    public float Gravity = -20f;


    public void Integrate(float deltaTime)
    {
        Velocity.y += Gravity * deltaTime;

        Position += Velocity * deltaTime;

        Velocity *= Damping;
    }


}