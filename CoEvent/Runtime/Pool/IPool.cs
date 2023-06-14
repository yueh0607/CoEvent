using System;

namespace CoEvents
{
    public interface IPool
    {
        public object Allocate(Type type);
        public void Recycle(Type type, object item);

        public T Allocate<T>()
        {
            return (T)Allocate(typeof(T));
        }
        public void Recycle<T>(object item)
        {
            Recycle(typeof(T), item);
        }
    }


}
