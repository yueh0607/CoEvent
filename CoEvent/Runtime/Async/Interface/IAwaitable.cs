/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 为了使抽象的编辑器要求变得具体，这里创建接口统一可等待的对象
 */

namespace CoEvents.Async
{
    /// <summary>
    /// 实现此接口使得一个对象可以被await关键字所支持
    /// </summary>
    /// <typeparam name="TAwaiter"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IAwaitable<out TAwaiter> where TAwaiter : IAwaiter
    {
        /// <summary>
        /// 获取一个实现IAwaiter对象
        /// </summary>
        /// <returns></returns>
        TAwaiter GetAwaiter();
    }
    /// <summary>
    /// 实现此接口使得一个对象可以被await关键字所支持
    /// </summary>
    /// <typeparam name="TAwaiter"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IAwaitable<out TAwaiter, out TResult> where TAwaiter : IAwaiter<TResult>
    {
        /// <summary>
        /// 获取一个实现IAwaiter<TResult>对象
        /// </summary>
        /// <returns></returns>
        TAwaiter GetAwaiter();
    }

}
