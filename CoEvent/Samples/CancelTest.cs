using CoEvents.Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelTest : MonoBehaviour
{

    public async CoTask DoTimeLog()
    {


        await CoTask.Delay(5);
        Debug.Log("End");
    }


   

    void Start()
    {

        DoTimeLog().WithToken(out var token);

        token.OnCanceled += () =>
        {
            Debug.Log($"{nameof(CancelTest)}.");
        };
        token.Cancel();

    }


}
