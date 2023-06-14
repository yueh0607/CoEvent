using CoEvents;
using CoEvents.Async;
using UnityEngine;



public interface IAsyncEvent : ICallEvent<int, CoTask> { }
public class AsyncEventTest : MonoBehaviour
{

    public async CoTask DoAsync(int x)
    {
        //等待X秒
        await CoTask.Delay(x);
        Debug.Log("hha");
    }


    async void Start()
    {
        //注册事件
        this.Operator<IAsyncEvent>().Subscribe(DoAsync);

        //调用第一个注册的事件
        await this.Operator<IAsyncEvent>().CallFirst(2);
    }

}
