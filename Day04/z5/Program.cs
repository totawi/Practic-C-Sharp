using System;

abstract class ElectronicDevice
{
    public abstract void TurnOn();

    // Виртуальный метод (можно переопределить, а можно оставить так)
    public virtual void TurnOff()
    {
        Console.WriteLine("Device is off");
    }
}

// Телевизор
class TV : ElectronicDevice
{
    public override void TurnOn()
    {
        Console.WriteLine("TV is turning on");
    }

    public override void TurnOff()
    {
        Console.WriteLine("TV is turning off");
    }
}

// Радио
class Radio : ElectronicDevice
{
    public override void TurnOn()
    {
        Console.WriteLine("Radio is turning on");
    }

    public override void TurnOff()
    {
        Console.WriteLine("Radio is turning off");
    }
}

class Program
{
    static void Main()
    {
        TV myTv = new TV();
        myTv.TurnOn();
        myTv.TurnOff();

        Radio myRadio = new Radio();
        myRadio.TurnOn();
        myRadio.TurnOff();
    }
}