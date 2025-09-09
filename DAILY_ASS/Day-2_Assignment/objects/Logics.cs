// write all logics for prime , odd num, even num
using System;

class Logics{

    public void getType(){
        Console.WriteLine("number logics");
    }

    public void Prime(){
       Console.WriteLine("Enter a number to check prime or not: ");
        int num = Convert.ToInt32(Console.ReadLine());

        if (num <= 1)
        {
            Console.WriteLine("Not prime");
            return;
        }

        bool isPrime = true;

        for (int i = 2; i <= Math.Sqrt(num); i++)
        {
            if (num % i == 0)
            {
                isPrime = false;
                break;
            }
        }

        Console.WriteLine(isPrime ? "Prime" : "Not prime");
    }

    public void Even(){
        Console.WriteLine("Enter a number to check even or not: ");
        int num = Convert.ToInt32(Console.ReadLine());

        if(num % 2 == 0){
            Console.WriteLine("even number");
        }
       
    }

    public void Odd(){
        Console.WriteLine("Enter a number to check odd ro not: ");
        int num = Convert.ToInt32(Console.ReadLine());

        if(num % 2 != 0){
            Console.WriteLine("odd number");
        }
    }
}