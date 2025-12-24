using UnityEngine;

public class BallActor : MonoBehaviour
{
    public CircleEntity _entity;

    void Start()
    {
        // create the circle entity
        _entity = new CircleEntity();
        _entity.Position = new Vector2(0, 3);
        _entity.Velocity = new Vector2(5, 0);

        // subscribe to the game loop tick event
        GameLoop.Instance.OnTick += HandleTick;
    }

    private void OnDestroy()
    {
        // unsubscribe from the game loop tick event
        if (GameLoop.Instance != null)
        {
            GameLoop.Instance.OnTick -= HandleTick;
        }
    }

    void HandleTick()
    {

        float dt = 1.0f / 60.0f; // standard tick rate

        float speed = _entity.Velocity.magnitude;
        float predictedDistance = speed * dt;

        // prevent tunneling by not moving more than 1/3 radius in step
        float maxSafeStepSize = _entity.Radius / 3.0f;

        int subSteps = Mathf.CeilToInt(predictedDistance / maxSafeStepSize);

        if (subSteps < 1) subSteps = 1;

        if (subSteps > 20) subSteps = 20; // cap to avoid too many steps

        float dtPerStep = dt / subSteps;

        for (int i = 0; i < subSteps; i++)
        {
            // integrate the entity
            _entity.Integrate(dtPerStep);

            // check collision with walls
            if (GameArena.Instance != null)
            {
                foreach (var wall in GameArena.Instance.Walls)
                {
                    Collision.resolveCircleAABBCollision(_entity, wall);
                }
            }
        }
    }

    // Update runs every rendered frame
    void Update()
    {
        transform.position = _entity.Position;
    }

    // Gizmo for visualizing ball
    void OnDrawGizmos()
    {
        if (_entity != null){
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _entity.Radius);
        }
    }
}