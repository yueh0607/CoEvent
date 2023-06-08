using CoEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FSMController))]
public class ConditionMachine : MonoBehaviour
{
    FSMController controller;


    private void Start()
    {
        controller= GetComponent<FSMController>();
    }


    public void SetConditionEnter<T,K>() where T: IState where K:ICallEvent<bool>
    {

    }
    
}
