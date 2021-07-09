using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntetieStateMachine : MonoBehaviour
{
    protected State State;
    public State GetState() => State;

    public void SetState(State state)
    {
        State = state;
        StartCoroutine(routine: State.Start());
    }
}
