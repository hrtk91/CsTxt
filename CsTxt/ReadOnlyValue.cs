using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt
{
    public class ReadOnlyValue<T> : IDisposable
    {
        private string Id { get; set; }
        public T Value { get; private set; }
        private IDictionary<string, ReadOnlyValue<T>> Pool { get; set; }

        private ReadOnlyValue(IDictionary<string, ReadOnlyValue<T>> pool, T value)
        {
            Id = Guid.NewGuid().ToString();
            Pool = pool;
            Value = value;

            if (pool.Keys.Contains(Id))
            {
                throw new Exception();
            }
            else
            {
                pool.Add(Id, this);
            }
        }

        public void Dispose()
        {
            if (Pool.Keys.Contains(Id))
            {
                Pool.Remove(Id);
            }
        }

        public static ReadOnlyValue<T> FromValue(IDictionary<string, ReadOnlyValue<T>> pool, T value)
        {
            return new ReadOnlyValue<T>(pool, value);
        }
    }
}
