using BrainAI.Pathfinding.AStar;
using Godot;
using System;
using System.Collections.Generic;

public class MapGraphData : IAstarGraph<Vector2>
{
    public MapGraphData(int width, int height)
    {
        Map = new Node2D[width, height];
        Node2Ds = new Dictionary<Node2D, Vector2>();
    }

    private static readonly Vector2[] CardinalDirs = {
            new Vector2( 1, 0 ),
            new Vector2( 0, -1 ),
            new Vector2( -1, 0 ),
            new Vector2( 0, 1 ),
            new Vector2( 1, 1 ),
            new Vector2( -1, -1 ),
            new Vector2( -1, 1 ),
            new Vector2( 1, -1 ),
        };

    public Node2D[,] Map;
    public Dictionary<Node2D, Vector2> Node2Ds;

    private readonly List<Vector2> neighbors = new List<Vector2>(4);

    public bool IsNodeInBounds(Vector2 node)
    {
        return 0 <= node.x && node.x < this.Map.GetLength(0) && 0 <= node.y && node.y < this.Map.GetLength(1);
    }

    public bool IsNodePassable(Vector2 node)
    {
        return this.Map[(int)node.x, (int)node.y] == null;
    }

    public IEnumerable<Vector2> GetNeighbors(Vector2 node)
    {
        this.neighbors.Clear();

        foreach (var dir in CardinalDirs)
        {
            var next = new Vector2(node.x + dir.x, node.y + dir.y);
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
        return (int)Math.Abs(node.x - goal.x) + (int)Math.Abs(node.y - goal.y);
    }

    public void AddNode2D(Node2D Node2D, Vector2 newPos)
    {
        System.Diagnostics.Debug.Assert(!this.Node2Ds.ContainsKey(Node2D));
        System.Diagnostics.Debug.Assert(this.Map[(int)newPos.x, (int)newPos.y] == null);

        this.Node2Ds[Node2D] = newPos;
        this.Map[(int)newPos.x, (int)newPos.y] = Node2D;
    }

    public void MoveNode2D(Node2D Node2D, Vector2 newPos)
    {
        System.Diagnostics.Debug.Assert(this.Node2Ds.ContainsKey(Node2D));
        System.Diagnostics.Debug.Assert(this.Map[(int)newPos.x, (int)newPos.y] == null);

        var prevPos = this.Node2Ds[Node2D];
        this.Node2Ds[Node2D] = newPos;

        this.Map[(int)prevPos.x, (int)prevPos.y] = null;
        this.Map[(int)newPos.x, (int)newPos.y] = Node2D;
    }

    public void RemoveNode2D(Node2D Node2D)
    {
        System.Diagnostics.Debug.Assert(this.Node2Ds.ContainsKey(Node2D));

        var prevPos = this.Node2Ds[Node2D];
        this.Node2Ds.Remove(Node2D);
        this.Map[(int)prevPos.x, (int)prevPos.y] = null;
    }


    public void ClearMap()
    {
        for (var x = 0; x < this.Map.GetLength(0); x++)
            for (var y = 0; y < this.Map.GetLength(1); y++)
                this.Map[x, y] = null;
        this.Node2Ds.Clear();
    }
}