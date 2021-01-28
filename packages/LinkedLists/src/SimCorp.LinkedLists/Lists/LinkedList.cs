using System.Collections;
using System.Collections.Generic;
using SimCorp.LinkedLists.Lists.Interfaces;
using SimCorp.LinkedLists.Nodes;

namespace SimCorp.LinkedLists.Lists
{
    /// <summary>
    ///     Base linked lists' functionality.
    /// </summary>
    /// <typeparam name="T">Any value or reference type.</typeparam>
    /// <typeparam name="TNode"><see cref="SinglyLinkedListNode{T}" /> or <see cref="DoublyLinkedListNode{T}" />.</typeparam>
    public abstract class LinkedList<T, TNode> : ILinkedList<T, TNode>
        where TNode : Nodes.LinkedListNode<T>
    {
        private static readonly IEqualityComparer<T> ValuesDefaultEqualityComparer = EqualityComparer<T>.Default;
        private readonly IEqualityComparer<T> _valuesEqualityComparer;
        private int _version;

        /// <summary>
        ///     First node of the list.
        /// </summary>
        protected TNode? FirstNode { get; set; }

        /// <summary>
        ///     Returns true when list does not contain any values.
        /// </summary>
        public bool IsEmpty => Count == default;

        /// <param name="comparer">Custom comparer, used to check values' equality.</param>
        protected LinkedList(IEqualityComparer<T>? comparer = null) => _valuesEqualityComparer = comparer ?? ValuesDefaultEqualityComparer;

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     <inheritdoc />
        ///     Uses enumerator to create result array.
        /// </summary>
        public T[] Values
        {
            get
            {
                int nodeIndex = default;
                var values = new T[Count];

                foreach (T value in this) values[nodeIndex++] = value;

                return values;
            }
        }

        /// <summary>
        ///     Creates enumerator for current version of list.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => new LinkedListEnumerator<T>(() => _version, FirstNode);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public TNode? GetNodeOrDefaultByValue(T value)
        {
            if (IsEmpty) return null;

            TNode? node = FirstNode!;
            do
            {
                if (_valuesEqualityComparer.Equals(node!.Value, value)) return node;
                node = node!.Next as TNode;
            } while (node != FirstNode);

            return null;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public bool TryRemove(T value)
        {
            TNode? node = GetNodeOrDefaultByValue(value);

            if (node == null) return false;

            if (Count == 1)
                FirstNode = null;
            else
                RemoveNotSingleNode(node);

            Count--;
            _version++;
            node.IsRemoved = true;

            return true;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public TNode AddToEnd(T value)
        {
            TNode node = IsEmpty ? AddSingleValue(value) : AddValueToEndOfNotEmptyList(value);

            Count++;
            _version++;

            return node;
        }

        /// <summary>
        ///     Removes specified node.
        /// </summary>
        /// <param name="node">Node of specified type. It's not single the list.</param>
        protected abstract void RemoveNotSingleNode(TNode node);

        /// <summary>
        ///     Adds single node with specified value to list.
        /// </summary>
        /// <param name="value">Value of specified type.</param>
        protected abstract TNode AddSingleValue(T value);

        /// <summary>
        ///     Adds node with specified value to end of the list.
        /// </summary>
        /// <param name="value">Value of specified type.</param>
        protected abstract TNode AddValueToEndOfNotEmptyList(T value);
    }
}