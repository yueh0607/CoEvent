/********************************************************************************************
 * Author : YueZhenpeng
 * Date : 2023.2.25
 * Description : 
 * 此类为异步令牌的底层实现，要求形成任务树结构
 */

namespace CoEvent.Async.Internal
{
    public class AsyncTreeTokenNode
    {

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
            //非Builder任务则空
            if (Current == Root)
            {
                Current.Authorization = false;
                // UnityEngine.Debug.Log($"{((Unit)Current).ID}-SetFalse,TaskToken:{Current.Token.ID}");
            }
            else
            {
                this.Current.Token.Yield();
            }
        }
        public void Continue()
        {
            if (Current == Root)
            {

                Current.Authorization = true;
            }
            else
            {
                this.Current.Token.Yield();
            }

        }
        public void Cancel()
        {

            if (Current == Root)
            {
                Current.Authorization = false;
                Current?.SetCancel();
                //Root.SetException(new AsyncTokenCancelException());
            }
            else
            {
                this.Current.Token.Yield();
                this.Current.Token.Cancel();
            }


        }
    }

}
