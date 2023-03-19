// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, Skip<T, TPrevious>> Skip<T, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, int toSkip)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<T, Skip<T, TPrevious>>(new Skip<T, TPrevious>(prev.enumerator, toSkip));
    }
}