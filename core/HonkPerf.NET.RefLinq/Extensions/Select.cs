// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<U, Select<T, U, TPrevious>> Select<T, U, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, Func<T, U> map)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<U, Select<T, U, TPrevious>>(new Select<T, U, TPrevious>(prev.enumerator, map));
    }
}