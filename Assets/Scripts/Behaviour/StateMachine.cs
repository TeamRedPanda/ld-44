using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TName>
{
    IDictionary<TName, State> m_States = new Dictionary<TName, State>();
    TName m_CurrentStateName = default(TName);
    State m_CurrentState;

    private class State
    {
        public Action OnStateEnter;
        public Action OnStateUpdate;
        public Action OnStateExit;

        public State(Action onStateEnter, Action onStateUpdate, Action onStateExit)
        {
            OnStateEnter = onStateEnter;
            OnStateUpdate = onStateUpdate;
            OnStateExit = onStateExit;
        }
    }

    public void AddState(TName name, Action OnStateEnter, Action OnStateUpdate, Action OnStateExit)
    {
        if (Exists(name))
            throw new ArgumentException("Cannot add state with a duplicate name.");

        var state = new State(OnStateEnter, OnStateUpdate, OnStateExit);
        m_States.Add(name, state);
    }

    internal TName GetState()
    {
        return m_CurrentStateName;
    }

    public void OnUpdate()
    {
        m_CurrentState?.OnStateUpdate?.Invoke();
    }

    public void SetState(TName name)
    {
        // Only transition states if state exists.
        if (m_States.ContainsKey(name) == false)
            throw new KeyNotFoundException();

        m_CurrentStateName = name;

        m_CurrentState?.OnStateExit?.Invoke();

        m_CurrentState = m_States[name];
        m_CurrentState.OnStateEnter?.Invoke();
    }

    public bool Exists(TName name)
    {
        return m_States.ContainsKey(name);
    }
}
