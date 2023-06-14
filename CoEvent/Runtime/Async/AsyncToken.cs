/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 提供异步令牌给用户进行操作，方便用户挂起，结束异步任务
 */


using CoEvents.Async.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvents.Async
{

    /// <summary>
    /// 每个小任务都只能有这三种状态
    /// </summary>

    public enum AsyncStatus
    {
        //等待ing
        Pending,
        //挂起
        Yield,
        //结束
        Completed
    }
    public sealed class AsyncToken
    {

        public event Action OnCanceled = null;

        public static AsyncToken Create()
        {
            AsyncToken token;
            if (CoEvent.Pool != null) token = (AsyncToken)CoEvent.Pool.Allocate(typeof(AsyncToken));
            else token = new AsyncToken();
            token.Status = AsyncStatus.Pending;
            token.node = default;
            return token;
        }
        public static void Recycle(AsyncToken token)
        {
            token.Status = AsyncStatus.Pending;
            token.node = default;
            if (CoEvent.Pool != null) CoEvent.Pool.Recycle(typeof(AsyncToken), token);

        }


        internal AsyncTreeTokenNode node;

        public AsyncStatus Status { get; private set; } = AsyncStatus.Pending;

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Yield()
        {
            if (Status == AsyncStatus.Completed) throw new InvalidOperationException("尝试挂起已经结束的任务是无效的");
            Status = AsyncStatus.Yield;
            node.Yield();
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Continue()
        {
            if (Status == AsyncStatus.Completed) throw new InvalidOperationException("尝试取消已经结束的任务是无效的");
            Status = AsyncStatus.Pending;
            node.Continue();
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            if (Status == AsyncStatus.Completed) throw new InvalidOperationException();
            Status = AsyncStatus.Completed;
            node.Cancel();
            OnCanceled?.Invoke();
        }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Recycle() => AsyncToken.Recycle(this);

    }
}
