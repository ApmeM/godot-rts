// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{
    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, OrderBy<T, TPrevious, TKey>> OrderBy<T, TPrevious, TKey>(this RefLinqEnumerable<T, TPrevious> prev, Func<T, TKey> keySelector)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<T, OrderBy<T, TPrevious, TKey>>(new OrderBy<T, TPrevious, TKey>(prev.enumerator, keySelector));

    }
}