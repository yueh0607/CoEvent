#CoEvent

CoEvent is a ** event system ** based on ** Observer pattern * *, which supports * * parameter type constraints * *, * * explicit event definitions * * to facilitate multi person collaboration. Compared with the event system based on integer and enumeration, CoEvent has great advantages, can prevent parameter type misjudgment, and can optimize communication specifications between modules during collaboration.
CoEvent supports the development of Unity3D 2021.1 or above and the. NETCORE platform. (If you need more support, please contact the author)
CoTask supports await protocols, CoTask transfer protocols, Tasks, Addressable and Dotween solutions, and allows for easy access to more solutions. CoTask supports single threaded operations such as WebGL JS.
##1、 Functions and modules
###1. CoEvent (Core)
```C#
1. Parameter type constraints have significant advantages over parameter inference types, avoiding misjudgment and facilitating collaborative search and definition
2. Supports two call types with and without return values
3. Perfect compatibility with asynchronous modules
```
###2. Single threaded asynchronous CoTask (if needed to be replaced with UniTask, the entire folder can be deleted)
```C#
1. Support single thread asynchronous
2. Support Await process, CoTask transfer process, and zero cost switching of projects that have already used the process
3. Support waiting for Dotween and Addressable
4. Support access to more new waiting devices, with extremely low learning costs.
5. Support automatic transmission of cancellation tokens, without the need for manual transmission of cancellation tokens, and support suspension and continuation.
```
##2、 Update Plan
-RPC remote call support
-CoTask performance improvement
##3、 Using Documents
###1. CoEvent covariant event system
1) Preliminary configuration:
```C#
Skip this step when using Unity2021 and above
If using NET CORE, locate the error message and modify the macro definition according to the prompts.
```
2) Event Definition
There are two interfaces for defining events, ISendEvent without a return value and ICallEvent with a return value
```Csharp
//If this section involves multiple developers, you can place all the events in one folder or also in the most relevant part of the event, which can be easily consulted
Public interface MyEvent: ISendEvent
Public interface MyEvent: ICallEvent<parameter type Return Value Type>
```
3) Registration and cancellation events
Sending and calling can only be performed after registration
```Csharp
This. Operator<Message Type Interface>(). Subscribe (MyAction);
//Please note that the delegation closure problem of C # means that if you do not cancel the event, the extracted reference objects in the delegation will not be automatically recycled by GC. This is a common Memory leak trap of C # delegation. This is a problem that exists in almost all event systems.
This. Operator<Message Type Interface>(). UnSubscribe (MyAction);
```
4) Calling and Sending
```Csharp
//Call All
This. Operator<Message Type Interface>(). Send (... parameters);
//Call All
Var results=this. Operator<Message Type Interface>(). Call (... parameters);
//Call the first
Var result=this. Operator<Message type interface>(). CallFirst (parameters);
```
5) Routine
```Csharp
Using CoEvent;
Using UnityEngine;
Public interface IMyTest: ISendEvent<int, int>{}
Public class test: MonoBehavior
{
Void Ttt (int t, int k)
{
Debug. Log ($"{t}: {k}");
}
Void Start()
{
This. Operator<IMyTest>(). Subscribe (Ttt);
This. Operator<IMyTest>(). Send (10100);
}
Void OnDestroy()
{
This. Operator<IMyTest>(). Unsubscribe (Ttt);
}
}
```
###3. CoTask automatic token single thread asynchronous
CoEvent implements a low GC reference TaskLike and is single threaded (supports Web Js). The solution meets the Async/Await model, which was first proposed by C # and aims to convert difficult to understand callback logic into synchronous thinking logic. Greatly simplifies the complexity of thinking.
1) Asynchronous method definition: By using CoTask or CoTask<T>as the return value and adding the async keyword, you can use await to perform asynchrony in a single thread.
Be cautious when using void as the return value, as it will result in using System. Threading. Tasks, and the operation will not be within the main thread.
```Csharp
Using CoEvent. Async;
//Waiting Tasks
Public async CoTask mTest()
{
//Waiting for 600 frames
Await CoTask. WaitForFrame (600);
Debug. Log ("HHa");
//Wait for 3 seconds
Await CoTask. Delay (3);
Debug. Log ("111");
}
```
2) Cancel warning wavy line
You may encounter a clear warning wavy line when calling CoTask as the return value, which can be used to eliminate the warning.
```C#
MTest(). Discrad();
_= MTest();
```
Either of the two methods is allowed.
3) Non asynchronous calls
Sometimes you cannot define the async keyword and can execute asynchrony by forcibly calling the Invoke method
```
Task. Invoke();
```
4) Cancel Token
The usage method is very simple, supporting operations such as cancel, suspend, and continue. You only need to call the extension method WithToken on CoTask to obtain the token. The biggest difference between UniTask and ETTask is that there is no need to manually pass the token, and the token is automatically passed.
```Csharp
//Obtain Token
MTest(). WithToken (out var token);
//Pause
Token. Yield();
//Continue
Token. Continue();
//Cancel
Token. OnCancelled+=()=>
{
Debug. Log ("Processing after task cancellation");
}
//Cancel Task
Token. Cancel();
```
5)  Event system support
The principle is quite simple, treating asynchronous methods as special methods for returning values
```Csharp
Public interface IMyEventAsync: ICallEvent<CoTask>{}
Private async CoTask Stt()
{
Await CoTask. CompletedTask;
}
Private async CoTask StartAsync()
{
This. Operator<IMyEventAsync>(). Subscribe (Stt);
Await this. Operator<IMyEventAsync>(). CallFirst();
}
```
6)  Expand CoTask support
There are macro definitions in the folder under the CoTask/Support directory, which can be opened with one click, making it easy to access * * DoTween, Addressable, etc**
7)  Expand support for CoEvent. Async
The common steps are as follows:
0. Introduce types into references to assembly CoEvents. (Important!!! You can also remove the assembly definition of CoEvent, which will be simpler)
1. Find the asynchronous operation base class of the corresponding library, for example, the AsyncOperationHandle class of Addressable is the asynchronous base class
2. Find the completion callback for the asynchronous base class, such as AsyncOperationHandle. Completed, which is what the base class calls upon completion
3. Define the extension method GetAwaiter for the asynchronous base class, create a CoTask, and add SetResult to the completion callback of the asynchronous base class
4. Return asynchronous task
Please refer to the following routine extension definition:
```Csharp
Using System. Collections;
Using System. Collections. Generic;
Using System. ComponentModel;
Using UnityEngine;
//Introducing Namespaces
Using UnityEngine. Addressable Assets;
Using UnityEngine. ResourceManagement. AsyncOperations;
//Choose a suitable namespace that can be accessed
Namespace CoEvents. Async
{
//Static classes, defining extension methods
Public static class AddressableEx
{
//Create a GetAwaiter extension method and add a SetResult delegate to the specified completion callback (eliminate duplicate GC if pooled)
Public static CoTask<AsyncOperationHandle>GetAwaiter (this AsyncOperationHandle handle)
{
//Create Task
Var task=CoTask<AsyncOperationHandle>. Create();
//Bind callback
Handle. Completed+=task. SetResult;
//Return to task
Return task;
}
//Generic support
Public static CoTask<AsyncOperationHandle<T>>GetAwaiter<T>(this AsyncOperationHandle<T>handle)
{
Var task=CoTask<AsyncOperationHandle<T>>. Create();
Handle. Completed+=task. SetResult;
Return task;
}
}
}
```
8) Common asynchronous tasks
In the CoTask class, these methods can be directly called
```
//Delay by one second
CoTask. Delay (1);
//Transfer process
CoTask. ToCoroutine (async()=>
{
Yield return new WaitForSceonds();
})
//Completed Tasks
CoTask. CompletedTask
//No more introduction
```
9)  Create a Cotask without calling it
```
Func<CoTask>task=async()=>
{
//This lambda can be an asynchronous method, which obtains the CoTask
}
```
##3、 Use of default built-in object pools
In order to optimize performance, CoEvent has to introduce object pools for object reuse. However, many independent toolsets, architectures and plug-ins have their own object pools, which can easily lead to Duplicate code of object pools after the introduction of a large number of plug-ins.
CoEvent introduces a high-performance and relatively simple lightweight object pool that allows users to replace it. In the CoEvent class, there is a Pool attribute defined, which is * * defaulted to CoDeaultPool * *. You can * * set it to null, and CoEvent will not use it
