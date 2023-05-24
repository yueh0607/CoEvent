using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEvents.Async
{
    public static class Coroutine_Ex
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        public static void Coroutine(this ICoTask task)
        {
            // empty 
        }

        public static async CoTask Invoke(this CoTask task)
        {
            await task;
        }
        public static async CoTask<T> Invoke<T>(this CoTask<T> task)
        {
            return await task;
        }
       

    }
}
