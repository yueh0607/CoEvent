using CoEvent;
using GluonGui.Dialog;
using System;
using System.Collections.Generic;
namespace CoEvent
{
    public sealed class CounterCall
    {


        public static CounterCall Create()
        {
            CounterCall call = null;
            if (CoEvents.Pool != null) call = (CounterCall)CoEvents.Pool.Allocate(typeof(CounterCall));
            else call = new CounterCall();

            return call;
        }
        public static void Recycle(CounterCall call)
        {
            call.OnClick = null;
            call._counter = 0;
            call.ClickValue = 0;
            call.OnceRecycle = false;
            if (CoEvents.Pool != null) return;
            CoEvents.Pool.Recycle(typeof(CounterCall),call);
        }



        /// <summary>
        /// 是否达到一次ClickValue就自动回收到对象池或者销毁
        /// </summary>
        public bool OnceRecycle { get; set; } = false;
        private int _counter;
        /// <summary>
        /// 当前计数数量
        /// </summary>
        public int Count
        {
            get => _counter;
            set
            {
                _counter = value;
                if (value == ClickValue)
                {
                    OnClick?.Invoke();
                    if (OnceRecycle) Recycle(this);
                }
            }
        }


        /// <summary>
        /// 预期触发值
        /// </summary>
        public int ClickValue { get; set; } = 0;

        /// <summary>
        /// 触发事件
        /// </summary>
        public event Action OnClick = null;

        private Action plusOne = null;
        /// <summary>
        /// 默认实现的加一行为，第一次访问产生GC，代替自定义lambda减少GC产出
        /// </summary>
        public Action PlusOne
        {
            get
            {
                if (plusOne == null)
                {
                    plusOne = () => { ++Count; };
                }
                return plusOne;
            }
        }


    }


    public class CounterCall<T>
    {

        public static CounterCall<T> Create()
        {
            CounterCall<T> call = null;
            if (CoEvents.Pool != null) call = (CounterCall<T>)CoEvents.Pool.Allocate(typeof(CounterCall<T>));
            else call = new CounterCall<T>();

            return call;
        }
        public static void Recycle(CounterCall<T> call)
        {
            call.OnClick = null;
            call._counter = 0;
            call.ClickValue = 0;
            call.OnceRecycle = false;
            call.Results.Clear();
            if (CoEvents.Pool != null) return;
            CoEvents.Pool.Recycle(typeof(CounterCall<T>),call);
        }




        /// <summary>
        /// 是否达到一次ClickValue就自动回收到对象池或者销毁
        /// </summary>
        public bool OnceRecycle { get; set; } = false;
        private int _counter;
        /// <summary>
        /// 当前计数数量
        /// </summary>
        public int Count
        {
            get => _counter;
            set
            {
                _counter = value;
                if (value == ClickValue)
                {
                    OnClick?.Invoke(Results);
                    if (OnceRecycle) Recycle(this);
                }
            }
        }


        /// <summary>
        /// 预期触发值
        /// </summary>
        public int ClickValue { get; set; } = 0;

        /// <summary>
        /// 触发事件
        /// </summary>
        public event Action<List<T>> OnClick = null;


        public List<T> Results { get; set; } = new List<T>();

        private Action<T> plusOne = null;
        /// <summary>
        /// 默认实现的加一行为，第一次访问产生GC，代替自定义lambda减少GC产出
        /// </summary>
        public Action<T> PlusOne
        {
            get
            {
                if (plusOne == null)
                {
                    plusOne = (x) => { ++Count; Results.Add(x); };
                }
                return plusOne;
            }
        }


    }
}
