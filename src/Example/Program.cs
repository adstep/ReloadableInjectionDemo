using System;
using System.Diagnostics;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = Process.GetCurrentProcess();

            Console.WriteLine($"My process id is {process.Id}.");
            Console.WriteLine("Waiting to get injected!");

            Console.ReadKey();
        }
    }
}
