using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Lab1.Atomics;

namespace Lab1.Algorithms
{
    public class SkipList<T>: ICollection<T> where T: IComparable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public AtomicReference<Node> RightNode { get; set; }
            public Node LowerNode { get; set; }

            public Node(T data)
            {
                Data = data;
                RightNode = new AtomicReference<Node>(null);
                LowerNode = null;
            }

            public Node(T data, AtomicReference<Node> rightNode, Node lowerNode) {
                Data = data;
                RightNode = rightNode;
                LowerNode = lowerNode;
            }
        }
        
        private int _maxHeight;
        private Node _headTop;
        private const int DefaultMaxHeight = 32;
                
        public int Count { get; private set; } = 0;
        public bool IsReadOnly => false;
        
        public SkipList() {
            var currNode = new Node(default);

            _maxHeight = DefaultMaxHeight;
            _headTop = currNode;

            for (var i = 1; i < _maxHeight; i++) {
                var nextNode = new Node(default);
                currNode.LowerNode = nextNode;
                currNode = nextNode;
            }
        }
        
        public void Add(T item) {
            var previousNode = new List<Node>();
            var previousRightNode = new List<Node>();
            var currentNode = _headTop;
            var height = GetNodeHeight();
            var currentLevel = _maxHeight;

            while (currentLevel > 0)
            {
                var rightNode = currentNode.RightNode.Value;

                if (currentLevel <= height)
                {
                    if (rightNode == null || rightNode.Data.CompareTo(item) >= 0)
                    {
                        previousNode.Add(currentNode);
                        previousRightNode.Add(rightNode);
                    }
                }

                if (rightNode != null && rightNode.Data.CompareTo(item) < 0)
                {
                    currentNode = rightNode;
                }
                else
                {
                    currentNode = currentNode.LowerNode;
                    currentLevel--;
                }
            }

            Node lowerNode = null;
            for (var i = previousNode.Count - 1; i >= 0; i--)
            {
                var newNode = new Node(item, new AtomicReference<Node>(previousRightNode[i]), null);
                if (lowerNode != null) newNode.LowerNode = lowerNode;
                if (!previousNode[i].RightNode.CompareAndSet(newNode, previousRightNode[i])) return;
                lowerNode = newNode;
            }

            Count++;
        }
        public bool Remove(T item) {
            var currentNode = _headTop;
            var currentLevel = _maxHeight;
            var towerUnmarked = true;

            while (currentLevel > 0)
            {
                var rightNode = currentNode.RightNode.Value;

                if (rightNode != null && rightNode.Data.CompareTo(item) == 0)
                {
                    var nextRightNode = rightNode.RightNode.Value;
                    if (towerUnmarked)
                    {
                        var towerNode = rightNode;
                        while (towerNode != null)
                        {
                            towerNode.RightNode.CompareAndSet(null, towerNode.RightNode.Value);
                            towerNode = towerNode.LowerNode;
                        }

                        towerUnmarked = false;
                    }

                    currentNode.RightNode.CompareAndSet(nextRightNode, rightNode);
                }

                if (rightNode != null && rightNode.Data.CompareTo(item) < 0)
                {
                    currentNode = rightNode;
                }
                else
                {
                    currentNode = currentNode.LowerNode;
                    currentLevel--;
                }
            }

            if (!towerUnmarked) Count--;
            return !towerUnmarked;
        }
        
        public bool Contains(T item) {
            var currentNode = _headTop;
            while (currentNode != null) {
                var rightNode = currentNode.RightNode.Value;
                if (currentNode.Data != null && currentNode.Data.CompareTo(item) == 0) return true;
                if (rightNode != null && rightNode.Data.CompareTo(item) <= 0) currentNode = rightNode;
                else currentNode = currentNode.LowerNode;
            }
            return false;
        }
        
        public void Clear()
        {
            var currNode = new Node(default);

            Count = 0;
            _maxHeight = DefaultMaxHeight;
            _headTop = currNode;

            for (var i = 1; i < _maxHeight; i++) {
                var nextNode = new Node(default);
                currNode.LowerNode = nextNode;
                currNode = nextNode;
            }
        }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public override string ToString() => string.Join(",", Items);

        private IEnumerable<T> Items
        {
            get
            {
                var currNode = GetHeadBottomNode().RightNode.Value;
                while (currNode != null) {
                    yield return currNode.Data;
                    currNode = currNode.RightNode.Value;
                }
            }
        }
        
        private Node GetHeadBottomNode()
        {
            return GetHeadNodeByIndex(0);
        }

        private Node GetHeadNodeByIndex(int index)
        {
            var currNode = _headTop;
            
            for (var i = _maxHeight - 1; i > index; i--) 
            {
                currNode = currNode.LowerNode;
            }

            return currNode;
        }
        
        private int GetNodeHeight() {
            var level = 1;
            while (level < _maxHeight && RandomNumberGenerator.GetInt32(0,2) == 0) 
            {
                level++;
            }
            return level;
        }
        
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
    }
}