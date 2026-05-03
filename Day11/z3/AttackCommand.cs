public class AttackCommand : ICommand
{
    private readonly GameCharacter _character;

    public AttackCommand(GameCharacter character)
    {
        _character = character;
    }

    public void Execute()
    {
        _character.Attack();
    }
}
