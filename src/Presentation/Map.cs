using BrainAI.Pathfinding;
using Godot;
using GodotAnalysers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[SceneReference("Map.tscn")]
public partial class Map
{
    public Context context;

    public class Context : IAstarGraph<Vector2>
    {
        #region AStar methods

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

        #endregion

        public Context(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
        {
            this.WorldToMap = worldToMap;
            this.MapToWorld = mapToWorld;
        }

        public readonly Dictionary<Vector2, TileMapObject.Context> Map = new Dictionary<Vector2, TileMapObject.Context>();
        private Dictionary<TileMapObject.Context, Vector2> KnownPositions = new Dictionary<TileMapObject.Context, Vector2>();
        private Func<Vector2, Vector2> MapToWorld;
        private Func<Vector2, Vector2> WorldToMap;

        public enum MapItemType
        {
            Water,
            Construction
        }

        public Dictionary<MapItemType, HashSet<TileMapObject.Context>> ItemsByType = new Dictionary<MapItemType, HashSet<TileMapObject.Context>>();

        public void AddItemByType(MapItemType itemType, TileMapObject.Context item)
        {
            if (!ItemsByType.ContainsKey(itemType))
            {
                ItemsByType[itemType] = new HashSet<TileMapObject.Context>();
            }

            ItemsByType[itemType].Add(item);
        }

        public void RemoveItemByType(MapItemType itemType, TileMapObject.Context item)
        {
            if (!ItemsByType.ContainsKey(itemType))
            {
                ItemsByType[itemType] = new HashSet<TileMapObject.Context>();
                return;
            }

            ItemsByType[itemType].Remove(item);
        }

        public TileMapObject.Context FindClosestItemByType(MapItemType itemType, Vector2 position)
        {
            if (!ItemsByType.ContainsKey(itemType))
            {
                ItemsByType[itemType] = new HashSet<TileMapObject.Context>();
                return null;
            }

            return ItemsByType[itemType].OrderBy(a => (a.Position - position).LengthSquared()).FirstOrDefault();
        }

        public bool ItemByTypeExists(MapItemType itemType)
        {
            if (!ItemsByType.ContainsKey(itemType))
            {
                ItemsByType[itemType] = new HashSet<TileMapObject.Context>();
                return false;
            }

            return ItemsByType[itemType].Any();
        }

        public void UpdatePosition(TileMapObject.Context context)
        {
            if (this.KnownPositions.ContainsKey(context))
            {
                this.RemovePosition(context);
            }

            this.AddPosition(context);
        }

        public Vector2 AddPosition(TileMapObject.Context context)
        {
            System.Diagnostics.Debug.Assert(!this.KnownPositions.ContainsKey(context));
            var newPos = this.WorldToMap(context.Position);
            this.KnownPositions[context] = newPos;
            foreach (var dir in context.BlockingCells)
            {
                var cell = newPos + dir;
                System.Diagnostics.Debug.Assert(!this.Map.ContainsKey(cell));
                this.Map[cell] = context;
            }

            return this.MapToWorld(newPos);
        }

        public void RemovePosition(TileMapObject.Context context)
        {
            System.Diagnostics.Debug.Assert(this.KnownPositions.ContainsKey(context));

            var prevPos = this.KnownPositions[context];
            this.KnownPositions.Remove(context);
            foreach (var dir in context.BlockingCells)
            {
                var cell = prevPos + dir;
                System.Diagnostics.Debug.Assert(this.Map.ContainsKey(cell));
                this.Map[cell] = null;
            }
        }

        public void ClearMap()
        {
            this.Map.Clear();
            this.KnownPositions.Clear();
        }

        public override string ToString()
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var cell in Map.Keys)
            {
                minX = Math.Min(minX, cell.x);
                minY = Math.Min(minY, cell.y);
                maxX = Math.Max(maxX, cell.x);
                maxY = Math.Max(maxY, cell.y);
            }

            var sb = new StringBuilder();
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    sb.Append(Map.ContainsKey(new Vector2(x, y)) ? "#" : ".");
                }
                sb.AppendLine("");
            }

            return sb.ToString();
        }

        public List<Vector2> FindPath(Vector2 from, Vector2 to)
        {
            var fromMap = this.WorldToMap(from);
            var toMap = this.WorldToMap(to);

            var pathMap = AStarPathfinder.Search(this, fromMap, toMap);
            if (pathMap == null)
            {
                return pathMap;
            }

            return pathMap.Select(a => this.MapToWorld(a)).ToList();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public void InitContext(int width, int height)
    {
        this.context = this.context ?? new Context(this.WorldToMap, (map) => this.MapToWorld(map));

        foreach (var child in GetChildren())
        {
            if (child is TileMapObject tile)
            {
                tile.InitContext(this.context);
            }
        }
    }
}
