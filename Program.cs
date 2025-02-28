using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Create a new thread to run the PrintNumbers method
        Thread thread1 = new Thread(PrintNumbers);
        Thread thread2 = new Thread(PrintLetters);

        // Start both threads
        thread1.Start();
        thread2.Start();

        // Wait for both threads to finish
        thread1.Join();
        thread2.Join();

        Console.WriteLine("Both threads have finished execution.");
    }

    static void PrintNumbers()
    {
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"Number: {i}");
            Thread.Sleep(1000); // Simulate some work with a 1-second delay
        }
    }

    static void PrintLetters()
    {
        for (char c = 'A'; c <= 'E'; c++)
        {
            Console.WriteLine($"Letter: {c}");
            Thread.Sleep(1000); // Simulate some work with a 1-second delay
        }
    }
}