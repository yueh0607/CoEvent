﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace CoEvent
{
    public static class CoEvents
    {
        internal static Dictionary<Type, CoOperator<ICoEventBase>> container = new Dictionary<Type, CoOperator<ICoEventBase>>();
        internal static HashSet<Type> removeMarks= new HashSet<Type>();
        public static void ReleaseEmpty()
        {
            foreach(var con in container)
            {
                if(con.Value.Count==0) removeMarks.Add(con.Key);
            }
            foreach(var tp in removeMarks)
            {
                container.Remove(tp);
            }
        }


        
    }

    public static class CoEventManagerEx1
    {
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
        public static ICoVarOperator<EventType> Operator<EventType>(this object cov) where EventType : ICallEventBase
        {
            Type type = typeof(EventType);
            if (!CoEvents.container.ContainsKey(type)) CoEvents.container.Add(type, new CoOperator<ICoEventBase>());
            CoOperator<ICoEventBase> cop = CoEvents.container[type];
            return CoUnsafeAs.As<CoOperator<ICoEventBase>, CoOperator<EventType>>(ref cop);
        }
    }
}