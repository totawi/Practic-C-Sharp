public class GameCharacter
{
    public string Name { get; private set; }

    public GameCharacter(string name)
    {
        Name = name;
    }

    public void Jump()
    {
        Console.WriteLine($"{Name} подпрыгнул!");
    }

    public void Attack()
    {
        Console.WriteLine($"{Name} наносит сокрушительный удар!");
    }

    public void Defend()
    {
        Console.WriteLine($"{Name} встал в защитную стойку.");
    }
}
