using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HonkPerf.NET.RefLinq.Enumerators
{
    public struct MultiHashSetWrapperEnumerator<T> : IRefEnumerator<T>
    {
        private T curr;
        private MultiHashSetWrapper<T> set;
        private HashSet<T>.Enumerator ie;
        private bool initialized;

        public MultiHashSetWrapperEnumerator(MultiHashSetWrapper<T> wrapper)
        {
            set = wrapper;
            curr = default;
            ie = default;
            initialized = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (!initialized)
            {
                ie = set.GetEnumerator();
                initialized = true;
            }

            bool t = ie.MoveNext();
            curr = ie.Current;
            if (!t)
                ie.Dispose();
            return t;
        }

        public T Current => curr;
    }
}