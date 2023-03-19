// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {
        public static T First<T, TEnumerator>(this RefLinqEnumerable<T, TEnumerator> seq)
            where TEnumerator : IRefEnumerator<T>
        {
            if (seq.enumerator.MoveNext())
                return seq.enumerator.Current;
            throw new InvalidOperationException("Sequence contains no elements");
        }
    }
}