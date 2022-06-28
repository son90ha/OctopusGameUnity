public class WrongGrabState : CharacterState
{
    public override ECharacterState getEnumState()
    {
        return ECharacterState.WRONG_GRAB;
    }

    public override void Begin()
    {
        _context.octopusEmoteAnimator.SetFloat("RandomNum", UnityEngine.Random.Range(0f, 1f));
        base.Begin();
    }
}