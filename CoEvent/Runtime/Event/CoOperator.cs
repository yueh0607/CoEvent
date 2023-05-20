using CoEvent.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvent
{
    public class CoOperator<T> : ICoVarOperator<T>
    {
        public List<Delegate> Events { get; private set; } = new();

        /// <summary>
        /// 委托数
        /// </summary>
        public int Count => Events.Count;

        public int IntervalIndex { get; private set; } = 0;


        /// <summary>
        /// 清空操作器
        /// </summary>
        public void Clear()
        {
            Events.Clear();
        }



        public void Add(Delegate dele)
        {
            Events.Add(dele);
        }

        public bool Remove(Delegate dele)
        {
            int index = Events.IndexOf(dele);
            if (index <= IntervalIndex) --IntervalIndex;

            return Events.Remove(dele);
        }

        public bool GetNext(out Delegate dele)
        {
            if (IntervalIndex >= 0 && IntervalIndex < Events.Count)
            {

                dele = Events[IntervalIndex++];
                return true;
            }
            dele = null;
            return false;
        }

        public void Reset()
        {
            IntervalIndex = 0;
        }

    }



    internal static class CoV_EX
    {
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static CoOperator<ICoEventBase> GetOperator<T>(this ICoVarOperator<T> ico)
        {
            return (CoOperator<ICoEventBase>)ico;
        }

    }
}
