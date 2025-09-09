// See https://aka.ms/new-console-template for more information


// import logics and write switch cases in this file

using System;

class Program{

    public static void Main(string[] args){

        Console.WriteLine("Enter a number: ");
        int number = Convert.ToInt32(Console.ReadLine());

        Logics l = new Logics();
        l.getType();

        switch(number){
            case 1:
                l.Prime();
                break;

            case 2:
                l.Even();
                break;

            case 3:
                l.Odd();
                break;

            default:
                Console.WriteLine("You entered a wrong logic number...");
                break;
        }
    }
}