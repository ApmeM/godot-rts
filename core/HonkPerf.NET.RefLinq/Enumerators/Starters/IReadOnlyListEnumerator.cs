// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HonkPerf.NET.RefLinq.Enumerators
{

    public struct IReadOnlyListEnumerator<T> : IRefEnumerator<T>
    {
        private readonly IReadOnlyList<T> list;
        private int curr;

        public IReadOnlyListEnumerator(IReadOnlyList<T> list)
        {
            this.list = list;
            this.curr = -1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            curr++;
            return curr < list.Count;
        }

        public T Current => list[curr];
    }
}