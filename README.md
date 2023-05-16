# CoEvent
CoEvent是一个能约束参数类型的，能约束调用类型的，参数和调用类型安全的轻量级本地消息系统，支持一切能支持Unsafe.As类似API的CSharp编程平台。
目前了解的支持平台有.NET CORE和Unity3D 2020.1 OR NEWER。
理论上版本更低的Unity3D也能实现(可能在后续版本提供支持)，不过没有提供原生的UnsafeUtility.As，可以自己实现一个替换上去。

## 一、优势
CoEvent 的优势主要在于“约束”  ，现在传统的消息系统一般有如下方式实现
- 1.反射(Unity的SendMessage已经被无情抛弃了)
- 2.int，string，enum做消息标识(注册和取消要手动写一大串泛型参数，调用要手动写泛型参数，如果写错了...后果不堪设想)
- 3.接口化的，基类化的(部分类似的基于观察者模式写了接口，继承基类可以比较容易实现这种约束，但是继承接口的很难去做约束，不好判断消息类型，很多接口约束只能有唯一消息)

给大家看看主流实现的方式怎么调用
```charp
//Unity原生SendMessage，通过反射，效率低，容易写错名字，写错时候字符串是没提示的，很难查错
Monobehaviour.SendMessage("Ttt");
//int，string，enum做标识的
EventCenter.Add<int,int>("name",MyAction);//在注册时需要写泛型
EventCenter.Send("name",10,20);  //调用时没有约束，我也可以传入错误的参数类型，当写多了就变成了灾难。

```


而CoEvent是怎么实现的？
CoEvent采用了第二种方式，但是不同的是消息标识是接口，能根据泛型推断省略注册和取消时的泛型参数，也能限制发布事件时的参数类型。
也就是说，CoEvent是不会发生那种写错泛型参数的类型情况的。


