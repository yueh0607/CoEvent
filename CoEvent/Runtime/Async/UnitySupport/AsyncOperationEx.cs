using CoEvent.Async;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvent.Async
{
#if UNITY_2017_1_OR_NEWER
    public static class AsyncOperationEx
    {
        public static CoTask<AsyncOperation> GetAwaiter(this AsyncOperation operation)
        {
            var task = CoTask<AsyncOperation>.Create();
            operation.completed += task.SetResult;
            return task;

        }
        public static AsyncOperation WithProgress(this AsyncOperation operation,Action<float> progress)
        {
            Action<float> callback = (x) =>
            {
                progress?.Invoke(operation.progress);
            };
            operation.completed += (x) =>
            {
                CoEvents.Instance.Operator<IUpdate>().UnSubscribe(callback);
            };
            CoEvents.Instance.Operator<IUpdate>().Subscribe(callback);
            return operation;
        }
    }
#endif
}
