// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using System.Runtime.CompilerServices;

namespace HonkPerf.NET.RefLinq.Enumerators
{

    public struct Where<T, TEnumerator>
        : IRefEnumerator<T>
        where TEnumerator : IRefEnumerator<T>
    {
        public Where(TEnumerator prev, Func<T, bool> map)
        {
            this.prev = prev;
            this.map = map;
        }
        private TEnumerator prev;
        private readonly Func<T, bool> map;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
        tryAgain:
            if (!prev.MoveNext())
                return false;
            if (!map.Invoke(prev.Current))
                goto tryAgain;
            return true;
        }
        public T Current => prev.Current;

        public Where<T, TEnumerator> GetEnumerator() => this;
    }
}