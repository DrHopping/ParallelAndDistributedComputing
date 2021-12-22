using System;
using System.Threading;
using Lab1.Algorithms;

namespace Lab1.TaskDemos
{
    public class Task2
    {
        public static void RunDemo()
        {
            var list = new SkipList<int>();
            var numbersToAdd = new List<int> {0, 8, 2, 1, 3, 5, 4, 6, 7, 9};
            var numbersToRemove = new List<int> {0, 2, 1, 4, 6, 9};
            
            numbersToAdd.ForEach(x =>
            {
                list.Add(x);
            });
            Console.WriteLine(list);

            numbersToRemove.ForEach(x =>
            {
                list.Remove(x);
            });
            
            Console.WriteLine(list);

        }
    }
}