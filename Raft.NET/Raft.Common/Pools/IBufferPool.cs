using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raft.Common.Pools
{
    public interface IBufferPool
    {
        byte[] Take(int bufferSize);
        void Return(byte[] buffer);
    }
}
