using System;

class Exception{

    static void Main(){

        try{
            int a = 10;
            int b = 0;

            int res = a / b;
            Console.WriteLine("result: " + res);
        }
        catch(DivideByZeroException e){
            Console.WriteLine("not divisible by zero");
        }
    }
}