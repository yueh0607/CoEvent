
using System;
namespace CoEvent
{
    public static partial class MessageExtensions
    {  /// <summary>
       /// 发布
       /// </summary>
       /// <param name="container"></param>
        public static void Send(this ICoVarOperator<ISendEvent> container)
        {
            var mop = container.GetOperator();
            while (mop.GetNext(out var current))
            {
                ((Action)current).Invoke();
            }
            mop.Reset();
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void Send<T1>(this ICoVarOperator<ISendEvent<T1>> container, T1 arg1)
        {
            var mop = container.GetOperator();
            while (mop.GetNext(out var current))
            {
                ((Action<T1>)current).Invoke(arg1);
            }
            mop.Reset();
        }
        public static void Send<T1, T2>(this ICoVarOperator<ISendEvent<T1, T2>> container, T1 arg1, T2 arg2)
        {
            var mop = container.GetOperator();
            while (mop.GetNext(out var current))
            {
                ((Action<T1, T2>)current).Invoke(arg1, arg2);
            }
            mop.Reset();
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void Send<T1, T2, T3>(this ICoVarOperator<ISendEvent<T1, T2, T3>> container, T1 arg1, T2 arg2, T3 arg3)
        {
            var mop = container.GetOperator();
            while (mop.GetNext(out var current))
            {
                ((Action<T1, T2, T3>)current).Invoke(arg1, arg2, arg3);
            }
            mop.Reset();
        }
        public static void Send<T1, T2, T3, T4>(this ICoVarOperator<ISendEvent<T1, T2, T3, T4>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var mop = container.GetOperator();
            while (mop.GetNext(out var current))
            {
                ((Action<T1, T2, T3, T4>)current).Invoke(arg1, arg2, arg3, arg4);
            }
            mop.Reset();
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void Send<T1, T2, T3, T4, T5>(this ICoVarOperator<ISendEvent<T1, T2, T3, T4, T5>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var mop = container.GetOperator();
            while (mop.GetNext(out var current))
            {
                ((Action<T1, T2, T3, T4, T5>)current).Invoke(arg1, arg2, arg3, arg4, arg5);
            }
            mop.Reset();
        }
    }
}
