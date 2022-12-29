using BrainAI.Pathfinding.AStar;
using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class MapGraphData : IAstarGraph<Vector2>
{
    public MapGraphData(int width, int height)
    {
        Map = new Node2D[width, height];
        Node2Ds = new Dictionary<Node2D, ValueTuple<Vector2, Vector2[]>>();
    }

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

    public Node2D[,] Map;
    public Dictionary<Node2D, ValueTuple<Vector2, Vector2[]>> Node2Ds;

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

        foreach (var dir in PathfindingDirs)
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

    public void AddNode2D(Node2D node2D, Vector2 newPos, Vector2[] blockingCells)
    {
        System.Diagnostics.Debug.Assert(!this.Node2Ds.ContainsKey(node2D));
        this.Node2Ds[node2D] = new ValueTuple<Vector2, Vector2[]>(newPos, blockingCells);
        foreach (var dir in blockingCells)
        {
            var cell = newPos + dir;
            System.Diagnostics.Debug.Assert(this.Map[(int)cell.x, (int)cell.y] == null);
            this.Map[(int)cell.x, (int)cell.y] = node2D;
        }
    }

    public void MoveNode2D(Node2D node2D, Vector2 newPos, Vector2[] blockingCells)
    {
        RemoveNode2D(node2D);
        AddNode2D(node2D, newPos, blockingCells);
    }

    public void RemoveNode2D(Node2D node2D)
    {
        System.Diagnostics.Debug.Assert(this.Node2Ds.ContainsKey(node2D));

        var (prevPos, prevBlockingCells) = this.Node2Ds[node2D];
        this.Node2Ds.Remove(node2D);
        foreach (var dir in prevBlockingCells)
        {
            var cell = prevPos + dir;
            System.Diagnostics.Debug.Assert(this.Map[(int)cell.x, (int)cell.y] != null);
            this.Map[(int)cell.x, (int)cell.y] = null;
        }
    }

    public void ClearMap()
    {
        for (var x = 0; x < this.Map.GetLength(0); x++)
            for (var y = 0; y < this.Map.GetLength(1); y++)
                this.Map[x, y] = null;
        this.Node2Ds.Clear();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var x = 0; x < Map.GetLength(0); x++)
        {
            for (var y = 0; y < Map.GetLength(1); y++)
            {
                sb.Append(Map[x,y] == null? "." : "#");
            }
            sb.AppendLine("");
        }

        return sb.ToString();
    }
}