
//打开这行注释以支持CoTask与协程的转换
#define CoEvent_Async_CoTask2Coroutine_Enable



#if CoEvent_Async_CoTask2Coroutine_Enable

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvents.Async
{
    public static class CoTask2Coroutine
    {


        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CoTask GetAwaiter(this IEnumerator enumerator)
        {
            CoTask task = CoTask.Create();
            IEnumerator Temp()
            {
                yield return enumerator;
                if (task.Token.Authorization)
                    task.SetResult();
            }

            CoEvent.Mono.StartCoroutine(Temp());
            return task;
        }

    }
}
#endif