using System.Linq.Struct;
using Leopotam.EcsLite;

public static class Builder
{
    public static RefLinqEnumerable<int, LeopotamEnumerator> Build(this EcsFilter c)
        => new RefLinqEnumerable<int, LeopotamEnumerator>(new LeopotamEnumerator(c));
}

public struct LeopotamEnumerator : IRefEnumerator<int>
{
    private EcsFilter.Enumerator ie;

    public LeopotamEnumerator(EcsFilter set)
    {
        this.ie = set.GetEnumerator();
        this.Current = default;
    }

    public bool MoveNext()
    {
        bool t = this.ie.MoveNext();
        this.Current = this.ie.Current;
        if (!t)
            this.ie.Dispose();
        return t;
    }

    public int Current { get; private set; }
}