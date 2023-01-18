using Godot;
using System.Collections.Generic;
using System.Linq;

public class ItemTypeCache
{

    public enum MapItemType
    {
        Water,
        Construction
    }

    public Dictionary<MapItemType, HashSet<PositionComponent>> ItemsByType = new Dictionary<MapItemType, HashSet<PositionComponent>>();

    public void AddItemByType(MapItemType itemType, PositionComponent item)
    {
        if (!ItemsByType.ContainsKey(itemType))
        {
            ItemsByType[itemType] = new HashSet<PositionComponent>();
        }

        ItemsByType[itemType].Add(item);
    }

    public void RemoveItemByType(MapItemType itemType, PositionComponent item)
    {
        if (!ItemsByType.ContainsKey(itemType))
        {
            ItemsByType[itemType] = new HashSet<PositionComponent>();
            return;
        }

        ItemsByType[itemType].Remove(item);
    }

    public PositionComponent FindClosestItemByType(MapItemType itemType, Vector2 position)
    {
        if (!ItemsByType.ContainsKey(itemType))
        {
            ItemsByType[itemType] = new HashSet<PositionComponent>();
            return null;
        }

        return ItemsByType[itemType].OrderBy(a => (a.Position - position).LengthSquared()).FirstOrDefault();
    }

    public bool ItemByTypeExists(MapItemType itemType)
    {
        if (!ItemsByType.ContainsKey(itemType))
        {
            ItemsByType[itemType] = new HashSet<PositionComponent>();
            return false;
        }

        return ItemsByType[itemType].Any();
    }
}
