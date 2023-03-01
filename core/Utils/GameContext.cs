using BrainAI.Pathfinding;
using System;
using System.Collections.Generic;
using System.Numerics;

public class GameContext
{
    public GameContext(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
    {
        this.WorldToMap = worldToMap;
        this.MapToWorld = mapToWorld;
        this.Map = new PathfindingMap();
        this.Pathfinder = new AStarPathfinder<Vector2>(Map);
    }

    public readonly PathfindingMap Map;
    public readonly AStarPathfinder<Vector2> Pathfinder;

    private readonly Dictionary<PositionComponent, Vector2> KnownPositions = new Dictionary<PositionComponent, Vector2>();
    private Func<Vector2, Vector2> MapToWorld;
    private Func<Vector2, Vector2> WorldToMap;
    private readonly List<Vector2> findPathResult = new List<Vector2>();

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

    public Vector2 GetCellPosition(Vector2 pos)
    {
        return this.MapToWorld(this.WorldToMap(pos));
    }

    public void AddPosition(PositionComponent context)
    {
        var newPos = this.WorldToMap(context.Position);
        if (context.BlockingCells.Length == 0)
        {
            return;
        }

        System.Diagnostics.Debug.Assert(!this.KnownPositions.ContainsKey(context));
        this.KnownPositions[context] = newPos;
        foreach (var dir in context.BlockingCells)
        {
            var cell = newPos + dir;
            System.Diagnostics.Debug.Assert(!this.Map.Map.ContainsKey(cell));
            this.Map.Map[cell] = context;
        }
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


    public IReadOnlyList<Vector2> FindPath(Vector2 from, Vector2 to)
    {
        var fromMap = this.WorldToMap(from);
        var toMap = this.WorldToMap(to);

        var pathMap = this.Pathfinder.Search(fromMap, toMap);

        if (pathMap == null)
        {
            return null;
        }

        findPathResult.Clear();
        foreach(var path in pathMap)
        {
            findPathResult.Add(this.MapToWorld(path));
        }
        return findPathResult;
    }
}
