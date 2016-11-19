using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raft.Common.Pools
{
    internal class StackObjectPool<T> : IObjectPool<T> where T : new()
    {
        private readonly SpinLock _stackLock;
        private readonly Stack<T> _stack;

        public StackObjectPool(int initialSize)
        {
            if (initialSize < 0) throw new ArgumentException(nameof(initialSize));

            _stackLock = new SpinLock();
            _stack = new Stack<T>();
            
            while (initialSize > 0)
            {
                _stack.Push(new T());
                initialSize--;
            }
        }

        public T Take()
        {
            var lockFlag = false;
            try
            {
                _stackLock.Enter(ref lockFlag);
                return _stack.Count == 0 ? new T() : _stack.Pop();
            }
            finally
            {
                if(lockFlag) _stackLock.Exit();   
            }
        }

        public void Return(T obj)
        {
            if (obj != null)
            {
                var lockFlag = false;
                try
                {
                    _stackLock.Enter(ref lockFlag);
                    _stack.Push(obj);
                }
                finally
                {
                    if (lockFlag) _stackLock.Exit();
                }
            }
        }
    }
}
