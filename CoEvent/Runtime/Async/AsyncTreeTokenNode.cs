/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 此类为异步令牌的底层实现，要求形成任务树结构
 */

namespace CoEvents.Async.Internal
{
    public class AsyncTreeTokenNode:IAuthorization
    {

        /// <summary>
        /// 授权状态，代表当前任务是否被挂起，也决定了状态机是否能够继续前进
        /// </summary>
        public bool Authorization { get; internal set; } = true;
        //当前MethodBuilder执行的任务
        public IAsyncTokenProperty Current;


        //MethodBuilder代表的任务
        public IAsyncTokenProperty Root;
        public AsyncTreeTokenNode(IAsyncTokenProperty Root, IAsyncTokenProperty Current)
        {
            this.Current = Current;
            this.Root = Root;
        }

        public void Yield()
        {
            Authorization = false;
            //非Builder任务则空
            if (Current != Root)
            {
                this.Current.Token?.Yield();
            }
        }
        public void Continue()
        {
            Authorization = true;
            if (Current != Root)
            {
                this.Current.Token?.Continue();
            }

        }
        public void Cancel()
        {
            Authorization = false;
            if (Current == Root)
            {
               
                Current?.SetCancel();

            }
            else
            {

                this.Current.Token?.Cancel();
            }


        }
    }

}
