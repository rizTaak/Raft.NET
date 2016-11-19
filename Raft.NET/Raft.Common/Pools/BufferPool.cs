using ssc = System.ServiceModel.Channels;

namespace Raft.Common.Pools
{
    public class BufferPool : IBufferPool
    {
        private readonly ssc.BufferManager _internalBufferManager;

        public BufferPool(long maxBufferPoolSize, int maxBufferSize)
        {
            _internalBufferManager = 
                ssc.BufferManager.CreateBufferManager(maxBufferPoolSize, maxBufferSize);
        }

        public byte[] Take(int bufferSize)
        {
            return _internalBufferManager.TakeBuffer(bufferSize);
        }

        public void Return(byte[] buffer)
        {
            _internalBufferManager.ReturnBuffer(buffer);
        }
    }
}
