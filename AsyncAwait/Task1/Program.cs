using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
//Открыть параллельно два файла и записывать из них информацию
//в третий файл c использованием конструкции async wait.

namespace Task1
{
    class Program
    {
              
        static Mutex mutex = new Mutex();
        
        static async void ReadMethod(object obj)
        {
            object[] arr = obj as object[];
            string str = ((StreamReader)arr[0]).ReadToEnd();
            mutex.WaitOne();
            {
                await ((StreamWriter)arr[1]).WriteAsync(str);
            }
            mutex.ReleaseMutex();
        }

        static void Main(string[] args)
        {
            string path = @"..\..\";
            StreamReader stream1 = File.OpenText(path + "TextFile1.txt");
            StreamReader stream2 = File.OpenText(path + "TextFile2.txt");
            StreamWriter stream3 = File.CreateText(path + "TextFile3.txt");
            object[] vs1 = {stream1, stream3};
            object[] vs2 = {stream2, stream3};
            Process.Start("explorer.exe", path);
            Task task1 = new Task(new Action<object>(ReadMethod),vs1);
            Task task2 = new Task(new Action<object>(ReadMethod),vs2);
            task1.Start();
            task2.Start();
            Task.WaitAll(task1, task2);
            stream1.Close();
            stream2.Close();
            stream3.Close();
            Console.WriteLine("Для выхода нажмите любую кнопку...");
            Console.ReadKey();
        }            
    }
}
