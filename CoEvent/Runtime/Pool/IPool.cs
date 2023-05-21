using System;
using System.Collections;
using System.Collections.Generic;

namespace CoEvents
{
    public interface IPool
    {
        public object Allocate(Type type);
        public void Recycle(Type type, object item);
    }

    public class CoDefaultPool:IPool
    {
        private Dictionary<Type, System.Collections.Queue> pool = new Dictionary<Type,Queue>();

        public int MaxCount { get; set; } = 1000;

        public CoDefaultPool() { }
        public CoDefaultPool(int maxCount)=> MaxCount=maxCount;

        public object Allocate(Type type)
        {
            if(!pool.ContainsKey(type))
            {
                pool.Add(type, new Queue());
            }
            var queue = pool[type];
            if(queue.Count==0)
            {
                queue.Enqueue(Activator.CreateInstance(type));
            }
            return queue.Dequeue();
        }

        public void Recycle(Type type, object item)
        {
            if (!pool.ContainsKey(type))
            {
                pool.Add(type, new Queue());
            }
            var queue = pool[type];
            if (queue.Count>=MaxCount)
            {
                queue.Enqueue(item);
            }
        }
    }
}
