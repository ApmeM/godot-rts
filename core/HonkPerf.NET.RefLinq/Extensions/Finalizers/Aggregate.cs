// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{
    static partial class ActiveLinqExtensions
    {
        public static TAccumulate Aggregate<T, TAccumulate, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, TAccumulate acc, Func<TAccumulate, T, TAccumulate> agg)
            where TPrevious : IRefEnumerator<T>
        {
            var res = acc;
            foreach (var el in prev)
                res = agg.Invoke(res, el);
            return res;
        }

        public static T Aggregate<T, TPrevious>(this RefLinqEnumerable<T, TPrevious> prev, Func<T, T, T> agg)
            where TPrevious : IRefEnumerator<T>
        {
            if (!prev.enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");
            var c = prev.enumerator.Current;

            while (prev.enumerator.MoveNext())
                c = agg.Invoke(c, prev.enumerator.Current);

            return c;
        }
    }
}