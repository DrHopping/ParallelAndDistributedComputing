using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Lab1.Algorithms;
using Xunit;

namespace Lab1.Tests;

public class Task3
{
    [Fact]
    public void ShouldAddAndRemoveElementsCorrectly()
    {
        var numbersToEnqueue = Enumerable.Range(0, 50).Select(_ => RandomNumberGenerator.GetInt32(0, 100)).Distinct().ToList();

        var queue = new MichaelAndScottQueue<int>();
        Parallel.ForEach(numbersToEnqueue, x => queue.Enqueue(x));

        var dequeued = new ConcurrentBag<int>();
        Parallel.ForEach(numbersToEnqueue, _ => dequeued.Add(queue.Dequeue()));
        
        dequeued.OrderBy(x => x).Should().Equal(numbersToEnqueue.OrderBy(x => x));
    }
}