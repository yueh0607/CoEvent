using CoEvents;
using UnityEngine;


//不带返回值
public interface IMyEvent : ISendEvent<int, string> { }

//带返回值
public interface ICustomCallEvent : ICallEvent<int, string> { }


public class SyncEventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //*********************注册/*****************************************
        //1.委托注册
        this.Operator<IMyEvent>().Subscribe((x, y) =>
        {
            Debug.Log(x);
            //Do something here.
        });

        //2.方法注册
        this.Operator<IMyEvent>().Subscribe(Aaaa);

        //带返回值
        this.Operator<ICustomCallEvent>().Subscribe(Bbbb);
        //*******************************************************************
        //可以在其他脚本和类，任意地方调用下面的方法
        this.Operator<IMyEvent>().Send(10, "10");
        string str = this.Operator<ICustomCallEvent>().CallFirst(10);

        //*********************************小建议
        //如果在静态类中无法使用this
        //CoEvent.Instance.Operator<>

        //快速开启协程
        //CoEvent.Mono.StartCoroutine()


        //*****************************************


    }



    private void OnDestroy()
    {
        this.Operator<IMyEvent>().UnSubscribeAll();
        this.Operator<ICustomCallEvent>().UnSubscribe(Bbbb);
    }


    void Aaaa(int x, string y)
    {
        Debug.Log(x);
    }

    string Bbbb(int y)
    {
        return y.ToString();
    }
}
