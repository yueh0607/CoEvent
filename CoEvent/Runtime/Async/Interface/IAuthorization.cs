/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.1.13
 * Description : 
 * 实现该接口以获取有效授权
 */

namespace CoEvent
{

    public interface IAuthorization
    {
        bool Authorization { get; internal set; }
    }
}
