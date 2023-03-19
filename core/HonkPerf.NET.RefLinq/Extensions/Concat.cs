// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, Concat<T, TEnumerator1, TEnumerator2>> Concat<T, TEnumerator1, TEnumerator2>(
            this RefLinqEnumerable<T, TEnumerator1> seq1, RefLinqEnumerable<T, TEnumerator2> seq2)
            where TEnumerator1 : IRefEnumerator<T>
            where TEnumerator2 : IRefEnumerator<T>
            => new RefLinqEnumerable<T, Concat<T, TEnumerator1, TEnumerator2>>(new Concat<T, TEnumerator1, TEnumerator2>(seq1.enumerator, seq2.enumerator));


    }
}