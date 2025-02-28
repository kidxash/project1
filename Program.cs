using System;
using System.Collections.Generic;
using System.Threading;

class SharedQueue
{
    private readonly Queue<int> queue = new Queue<int>();
    private readonly int capacity = 5;
    private readonly object lockObject = new object();

    // Producer adds data to the queue
    public void Produce(int value)
    {
        lock (lockObject)
        {
            while (queue.Count == capacity)
            {
                Console.WriteLine("Product bins are full");
                Monitor.Wait(lockObject); // Wait if the queue is full
            }

            queue.Enqueue(value);
            Console.WriteLine($"Produced: {value}");
            Monitor.Pulse(lockObject); // Notify the consumer that data is available
        }
    }

    // Consumer removes data from the queue
    public void Consume()
    {
        lock (lockObject)
        {
            while (queue.Count == 0)
            {
                Console.WriteLine("There are no more products to sell please wait");
                Monitor.Wait(lockObject); // Wait if the queue is empty
            }

            int value = queue.Dequeue();
            Console.WriteLine($"Purchased: {value}");
            Monitor.Pulse(lockObject); // Notify the producer that space is available
        }
    }
}

class Producer
{
    private readonly SharedQueue _sharedQueue;

    public Producer(SharedQueue sharedQueue)
    {
        _sharedQueue = sharedQueue;
    }

    public void Run()
    {
        int value = 0;
        while (true)
        {
            _sharedQueue.Produce(value++);
            Thread.Sleep(1000); // Simulate some work
        }
    }
}

class Consumer
{
    private readonly SharedQueue _sharedQueue;

    public Consumer(SharedQueue sharedQueue)
    {
        _sharedQueue = sharedQueue;
    }

    public void Run()
    {
        while (true)
        {
            _sharedQueue.Consume();
            Thread.Sleep(1500); // Simulate some work
        }
    }
}

class Program
{
    static void Main()
    {
        SharedQueue sharedQueue = new SharedQueue();

        Thread producerThread = new Thread(() => new Producer(sharedQueue).Run());
        Thread consumerThread = new Thread(() => new Consumer(sharedQueue).Run());

        producerThread.Start();
        consumerThread.Start();

        producerThread.Join();
        consumerThread.Join();
    }
}
