using CoEvents.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvents
{
    public static class CoEvent
    {
        //事件容器
        internal static Dictionary<Type, CoOperator<ICoEventBase>> container = new Dictionary<Type, CoOperator<ICoEventBase>>();
        //移除标记
        internal static HashSet<Type> removeMarks = new HashSet<Type>();

        //如果认为容器中空余的事件Operator占内存，则随便写个脚本计时调用这个方法即可。
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

        //初始化标记
        internal static bool Initialized { get; private set; } = false;

        private static CoEventPublisher monoPublisher = null;

        //自动创建Publisher发布简单生命周期
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitPublisher()
        {
            if (Initialized) return;
            GameObject publisher = new GameObject("CoEventPublisher");
            GameObject.DontDestroyOnLoad(publisher);
            monoPublisher = publisher.AddComponent<CoEventPublisher>();
            Initialized = true;
        }

        /// <summary>
        /// 在静态类中无法使用this，则使用CoEvents.Instance.Operator
        /// </summary>
        public readonly static object Instance = new object();

        /// <summary>
        /// 默认对象池，如果为null则不开启，默认最大容量为1000个
        /// </summary>
        public static IPool Pool { get; set; } = new CoDefaultPool(1000);

        /// <summary>
        /// 异常处理器，所有异常都使用此方法处理
        /// </summary>

        public static Action<Exception> ExceptionHandler = (x) => throw x;

        public static MonoBehaviour Mono => monoPublisher;
        public static ICoVarOperator<IUpdate> Update { get; } = Instance.Operator<IUpdate>();
        public static ICoVarOperator<ILateUpdate> LateUpdate { get; } = Instance.Operator<ILateUpdate>();
        public static ICoVarOperator<IFixedUpdate> FixedUpdate { get; } = Instance.Operator<IFixedUpdate>();
    }


}
