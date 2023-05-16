# CoEvent
CoEvent是一个能约束参数类型的，能约束调用类型的，参数和调用类型安全的轻量级本地消息系统，支持一切能支持Unsafe.As类似API的CSharp编程平台。
目前了解的支持平台有.NET CORE和Unity3D 2020.1 OR NEWER。
理论上版本更低的Unity3D也能实现(可能在后续版本提供支持)，不过没有提供原生的UnsafeUtility.As，可以自己实现一个替换上去。

## 一、优势
CoEvent 的优势主要在于“约束”  ，现在传统的消息系统一般有如下方式实现
- 1.反射(Unity的SendMessage已经被无情抛弃了)
- 2.int，string，enum做消息标识(注册和取消要手动写一大串泛型参数，调用要手动写泛型参数，如果写错了...后果不堪设想)
- 3.接口化的，基类化的(部分类似的基于观察者模式写了接口，继承基类可以比较容易实现这种约束，但是继承接口的很难去做约束，不好判断消息类型，很多接口约束只能有唯一消息)

给大家看看主流实现的方式怎么调用和实现的
```csharp
//Unity原生SendMessage，通过反射，效率低，容易写错名字，写错时候字符串是没提示的，很难查错
Monobehaviour.SendMessage("Ttt");
//int，string，enum做标识的
EventCenter.Add<int,int>("name",MyAction);//在注册时需要写泛型
EventCenter.Send("name",10,20);  //调用时没有约束，我也可以传入错误的参数类型，当写多了就变成了灾难。
//基类和接口继承的
//基类不用说了，侵入性太强了
//接口继承的，一般一个类只能有一个事件，不然很难判断类型的参数
//当然也有写分析器的，那都是大牛，QAQ，本渣做不到
```

而CoEvent是怎么实现的？
CoEvent采用了第二种方式，但是不同的是消息标识是接口，很巧妙的利用协变实现了约束，能根据泛型推断省略注册和取消时的泛型参数，也能限制发布事件时的参数类型。
也就是说，CoEvent是不会发生那种写错泛型参数的类型情况的，也不会发生Send注册，结果Call调用这种情况。极大的降低了开发者的代码失误几率，减少了Debug的时间。

## 二、使用
CoEvent本着简单易用的原则实现了基于观察者模式的一个事件系统。

1.前期配置：
如果你是在Unity3D 2020.1以上使用，无需配置任何内容。
如果你是在.NetCore使用，需要在任意一个参与编译的文件里加上宏定义，#define NETCORE
如果你在其他平台使用，可以在CoUnsafeAs.cs里去给出Unsafe.As的类似实现。
无法实现？不支持该平台QAQ

2.事件定义
```csharp

//通用事件，可以Send可以Call，但是只能用一个！！！
public interface MyEvent: IGenericEvent<参数类型,参数类型...>
//可发送
public interface MyEvent: ISendEvent<...>
//可调用(最后一个泛型参数是返回值)
public interface MyEvent: ICallEvent<...>

```csharp

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


