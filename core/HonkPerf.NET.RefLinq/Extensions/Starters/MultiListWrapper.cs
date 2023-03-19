using System.Collections;
using System.Collections.Generic;

public class MultiListWrapper<T> : IReadOnlyList<T>
{
    public List<T> data;

    public T this[int index] => data[index];

    public int Count => data.Count;

    public IEnumerator<T> GetEnumerator()
    {
        return data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return data.GetEnumerator();
    }
}