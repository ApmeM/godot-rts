using BrainAI.Pathfinding;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

public class PathfindingMap : IAstarGraph<Vector2>
{
    public readonly Dictionary<Vector2, PositionComponent> Map = new Dictionary<Vector2, PositionComponent>();

    private static readonly Vector2[] PathfindingDirs = {
            new Vector2( 1, 0 ),
            new Vector2( 0, -1 ),
            new Vector2( -1, 0 ),
            new Vector2( 0, 1 ),
            new Vector2( 1, 1 ),
            new Vector2( -1, -1 ),
            new Vector2( -1, 1 ),
            new Vector2( 1, -1 ),
        };
    private readonly List<Vector2> neighbors = new List<Vector2>(4);

    public bool IsNodeInBounds(Vector2 node)
    {
        return true;
    }

    public bool IsNodePassable(Vector2 node)
    {
        return !this.Map.ContainsKey(node);
    }

    public List<Vector2> GetNeighbors(Vector2 node)
    {
        this.neighbors.Clear();

        foreach (var dir in PathfindingDirs)
        {
            var next = new Vector2(node.X + dir.X, node.Y + dir.Y);
            if (this.IsNodeInBounds(next) && this.IsNodePassable(next))
                this.neighbors.Add(next);
        }

        return this.neighbors;
    }

    public int Cost(Vector2 from, Vector2 to)
    {
        return 1;
    }

    public int Heuristic(Vector2 node, Vector2 goal)
    {
        return (int)Math.Abs(node.X - goal.X) + (int)Math.Abs(node.Y - goal.Y);
    }

    public void Clear()
    {
        this.Map.Clear();
    }

    public override string ToString()
    {
        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        foreach (var cell in Map.Keys)
        {
            minX = Math.Min(minX, cell.X);
            minY = Math.Min(minY, cell.Y);
            maxX = Math.Max(maxX, cell.X);
            maxY = Math.Max(maxY, cell.Y);
        }

        var sb = new StringBuilder();
        for (var y = minY; y < maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                sb.Append(Map.ContainsKey(new Vector2(x, y)) ? "#" : ".");
            }
            sb.AppendLine("");
        }

        return sb.ToString();
    }
}
