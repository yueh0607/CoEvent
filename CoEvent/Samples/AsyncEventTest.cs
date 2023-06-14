using CoEvents;
using CoEvents.Async;
using UnityEngine;



public interface IAsyncEvent : ICallEvent<int, CoTask> { }
public class AsyncEventTest : MonoBehaviour
{

    public async CoTask DoAsync(int x)
    {
        await CoTask.Delay(x);
        Debug.Log("hha");
    }


    async void Start()
    {
        this.Operator<IAsyncEvent>().Subscribe(DoAsync);


        await this.Operator<IAsyncEvent>().CallFirst(2);
    }

}
