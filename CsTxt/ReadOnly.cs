using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt
{
    public static class ReadOnly<T>
    {
        private static IDictionary<string, ReadOnlyValue<T>> Pool { get; } = new Dictionary<string, ReadOnlyValue<T>>();

        public static ReadOnlyValue<T> Register(T value)
        {
            return ReadOnlyValue<T>.FromValue(Pool, value);
        }
    }
}
