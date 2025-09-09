// using System.IO;

// class Program{

//     static void Main(){

//         string path = "file1.txt";
//         string msg = " File data related to c#";

//         using (FileSystem fs = new FileSystem(path, FileMode.Create, FileAccess.Write))
//         {
//             byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
//             fs.Write(data, 0, data.Length);
//         };
//     }
// }