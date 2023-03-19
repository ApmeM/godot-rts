// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System.Collections.Generic;
using HonkPerf.NET.RefLinq.Enumerators;
using LocomotorECS;

namespace HonkPerf.NET.RefLinq
{

    public static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, IReadOnlyListEnumerator<T>> ToRefLinq<T>(this IReadOnlyList<T> c)
            => new RefLinqEnumerable<T, IReadOnlyListEnumerator<T>>(new IReadOnlyListEnumerator<T>(c));
        public static RefLinqEnumerable<T, ArrayEnumerator<T>> ToRefLinq<T>(this T[] c)
            => new RefLinqEnumerable<T, ArrayEnumerator<T>>(new ArrayEnumerator<T>(c));
        public static RefLinqEnumerable<int, RangeEnumerator> Range(int start, int count)
            => new RefLinqEnumerable<int, RangeEnumerator>(new RangeEnumerator(start, count));
        public static RefLinqEnumerable<T, HashSetEnumerator<T>> ToRefLinq<T>(this HashSet<T> c)
            => new RefLinqEnumerable<T, HashSetEnumerator<T>>(new HashSetEnumerator<T>(c));
        public static RefLinqEnumerable<T, MultiHashSetWrapperEnumerator<T>> ToRefLinq<T>(this MultiHashSetWrapper<T> c)
            => new RefLinqEnumerable<T, MultiHashSetWrapperEnumerator<T>>(new MultiHashSetWrapperEnumerator<T>(c));
        public static RefLinqEnumerable<KeyValuePair<T, EntityListChangeNotificator>, EntityLookupEnumerator<T>> ToRefLinq<T>(this EntityLookup<T> c)
            => new RefLinqEnumerable<KeyValuePair<T, EntityListChangeNotificator>, EntityLookupEnumerator<T>>(new EntityLookupEnumerator<T>(c));
    }
}