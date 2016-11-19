using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raft.Common.Pools
{
    public interface IObjectPool<T>
    {
        T Take();
        void Return(T obj);
    }
}
