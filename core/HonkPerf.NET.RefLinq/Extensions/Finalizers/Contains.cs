// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {
        public static bool Contains<T, TEnumerator>(this RefLinqEnumerable<T, TEnumerator> seq, T toFind)
            where TEnumerator : IRefEnumerator<T>
        {
            foreach (var el in seq)
            {
                if (toFind != null && toFind.Equals(el) || toFind == null && el == null)
                    return true;
            }
            return false;
        }
    }
}