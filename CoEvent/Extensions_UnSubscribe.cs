﻿using System;
namespace CoEvent
{
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe(this ICoVarOperator<IGenericEvent> container, Action message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1>(this ICoVarOperator<IGenericEvent<T1>> container, Action<T1> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2>(this ICoVarOperator<IGenericEvent<T1, T2>> container, Action<T1, T2> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2, T3>(this ICoVarOperator<IGenericEvent<T1, T2, T3>> container, Action<T1, T2, T3> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2, T3, T4>(this ICoVarOperator<IGenericEvent<T1, T2, T3, T4>> container, Action<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>


        public static void UnSubscribe<T1, T2, T3, T4, T5>(this ICoVarOperator<IGenericEvent<T1, T2, T3, T4, T5>> container, Action<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Remove(message);
        //-----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1>(this ICoVarOperator<IGenericEvent<T1>> container, Func<T1> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2>(this ICoVarOperator<IGenericEvent<T1, T2>> container, Func<T1, T2> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2, T3>(this ICoVarOperator<IGenericEvent<T1, T2, T3>> container, Func<T1, T2, T3> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2, T3, T4>(this ICoVarOperator<IGenericEvent<T1, T2, T3, T4>> container, Func<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        public static void UnSubscribe<T1, T2, T3, T4, T5>(this ICoVarOperator<IGenericEvent<T1, T2, T3, T4, T5>> container, Func<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Remove(message);

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        public static void UnSubscribe<T1, T2, T3, T4, T5, T6>(this ICoVarOperator<IGenericEvent<T1, T2, T3, T4, T5, T6>> container, Func<T1, T2, T3, T4, T5, T6> message)
            => container.GetOperator().Events.Remove(message);
    }
}