using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CircleEntity _playerEntity;
    public BallActor TheBall;

    public float MoveSpeed = 10f;
    public float JumpForce = 15f;
    public float PlayerFriction = 0.9f;

    public float arcadyMultiplier = 5.0f;

    public float slowGravityScale = 0.3f;

    // Input states
    private float _inputX;
    private bool _wantsToJump;

    void Start()
    {
        // create the player entity
        _playerEntity = new CircleEntity();
        _playerEntity.Position = new Vector2(0, 2);
        _playerEntity.Radius = 0.5f;
        _playerEntity.Gravity *= slowGravityScale;

        // subscribe to the game loop tick event
        GameLoop.Instance.OnTick += HandleTick;
    }

    private void OnDestroy()
    {
        if (GameLoop.Instance != null)
        {
            GameLoop.Instance.OnTick -= HandleTick;
        }
    }

    // Updates the input states and graphics
    void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");

        // jump
        if (Input.GetButtonDown("Jump"))
        {
            _wantsToJump = true;
        }

        // update graphics position
        transform.position = _playerEntity.Position;
    }

    // Handle physics tick
    void HandleTick()
    {
        float dt = 1.0f / 60.0f;

        // apply horizontal movement
        if (_inputX != 0)
        {
            _playerEntity.Velocity.x = _inputX * MoveSpeed;
        }
        else
        {
            // apply friction when no input
            _playerEntity.Velocity.x *= PlayerFriction;
        }

        // JUMP
        // TODO: implement ground check
        if (_wantsToJump)
        {
            _playerEntity.Velocity.y = JumpForce;
            _wantsToJump = false;
        }

        _playerEntity.Integrate(dt);

        // Collision with walls
        if (GameArena.Instance != null)
        {
            foreach (var wall in GameArena.Instance.Walls)
            {
                Collision.resolveCircleAABBCollision(_playerEntity, wall);
            }
        }

        // Kick logic with the ball
        if (TheBall != null)
        {
            Collision.resolveCircleCircleCollision(_playerEntity, TheBall._entity);
        }
    }

    // Debug gizmos
    void OnDrawGizmos()
    {
        if (_playerEntity != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_playerEntity.Position, _playerEntity.Radius);
        }
    }
}