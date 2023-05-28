//#define CoEvent_Addressable_Enable


using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#if CoEvent_Addressable_Enable
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif
namespace CoEvents.Async
{
#if CoEvent_Addressable_Enable
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
#endif
}
