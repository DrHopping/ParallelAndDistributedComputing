using System;
using Lab1.Algorithms;

namespace Lab1.TaskDemos
{
    public class Task3
    {
        public static void RunDemo()
        {
            var list = new MichaelAndScottQueue<int>();
            list.Enqueue(3);
            list.Enqueue(6);
            list.Enqueue(1);
            list.Enqueue(2);
            list.Enqueue(5);

            Console.WriteLine(list.ToString());
            while (!list.IsEmpty())
            {
                Console.WriteLine(list.Dequeue());
            }
        }
    }
}