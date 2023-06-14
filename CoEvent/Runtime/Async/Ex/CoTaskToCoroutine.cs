using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvents.Async
{
    public partial class CoTask
    {
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator ToCoroutine(Func<CoTask> method)
        {
            if (method == null) throw new ArgumentNullException("method null ");
            CoTask task = method();
            while (!task.IsCompleted) yield return null;
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> ToCoroutine<T>(Func<CoTask<T>> method)
        {
            if (method == null) throw new ArgumentNullException("method null ");
            CoTask<T> task = method();
            while (!task.IsCompleted) yield return default;
        }


    }
}
