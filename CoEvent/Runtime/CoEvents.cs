using CoEvent.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CoEvent
{
    public static class CoEvents
    {
        internal static Dictionary<Type, CoOperator<ICoEventBase>> container = new Dictionary<Type, CoOperator<ICoEventBase>>();
        internal static HashSet<Type> removeMarks = new HashSet<Type>();

        public static void ReleaseEmpty()
        {
            foreach (var con in container)
            {
                if (con.Value.Count == 0) removeMarks.Add(con.Key);
            }
            foreach (var tp in removeMarks)
            {
                container.Remove(tp);
            }
            removeMarks.Clear();
        }

        public static void InitPublisher()
        {
            GameObject publisher = new GameObject("CoEventPublisher");
            GameObject.DontDestroyOnLoad(publisher);
            publisher.AddComponent<CoEventPublisher>();
        }

        public readonly static object Instance = new object();
        public static IPool Pool { get; set; } = null;

        public static Action<Exception> ExceptionHandler = (x) => throw x;
    }

    public static class CoEventManagerEx1
    {
        [DebuggerHidden]
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ISendEventBase
        {
            Type type = typeof(EventType);
            if (!CoEvents.container.ContainsKey(type)) CoEvents.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop = CoEvents.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
    public static class CoEventManagerEx2
    {
        [DebuggerHidden]
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ICallEventBase
        {
            Type type = typeof(EventType);
            if (!CoEvents.container.ContainsKey(type)) CoEvents.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop = CoEvents.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
}
