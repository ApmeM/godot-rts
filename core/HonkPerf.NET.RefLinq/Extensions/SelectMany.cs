// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    static partial class LazyLinqExtensions
    {
        public static RefLinqEnumerable<T, SelectMany<T, TEnumerator, TEnumeratorOfEnumerators>> SelectMany<T, TEnumerator, TEnumeratorOfEnumerators>(this RefLinqEnumerable<RefLinqEnumerable<T, TEnumerator>, TEnumeratorOfEnumerators> prev)
            where TEnumerator : IRefEnumerator<T>
            where TEnumeratorOfEnumerators : IRefEnumerator<RefLinqEnumerable<T, TEnumerator>>
            => new RefLinqEnumerable<T, SelectMany<T, TEnumerator, TEnumeratorOfEnumerators>>(new SelectMany<T, TEnumerator, TEnumeratorOfEnumerators>(prev.enumerator));

        public static RefLinqEnumerable<U, SelectMany<U, TUEnumerator, Select<T, RefLinqEnumerable<U, TUEnumerator>, TEnumerator>>> SelectMany<T, U, TEnumerator, TUEnumerator>(this RefLinqEnumerable<T, TEnumerator> prev, Func<T, RefLinqEnumerable<U, TUEnumerator>> func)
            where TEnumerator : IRefEnumerator<T>
            where TUEnumerator : IRefEnumerator<U>
        {
            var res = prev.Select<T, RefLinqEnumerable<U, TUEnumerator>, TEnumerator>(func);
            var r = res.SelectMany();
            return r;
        }
    }
}