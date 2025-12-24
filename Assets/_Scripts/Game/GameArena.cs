using UnityEngine;
using System.Collections.Generic;

public class GameArena : MonoBehaviour
{
    public List<AABB> Walls = new List<AABB>();

    // have instance to game arena
    public static GameArena Instance;

    void Awake()
    {
        Instance = this;

        Walls.Add(new AABB(new Vector2(0, -5), new Vector2(10, 1))); // Floor
        Walls.Add(new AABB(new Vector2(0, 5), new Vector2(10, 1)));  // Ceiling
        Walls.Add(new AABB(new Vector2(-5, 0), new Vector2(1, 10))); // Left Wall
        Walls.Add(new AABB(new Vector2(5, 0), new Vector2(1, 10)));  // Right Wall
        Walls.Add(new AABB(new Vector2(2, -2), new Vector2(1, 3)));  // Inner Wall
    }

    // Gizmo for visualizing walls
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var wall in Walls)
        {
            Gizmos.DrawWireCube(wall.Center, wall.HalfSize * 2);
        }
    }
}