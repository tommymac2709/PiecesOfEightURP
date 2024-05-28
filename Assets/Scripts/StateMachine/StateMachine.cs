using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    /*
        Stores the current state that it is in
        Has a way to switch between states
        Player and Enemy State Machines will inherit form this
        Making this abstract stops us from adding this particular class to a game object
        We will instead attach classess that inherit from this

    */

    //Hold the current state
    private State currentState;

    private void Update()
    {
        if (currentState != null)
        {
            //? is the same as if current state != null (i.e. checking if there is a current state)
            //If it's not null, do the thing after the dot
            currentState?.Tick(Time.deltaTime);
        }
        
    }

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
