/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 创建IAsyncTask来统一任务在awaiter标准以外的特殊行为
 */


using System;
namespace CoEvent.Async
{


    /// <summary>
    /// 异步任务接口
    /// </summary>
    public interface IAsyncTask : ICriticalAwaiter
    {
        /// <summary>
        /// 结束当前任务
        /// </summary>
        Action SetResult { get; }

        /// <summary>
        /// 当有异常时调用
        /// </summary>
        /// <param name="exception"></param>
        void SetException(Exception exception);


    }
    /// <summary>
    /// 异步任务接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncTask<T> : ICriticalAwaiter<T>
    {
        /// <summary>
        /// 结束当前任务
        /// </summary>
        Action<T> SetResult { get; }

        /// <summary>
        /// 当有异常时调用
        /// </summary>
        /// <param name="exception"></param>
        void SetException(Exception exception);



    }
}
