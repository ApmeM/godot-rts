// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, Where<T, TPrevious>> Where<T, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, Func<T, bool> predicate)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<T, Where<T, TPrevious>>(new Where<T, TPrevious>(prev.enumerator, predicate));
    }
}