using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvents
{
    public class CoDefaultPool : IPool
    {
        private Dictionary<Type, System.Collections.Queue> pool = new Dictionary<Type, Queue>();

        public int MaxCount { get; set; } = 1000;

        public CoDefaultPool() { }
        public CoDefaultPool(int maxCount) => MaxCount = maxCount;

        public object Allocate(Type type)
        {
            if (!pool.ContainsKey(type))
            {
                pool.Add(type, new Queue());
            }
            var queue = pool[type];
            if (queue.Count == 0)
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
            if (queue.Count >= MaxCount)
            {
                queue.Enqueue(item);
            }
        }


        public T Allocate<T>()
        {
            return (T)Allocate(typeof(T));
        }
        public void Recycle<T>(object item)
        {
            Recycle(typeof(T), item);
        }


        public int GameObjectMaxCount { get; set; } = 1000;


        private Dictionary<int, Queue<GameObject>> gos = new Dictionary<int, Queue<GameObject>>();

        private Dictionary<int, Func<GameObject>> createFuncs = new Dictionary<int, Func<GameObject>>();
        private Dictionary<int, Action<GameObject>> destroyActions = new Dictionary<int, Action<GameObject>>();

        public void SetGameObjectBehaviour(int key, Func<GameObject> onCreate, Action<GameObject> onDestroy = null)
        {
            if (createFuncs.ContainsKey(key))
            {
                createFuncs[key] = onCreate;
                destroyActions[key] = onDestroy;
            }
            else
            {
                createFuncs.Add(key, onCreate);
                destroyActions.Add(key, onDestroy);
            }
        }

        public GameObject AllocateGameObject(int key)
        {
            if (!createFuncs.ContainsKey(key)) throw new InvalidOperationException("Please set CreateFunc before call Allocate");
            if (!gos.ContainsKey(key))
            {
                gos.Add(key, new Queue<GameObject>());
            }
            if (gos[key].Count < 0) gos[key].Enqueue(createFuncs[key]());

            return gos[key].Dequeue();
        }

        public void RecycleGameObject(int key, GameObject go)
        {
            if (!createFuncs.ContainsKey(key)) throw new InvalidOperationException("Please set DestroyAction before call Recycle");
            if (!gos.ContainsKey(key))
            {
                gos.Add(key, new Queue<GameObject>());
            }
            if (gos[key].Count >= GameObjectMaxCount) destroyActions[key]?.Invoke(go);
            else gos[key].Enqueue(go);
        }

    }
}
