using CoEvents.Internal;
using System;

namespace CoEvents
{
    public static class CoEventManagerEx1
    {
        //[DebuggerHidden]
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ISendEventBase
        {
            Type type = typeof(EventType);
            if (!CoEvent.container.ContainsKey(type)) CoEvent.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop = CoEvent.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
    public static class CoEventManagerEx2
    {
        //[DebuggerHidden]
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ICallEventBase
        {
            Type type = typeof(EventType);
            if (!CoEvent.container.ContainsKey(type)) CoEvent.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop = CoEvent.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
}
