using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//Используя AsyncAwait, создайте метод, который будет в цикле for (допустим, на 10
//итераций) увеличивать счетчик на единицу и выводить на экран счетчик и текущий поток.
//Метод запускается в трех потоках. Каждый поток должен выполниться поочередно, т.е.в
//результате на экран должны выводиться числа (значения счетчика) с 1 до 30 по порядку, а не в
//произвольном порядке.
namespace Task2
{
    class Program
    {
        static int counter=0;
        static private object obj = new object();
        static private async Task Method()
        {
            await Task.Run(() =>
            {
                lock (obj)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("{0} в потоке {1}", ++counter,Thread.CurrentThread.ManagedThreadId.ToString());
                    }
                }
            });
        }
        static void Main(string[] args)
        {
            Task task1 = Method();
            Task task2 = Method();
            Task task3 = Method();
            Task.WaitAll(task1,task2,task3);
            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
