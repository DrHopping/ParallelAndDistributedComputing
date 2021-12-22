using Lab1.Algorithms;

namespace Lab1.TaskDemos;

public class Task4
{
    public static void RunDemo()
    {
        var numbersToAdd = new List<int> {0, 8, 2, 1, 3, 5, 4, 6, 7, 9};
        var numbersToRemove = new List<int> {0, 2, 1, 4, 6, 9};
        var harrisList = new HarrisLinkedList<int>();
        Parallel.ForEach(numbersToAdd, x =>
        {
            harrisList.Add(x);
        });

        Console.WriteLine(harrisList);
        
        Parallel.ForEach(numbersToRemove, x =>
        {
            harrisList.Remove(x);
        });
        
        Console.WriteLine(harrisList);
    }
}