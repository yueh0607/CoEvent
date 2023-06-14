
//打开下面这行注释即可支持Addressable操作完成的等待

//#define CoEvent_Addressable_Enable

#if CoEvent_Async_Addressable_Enable
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CoEvents.Async
{
    public static class AddressableEx 
    {
        public static CoTask<AsyncOperationHandle> GetAwaiter(this AsyncOperationHandle handle)
        {
            var task = CoTask<AsyncOperationHandle>.Create();
            handle.Completed += task.SetResult;
            return task;
        }
        public static CoTask<AsyncOperationHandle<T>> GetAwaiter<T>(this AsyncOperationHandle<T> handle)
        {
            var task = CoTask<AsyncOperationHandle<T>>.Create();
            handle.Completed += task.SetResult;
            return task;
        }
    }

}
#endif