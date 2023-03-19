﻿// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System.Runtime.CompilerServices;

namespace HonkPerf.NET.RefLinq.Enumerators
{

    public struct ArrayEnumerator<T> : IRefEnumerator<T>
    {
        private readonly T[] array;
        private int curr;

        public ArrayEnumerator(T[] array)
        {
            this.array = array;
            this.curr = -1;
            Current = default(T);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            curr++;
            if (curr < array.Length)
            {
                Current = array[curr];
                return true;
            }
            return false;
        }

        public T Current { get; private set; }
    }
}