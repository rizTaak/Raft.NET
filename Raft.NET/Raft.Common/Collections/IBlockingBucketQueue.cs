using System;
using System.ServiceModel.Channels;

namespace Raft.Common.Collections
{
    public interface IBlockingBucketQueue<T> : IDisposable
    {   
        void Enqueue(int bucketIndex, T item);
        T Dequeue();
    }
}
