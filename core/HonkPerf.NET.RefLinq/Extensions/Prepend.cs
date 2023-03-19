// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {
        public static RefLinqEnumerable<T, Prepend<T, TPrevious>> Prepend<T, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, T toPrepend)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<T, Prepend<T, TPrevious>>(new Prepend<T, TPrevious>(prev.enumerator, toPrepend));
    }
}