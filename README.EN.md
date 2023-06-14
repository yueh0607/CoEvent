# CoEvent
# [Chinese](https://github.com/yueh0607/CoEvent/edit/main/README.md)|[English](https://github.com/yueh0607/CoEvent/edit/main/README.EN.md)
CoEvent is based on **Observer mode** **Event System**, it supports **Constraints on parameter type**, **Explicit event definition** in order to facilitate multi-person collaboration, compared with the event system based on integer and enumeration, it has great advantages, can prevent the misjudgment of parameter types, and can optimize the communication specifications between modules during collaboration.

CoEvent supports the development of platforms above Unity3D 2021.1 and.NET core. (If you need more support, please contact the author)



CoTask supports await coroutines, supports CoTask to coroutines, supports Task, supports Addressable and Dotween schemes, and allows easy access to more schemes. CoTask supports single-threaded operations such as WebGL JS.

## I. Functions and Modules
### 1.Covariant Event System CoEvent (Core)

```c#
1.参数类型约束，相对于参数推断类型有很大优势，不会错判，便于协作查找定义
2.支持带返回值不带返回值两种调用类型
3.与异步模块完美兼容
```
### 2.Single thread asynchronous CoTask (can delete the whole folder if you need to replace with UniTask, etc.)

```c#
1. 支持单线程异步
2. 支持await协程，CoTask转协程，已经用协程的项目零成本切换
3. 支持Dotween，Addressable的等待
4. 支持接入更多新的等待，学习成本极低。
5. 支持取消令牌自动传递，无需手动传递取消令牌，支持挂起和继续。
```
## II. Update Plan
- RPC remote invocation support
- CoTask performance improves

## III. Use of documents

### 1.CoEvent covariant event system

1). Pre-configuration:


```c#
在Unity2021及以上使用跳过这一步
如果在NET CORE使用请定位错误信息并根据提示修改宏定义。
```
2). Event definition

There are two interfaces for defining events. ISendEvent does not have a return value, and ICallEvent has a return value


```csharp
//这一部分如果涉及多人开发，您可以把所有事件放在一个文件夹下，也可也放在事件的最相关处，这样做可以方便查阅

public interface MyEvent: ISendEvent<参数类型...>
public interface MyEvent: ICallEvent<参数类型...返回值类型>
```

3). Registration and Cancellation Event

Send and call can only be performed after registration


``` csharp
this.Operator<消息类型接口>().Subcribe(MyAction);

//请注意，C#的委托闭包问题，也就是说如果您不取消事件，委托内被提取的引用对象将不会被GC自动回收，这是C#委托常见的一个内存泄漏陷阱。这是一个几乎全部事件系统都存在的问题。
this.Operator<消息类型接口>().UnSubcribe(MyAction);
```

4). Call and send


``` csharp
//调用全部
this.Operator<消息类型接口>().Send(...参数们);
//调用全部
var results = this.Operator<消息类型接口>().Call(...参数们);
//调用第一个
var result = this.Operator<消息类型接口>().CallFirst(参数们);
```

5).Routine


```csharp
using CoEvent;
using UnityEngine;

public interface IMyTest : ISendEvent<int, int> { }
public class Test : MonoBehaviour
{

    void Ttt(int t,int k)
    {
        Debug.Log($"{t}:{k}");
    }

    void Start()
    {
        this.Operator<IMyTest>().Subscribe(Ttt);
        this.Operator<IMyTest>().Send(10,100);
    }
    
    void OnDestroy()
    {
        this.Operator<IMyTest>().UnSubscribe(Ttt);
    }
}
```



### 3.CoTask Auto Token Single Thread Asynchronous

CoEvent implements a low-GC reference TaskLike and is single-threaded (supports Web Js). The solution meets the Async/Await model, which was first proposed by C # to convert the incomprehensible callback logic into the logic of synchronous thinking. It greatly simplifies the complexity of thinking.

1). Async method definition: Use await to asynchronize in a single thread by adding the async keyword with CoTask or CoTask <T> as the return value.

Be careful to use void as the return value, as this will result in the System. Threading. Tasks being used and the operation being outside the main thread.


```csharp
using CoEvent.Async;

    //可等待的任务
   public async CoTask mTest()
    {
    //等待600帧
        await CoTask.WaitForFrame(600);
        Debug.Log("Hha");
        //等待3秒
        await CoTask.Delay(3);
        Debug.Log("111");
    }

```
2). Cancels the warning tilde

