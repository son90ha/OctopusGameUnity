public class CusFailedState : CharacterState
{
    public override ECharacterState getEnumState()
    {
        return ECharacterState.CUS_FAILED;
    }

    public override void Begin()
    {
        _context.octopusEmoteAnimator.SetFloat("RandomNum", UnityEngine.Random.Range(0f, 1f));
        base.Begin();
    }
}
