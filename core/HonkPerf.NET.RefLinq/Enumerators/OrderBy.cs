// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HonkPerf.NET.RefLinq.Enumerators
{

    public struct OrderBy<T, TEnumerator, TKey>
        : IRefEnumerator<T>
        where TEnumerator : IRefEnumerator<T>
    {
        internal OrderBy(TEnumerator prev, Func<T, TKey> keySelector)
        {
            this.prev = prev;
            this.keySelector = keySelector;
            Current = default(T);
            sortList = new List<T>();
            initialized = false;
            sortListEnumerator = default;
            comparer = (a, b) => Comparer<TKey>.Default.Compare(keySelector(a), keySelector(b));
        }
        
        private TEnumerator prev;
        private readonly Func<T, TKey> keySelector;
        private readonly List<T> sortList;
        private List<T>.Enumerator sortListEnumerator;
        private bool initialized;
        private Comparison<T> comparer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (!initialized)
            {
                initialized = true;
                sortList.Clear();
                while (prev.MoveNext())
                {
                    sortList.Add(prev.Current);
                }
                sortList.Sort(comparer);
                sortListEnumerator = sortList.GetEnumerator();
            }

            var res = sortListEnumerator.MoveNext();
            if (res)
                Current = sortListEnumerator.Current;
            return res;
        }
        public T Current { get; private set; }

        public OrderBy<T, TEnumerator, TKey> GetEnumerator() => this;
    }
}