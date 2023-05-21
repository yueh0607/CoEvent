using CoEvents.Internal;

//建议不要随便使用这个命名空间下的东西
namespace CoEvents.Internal
{

    //基础接口是没法处理事件的，没有任何匹配的功能，简称没用
    public interface ICoEventBase { }



    //泛型接口不够安全，能Send也能Call
    public interface IGenericEvent : ICoEventBase { }
    public interface IGenericEvent<T1> : ICoEventBase { }
    public interface IGenericEvent<T1, T2> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3, T4> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3, T4, T5> : ICoEventBase { }
    public interface IGenericEvent<T1, T2, T3, T4, T5, T6> : ICoEventBase { }


    //这两个是Base，也没能处理的事件，简称没用
    public interface ISendEventBase : ICoEventBase { }

    public interface ICallEventBase : ICoEventBase { }
}


namespace CoEvents
{
#pragma warning disable




    public interface ISendEvent : ISendEventBase, IGenericEvent { }
    public interface ISendEvent<T1> : ISendEventBase, IGenericEvent<T1> { }
    public interface ISendEvent<T1, T2> : ISendEventBase, IGenericEvent<T1, T2> { }
    public interface ISendEvent<T1, T2, T3> : ISendEventBase, IGenericEvent<T1, T2, T3> { }
    public interface ISendEvent<T1, T2, T3, T4> : ISendEventBase, IGenericEvent<T1, T2, T3, T4> { }
    public interface ISendEvent<T1, T2, T3, T4, T5> : ISendEventBase, IGenericEvent<T1, T2, T3, T4, T5> { }
    public interface ISendEvent<T1, T2, T3, T4, T5, T6> : ISendEventBase, IGenericEvent<T1, T2, T3, T4, T5, T6> { }

    public interface ICallEvent : ICallEventBase, IGenericEvent { }
    public interface ICallEvent<T1> : ICallEventBase, IGenericEvent<T1> { }
    public interface ICallEvent<T1, T2> : ICallEventBase, IGenericEvent<T1, T2> { }
    public interface ICallEvent<T1, T2, T3> : ICallEventBase, IGenericEvent<T1, T2, T3> { }
    public interface ICallEvent<T1, T2, T3, T4> : ICallEventBase, IGenericEvent<T1, T2, T3, T4> { }
    public interface ICallEvent<T1, T2, T3, T4, T5> : ICallEventBase, IGenericEvent<T1, T2, T3, T4, T5> { }
    public interface ICallEvent<T1, T2, T3, T4, T5, T6> : ICallEventBase, IGenericEvent<T1, T2, T3, T4, T5, T6> { }




#pragma warning restore
}
