using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Lab1.Algorithms
{
    public class SkipList<T>: ICollection<T> where T: IComparable<T>
    {
        private class Node
        {
            public T Value { get; set; }
            public Node RightNode { get; set; }
            public Node LowerNode { get; set; }

            public Node(T value)
            {
                Value = value;
                RightNode = null;
                LowerNode = null;
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
        
        public void Add(T elem) {
            var height = GetNodeHeight();

            var currNode = GetHeadNodeByIndex(height - 1);
            Node prevAddedNode = null;
            while (currNode != null)
            {
                var rightNode = currNode.RightNode;
                if (rightNode != null && rightNode.Value.CompareTo(elem) == -1)
                {
                    currNode = rightNode;
                }
                else
                {
                    var node = new Node(elem) {RightNode = rightNode};
                    currNode.RightNode = node;
                    currNode = currNode.LowerNode;
                    if (prevAddedNode != null)
                    {
                        prevAddedNode.LowerNode = node;
                    }

                    prevAddedNode = node;
                }
            }

            Count++;
        }
        public bool Remove(T elem) {
            var currNode = GetHeadBottomNode();

            while (currNode != null) {
                var rightNode = currNode.RightNode;

                if (rightNode != null && rightNode.Value.CompareTo(elem) == -1)
                {
                    currNode = rightNode;
                }
                else if (rightNode != null && rightNode.Value.CompareTo(elem) == 0)
                {
                    currNode.RightNode = rightNode.RightNode;
                    currNode = currNode.LowerNode;
                }
                else
                {
                    return false;
                }
            }

            Count--;
            return true;
        }
        
        public bool Contains(T elem) {
            var currNode = GetHeadBottomNode().RightNode;

            while (currNode != null && !currNode.Value.Equals(elem)) {
                var rightNode = currNode.RightNode;

                if (rightNode != null && rightNode.Value.CompareTo(elem) != 1)
                {
                    currNode = rightNode;
                }
                else
                {
                    currNode = currNode.LowerNode;
                }
            }

            return currNode != null;
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
                var currNode = GetHeadBottomNode().RightNode;
                while (currNode != null) {
                    yield return currNode.Value;
                    currNode = currNode.RightNode;
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
            
            for (var i = _maxHeight - 1; i > index; i--) {
                currNode = currNode.LowerNode;
            }

            return currNode;
        }
        
        private int GetNodeHeight() {
            var level = 1;
            while (level < _maxHeight && RandomNumberGenerator.GetInt32(0,2) == 0) {
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