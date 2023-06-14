using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvents.Async
{
    public static partial class CoTask_Ex
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Discard(this ICoTask task)
        {
            // empty 
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async CoTask Invoke(this CoTask task)
        {
            await task;
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async CoTask<T> Invoke<T>(this CoTask<T> task)
        {
            return await task;
        }


        /// <summary>
        /// 为指定异步任务设置令牌，可以取消和挂起
        /// </summary>
        /// <param name="task"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoTask WithToken(this CoTask task, out AsyncToken token)
        {
            var tok = AsyncToken.Create();
            tok.node = task.Token;
            token = tok;
            return task;
        }

        /// <summary>
        /// 为指定异步任务设置令牌，可以取消和挂起
        /// </summary>
        /// <param name="task"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoTask<T> WithToken<T>(this CoTask<T> task, out AsyncToken token, Action cancelCallback = null)
        {
            var tok = AsyncToken.Create();
            tok.node = task.Token;
            token = tok;
            if (cancelCallback != null) tok.OnCanceled += cancelCallback;
            return task;
        }




    }
}
