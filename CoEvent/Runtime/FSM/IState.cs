using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public FSMController Machine { get; set; }

    public void OnInit();


    public void OnEnter();



    public void OnUpdate();

    public void OnFixedUpdate();
    public void OnExit();
    public void OnDestroy();


}

public abstract class AbstractState : IState
{
    public FSMController Machine { get ; set ; }
    public abstract void OnInit();
    
    public abstract void OnEnter();

    public abstract void OnFixedUpdate();
 
    public abstract void OnUpdate();
    public abstract void OnExit();
    public abstract void OnDestroy();
}
