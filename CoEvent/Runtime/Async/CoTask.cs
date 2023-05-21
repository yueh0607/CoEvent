using CoEvents.Async.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvents.Async
{
    [AsyncMethodBuilder(typeof(CoTaskBuilder))]
    public class CoTask : IAsyncTask, IAsyncTokenProperty
    {
        //在结束时调用，无论是否成功
        public event Action OnTaskCompleted = null;
        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;

        public static CoTask Create()
        {
            CoTask task = null;
            if (CoEvent.Pool != null) task = (CoTask)CoEvent.Pool.Allocate(typeof(CoTask));
            else task = new CoTask();

            task.Token = new AsyncTreeTokenNode(task, task);
            task.Token.Current = task;
            task.Token.Root = task;
            task.IsCompleted = false;
            task.Authorization = true;

            return task;
        }
        public static void Recycle(CoTask task)
        {
            task.Authorization = false;

            if (CoEvent.Pool != null) return;
            CoEvent.Pool?.Recycle(typeof(CoTask), task);
        }


        private Action setResult = null;
        public Action SetResult
        {
            get
            {
                setResult ??= SetResultMethod;
                return setResult;
            }
        }



        #region Token
        /// <summary>
        /// 异步令牌，与AsyncToken作用相同
        /// </summary>
        AsyncTreeTokenNode IAsyncTokenProperty.Token { get => Token; set => Token = value; }
        public AsyncTreeTokenNode Token { get; internal set; }


        /// <summary>
        /// 授权状态，代表当前任务是否被挂起，也决定了状态机是否能够继续前进
        /// </summary>
        public bool Authorization { get; internal set; } = true;
        bool IAuthorization.Authorization { get => Authorization; set => Authorization = value; }
        #endregion


        public void GetResult() { }


        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod();
        }
        public void SetCancel()
        {
            SetResultMethod();
        }

        private void SetResultMethod()
        {
            if (Authorization)
            {
                if (IsCompleted) throw new InvalidOperationException("AsyncTask dont allow SetResult repeatly.");
                //执行await以后的代码
                continuation?.Invoke();
                continuation = null;
            }
            IsCompleted = true;
            OnTaskCompleted?.Invoke();
            //回收到Pool
            Recycle(this);
        }

        [DebuggerHidden]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden]
        public CoTask GetAwaiter() => this;
        [DebuggerHidden]
        public async void Coroutine() => await this;


        public static CoTask CompletedTask
        {
            get
            {
                var task = CoTask.Create();
                task.SetResultMethod();
                return task;
            }
        }

    }


    [AsyncMethodBuilder(typeof(CoTaskBuilder<>))]
    public class CoTask<T> : IAsyncTask<T>, IAsyncTokenProperty
    {
        //在结束时调用，无论是否成功
        public event Action<T> OnTaskCompleted = null;
        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;
        public T Result { get; set; } = default;
        public static CoTask<T> Create()
        {
            CoTask<T> task = null;
            if (CoEvent.Pool != null) task = (CoTask<T>)CoEvent.Pool.Allocate(typeof(CoTask<T>));
            else task = new CoTask<T>();


            task.Token = new AsyncTreeTokenNode(task, task);
            task.Token.Current = task;
            task.Token.Root = task;
            task.IsCompleted = false;
            task.Authorization = true;

            return task;
        }
        public static void Recycle(CoTask<T> task)
        {
            task.Authorization = false;
            task.continuation = null;
            if (CoEvent.Pool != null) return;
            CoEvent.Pool?.Recycle(typeof(CoTask<T>), task);
        }


        private Action<T> setResult = null;
        public Action<T> SetResult
        {
            get
            {
                setResult ??= SetResultMethod;
                return setResult;
            }
        }



        #region Token
        /// <summary>
        /// 异步令牌，与AsyncToken作用相同
        /// </summary>
        AsyncTreeTokenNode IAsyncTokenProperty.Token { get => Token; set => Token = value; }
        public AsyncTreeTokenNode Token { get; internal set; }


        /// <summary>
        /// 授权状态，代表当前任务是否被挂起，也决定了状态机是否能够继续前进
        /// </summary>
        public bool Authorization { get; internal set; } = true;
        bool IAuthorization.Authorization { get => Authorization; set => Authorization = value; }
        #endregion


        public T GetResult()
        {
            return Result;
        }


        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod(default);
        }
        public void SetCancel()
        {
            SetResultMethod(default);
        }

        private void SetResultMethod(T result)
        {
            if (Authorization)
            {
                if (IsCompleted) throw new InvalidOperationException("AsyncTask dont allow SetResult repeatly.");
                this.Result = result;
                //执行await以后的代码
                continuation?.Invoke();
                continuation = null;
            }
            IsCompleted = true;
            OnTaskCompleted?.Invoke(Result);
            //回收到Pool
            Recycle(this);
        }

        private Action unsafeSetResult = null;
        public Action UnsafeSetResult
        {
            get
            {
                if (unsafeSetResult == null)
                {
                    unsafeSetResult = UnsafeSetResultMethod;
                }
                return unsafeSetResult;
            }
        }
        private void UnsafeSetResultMethod()
        {
            SetResult(this.Result);
        }
        [DebuggerHidden]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden]
        public CoTask<T> GetAwaiter() => this;
        [DebuggerHidden]
        public async void Coroutine() => await this;

    }


    public class CoTaskTimer : IAsyncTask, IAsyncTokenProperty
    {

        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;

        public static CoTaskTimer Create()
        {
            CoTaskTimer task = null;
            if (CoEvent.Pool != null) task = (CoTaskTimer)CoEvent.Pool.Allocate(typeof(CoTaskTimer));
            else task = new CoTaskTimer();

            task.Token = new AsyncTreeTokenNode(task, task);
            task.Token.Current = task;
            task.Token.Root = task;
            task.IsCompleted = false;
            task.currentTime = 0;
            task.EndTime = 1000f;
            task.Authorization = true;

            CoEvent.Instance.Operator<IUpdate>().Subscribe(task.Update);

            return task;
        }
        public static void Recycle(CoTaskTimer task)
        {
            task.Authorization = false;
            task.continuation = null;
            CoEvent.Instance.Operator<IUpdate>().UnSubscribe(task.Update);
            if (CoEvent.Pool != null) return;
            CoEvent.Pool?.Recycle(typeof(CoTask), task);
        }


        private Action setResult = null;
        public Action SetResult
        {
            get
            {
                setResult ??= SetResultMethod;
                return setResult;
            }
        }



        #region Token
        /// <summary>
        /// 异步令牌，与AsyncToken作用相同
        /// </summary>
        AsyncTreeTokenNode IAsyncTokenProperty.Token { get => Token; set => Token = value; }
        public AsyncTreeTokenNode Token { get; internal set; }


        /// <summary>
        /// 授权状态，代表当前任务是否被挂起，也决定了状态机是否能够继续前进
        /// </summary>
        public bool Authorization { get; internal set; } = true;
        bool IAuthorization.Authorization { get => Authorization; set => Authorization = value; }
        #endregion


        public float EndTime { get; set; } = 1000f;
        private float currentTime = 0f;
        void Update(float deltaTime)
        {
            if (Authorization)
            {
                currentTime += deltaTime;
                if (currentTime >= EndTime)
                {
                    SetResult();
                }
            }
        }
        public void GetResult() { }


        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod();
        }
        public void SetCancel()
        {
            SetResultMethod();
        }

        private void SetResultMethod()
        {
            if (Authorization)
            {
                //执行await以后的代码
                continuation?.Invoke();
            }
            //回收到Pool
            Recycle(this);
        }

        [DebuggerHidden]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden]
        public CoTaskTimer GetAwaiter() => this;
        [DebuggerHidden]
        public async void Coroutine() => await this;

    }

    public class CoTaskUpdate : IAsyncTask, IAsyncTokenProperty
    {
        //在结束时调用，无论是否成功
        public event Action OnTaskCompleted = null;
        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;

        public static CoTaskUpdate Create()
        {
            CoTaskUpdate task = null;
            if (CoEvent.Pool != null) task = (CoTaskUpdate)CoEvent.Pool.Allocate(typeof(CoTaskUpdate));
            else task = new CoTaskUpdate();

            task.Token = new AsyncTreeTokenNode(task, task);
            task.Token.Current = task;
            task.Token.Root = task;
            task.IsCompleted = false;
            task.FrameCount = 1;
            task.current = 0;
            task.Authorization = true;

            CoEvent.Instance.Operator<IUpdate>().Subscribe(task.Update);

            return task;
        }
        public static void Recycle(CoTaskUpdate task)
        {
            task.Authorization = false;
            task.continuation = null;
            CoEvent.Instance.Operator<IUpdate>().UnSubscribe(task.Update);
            if (CoEvent.Pool != null) return;
            CoEvent.Pool?.Recycle(typeof(CoTaskUpdate), task);
        }


        private Action setResult = null;
        public Action SetResult
        {
            get
            {
                setResult ??= SetResultMethod;
                return setResult;
            }
        }



        #region Token
        /// <summary>
        /// 异步令牌，与AsyncToken作用相同
        /// </summary>
        AsyncTreeTokenNode IAsyncTokenProperty.Token { get => Token; set => Token = value; }
        public AsyncTreeTokenNode Token { get; internal set; }


        /// <summary>
        /// 授权状态，代表当前任务是否被挂起，也决定了状态机是否能够继续前进
        /// </summary>
        public bool Authorization { get; internal set; } = true;
        bool IAuthorization.Authorization { get => Authorization; set => Authorization = value; }
        #endregion


        public int FrameCount { get; set; } = 1;
        int current = 0;

        void Update(float deltaTime)
        {
            if (current++ >= FrameCount)
            {
                SetResult();
            }

        }
        public void GetResult() { }


        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod();
        }
        public void SetCancel()
        {
            SetResultMethod();
        }

        private void SetResultMethod()
        {
            if (Authorization)
            {
                if (IsCompleted) throw new InvalidOperationException("AsyncTask dont allow SetResult repeatly.");
                //执行await以后的代码
                continuation?.Invoke();
                continuation = null;
            }
            IsCompleted = true;
            OnTaskCompleted?.Invoke();
            //回收到Pool
            Recycle(this);
        }

        [DebuggerHidden]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden]
        public CoTaskUpdate GetAwaiter() => this;
        [DebuggerHidden]
        public async void Coroutine() => await this;

    }

}
