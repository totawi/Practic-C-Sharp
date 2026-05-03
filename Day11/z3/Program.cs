class Program
{
    static void Main(string[] args)
    {
        GameCharacter warrior = new GameCharacter("Воин");

        GameController controller = new GameController();

        controller.SetCommand(new JumpCommand(warrior));
        controller.PressButton();

        controller.SetCommand(new AttackCommand(warrior));
        controller.PressButton();

        controller.SetCommand(new DefendCommand(warrior));
        controller.PressButton();

        Console.ReadLine();
    }
}