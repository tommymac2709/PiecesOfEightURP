using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    //reference to the player
    //protected means only classes that inherit this class can use it (similar to public, but only for inherited classes)
    protected PlayerStateMachine stateMachine;

    //constructor is called whenever you create a new isntance of the class
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }


}
