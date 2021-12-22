using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Lab1.Algorithms;
using Xunit;

namespace Lab1.Tests;

public class Task2
{
    [Fact]
    public void ShouldAddAndRemoveElementsCorrectly()
    {
        var numbersToAdd = Enumerable.Range(0, 40).Select(_ => RandomNumberGenerator.GetInt32(0, 100)).Distinct().ToList();
        var numbersToRemove = Enumerable.Range(0, 20).Select(_ => RandomNumberGenerator.GetInt32(0, 100)).Distinct().ToList();
        var expected = numbersToAdd.Except(numbersToRemove).OrderBy(x => x).ToList();

        var skipList = new SkipList<int>();
        Parallel.ForEach(numbersToAdd, x => skipList.Add(x));
        Parallel.ForEach(numbersToRemove, x => skipList.Remove(x));
        
        skipList.Select(x => x).Should().Equal(expected);
    }
}