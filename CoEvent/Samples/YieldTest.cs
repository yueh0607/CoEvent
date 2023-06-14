using CoEvents.Async;
using UnityEngine;

public class YieldTest : MonoBehaviour
{

    //测试方法
    public async CoTask Test()
    {
        //延迟5秒输出
        await CoTask.Delay(5f);
        Debug.Log(11);
    }

    //测试流程
    public async CoTask DealyTimePause()
    {
        //取得令牌
        Test().WithToken(out var token).Discard();
        //三秒后暂停
        await CoTask.Delay(3);
        Debug.Log("暂停");
        token.Yield();
        //五秒后继续
        await CoTask.Delay(5);
        token.Continue();
    }

    void Start()
    {
        DealyTimePause().Discard();
    }



}
