public class JumpCommand : ICommand
{
    private readonly GameCharacter _character;

    public JumpCommand(GameCharacter character)
    {
        _character = character;
    }

    public void Execute()
    {
        _character.Jump();
    }
}
