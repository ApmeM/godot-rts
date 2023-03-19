// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {
        public static RefLinqEnumerable<T, Append<T, TPrevious>> Append<T, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, T toAppend)
            where TPrevious : IRefEnumerator<T>
            => new RefLinqEnumerable<T, Append<T, TPrevious>>(new Append<T, TPrevious>(prev.enumerator, toAppend));
    }
}