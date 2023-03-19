// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {
        public static int Count<T, TEnumerator>(this RefLinqEnumerable<T, TEnumerator> seq)
            where TEnumerator : IRefEnumerator<T>
        {
            var c = 0;
            foreach (var _ in seq)
                c++;
            return c;
        }
    }
}