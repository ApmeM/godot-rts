using System.Collections;
using System.Collections.Generic;

public class MultiHashSetWrapper<T>
{
    public HashSet<T> Set;

    public HashSet<T>.Enumerator GetEnumerator() => Set.GetEnumerator();
}