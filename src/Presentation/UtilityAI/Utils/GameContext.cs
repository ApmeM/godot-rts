using BrainAI.Pathfinding;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameContext
{
    public GameContext(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
    {
        this.WorldToMap = worldToMap;
        this.MapToWorld = mapToWorld;
    }

    public PathfindingMap Map = new PathfindingMap();

    private Dictionary<PositionComponent, Vector2> KnownPositions = new Dictionary<PositionComponent, Vector2>();
    private Func<Vector2, Vector2> MapToWorld;
    private Func<Vector2, Vector2> WorldToMap;

    public void UpdatePosition(PositionComponent context)
    {
        if (context.BlockingCells.Length == 0)
        {
            return;
        }

        if (this.KnownPositions.ContainsKey(context))
        {
            var newPos = this.WorldToMap(context.Position);
            var oldPos = this.KnownPositions[context];
            if (oldPos == newPos)
            {
                return;
            }
            this.RemovePosition(context);
        }

        this.AddPosition(context);
    }

    public Vector2 AddPosition(PositionComponent context)
    {
        var newPos = this.WorldToMap(context.Position);
        if (context.BlockingCells.Length == 0)
        {
            return this.MapToWorld(newPos);
        }

        System.Diagnostics.Debug.Assert(!this.KnownPositions.ContainsKey(context));
        this.KnownPositions[context] = newPos;
        foreach (var dir in context.BlockingCells)
        {
            var cell = newPos + dir;
            System.Diagnostics.Debug.Assert(!this.Map.Map.ContainsKey(cell));
            this.Map.Map[cell] = context;
        }

        return this.MapToWorld(newPos);
    }

    public void RemovePosition(PositionComponent context)
    {
        if (context.BlockingCells.Length == 0)
        {
            return;
        }

        System.Diagnostics.Debug.Assert(this.KnownPositions.ContainsKey(context));

        var prevPos = this.KnownPositions[context];
        this.KnownPositions.Remove(context);
        foreach (var dir in context.BlockingCells)
        {
            var cell = prevPos + dir;
            System.Diagnostics.Debug.Assert(this.Map.Map.ContainsKey(cell));
            this.Map.Map.Remove(cell);
        }
    }

    public void ClearMap()
    {
        this.Map.Clear();
        this.KnownPositions.Clear();
    }


    public List<Vector2> FindPath(Vector2 from, Vector2 to)
    {
        var fromMap = this.WorldToMap(from);
        var toMap = this.WorldToMap(to);

        var pathMap = AStarPathfinder.Search(this.Map, fromMap, toMap);

        if (pathMap == null)
        {
            return pathMap;
        }

        return pathMap.Select(a => this.MapToWorld(a)).ToList();
    }
}
