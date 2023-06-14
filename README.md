# CoEvent 


# [中文](https://github.com/yueh0607/CoEvent/edit/main/README.md)|[English](https://github.com/yueh0607/CoEvent/edit/main/README.EN.md)
CoEvent是一个基于**观察者模式**的**事件系统**，它支持**参数类型的约束**，**显式化事件定义**以便于多人协，相对于基于整型和枚举的事件系统有巨大优势，能防止参数类型误判，能优化协作时模块之间通信规范。

CoEvent支持Unity3D 2021.1以上和.NETCORE平台的开发。(如果需要更多支持请联系作者)



CoTask的支持await协程，支持CoTask转协程，支持Task，支持Addressable和Dotween等方案，且允许很容易的接入更多方案。CoTask支持WebGL JS等单线程操作。

## 一、功能与模块

### 1.协变事件系统CoEvent(核心)

```c#
1.参数类型约束，相对于参数推断类型有很大优势，不会错判，便于协作查找定义
2.支持带返回值不带返回值两种调用类型
3.与异步模块完美兼容
```

### 2.单线程异步CoTask ( 如果需要替换为UniTask等可以删除整个文件夹 )

```c#
1. 支持单线程异步
2. 支持await协程，CoTask转协程，已经用协程的项目零成本切换
3. 支持Dotween，Addressable的等待
4. 支持接入更多新的等待，学习成本极低。
5. 支持取消令牌自动传递，无需手动传递取消令牌，支持挂起和继续。
```

## 二、更新计划

- RPC远程调用支持
- CoTask性能提高

## 三、使用文档

### 1.CoEvent 协变事件系统

1).前期配置：

```c#
在Unity2021及以上使用跳过这一步
如果在NET CORE使用请定位错误信息并根据提示修改宏定义。
```

2).事件定义

定义事件有两种接口，ISendEvent是不带返回值的,ICallEvent是带返回值的

```csharp
//这一部分如果涉及多人开发，您可以把所有事件放在一个文件夹下，也可也放在事件的最相关处，这样做可以方便查阅

public interface MyEvent: ISendEvent<参数类型...>
public interface MyEvent: ICallEvent<参数类型...返回值类型>
```

3).注册和取消事件

仅在注册后才能执行发送和调用

``` csharp
this.Operator<消息类型接口>().Subcribe(MyAction);

//请注意，C#的委托闭包问题，也就是说如果您不取消事件，委托内被提取的引用对象将不会被GC自动回收，这是C#委托常见的一个内存泄漏陷阱。这是一个几乎全部事件系统都存在的问题。
this.Operator<消息类型接口>().UnSubcribe(MyAction);
```

4).调用和发送

``` csharp
//调用全部
this.Operator<消息类型接口>().Send(...参数们);
//调用全部
var results = this.Operator<消息类型接口>().Call(...参数们);
//调用第一个
var result = this.Operator<消息类型接口>().CallFirst(参数们);
```

5).例程

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



### 3.CoTask 自动令牌单线程异步

CoEvent实现了一个低GC的引用TaskLike，且为单线程(支持Web Js)。改方案满足Async/Await模型，该模型由C#首次提出，旨在把难以理解的回调逻辑转换为同步思维的逻辑。极大的简化了思维复杂度。

1).异步方法定义：以CoTask或CoTask<T>作为返回值，添加async关键字，即可使用await在单线程中进行异步。

注意谨慎使用void作为返回值，那将导致使用System.Threading.Tasks，操作不在主线程内。

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

2).取消警告波浪线

您会遇到调用CoTask作为返回值时，出现警告波浪线的清空，您可以这样来消除警告。

```c#
mTest().Discrad();
_ = mTest();
```

两种方式的任意一种都被允许。

3).非async调用

有时您无法定义async关键字，可以通过强行调用Invoke方法的形式执行异步

```
task.Invoke();
```

4).取消令牌

使用方式非常的简单，支持取消，挂起，继续等操作，只需要对CoTask调用拓展方法WithToken即可取得令牌,和UniTask与ETTask最大的不同在于无需手动传递令牌，**令牌自动传递**。

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

5). 事件系统支持

原理相当简单，就是把异步方式视作返回值特殊的方法

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



6). 拓展CoTask支持

在CoTask/Supports 目录下的文件夹内均有宏定义，**一键开启**，很方便接入**DoTween，Addressable等**



7). 拓展CoEvent.Async的支持

常见步骤如下：

0. 引入类型到程序集CoEvents的引用中。（很重要！！！也可删除CoEvent的程序集定义，这种会更简单一些）

1. 找到对应库的异步操作基类 ，例如Addressable的AsyncOperationHandle类就是异步基类

2. 找到异步基类的完成回调，例如AsyncOperationHandle.Completed，也就是该基类在完成时调用的东西

3. 定义异步基类的拓展方法GetAwaiter，创建CoTask将SetResult加入到异步基类的完成回调中

4. 返回异步任务

   请看下面的例程拓展定义：

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



8).常用异步任务

在CoTask类中，可以直接调用这些方法

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

9). 创建Cotask却不调用

```
Func<CoTask> task = async ()=>
{
	//这个lambda可以是异步方法，这样就获取了这个CoTask
}
```





## 三、默认内置对象池的使用

CoEvent为了优化性能，不得不引入对象池来进行对象复用，但是许多独立工具集和架构，插件，都含有自己的对象池，这样在引入大量插件后容易导致对象池代码重复。

CoEvent引入了一个性能较高，且功能较为简单的轻量对象池，且允许用户进行替换，CoEvent类中定义里一个Pool属性，**默认指定为CoDeaultPool**，您可以**将他设置为null，这时CoEvent将不会使用Poo**l。

同样的，您可以使用Pool进行一些其他对象的存储和复用，例如

```csharp
T item = CoEvent.Pool?.Allocate<T>();
CoEvent.Pool?Recycle(item);
```

同样的，也有针对于GameObject的API，都采用轻量化实现。

