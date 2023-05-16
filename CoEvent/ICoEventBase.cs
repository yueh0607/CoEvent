using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvent
{
#pragma warning disable
    public interface ICoEventBase { }
    


    public interface IGenericEvent : ICoEventBase { }
    public interface IGenericEvent<T1> : ICoEventBase { }
    public interface IGenericEvent<T1, T2> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3, T4> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3, T4, T5> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3, T4, T5, T6> : ICoEventBase { }



    public interface ISendEventBase : ICoEventBase { }
    
    public interface ICallEventBase : ICoEventBase { }


    public interface ISendEvent : ISendEventBase,IGenericEvent{ }
    public interface ISendEvent<T1> : ISendEventBase,IGenericEvent<T1> { }
    public interface ISendEvent<T1, T2> : ISendEventBase, IGenericEvent<T1,T2> { }
    public interface ISendEvent<T1, T2, T3> : ISendEventBase, IGenericEvent<T1, T2,T3> { }
    public interface ISendEvent<T1, T2, T3, T4> : ISendEventBase, IGenericEvent<T1, T2, T3,T4> { }
    public interface ISendEvent<T1, T2, T3, T4, T5> : ISendEventBase, IGenericEvent<T1, T2, T3, T4,T5> { }
    public interface ISendEvent<T1, T2, T3, T4, T5, T6> : ISendEventBase, IGenericEvent<T1, T2, T3, T4,T5,T6> { }

    public interface ICallEvent : ICallEventBase,IGenericEvent { }
    public interface ICallEvent<T1> : ICallEventBase, IGenericEvent<T1> { }
    public interface ICallEvent<T1, T2> : ICallEventBase, IGenericEvent<T1, T2> { }
    public interface ICallEvent<T1, T2, T3> : ICallEventBase, IGenericEvent<T1, T2, T3> { }
    public interface ICallEvent<T1, T2, T3, T4> : ICallEventBase, IGenericEvent<T1, T2, T3, T4> { }
    public interface ICallEvent<T1, T2, T3, T4, T5> : ICallEventBase, IGenericEvent<T1, T2, T3, T4,T5>{ }
    public interface ICallEvent<T1, T2, T3, T4, T5, T6> : ICallEventBase, IGenericEvent<T1, T2, T3, T4,T5,T6> { }




#pragma warning restore
}
