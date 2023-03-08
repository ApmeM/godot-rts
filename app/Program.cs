using System.Numerics;

public class Program
{
    public static void Main(string[] args)
    {
        var w = new World(a => a, a => a);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.House, 0)).GetComponent<PositionComponent>().Position = new Vector2(5, 5);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Well, 0)).GetComponent<PositionComponent>().Position = new Vector2(5, 3);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.ArtificialWell, 0)).GetComponent<PositionComponent>().Position = new Vector2(2, 5);

        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(7, 7);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(7, 8);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(7, 9);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(8, 7);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(8, 8);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(8, 9);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(9, 7);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(9, 8);
        w.el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, 0)).GetComponent<PositionComponent>().Position = new Vector2(9, 9);
        w.BuildFence(60, 1, 1);

        for (var i = 0; i < 200; i++)
        {
            w.Process(0.1f);
        }
    }
}