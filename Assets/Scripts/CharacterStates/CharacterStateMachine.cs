using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class CharacterStateMachine : FSM.StateMachine<CharacterController>
{
    public CharacterStateMachine(CharacterController context, State<CharacterController> initialState) : base(context, initialState)
    {
    }
}
