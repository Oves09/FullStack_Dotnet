using System;

interface IWorker
{
    void Work();
    void Eat();
}

class Human : IWorker
{
    public void Work() => Console.WriteLine("Human working");
    public void Eat() => Console.WriteLine("Human eating");
}

class Robot : IWorker
{
    public void Work() => Console.WriteLine("Robot working");
    public void Eat() => throw new NotImplementedException();
}

class Program
{
    static void Main()
    {
        IWorker robot = new Robot();
        robot.Work();
        // robot.Eat(); // causes error
    }
}
