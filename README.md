# CoEvent

CoEvent是一个参数和调用类型安全的轻量级事件系统。
目前了解的支持平台有.NET CORE和Unity3D 2020.1 OR NEWER。
理论上版本更低的Unity3D也能实现(可能在后续版本提供支持)，不过没有提供原生的UnsafeUtility.As，可以自己实现一个替换上去。
## 更新计划
- 适配async到Addressable，YooAsset。
- RPC支持

## 二、使用
CoEvent本着简单易用的原则实现了基于观察者模式的一个事件系统。能很轻松实现跨对象，跨模块之间的事件通信。也实现了单线程异步这样的功能，旨在顺从同步思路，消除回调等待的逻辑，优化开发体验。


1.前期配置：
```
如果你是在Unity3D 2020.1以上使用，无需配置任何内容。
如果你是在.NetCore使用，需要在任意一个参与编译的文件里加上宏定义，#define NETCORE
如果你在其他平台使用，可以在CoUnsafeAs.cs里去给出Unsafe.As的类似实现。
无法实现？不支持该平台QAQ
```
2.事件定义
```csharp

//通用事件，可以Send可以Call，但是只能用一个！！！
public interface MyEvent: IGenericEvent<参数类型,参数类型...>
//可发送
public interface MyEvent: ISendEvent<...>
//可调用(最后一个泛型参数是返回值)
public interface MyEvent: ICallEvent<...>
```


3.注册和取消事件(MyAction也是被约束类型的)
``` csharp
this.Operator<消息类型接口>().Subcribe(MyAction);
this.Operator<消息类型接口>().UnSubcribe(MyAction);
```
4.调用和发送
``` csharp
this.Operator<消息类型接口>().Send(...参数们);
var results = this.Operator<消息类型接口>().Call(...参数们);
```


## 三、例子
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
        this.Operator<IMyTest>().UnSubscribe(Ttt);
        this.Operator<IMyTest>().Send(10,100);
    }

}
```

## 四、单线程异步

CoEvent提供了一种极佳的方式来处理回调等异步逻辑

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
使用方式非常的简单，都放在了Async类里，当然也支持取消，挂起等，只需要对CoTask调用拓展方法WithToken
```csharp
mTest().WithToken(out var token);
token.Yield();
```

如果您需要进一步的拓展CoTask，只需要写一个方法,CoTask.Create拿到新的CoTask，然后在任务结束时调用它的SetResult即可，可以参考Async类的实现。
可以简单的与YooAsset等进行对接


