using System;
using System.Threading;

namespace Lab1.TaskDemos
{
    public class Task1
    {
        private static Thread CreateTestThread(Mutex mutex, string name)
        {
            return new Thread(() =>
            {
                Thread.CurrentThread.Name = name;
                mutex.Lock();
                Console.WriteLine($"Lock - {Thread.CurrentThread.Name}");
                mutex.Notify();
                Console.WriteLine($"Notify - {Thread.CurrentThread.Name}");
                mutex.Wait();
                Console.WriteLine($"Wait - {Thread.CurrentThread.Name}");
                mutex.Release();
            });
        } 

        public static void RunDemo()
        {
            var mutex = new Mutex();
            var thread1 = CreateTestThread(mutex, "Thread 1");
            var thread2 = CreateTestThread(mutex, "Thread 2");
            var thread3 = CreateTestThread(mutex, "Thread 3");

            thread1.Start();
            thread2.Start();
            thread3.Start();
        }
    }
}