using System;
using System.Collections.Generic;

namespace CoEvent
{

    public static partial class MessageExtensions
    {
        public static List<T1> Call<T1>(this ICoVarOperator<ICallEvent<T1>> container)
        {
            var mop = container.GetOperator();
            List<T1> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1>)dele).Invoke());
            }
            mop.Reset();
            return result;
        }


        public static List<T2> Call<T1, T2>(this ICoVarOperator<ICallEvent<T1, T2>> container, T1 arg1)
        {
            var mop = container.GetOperator();
            List<T2> result = new();

            while (mop.GetNext(out var dele))
            {
                result.Add(((Func<T1, T2>)dele).Invoke(arg1));
            }
            mop.Reset();
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
            mop.Reset();
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
            mop.Reset();
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
            mop.Reset();
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
            mop.Reset();
            return result;
        }





        public static T1 CallFirst<T1>(this ICoVarOperator<ICallEvent<T1>> container)
        {
            var mop = container.GetOperator();

            if (mop.GetNext(out var dele))
            {
                mop.Reset();
                return ((Func<T1>)dele).Invoke();
            }

            return default;
        }


        public static T2 CallFirst<T1, T2>(this ICoVarOperator<ICallEvent<T1, T2>> container, T1 arg1)
        {
            var mop = container.GetOperator();

            if (mop.GetNext(out var dele))
            {
                mop.Reset();
                return ((Func<T1, T2>)dele).Invoke(arg1);
            }

            return default;
        }

        public static T3 CallFirst<T1, T2, T3>(this ICoVarOperator<ICallEvent<T1, T2, T3>> container, T1 arg1, T2 arg2)
        {
            var mop = container.GetOperator();

            if (mop.GetNext(out var dele))
            {
                mop.Reset();
                return ((Func<T1, T2, T3>)dele).Invoke(arg1, arg2);
            }

            return default;
        }



        public static T4 CallFirst<T1, T2, T3, T4>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4>> container, T1 arg1, T2 arg2, T3 arg3)
        {
            var mop = container.GetOperator();

            if (mop.GetNext(out var dele))
            {
                mop.Reset();
                return ((Func<T1, T2, T3, T4>)dele).Invoke(arg1, arg2, arg3);
            }

            return default;
        }


        public static T5 CallFirst<T1, T2, T3, T4, T5>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4, T5>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var mop = container.GetOperator();

            if (mop.GetNext(out var dele))
            {
                mop.Reset();
                return ((Func<T1, T2, T3, T4, T5>)dele).Invoke(arg1, arg2, arg3, arg4);
            }

            return default;
        }


        public static T6 CallFirst<T1, T2, T3, T4, T5, T6>(this ICoVarOperator<ICallEvent<T1, T2, T3, T4, T5, T6>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var mop = container.GetOperator();

            if (mop.GetNext(out var dele))
            {
                mop.Reset();
                return ((Func<T1, T2, T3, T4, T5, T6>)dele).Invoke(arg1, arg2, arg3, arg4, arg5);
            }
            return default;
        }

    }
}
