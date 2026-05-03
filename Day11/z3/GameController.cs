// Команда для защиты
public class GameController
{
    private ICommand _command;

    // Устанавливаем команду (например, при нажатии определенной кнопки)
    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    // Выполняем команду
    public void PressButton()
    {
        if (_command != null)
        {
            _command.Execute();
        }
    }
}
