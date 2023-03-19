// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using System.Runtime.CompilerServices;

namespace HonkPerf.NET.RefLinq.Enumerators
{

    public struct Select<T, U, TEnumerator>
        : IRefEnumerator<U>
        where TEnumerator : IRefEnumerator<T>
    {
        internal Select(TEnumerator prev, Func<T,U> map)
        {
            this.prev = prev;
            this.map = map;
            Current = default(U);
        }
        private TEnumerator prev;
        private readonly Func<T,U> map;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var res = prev.MoveNext();
            if (res)
                Current = map.Invoke(prev.Current);
            return res;
        }
        public U Current { get; private set; }

        public Select<T, U, TEnumerator> GetEnumerator() => this;
    }
}