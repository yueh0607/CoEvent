/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 创建IAsyncTokenProperty接口来对接异步令牌需要的属性和方法，就是提供一个对接口，仅此而已
 */

namespace CoEvents.Async.Internal
{
    /// <summary>
    /// 使用该接口统一支持异步令牌
    /// </summary>
    public interface IAsyncTokenProperty
    {
        AsyncTreeTokenNode Token { get; internal set; }

        public void SetCancel();
    }

}
