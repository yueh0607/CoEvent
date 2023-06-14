# CoEvent
# [Chinese](https://github.com/yueh0607/CoEvent/blob/main/README.md)|[English](https://github.com/yueh0607/CoEvent/blob/main/README.EN.md)


CoEvent is based on **Observer mode** **Event System**, it supports **Constraints on parameter type**, **Explicit event definition** in order to facilitate multi-person collaboration, compared with the event system based on integer and enumeration, it has great advantages, can prevent the misjudgment of parameter types, and can optimize the communication specifications between modules during collaboration.

CoEvent supports the development of platforms above Unity3D 2021.1 and.NET core. (If you need more support, please contact the author)



CoTask supports await coroutines, supports CoTask to coroutines, supports Task, supports Addressable and Dotween schemes, and allows easy access to more schemes. CoTask supports single-threaded operations such as WebGL JS.

## I. Functions and Modules
### 1.Covariant Event System CoEvent (Core)

```c#
1. Parameter type constraints have significant advantages over parameter inference types, avoiding misjudgment and facilitating collaborative search and definition
2. Supports two call types with and without return values
3. Perfect compatibility with asynchronous modules
```
### 2.Single thread asynchronous CoTask (can delete the whole folder if you need to replace with UniTask, etc.)

```c#
1. Support single thread asynchronous
2. Support Await process, CoTask transfer process, and zero cost switching of projects that have already used the process
3. Support waiting for Dotween and Addressable
4. Support access to more new waiting devices, with extremely low learning costs.
5. Support automatic transmission of cancellation tokens, without the need for manual transmission of cancellation tokens, and support suspension and continuation.
```
## II. Update Plan
- RPC remote invocation support
- CoTask performance improves

## III. Use of documents

### 1.CoEvent covariant event system

1). Pre-configuration:


```c#
Skip this step when using Unity2021 and above
If using NET CORE, locate the error message and modify the macro definition according to the prompts.
```
2). Event definition

There are two interfaces for defining events. ISendEvent does not have a return value, and ICallEvent has a return value


```csharp
//If this section involves multiple developers, you can place all the events in one folder or also in the most relevant part of the event, which can be easily consulted
public interface MyEvent: ISendEvent<Type of parameter...>
public interface MyEvent: ICallEvent<Type of parameter...Type of returnValue>
```

3). Registration and Cancellation Event

Send and call can only be performed after registration


``` csharp
this.Operator<Message Type Interface>().Subcribe(MyAction);

//Please note that the delegation closure problem of C # means that if you do not cancel the event, the extracted reference objects in the delegation will not be automatically recycled by GC. This is a common Memory leak trap of C # delegation. This is a problem that exists in almost all event systems.

this.Operator<Message Type Interface>().UnSubcribe(MyAction);
```

4). Call and send


``` csharp
//调用全部
this.Operator<消息类型接口>().Send(...参数们);
//调用全部
var results = this.Operator<Message Type Interface>().Call(...parameters);
//调用第一个
var result = this.Operator<Message Type Interface>().CallFirst(..parameters);
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

   //Waitable Task
   public async CoTask mTest()
    {
    	//Wait 600 frame
        await CoTask.WaitForFrame(600);
        Debug.Log("Hha");
        //wait 3 seconds
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
//Get Token
mTest().WithToken(out var token);
//Pause
token.Yield();
//Continue
token.Continue();

//Cancel
token.OnCanceled +=()=>
{
    Debug.Log("Handling after task cancellation");
}
//Cancel Task
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

//using namespace
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//Choose a suitable namespace that can be accessed
namespace CoEvents.Async
{
    //Static class, defining extension methods
    public static class AddressableEx 
    {
        //Create a GetAwaiter extension method and add a SetResult delegate to the specified completion callback (eliminate duplicate GC if pooled)
        public static CoTask<AsyncOperationHandle> GetAwaiter(this AsyncOperationHandle handle)
        {
            //create Task
            var task = CoTask<AsyncOperationHandle>.Create();
            //bind callback
            handle.Completed += task.SetResult;
            //return task
            return task;
        }
        //generic type support
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
//delay on seconds
CoTask.Delay(1);
//to unity coroutine
CoTask.ToCoroutine(async ()=>
{
	yield return new WaitForSceonds();
})
//a completed task
CoTask.CompletedTask

//No more introduction
	
```

9). Create Cotask without calling


```
Func<CoTask> task = async ()=>
{
	//This lambda can be an asynchronous method, which obtains the CoTask
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

