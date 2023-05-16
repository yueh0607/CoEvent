using CoEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMyTest : ICallEvent<int, int> { }
public class Test : MonoBehaviour
{

    int Ttt(int t) { return 1000; }
    // Start is called before the first frame update
    void Start()
    {
        this.Operator<IMyTest>().Subscribe(Ttt);
        var x = this.Operator<IMyTest>().Call(10);

        Debug.Log(x[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
