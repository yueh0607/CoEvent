/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 在IAwairer的基础上，我们创建ICriticalAwaiter，能够使得Awaiter可以处理异常
 */


using System.Runtime.CompilerServices;
namespace CoEvents.Async
{
    /// <summary>
    /// 实现可以被GetAwaiter返回支持的Awaiter对象(当执行代码可能给程序造成负面影响时)
    /// </summary>
    public interface ICriticalAwaiter : ICriticalNotifyCompletion, IAwaiter
    {

    }

    /// <summary>
    /// 实现可以被GetAwaiter返回支持的Awaiter对象(当执行代码可能给程序造成负面影响时)
    /// </summary>
    public interface ICriticalAwaiter<T> : ICriticalNotifyCompletion, IAwaiter<T>
    {

    }
}
