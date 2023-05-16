using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoEvent
{

    public static partial class MessageExtensions
    {
        public static List<T1> Call<T1>(this ICoVarOperator<ICallEvent<T1>> container)
        {
            var mop = container.GetOperator();
            List<T1> result = new();

            while(mop.GetNext(out var dele))
            {
                result.Add(((Func<T1>)dele).Invoke());
            }
            return result;
        }


        public static List<T2> Call<T1, T2>(this ICoVarOperator<ICallEvent<T1, T2>> container, T1 arg1)
        {
            var mop = container.GetOperator();
            List<T2> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1,T2>)dele).Invoke(arg1));
            }
            return result;
        }

        public static List<T3> Call<T1, T2, T3>(this ICoVarOperator<ICallEvent<T1, T2, T3>> container, T1 arg1, T2 arg2)
        {
            var mop = container.GetOperator();
            List<T3> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1, T2, T3>)dele).Invoke(arg1, arg2));
            }
            return result;
        }



        public static List<T4> Call<T1, T2, T3, T4>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4>> container, T1 arg1, T2 arg2, T3 arg3)
        {
            var mop = container.GetOperator();
            List<T4> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1, T2, T3, T4>)dele).Invoke(arg1, arg2, arg3));
            }
            return result;
        }


        public static List<T5> Call<T1, T2, T3, T4, T5>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4, T5>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var mop = container.GetOperator();
            List<T5> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1, T2, T3, T4, T5>)dele).Invoke(arg1, arg2, arg3, arg4));
            }
            return result;
        }


        public static List<T6> Call<T1, T2, T3, T4, T5, T6>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4, T5, T6>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var mop = container.GetOperator();
            List<T6> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1, T2, T3, T4, T5, T6>)dele).Invoke(arg1, arg2, arg3, arg4, arg5));
            }
            return result;
        }

    }
}
