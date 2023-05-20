using System;

namespace CoEvent
{
    public interface IPool
    {
        public object Allocate(Type type);
        public void Recycle(Type type, object item);
    }
}
