using System;

class Tution{

    public delegate int Calculate(int x , int y);
    public delegate void Print(int res);

    public static int Sum(int tution, int mess){
        return tution + mess;
    }

    public static void Show(int ans){
        Console.WriteLine("total fees is: " + ans);
    }

    public static void Main(){

        Calculate cal = Sum;
        Print print = Show;

        int total = cal(5000, 2000);
        print(total);
    }
}