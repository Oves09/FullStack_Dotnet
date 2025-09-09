// using System;

// class StringPrograms{

//     static void Main(){

//         string text = "csharp language invented in 2002";
        

//         int len = text.Length;

//         Console.WriteLine("length of string : " + len);

//         string sub = text.Substring(1, 5);

//         Console.WriteLine("sub string of string : " + sub);

//         Console.WriteLine(text.IndexOf("harp"));
//         Console.WriteLine(text.ToUpper());

//         string newString = text.Replace("csharp", "Java");
//         Console.WriteLine(newString);

//         String noPlace =  text.Replace(" ", "");
//         Console.WriteLine("Without space: " + noPlace.Length);

//         int pos = text.IndexOf("language");
//         // Console.WriteLine(pos);
//         string newText = text.Substring(pos , 8);

//         Console.WriteLine("New Text value: " + newText);

//         string data = "Csharp,Language";
//         string[] lang = data.Split(',');

//         foreach(string values in lang){
//             Console.WriteLine(values);
//         }

//         // count al the blank spaces
//         // count al the special characters
//         int cnt = 0;
//         for(int i = 0; i < text.Length; i++){

//             if(text[i] == ' '){
//                 cnt++;
//             }
//         }

//         Console.WriteLine( "white spaces : " + cnt);

//         string special = "csharp$language@invented_in#2002";

//         int spl = 0;

//         foreach(char c in special){
//              if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))              
//             spl++;
//         }

//         Console.WriteLine("special chars: " +  spl);

//         // word count
//         string words = "i love c sharp language";
//         string[] total = words.Split(" ");
//         Console.WriteLine("Total words is : " + total.Length);
//         // vowels

//         int vowels = 0;
//         string str = "learning strings today in c#";

//         foreach(char ch in str){

//             if( ch == 'a' || ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u' || ch == 'A' || ch == 'E' || ch == 'I' || ch == 'O' || ch == 'U'){
//                 vowels++;
//             }
//         }

//         Console.WriteLine("vowels count is : " + vowels);

//         string data1 = "This is the day four training class";
//         string[] lang1 = data1.Split(' ');
//         Console.WriteLine("The blank spaces in the above statement : " + (lang1.Length - 1));

//     }
// }