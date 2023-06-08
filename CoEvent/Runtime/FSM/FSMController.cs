using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public sealed class FSMController : MonoBehaviour
{

    [SerializeField] string defaultState = String.Empty;

    private Dictionary<Type, (IState, bool)> states = new Dictionary<Type, (IState, bool)>();

    public void GetRunningStateTypes(List<Type> runningStatesList)
    {
        foreach (var state in states)
        {
            if (state.Value.Item2)
            {
                runningStatesList.Add(state.Key);
            } 
        }
    }
    public bool IsRunning<T>() => IsRunning(typeof(T));
    bool IsRunning(Type type)
    {
        if (states.ContainsKey(type))
        {
            if (states[type].Item2 == true) return true;
        }
        return false;
    }

    public void Enter<T>() where T : IState
    {
        Enter(typeof(T));
    }
    public void Exit<T>() where T : IState
    {
        Exit(typeof(T));
    }

    void Enter(Type type)
    {
        if (!states.ContainsKey(type))
        {
            var st = (IState)Activator.CreateInstance(type);
            st.Machine = this;
            st.OnInit();
            states.Add(type, (st, false));
        }
        var state = states[type];
        state.Item2 = true;
        state.Item1.OnEnter();
    }


    void Exit(Type type)
    {
        if (!states.ContainsKey(type)) return;
        var state = states[type];
        if (state.Item2 == false) return;
        state.Item2 = false;
        state.Item1.OnExit();
    }

    public void ExitAll()
    {
        foreach (var state in states)
        {
            Exit(state.Key);
        }
    }

    void Start()
    {
        if (defaultState != String.Empty)
        {
            Type defaultType = Type.GetType(defaultState);
            Enter(defaultType);
        }
    }

    void Update()
    {
        foreach (var state in states)
        {
            if (state.Value.Item2)
                state.Value.Item1.OnUpdate();
        }

    }
    private void FixedUpdate()
    {
        foreach (var state in states)
        {
            if (state.Value.Item2)
                state.Value.Item1.OnFixedUpdate();
        }
    }

    private void OnDestroy()
    {
        ExitAll();
        foreach (var state in states)
        {
            state.Value.Item1.OnDestroy();
        }
        states.Clear();
    }
}
