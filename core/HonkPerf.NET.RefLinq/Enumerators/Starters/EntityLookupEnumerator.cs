using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LocomotorECS;

namespace HonkPerf.NET.RefLinq.Enumerators
{
    public struct EntityLookupEnumerator<T> : IRefEnumerator<KeyValuePair<T, EntityListChangeNotificator>>
    {
        private KeyValuePair<T, EntityListChangeNotificator> curr;
        private Dictionary<T, EntityListChangeNotificator>.Enumerator ie;

        public EntityLookupEnumerator(EntityLookup<T> set)
        {
            ie = set.GetEnumerator();
            curr = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            bool t = ie.MoveNext();
            curr = ie.Current;
            if (!t)
                ie.Dispose();
            return t;
        }

        public KeyValuePair<T, EntityListChangeNotificator> Current => curr;
    }
}