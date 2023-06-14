using CoEvents.Async;
using System.Collections;
using UnityEngine;

public class CoTask2Coroutine : MonoBehaviour
{
    public async CoTask DoAsync()
    {
        await CoTask.Delay(5);
        Debug.Log("hh");
    }
    void Start()
    {
        IEnumerator enumerator = CoTask.ToCoroutine(DoAsync);
        StartCoroutine(enumerator);


        StartCoroutine(CoTask.ToCoroutine(async () =>
        {
            await CoTask.Delay(5);
            Debug.Log("hh2");
        }));


    }


}
