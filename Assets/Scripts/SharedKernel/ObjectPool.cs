using System;
using System.Collections.Generic;

namespace Assets.Scripts.SharedKernel
{
    public class ObjectPool<T> : IObjectPool<T>
    {
        private readonly Queue<T> _pool;
        private readonly Func<T> _factory;

        public ObjectPool(Func<T> factory, int capacity)
        {
            _factory = factory;
            _pool = new Queue<T>(capacity);

            for (int i = 0; i < capacity; i++)
                _pool.Enqueue(_factory());
        }

        public T Get()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : _factory();
        }

        public void Return(T item)
        {
            _pool.Enqueue(item);
        }
    }
}
