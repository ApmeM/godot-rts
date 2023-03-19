// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{
    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, Take<T, TPrevious>> Take<T, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, int toTake)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<T, Take<T, TPrevious>>(new Take<T, TPrevious>(prev.enumerator, toTake));

    }
}