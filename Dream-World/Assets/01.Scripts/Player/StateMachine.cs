using System.Collections;
using System.Collections.Generic;

public class StateMachine<T> where T : class
{
    private T entity;
    private State<T> currentState;
    private State<T> globalState;
    private State<T> previousState;

    public virtual void Setup(T _entity, State<T> _firstState)
    {
        entity = _entity;
        currentState = null;
        globalState = null;
        previousState = null;

        ChangeState(_firstState);

    }

    public void ChangeState(State<T> _newState)
    {
        if (_newState == null) return;

        if (currentState != null)
            currentState.ExitState(entity);

        previousState = currentState;
        currentState = _newState;
        currentState.EnterState(entity);
    }

    public void Execute()
    {
        // if (globalState != null) 을 축약
        globalState?.UpdateState(entity);

        // if (currentState != null) 을 축약
        currentState?.UpdateState(entity);
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }
}

public abstract class State<T> where T : class
{
    public abstract void EnterState(T _entity);
    public abstract void UpdateState(T _entity);
    public abstract void ExitState(T _entity);
}
