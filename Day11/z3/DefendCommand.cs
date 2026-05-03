// Команда для защиты
public class DefendCommand : ICommand
{
    private readonly GameCharacter _character;

    public DefendCommand(GameCharacter character)
    {
        _character = character;
    }

    public void Execute()
    {
        _character.Defend();
    }
}
