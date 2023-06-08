using CoEvents.Async.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CoEvents.Async
{
    [AsyncMethodBuilder(typeof(CoTaskBuilder))]
    public class CoTask : IAsyncTask, IAsyncTokenProperty, ICoTask
    {
        //在结束时调用，无论是否成功
        public event Action OnTaskCompleted = null;
        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;
        [DebuggerHidden]
        public static CoTask Create()
        {
            CoTask task = null;
            if (CoEvent.Pool != null) task = (CoTask)CoEvent.Pool.Allocate(typeof(CoTask));
            else task = new CoTask();

            task.Token = new AsyncTreeTokenNode(task, task);
            task.Token.Current = task;
            task.Token.Root = task;
            task.IsCompleted = false;
            task.Token.Authorization = true;

            return task;
        }
        [DebuggerHidden]
        public static void Recycle(CoTask task)
        {
            task.Token.Authorization = false;

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



        #endregion

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod();
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCancel()
        {
            SetResultMethod();
        }
        [DebuggerHidden]
        private void SetResultMethod()
        {
            if (Token.Authorization)
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

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoTask GetAwaiter() => this;

    }


    [AsyncMethodBuilder(typeof(CoTaskBuilder<>))]
    public class CoTask<T> : IAsyncTask<T>, IAsyncTokenProperty, ICoTask
    {
        //在结束时调用，无论是否成功
        public event Action<T> OnTaskCompleted = null;
        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;
        public T Result { get; set; } = default;
        [DebuggerHidden]
        public static CoTask<T> Create()
        {
            CoTask<T> task = null;
            if (CoEvent.Pool != null) task = (CoTask<T>)CoEvent.Pool.Allocate(typeof(CoTask<T>));
            else task = new CoTask<T>();


            task.Token = new AsyncTreeTokenNode(task, task);
            task.Token.Current = task;
            task.Token.Root = task;
            task.IsCompleted = false;
            task.Token.Authorization = true;

            return task;
        }
        [DebuggerHidden]
        public static void Recycle(CoTask<T> task)
        {
            task.Token.Authorization = false;
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


        #endregion

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            return Result;
        }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod(default);
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCancel()
        {
            SetResultMethod(default);
        }
        [DebuggerHidden]
        private void SetResultMethod(T result)
        {
            if (Token.Authorization)
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
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UnsafeSetResultMethod()
        {
            SetResult(this.Result);
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoTask<T> GetAwaiter() => this;

    }


    public class CoTaskTimer : IAsyncTask, IAsyncTokenProperty, ICoTask
    {

        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;
        [DebuggerHidden]
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
            task.Token.Authorization = true;



            return task;
        }
        [DebuggerHidden]
        public static void Recycle(CoTaskTimer task)
        {
            task.Token.Authorization = false;
            task.continuation = null;
            //CoEvent.Instance.Operator<IUpdate>().UnSubscribe(task.Update);
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



        #endregion


        public float EndTime { get; set; } = 1000f;
        private float currentTime = 0f;
        void Update(float deltaTime)
        {
            //UnityEngine.Debug.Log(Token.Authorization);
            if (Token.Authorization)
            {
                currentTime += deltaTime;
                if (currentTime >= EndTime)
                {
                    SetResult();
                }
            }
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod();
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCancel()
        {
            SetResultMethod();
        }
        [DebuggerHidden]
        private void SetResultMethod()
        {
            if (Token.Authorization)
            {
                //执行await以后的代码
                continuation?.Invoke();
            }
            CoEvent.Instance.Operator<IUpdate>().UnSubscribe(Update);
            //回收到Pool
            Recycle(this);
        }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoTaskTimer GetAwaiter()
        {
            CoEvent.Instance.Operator<IUpdate>().Subscribe(Update);
            return this;
        }
    }

    public class CoTaskUpdate : IAsyncTask, IAsyncTokenProperty, ICoTask
    {
        //在结束时调用，无论是否成功
        public event Action OnTaskCompleted = null;
        private Action continuation = null;
        public bool IsCompleted { get; set; } = false;
        [DebuggerHidden]
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
            task.Token.Authorization = true;

            CoEvent.Instance.Operator<IUpdate>().Subscribe(task.Update);

            return task;
        }
        [DebuggerHidden]
        public static void Recycle(CoTaskUpdate task)
        {
            task.Token.Authorization = false;
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



        #endregion


        public int FrameCount { get; set; } = 1;
        int current = 0;

        void Update(float deltaTime)
        {
            if (Token.Authorization)
                if (current++ >= FrameCount)
                {
                    SetResult();
                }

        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
            SetResultMethod();
        }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCancel()
        {
            SetResultMethod();
        }
        [DebuggerHidden]
        private void SetResultMethod()
        {
            if (Token.Authorization)
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

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeOnCompleted(Action continuation) => this.continuation = continuation;


        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoTaskUpdate GetAwaiter() => this;

    }


    public struct CoTaskCompleted : INotifyCompletion, ICoTask
    {

        public bool IsCompleted => true;
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetException(Exception exception)
        {
            CoEvent.ExceptionHandler?.Invoke(exception);
        }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoTaskCompleted GetAwaiter() => this;
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation) { }
    }
}
