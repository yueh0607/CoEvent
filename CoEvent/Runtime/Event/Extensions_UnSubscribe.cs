using System;
namespace CoEvents
{
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe(this ICoVarOperator<ISendEvent> container, Action message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1>(this ICoVarOperator<ISendEvent<T1>> container, Action<T1> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2>(this ICoVarOperator<ISendEvent<T1, T2>> container, Action<T1, T2> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2, T3>(this ICoVarOperator<ISendEvent<T1, T2, T3>> container, Action<T1, T2, T3> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2, T3, T4>(this ICoVarOperator<ISendEvent<T1, T2, T3, T4>> container, Action<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2, T3, T4, T5>(this ICoVarOperator<ISendEvent<T1, T2, T3, T4, T5>> container, Action<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Remove(message);
        //-----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1>(this ICoVarOperator<ICallEvent<T1>> container, Func<T1> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2>(this ICoVarOperator<ICallEvent<T1, T2>> container, Func<T1, T2> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2, T3>(this ICoVarOperator<ICallEvent<T1, T2, T3>> container, Func<T1, T2, T3> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2, T3, T4>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4>> container, Func<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2, T3, T4, T5>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4, T5>> container, Func<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Remove(message);

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        public static void UnSubscribe<T1, T2, T3, T4, T5, T6>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4, T5, T6>> container, Func<T1, T2, T3, T4, T5, T6> message)
            => container.GetOperator().Events.Remove(message);

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribeAll(this ICoVarOperator<ISendEvent> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribeAll<T1>(this ICoVarOperator<ISendEvent<T1>> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribeAll<T1, T2>(this ICoVarOperator<ISendEvent<T1, T2>> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribeAll<T1, T2, T3>(this ICoVarOperator<ISendEvent<T1, T2, T3>> container) 
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribeAll<T1, T2, T3, T4>(this ICoVarOperator<ISendEvent<T1, T2, T3, T4>> container) 
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribeAll<T1, T2, T3, T4, T5>(this ICoVarOperator<ISendEvent<T1, T2, T3, T4, T5>> container) 
            => container.GetOperator().Clear();
    }
}
