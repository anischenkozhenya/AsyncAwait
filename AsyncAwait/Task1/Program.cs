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
        static async Task OperationMethodAsync(object obj)
        {
            await Task.Factory.StartNew(()=>{ object[] strArr = obj as object[];
                string str=((StreamReader)strArr[0]).ReadToEnd();
                mutex.WaitOne();
                {
                    ((StreamWriter)strArr[1]).WriteAsync(str);
                }
                mutex.ReleaseMutex();
            });
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
            Task task1 = OperationMethodAsync(vs1);
            Task task2 = OperationMethodAsync(vs2);
            Task.WaitAll(task1, task2);
            stream1.Close();
            stream2.Close();
            stream3.Close();
            Console.WriteLine("Для выхода нажмите любую кнопку...");
            Console.ReadKey();
        }            
    }
}
