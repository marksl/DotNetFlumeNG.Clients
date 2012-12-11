// 
//    Copyright 2013 Mark Lamley
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DotNetFlumeNG.Client.Core
{
    // Based on:
    // http://stackoverflow.com/questions/2510975/c-sharp-object-pooling-pattern-implementation
    // http://pastebin.com/he1fYC29
    public class Pool<T> : IDisposable
    {
        private readonly Func<T> _factory;
        private readonly IItemStore _itemStore;
        private readonly LoadingMode _loadingMode;
        private readonly int _size;
        private readonly Semaphore _sync;
        private int _count;
        private bool _disposed;

        public Pool(int size, Func<T> factory)
            : this(size, factory, LoadingMode.Lazy, AccessMode.FIFO)
        {
        }

        public Pool(int size, Func<T> factory,
                    LoadingMode loadingMode, AccessMode accessMode)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", size, "Argument 'size' must be greater than zero.");
            if (factory == null)
                throw new ArgumentNullException("factory");

            _size = size;
            _factory = factory;
            _sync = new Semaphore(size, size);
            _loadingMode = loadingMode;
            _itemStore = CreateItemStore(accessMode, size);
            if (loadingMode == LoadingMode.Eager)
            {
                PreloadItems();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Acquisition

        private T AcquireEager()
        {
            lock (_itemStore)
            {
                return _itemStore.Fetch();
            }
        }

        private T AcquireLazy()
        {
            lock (_itemStore)
            {
                if (_itemStore.Count > 0)
                {
                    return _itemStore.Fetch();
                }
            }
            Interlocked.Increment(ref _count);
            return _factory();
        }

        private T AcquireLazyExpanding()
        {
            bool shouldExpand = false;
            if (_count < _size)
            {
                int newCount = Interlocked.Increment(ref _count);
                if (newCount <= _size)
                {
                    shouldExpand = true;
                }
                else
                {
                    // Another thread took the last spot - use the store instead
                    Interlocked.Decrement(ref _count);
                }
            }

            if (shouldExpand)
            {
                return _factory();
            }

            lock (_itemStore)
            {
                return _itemStore.Fetch();
            }
        }

        private void PreloadItems()
        {
            for (int i = 0; i < _size; i++)
            {
                T item = _factory();
                _itemStore.Store(item);
            }
            _count = _size;
        }

        #endregion

        #region Collection Wrappers

        private static IItemStore CreateItemStore(AccessMode mode, int capacity)
        {
            switch (mode)
            {
                case AccessMode.FIFO:
                    return new QueueStore(capacity);
                case AccessMode.LIFO:
                    return new StackStore(capacity);
                default:
                    Debug.Assert(mode == AccessMode.Circular,
                                 "Invalid AccessMode in CreateItemStore");
                    return new CircularStore(capacity);
            }
        }

        private class CircularStore : IItemStore
        {
            private readonly List<Slot> slots;
            private int position = -1;

            public CircularStore(int capacity)
            {
                slots = new List<Slot>(capacity);
            }

            public T Fetch()
            {
                if (Count == 0)
                    throw new InvalidOperationException("The buffer is empty.");

                int startPosition = position;
                do
                {
                    Advance();
                    Slot slot = slots[position];
                    if (!slot.IsInUse)
                    {
                        slot.IsInUse = true;
                        --Count;
                        return slot.Item;
                    }
                } while (startPosition != position);
                throw new InvalidOperationException("No free slots.");
            }

            public void Store(T item)
            {
                Slot slot = slots.Find(s => Equals(s.Item, item));
                if (slot == null)
                {
                    slot = new Slot(item);
                    slots.Add(slot);
                }
                slot.IsInUse = false;
                ++Count;
            }

            public int Count { get; private set; }

            private void Advance()
            {
                position = (position + 1)%slots.Count;
            }

            private class Slot
            {
                public Slot(T item)
                {
                    Item = item;
                }

                public T Item { get; private set; }
                public bool IsInUse { get; set; }
            }
        }

        private interface IItemStore
        {
            int Count { get; }
            T Fetch();
            void Store(T item);
        }

        private class QueueStore : Queue<T>, IItemStore
        {
            public QueueStore(int capacity)
                : base(capacity)
            {
            }

            public T Fetch()
            {
                return Dequeue();
            }

            public void Store(T item)
            {
                Enqueue(item);
            }
        }

        private class StackStore : Stack<T>, IItemStore
        {
            public StackStore(int capacity)
                : base(capacity)
            {
            }

            public T Fetch()
            {
                return Pop();
            }

            public void Store(T item)
            {
                Push(item);
            }
        }

        #endregion

        public T Acquire()
        {
            _sync.WaitOne();
            switch (_loadingMode)
            {
                case LoadingMode.Eager:
                    return AcquireEager();
                case LoadingMode.Lazy:
                    return AcquireLazy();
                default:
                    Debug.Assert(_loadingMode == LoadingMode.LazyExpanding,
                                 "Unknown LoadingMode encountered in Acquire method.");
                    return AcquireLazyExpanding();
            }
        }

        public void ReturnToPool(T item)
        {
            lock (_itemStore)
            {
                _itemStore.Store(item);
            }

            Release();
        }

        public void Release()
        {
            _sync.Release();
        }

        ~Pool()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (typeof (IDisposable).IsAssignableFrom(typeof (T)))
                    {
                        lock (_itemStore)
                        {
                            while (_itemStore.Count > 0)
                            {
                                var disposable = (IDisposable) _itemStore.Fetch();
                                disposable.Dispose();
                            }
                        }
                    }
                    _sync.Close();
                }
            }

            _disposed = true;
        }
    }
}