// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {
        public static T SingleOrDefault<T, TEnumerator>(this RefLinqEnumerable<T, TEnumerator> seq)
            where TEnumerator : IRefEnumerator<T>
        {
            if (!seq.enumerator.MoveNext())
                return default(T);
            var res = seq.enumerator.Current;
            if (seq.enumerator.MoveNext())
                return default(T);
            return res;
        }
    }
}