# CoEvent 


# [Chinese](https://github.com/yueh0607/CoEvent/blob/main/README.md)|[English](https://github.com/yueh0607/CoEvent/blob/main/README.EN.md)
CoEvent是一个基于**观察者模式**的**事件系统**，它支持**参数类型的约束**，**显式化事件定义**以便于多人协作，能防止参数类型误判，优化协作时的通信规范。
支持Unity3D 2021.1以上和.NETCORE平台的开发。


## 一、功能与模块

### 1.协变事件系统CoEvent

```c#
1.参数类型约束，相对于参数推断类型有很大优势，不会错判，便于协作查找定义
2.支持带返回值不带返回值两种调用类型
3.与异步模块完美兼容
```



## 二、使用


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


## 三、默认内置对象池的使用

CoEvent为了优化性能，不得不引入对象池来进行对象复用，但是许多独立工具集和架构，插件，都含有自己的对象池，这样在引入大量插件后容易导致对象池代码重复。

CoEvent引入了一个性能较高，且功能较为简单的轻量对象池，且允许用户进行替换，CoEvent类中定义里一个Pool属性，**默认指定为CoDeaultPool**，您可以**将他设置为null，这时CoEvent将不会使用Poo**l。

同样的，您可以使用Pool进行一些其他对象的存储和复用，例如

```csharp
T item = CoEvent.Pool?.Allocate<T>();
CoEvent.Pool?Recycle(item);
```

同样的，也有针对于GameObject的API，都采用比较轻量化的实现。