You will experience the emptying of the warning wavy line when CoTask is called as a return value, which you can do to eliminate the warning.


```c#
mTest().Discrad();
_ = mTest();
```

Either way is allowed.

3). Non-async call

Sometimes you cannot define the async keyword, and you can perform asynchrony by forcing a call to the Invoke method


```
task.Invoke();
```

4). Cancel the token

The usage is very simple. It supports operations such as cancel, suspend, and continue. You only need to call the extension method With Token on CoTask to get the token. The biggest difference between UniTask and ETTask is that you do not need to pass the token manually. **The token is passed automatically**.


```csharp
//取得令牌
mTest().WithToken(out var token);
//暂停
token.Yield();
//继续
token.Continue();

//取消
token.OnCanceled +=()=>
{
    Debug.Log("任务取消后的处理");
}
//取消任务
token.Cancel();
```

5). Event system support

The principle is quite simple, which is to treat the asynchronous mode as a special method of returning values.


```csharp
public interface IMyEventAsync : ICallEvent<CoTask> { }

private async CoTask Stt()
{
    await CoTask.CompletedTask;
}

private async CoTask StartAsync()
{
     this.Operator<IMyEventAsync>().Subscribe(Stt);
     await this.Operator<IMyEventAsync>().CallFirst();
}

```



6). Expand CoTask support

There are macro definitions **One key to open** in the folders under the CoTask/Supports directory, which are very convenient to access **DoTween, Addressable et al.**.



7). Support for expanding CoEvent. Async.

Common steps are as follows:
0. Introduces a type into a reference to the assembly CoEvents. (Very important!!! You can also delete the assembly definition for CoEvent, which is simpler.)

1. Find the asynchronous operation base class of the corresponding library. For example, the AsyncOperationHandle class of Addressable is the asynchronous base class.

2. Locate the completion callback of the asynchronous base class, such as the AsyncOperationHandle. Completed, which is what the base class calls on completion

3. Define the extended method GetAwaiter of the asynchronous base class, create CoTask, and add SetResult to the completion callback of the asynchronous base class

4. Returns an asynchronous task

   Consider the following definition of routine extension:


```csharp
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//引入命名空间
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//选一个合适的命名空间，能访问都可以
namespace CoEvents.Async
{
    //静态类，定义拓展方法
    public static class AddressableEx 
    {
        //创建GetAwaiter拓展方法，向指定完成回调里加入SetResult委托（如果入池则消除重复GC）
        public static CoTask<AsyncOperationHandle> GetAwaiter(this AsyncOperationHandle handle)
        {
            //创建Task
            var task = CoTask<AsyncOperationHandle>.Create();
            //绑定回调
            handle.Completed += task.SetResult;
            //返回任务
            return task;
        }
        //泛型支持
        public static CoTask<AsyncOperationHandle<T>> GetAwaiter<T>(this AsyncOperationHandle<T> handle)
        {
            var task = CoTask<AsyncOperationHandle<T>>.Create();
            handle.Completed += task.SetResult;
            return task;
        }
    }

}
```


8). Common asynchronous task

In the CoTask class, you can call these methods directly


```
//延迟一秒
CoTask.Delay(1);
//转协程
CoTask.ToCoroutine(async ()=>
{
	yield return new WaitForSceonds();
})
//已经完成的Task
CoTask.CompletedTask

//更多不再介绍...
```

9). Create Cotask without calling


```
Func<CoTask> task = async ()=>
{
	//这个lambda可以是异步方法，这样就获取了这个CoTask
}
```





## 3. Use of default built-in object pool

In order to optimize performance, CoEvent has to introduce object pool for object reuse, but many independent toolsets and architectures, plug-ins, have their own object pool, so it is easy to duplicate object pool code after introducing a large number of plug-ins.

CoEvent introduces a lightweight object pool with high performance and simple functions, and allows users to replace a Pool property **CoDeaultPool is specified by default** defined in the CoEvent class. You can **Set it to null, and CoEvent will not use Poo** l.

Similarly, you can use Pool to store and reuse some other objects, such as

```csharp
T item = CoEvent.Pool?.Allocate<T>();
CoEvent.Pool?Recycle(item);
```
Similarly, there are APIs for GameObjects, all with lightweight implementations.

