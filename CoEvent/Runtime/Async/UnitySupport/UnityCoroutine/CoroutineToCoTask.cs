using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace CoEvents.Async
{
    public static partial class CoTask_Ex
    {


        //[DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
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
