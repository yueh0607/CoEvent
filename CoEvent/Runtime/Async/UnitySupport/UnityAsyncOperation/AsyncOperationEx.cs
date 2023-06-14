

//打开这行注释以支持DoTween完成的等待

#define CoEvent_Async_AsyncOperation_Enable

#if CoEvent_Async_AsyncOperation_Enable

using System;
using UnityEngine;

namespace CoEvents.Async
{

    public static class AsyncOperationEx
    {
        public static CoTask<AsyncOperation> GetAwaiter(this AsyncOperation operation)
        {
            var task = CoTask<AsyncOperation>.Create();
            operation.completed += task.SetResult;
            return task;

        }
        public static AsyncOperation WithProgress(this AsyncOperation operation, Action<float> progress)
        {
            Action<float> callback = (x) =>
            {
                progress?.Invoke(operation.progress);
            };
            operation.completed += (x) =>
            {
                CoEvent.Instance.Operator<IUpdate>().UnSubscribe(callback);
            };
            CoEvent.Instance.Operator<IUpdate>().Subscribe(callback);
            return operation;
        }



    }
}
#endif