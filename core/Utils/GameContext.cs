using BrainAI.Pathfinding;
using System;
using System.Collections.Generic;
using System.Numerics;

public class GameContext
{
    public GameContext(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
    {
        this.worldToMap = worldToMap;
        this.mapToWorld = mapToWorld;
        this.Map = new PathfindingMap();
        this.Pathfinder = new AStarPathfinder<Vector2>(Map);
    }

    public readonly PathfindingMap Map;
    public readonly IPathfinder<Vector2> Pathfinder;

    private readonly Dictionary<PositionComponent, Vector2> knownPositions = new Dictionary<PositionComponent, Vector2>();
    private readonly Func<Vector2, Vector2> mapToWorld;
    private readonly Func<Vector2, Vector2> worldToMap;
    private readonly List<Vector2> findPathResult = new List<Vector2>();

    public void UpdatePosition(PositionComponent context)
    {
        if (context.BlockingCells.Length == 0)
        {
            return;
        }

        if (this.knownPositions.ContainsKey(context))
        {
            var newPos = this.worldToMap(context.Position);
            var oldPos = this.knownPositions[context];
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
        return this.mapToWorld(this.worldToMap(pos));
    }

    public void AddPosition(PositionComponent context)
    {
        var newPos = this.worldToMap(context.Position);
        if (context.BlockingCells.Length == 0)
        {
            return;
        }

        System.Diagnostics.Debug.Assert(!this.knownPositions.ContainsKey(context));
        this.knownPositions[context] = newPos;
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

        System.Diagnostics.Debug.Assert(this.knownPositions.ContainsKey(context));

        var prevPos = this.knownPositions[context];
        this.knownPositions.Remove(context);
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
        this.knownPositions.Clear();
    }


    public List<Vector2> FindPath(Vector2 from, Vector2 to)
    {
        var fromMap = this.worldToMap(from);
        var toMap = this.worldToMap(to);

        var pathMap = this.Pathfinder.Search(fromMap, toMap);

        if (pathMap == null)
        {
            return null;
        }

        findPathResult.Clear();
        foreach(var path in (List<Vector2>)pathMap)
        {
            findPathResult.Add(this.mapToWorld(path));
        }
        return findPathResult;
    }
}
