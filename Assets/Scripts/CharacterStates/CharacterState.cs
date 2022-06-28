using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : FSM.State<CharacterController>
{
    public virtual ECharacterState getEnumState()
    {
        return ECharacterState.NORMAL;
    }
    public override void Begin()
    {
        Debug.Log("Begin: " + getEnumState().ToString());
        _context.octopusEmoteAnimator.SetInteger("CharacterState", ((int)getEnumState()));
    }
}
