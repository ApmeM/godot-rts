namespace System.Numerics
{
    public static class Vector2Ext
    {
        public static Vector2 Inf = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        public static Vector2 Up = new Vector2(0, -1);
        public static Vector2 Down = new Vector2(0, 1);
        public static Vector2 Left = new Vector2(-1, 0);
        public static Vector2 Right = new Vector2(1, 0);
        public static Vector2 Normalized(this Vector2 original)
        {
            return Vector2.Normalize(original);
        }
    }
}