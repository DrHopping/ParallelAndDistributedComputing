using System;
using System.Collections.Generic;
using System.Text;
using Lab1.Atomics;

namespace Lab1.Algorithms
{
    public class MichaelAndScottQueue<T>
    {
        private class Node {
            public T Value { get; set; }

            private readonly AtomicReference<Node> _next;
            public Node Next => _next.Value;

            public Node(T value) {
                Value = value;
                _next = new AtomicReference<Node>(null);
            }

            public bool CompareAndSet(Node next, Node expected) {
                return _next.CompareAndSet(next, expected);
            }
        }
        
        private volatile Node _head;
        private volatile Node _tail;
        
        public MichaelAndScottQueue() {
            var start = new Node(default);
            _head = start;
            _tail = start;
        }

        public void Enqueue(T elem)
        {
            var node = new Node(elem);

            while (true)
            {
                if (_tail.CompareAndSet(node, null))
                {
                    _tail = node;
                    break;
                }
                var nextNode = _tail.Next;
                if (nextNode != null) _tail = nextNode;
            }
        }

        public T Dequeue()
        {
            while (true)
            {
                var node = _head.Next;
                if (node == null) return default;
                if (_head == _tail) _tail = node;
                else if (_head.CompareAndSet(node.Next, node)) 
                    return node.Value;
            }
        }

        public bool IsEmpty() => _head.Next == null;

        public override string ToString()
        {
            var currNode = _head.Next;
            var result = new List<string>();

            while (currNode != null) {
                result.Add(currNode.Value.ToString());
                currNode = currNode.Next;
            }

            return string.Join(",", result);
        }
    }
}