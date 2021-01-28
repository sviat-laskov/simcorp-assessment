using System;
using System.Collections;
using System.Collections.Generic;

namespace SimCorp.LinkedLists
{
    internal struct LinkedListEnumerator<T> : IEnumerator<T>
    {
        private readonly Func<int> _listVersionFactory;
        private readonly Nodes.LinkedListNode<T>? _firstNode;
        private Nodes.LinkedListNode<T>? _currentNode;
        private readonly int _initialListVersion;

        internal LinkedListEnumerator(Func<int> listVersionFactory, Nodes.LinkedListNode<T>? firstNode)
        {
            _listVersionFactory = listVersionFactory;
            _firstNode = firstNode;
            _currentNode = null;
            _initialListVersion = listVersionFactory();
        }

        public T Current
        {
            get
            {
                if (_currentNode == null) throw new InvalidOperationException();

                return _currentNode.Value!;
            }
        }

        public bool MoveNext()
        {
            ThrowIfListIsUpdated();

            if (_currentNode == null)
            {
                _currentNode = _firstNode;
                return true;
            }

            if (_currentNode.Next == _firstNode) return false;

            _currentNode = _currentNode.Next;
            return true;
        }

        public void Dispose() { }

        void IEnumerator.Reset()
        {
            ThrowIfListIsUpdated();

            _currentNode = null;
        }

        object IEnumerator.Current => Current!;

        private void ThrowIfListIsUpdated()
        {
            if (_listVersionFactory() > _initialListVersion) throw new InvalidOperationException();
        }
    }
}