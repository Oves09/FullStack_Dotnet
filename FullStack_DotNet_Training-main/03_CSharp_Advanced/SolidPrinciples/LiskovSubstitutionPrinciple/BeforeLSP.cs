// using System;

// class Bird
// {
//     public virtual void Fly()
//     {
//         Console.WriteLine("Bird is flying");
//     }
// }

// class Ostrich : Bird
// {
//     public override void Fly()
//     {
//         throw new Exception("Ostriches can't fly");
//     }
// }

// class Program
// {
//     static void Main()
//     {
//         Bird bird = new Ostrich();
//         bird.Fly(); // Runtime error
//     }
// }
