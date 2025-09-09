using System;

class JaggedArray{

    public static void Main(){
       
       string[] students = new string[3];
       string[][] studentSub = new string[3][];
       
        Console.WriteLine("Enter names of 3 students: ");
       for(int i = 0; i < 3; i++){
            Console.WriteLine("Enter name of student " + (i+1));
            students[i] = Console.ReadLine();

            Console.WriteLine("How many subjects you want to store " );
            int subCount = Convert.ToInt32(Console.ReadLine());

            studentSub[i] = new string[subCount];

            for(int j = 0; j < subCount; j++){
                Console.WriteLine("Enter subjects: ");
                studentSub[i][j] = Console.ReadLine();
                Console.WriteLine(studentSub[i][j]);
            }
       }

       Console.WriteLine("Student data name and subject wise ");

       for(int i = 0; i < 3; i++){

            Console.WriteLine("The " + (i+1) + " student name is " + students[i]);

            for(int j = 0; j < studentSub[i].Length; j++){

                Console.WriteLine(studentSub[i][j]);
            }
       }
    }
}