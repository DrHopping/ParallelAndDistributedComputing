using System.Collections;
using Lab1.Atomics;

namespace Lab1.Algorithms;

public class HarrisLinkedList<T>: ICollection<T> where T: IComparable
{
    class Node
    {
        public T Data { get; set; }
        public AtomicReference<Node> Next { get; set; }

        public Node(T data, AtomicReference<Node> next)
        {
            Data = data;
            Next = next;
        }
    }
    
    private Node _head = new Node(default, new AtomicReference<Node>(null));

    public int Count { get; private set; } = 0;
    public bool IsReadOnly => false;
    
    public void Add(T item)
    {
        var newNode = new Node(item, new AtomicReference<Node>(null));
        var currentNode = _head;

        while (true)
        {
            var nextNode = currentNode.Next.Value;
            if (nextNode != null)
            {
                if (nextNode.Data.CompareTo(item) >= 0)
                {
                    newNode.Next = new AtomicReference<Node>(nextNode);
                    if (currentNode.Next.CompareAndSet(newNode, nextNode))
                    {
                        Count++;
                        return;
                    }
                }
                else
                {
                    currentNode = nextNode;
                }
            }
            else if(currentNode.Next.CompareAndSet(newNode, null)) return;
        }
    }

    public bool Remove(T item)
    {
        var previousNode = _head;
        while (previousNode.Next.Value != null)
        {
            var currentNode = previousNode.Next.Value;
            var nextNode = currentNode.Next.Value;

            if (currentNode.Data.CompareTo(item) == 0)
            {
                if (currentNode.Next.CompareAndSet(null, nextNode) && 
                    previousNode.Next.CompareAndSet(nextNode, currentNode))
                {
                    Count--;
                    return true;
                }
            }
            else
            {
                previousNode = currentNode;
            }
        }

        return false;
    }

    public void Clear()
    {
        _head = new Node(default, new AtomicReference<Node>(null));
        Count = 0;
    }

    public bool Contains(T item)
    {
        var currentNode = _head.Next.Value;
        while (currentNode != null)
        {
            if (currentNode.Data.CompareTo(item) == 0) return true;
            currentNode = currentNode.Next.Value;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }
    
    private IEnumerable<T> Items
    {
        get
        {
            var currentNode = _head.Next.Value;
            while (currentNode != null) {
                yield return currentNode.Data;
                currentNode = currentNode.Next.Value;
            }
        }
    }
    
    public override string ToString() => string.Join(",", Items);
    
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}