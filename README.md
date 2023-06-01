# CoEvent

CoEvent是一个参数和调用类型安全的轻量级事件系统。本着简单易用的原则实现了基于观察者模式的事件系统。能轻松实现跨对象，跨模块之间的事件通信。同时也实现了单线程异步的功能，采用同步思路消除回调等待的逻辑，降低逻辑复杂度，优化开发体验。

目前了解的支持平台有.NET CORE和Unity3D 2021.1 OR NEWER。
理论上版本更低的Unity3D也能实现(可能在后续版本提供支持)，不过没有提供原生的UnsafeUtility.As，可以自己实现一个替换上去。
## 目前支持的功能
1.协变事件系统 （核心）
2.单线程异步（如果不喜欢可以单独删除Async文件夹相关内容，您还可以采用UniTask和ETTask等，甚至使用Unity协程或回调方式）
3.简单池管理器（如果不需要可以在CoEvent类里取消实例化）
## 更新计划
- 适配async到Addressable(已支持)，YooAsset(未支持，可以参考ETTask接入)。
- RPC支持
- FSM/BehaviourTree支持

## 协变事件系统的使用

1.前期配置：
```
建议在Unity2021及以上使用，在这个版本以下使用，请先实现Unsafe.As或指定相关API，
如果在NET CORE使用请定位错误信息并根据提示修改宏定义。
```
2.事件定义
```csharp
//这一部分如果涉及多人开发，您可以把所有事件放在一个文件夹下，也可也放在事件的最相关处，这样做可以方便查阅

//通用事件，可以Send可以Call，但是只能用一个！！！
public interface MyEvent: IGenericEvent<参数类型,参数类型...>
//可发送
public interface MyEvent: ISendEvent<...>
//可调用(最后一个泛型参数是返回值)
public interface MyEvent: ICallEvent<...>
```


3.注册和取消事件
``` csharp
this.Operator<消息类型接口>().Subcribe(MyAction);

//请注意，C#的委托闭包问题，也就是说如果您不取消事件，委托内被提取的引用对象将不会被GC自动回收，这是C#委托常见的一个内存泄漏陷阱。
this.Operator<消息类型接口>().UnSubcribe(MyAction);
```

4.调用和发送
``` csharp
this.Operator<消息类型接口>().Send(...参数们);
var results = this.Operator<消息类型接口>().Call(...参数们);
var result = this.Operator<消息类型接口>().CallFirst(参数们);
```

5.例程
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



## 单线程异步的使用

CoEvent提供了一种极佳的方式来处理回调等异步逻辑，您如果使用过ETTask和UniTsak就会明白该方案的优越性，CoEvent实现了一个低GC的引用Task，且为单线程，不存在与Unity主线程资源交互的回调地狱。

设计思想核心在于回调的消除，极大的优化了逻辑思维复杂度，优化了异步编程体验和性能。

```csharp
using CoEvent.Async;

    //可等待的任务
   public async CoTask mTest()
    {
    //等待600帧
        await Async.WaitForFrame(600);
        Debug.Log("Hha");
        //等待3秒
        await Async.Delay(3);
        Debug.Log("111");
    }

```
使用方式非常的简单，都内置于静态类Async内，当然也支持取消，挂起等，只需要对CoTask调用拓展方法WithToken即可取得令牌
```csharp
mTest().WithToken(out var token);
token.Yield();
```


我们可以把异步方法视作一种特殊返回值的同步方法

```csharp
 using CoEvents;
using CoEvents.Async;
using UnityEngine;

public interface IMyEventAsync : ICallEvent<CoTask> { }

public class Test : MonoBehaviour
{

    private async CoTask Stt()
    {
        await Async.CompletedTask;
    }

    private async CoTask StartAsync()
    {
        this.Operator<IMyEventAsync>().Subscribe(Stt);
        await this.Operator<IMyEventAsync>().CallFirst();
    }
}
```

拓展CoEvent.Async的支持，您可以参照下面的方式将CoTask拓展到Addressable的异步解决方案。同样的，您可以将它接入到YooAsset等方案中。

常见步骤如下：
0. 将Addressable引入到程序集CoEvents的引用中。（很重要！！！也可删除CoEvent的程序集定义将CoEvent合并到默认程序集内）
1. 找到对应库的异步操作基类 ，例如Addressable的AsyncOperationHandle
2. 找到异步基类的完成回调，例如AsyncOperationHandle.Completed
3. 定义异步基类的拓展方法GetAwaiter，创建CoTask将SetResult加入到异步基类的完成回调中
4. 返回异步任务

```csharp
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CoEvents.Async
{
    public static class AddressableEx 
    {
        //创建GetAwaiter拓展方法，向指定完成回调里加入SetResult委托（如果入池则消除重复GC）
        public static CoTask<AsyncOperationHandle> GetAwaiter(this AsyncOperationHandle handle)
        {
            var task = CoTask<AsyncOperationHandle>.Create();
            handle.Completed += task.SetResult;
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
## 默认对象池的使用
CoEvent为了优化性能，不得不引入对象池来进行对象复用，但是许多独立工具集和架构，插件，都含有自己的对象池，这样在引入大量插件后容易导致对象池代码重复。

CoEvent引入了一个性能较高，且较为简单的对象池，且允许用户进行替换，CoEvent类中定义里一个Pool属性，默认指定为CoDeaultPool，您可以将他设置为null，这时CoEvent将不会使用Pool。

同样的，您可以使用Pool进行一些其他对象的存储和复用，例如
```csharp
T item = CoEvent.Pool?.Allocate<T>();
CoEvent.Pool?Recycle(item);
```
同样的，也有针对于GameObject的API，都采用轻量化实现。


