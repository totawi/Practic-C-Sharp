using System;
using System.Collections.Generic;

// Ассоциация: Разработчик существует сам по себе
class Developer { public string Name = "Epic Games"; }

// Агрегация: Игрок может менять игры (существует вне игры)
class Player { public string Nickname; public Player(string n) => Nickname = n; }

// Композиция: Мир создается внутри игры и удаляется вместе с ней
class GameWorld { public string Name = "Открытый мир"; }

class VideoGame
{
    public Developer Dev; // Ассоциация
    public Player[] Players; // Агрегация (массив)
    public GameWorld World; // Композиция

    public VideoGame(Player[] p, Developer d)
    {
        Dev = d;
        Players = p;
        World = new GameWorld(); // Создается внутри
    }

    public void StartGame()
    {
        Console.WriteLine($"Игра от {Dev.Name} запущена в мире {World.Name}");
        foreach (var p in Players) Console.WriteLine("Игрок в сети: " + p.Nickname);
    }
}

class Program
{
    static void Main()
    {
        Developer dev = new Developer();
        Player[] players = { new Player("Rex"), new Player("Alex") };

        VideoGame myGame = new VideoGame(players, dev);
        myGame.StartGame();
    }
}