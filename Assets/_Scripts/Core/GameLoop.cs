using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // Static instance of the GameLoop
    public static GameLoop Instance;

    // Simulation
    private const float TICK_RATE = 1.0f / 60.0f; // 60 ticks per second
    private float _accumulatedTime = 0.0f;

    public delegate void TickEvent();
    public event TickEvent OnTick;

    // Awake: first thing that is called when script loaded
    void Awake()
    {
        // Create one instance of GameLoop
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Application.targetFrameRate = 120;
    }

    // Update once per frame
    void Update()
    {
        _accumulatedTime += Time.deltaTime;

        while(_accumulatedTime >= TICK_RATE)
        {
            // Do the physics
            OnTick?.Invoke();

            // Decrease accumulated time
            _accumulatedTime -= TICK_RATE;
        }

        // TODO: For smooth rendering, have interpolation here
    }
}
