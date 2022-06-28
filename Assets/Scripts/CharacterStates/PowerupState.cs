public class PowerupState : CharacterState
{
    public override ECharacterState getEnumState()
    {
        return ECharacterState.POWER_UP;
    }

    public override void Begin()
    {
        _context.octopusEmoteAnimator.SetFloat("RandomNum", UnityEngine.Random.Range(0f, 1f));
        base.Begin();
    }
}