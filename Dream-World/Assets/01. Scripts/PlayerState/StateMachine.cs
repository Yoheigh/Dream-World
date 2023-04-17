using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class StateMachine<T> where T : class
{
    private T entity;
    private State<T> currentState;

    public virtual void Setup(T _entity, State<T> _firstState)
    {
        entity = _entity;
        currentState = null;

        ChangeState(_firstState);

    }

    public void ChangeState(State<T> _newState)
    {
        if (_newState == null) return;

        if (currentState != null)
            currentState.ExitState(entity);

        currentState = _newState;
        currentState.EnterState(entity);
    }

    public void Execute()
    {
        if(currentState != null)
            currentState.UpdateState(entity);
    }
    
}

public abstract class State<T> where T : class
{
    public abstract void EnterState(T _entity);
    public abstract void UpdateState(T _entity);
    public abstract void ExitState(T _entity);
}
