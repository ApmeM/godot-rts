// Copyright (c) Angouri 2021.
// This file from HonkPerf.NET project is MIT-licensed.
// Read more: https://github.com/asc-community/HonkPerf.NET

using HonkPerf.NET.RefLinq.Enumerators;

namespace HonkPerf.NET.RefLinq
{

    public static partial class ActiveLinqExtensions
    {

        public static int Multiply<TEnumerator>(this RefLinqEnumerable<int, TEnumerator> seq)
            where TEnumerator : IRefEnumerator<int>
        {
            var result = 1;
            foreach (var el in seq)
                result += el;
            return result;
        }    }
}