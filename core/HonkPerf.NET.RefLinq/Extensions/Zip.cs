// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<(T1, T2), Zip<T1, T2, TEnumerator1, TEnumerator2>> Zip<T1, T2, TEnumerator1, TEnumerator2>(
            this RefLinqEnumerable<T1, TEnumerator1> seq1, RefLinqEnumerable<T2, TEnumerator2> seq2)
            where TEnumerator1 : IRefEnumerator<T1>
            where TEnumerator2 : IRefEnumerator<T2>
            => new RefLinqEnumerable<(T1, T2), Zip<T1, T2, TEnumerator1, TEnumerator2>>(new Zip<T1, T2, TEnumerator1, TEnumerator2>(seq1.enumerator, seq2.enumerator));
    }
}