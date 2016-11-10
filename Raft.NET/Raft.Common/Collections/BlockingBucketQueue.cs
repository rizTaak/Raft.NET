using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Raft.Common.Collections
{
    public class BlockingBucketQueue<T> : IBlockingBucketQueue<T>
    {
        private bool _isDisposed;
        private readonly BlockingCollection<T>[] _buckets;

        public BlockingBucketQueue(int bucketsCount, int bucketCapacity = 1000)
        {
            if(bucketsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(bucketsCount));
            if(bucketCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(bucketCapacity));

            _buckets = new BlockingCollection<T>[bucketsCount];
            foreach (var index in Enumerable.Range(0, _buckets.Length))
                _buckets[index] = new BlockingCollection<T>(bucketCapacity);
        }
        
        public void Enqueue(int bucketIndex, T item)
        {
            VerifyDisposed();
            VerifyBucketIndex(bucketIndex);
        }

        public T Dequeue()
        {
            VerifyDisposed();
            T fetched;
            BlockingCollection<T>.TakeFromAny(_buckets, out fetched);
            return fetched;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            foreach (var bucket in _buckets)
                bucket.Dispose();

            _isDisposed = true;
        }

        private void VerifyBucketIndex(int bucketIndex)
        {
            if(bucketIndex >= _buckets.Length)
                throw new ArgumentOutOfRangeException(nameof(bucketIndex));
        }

        private void VerifyDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(BlockingBucketQueue<T>));
        }
    }
}
