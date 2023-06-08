using CoEvents.Async.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace CoEvents.Async
{
    public struct CoTaskBuilder
    {

        // 1. Static Create method
        [DebuggerHidden]
        public static CoTaskBuilder Create() => new CoTaskBuilder(CoTask.Create());
        public CoTaskBuilder(CoTask task) => this.task = task;

        private CoTask task;
        // 2. TaskLike Current
        [DebuggerHidden]
        public CoTask Task => task;



        // 3. Start 构造之后开启状态机
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 4. SetException 
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            UnityEngine.Debug.Log(exception.ToString());
            task.SetException(exception);
        }

        // 5. SetResult 
        [DebuggerHidden]
        public void SetResult()
        {
            task.SetResult();
        }

        // 6. AwaitOnCompleted  
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            task.Token.Current = awaiter as IAsyncTokenProperty;
            awaiter.OnCompleted(stateMachine.MoveNext);
            //UnityEngine.Debug.Log("100");
        }

        // 7. AwaitUnsafeOnCompleted 
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {

            task.Token.Current = awaiter as IAsyncTokenProperty;
            awaiter.OnCompleted(stateMachine.MoveNext);
            //UnityEngine.Debug.Log("100");
        }

        // 9. SetStateMachine 
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }

    public struct CoTaskBuilder<T>
    {

        // 1. Static Create method
        [DebuggerHidden]
        public static CoTaskBuilder<T> Create() => new CoTaskBuilder<T>(CoTask<T>.Create());
        public CoTaskBuilder(CoTask<T> task) => this.task = task;

        private CoTask<T> task;
        // 2. TaskLike Current
        [DebuggerHidden]
        public CoTask<T> Task => task;



        // 3. Start 构造之后开启状态机
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 4. SetException 
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            task.SetException(exception);
        }

        // 5. SetResult 
        [DebuggerHidden]
        public void SetResult(T result)
        {
            task.SetResult(result);
        }

        // 6. AwaitOnCompleted  
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            task.Token.Current = awaiter as IAsyncTokenProperty;
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        // 7. AwaitUnsafeOnCompleted 
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {

            task.Token.Current = awaiter as IAsyncTokenProperty;
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        // 9. SetStateMachine 
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }

}
