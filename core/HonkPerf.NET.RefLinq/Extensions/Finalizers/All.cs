// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{
    public static partial class ActiveLinqExtensions
    {
        public static bool All<T, TDelegate, TEnumerator>(this RefLinqEnumerable<T, TEnumerator> seq, Func<T, bool> pred)
            where TEnumerator : IRefEnumerator<T>
        {
            foreach (var el in seq)
                if (!pred.Invoke(el))
                    return false;
            return true;
        }
    }
}