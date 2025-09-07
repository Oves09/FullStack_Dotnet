using System;

abstract class Bird
{
    public abstract void Move();
}

class Sparrow : Bird
{
    public override void Move() => Console.WriteLine("Flying");
}

class Ostrich : Bird
{
    public override void Move() => Console.WriteLine("Running");
}

class Program
{
    static void Main()
    {
        Bird bird = new Ostrich();
        bird.Move(); // Valid for both Sparrow & Ostrich
    }
}
