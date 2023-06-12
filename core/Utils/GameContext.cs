using BrainAI.Pathfinding;
using System;
using System.Collections.Generic;
using System.Numerics;

public interface IPathFinder
{
    List<Vector2> FindPath(Vector2 from, Vector2 to);
}

public interface IMazeBuilder
{
    void UpdatePosition(int entity, PositionComponent context);
    Vector2 GetCellPosition(Vector2 pos);
    void AddPosition(int entity, PositionComponent context);
    void RemovePosition(int entity, PositionComponent context);
    void ClearMap();
}

public class GameContext : IMazeBuilder, IPathFinder
{
    public GameContext(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
    {
        this.worldToMap = worldToMap;
        this.mapToWorld = mapToWorld;
        this.Map = new PathfindingMap();
        this.Pathfinder = new AStarPathfinder<Vector2>(Map);
    }

    public PathfindingMap Map { get; private set; }
    public IPathfinder<Vector2> Pathfinder { get; private set; }

    private readonly Dictionary<int, Vector2> knownPositions = new Dictionary<int, Vector2>();
    private readonly Func<Vector2, Vector2> mapToWorld;
    private readonly Func<Vector2, Vector2> worldToMap;
    private readonly List<Vector2> findPathResult = new List<Vector2>();

    public void UpdatePosition(int entity, PositionComponent context)
    {
        if (context.BlockingCells == null || context.BlockingCells.Length == 0)
        {
            return;
        }

        if (this.knownPositions.ContainsKey(entity))
        {
            var newPos = this.worldToMap(context.Position);
            var oldPos = this.knownPositions[entity];
            if (oldPos == newPos)
            {
                return;
            }
            this.RemovePosition(entity, context);
        }

        this.AddPosition(entity, context);
    }

    public Vector2 GetCellPosition(Vector2 pos)
    {
        return this.mapToWorld(this.worldToMap(pos));
    }

    public void AddPosition(int entity, PositionComponent context)
    {
        var newPos = this.worldToMap(context.Position);
        if (context.BlockingCells == null || context.BlockingCells.Length == 0)
        {
            return;
        }

        System.Diagnostics.Debug.Assert(!this.knownPositions.ContainsKey(entity));
        this.knownPositions[entity] = newPos;
        foreach (var dir in context.BlockingCells)
        {
            var cell = newPos + dir;
            System.Diagnostics.Debug.Assert(!this.Map.Map.ContainsKey(cell));
            this.Map.Map[cell] = context;
        }
    }

    public void RemovePosition(int entity, PositionComponent context)
    {
        if (context.BlockingCells == null || context.BlockingCells.Length == 0)
        {
            return;
        }

        System.Diagnostics.Debug.Assert(this.knownPositions.ContainsKey(entity));

        var prevPos = this.knownPositions[entity];
        this.knownPositions.Remove(entity);
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

        if (this.Map.Map.ContainsKey(toMap))
        {
            return null;
        }

        var pathMap = this.Pathfinder.Search(fromMap, toMap);

        if (pathMap == null)
        {
            return null;
        }

        findPathResult.Clear();
        foreach (var path in (List<Vector2>)pathMap)
        {
            findPathResult.Add(this.mapToWorld(path));
        }
        return findPathResult;
    }
}
