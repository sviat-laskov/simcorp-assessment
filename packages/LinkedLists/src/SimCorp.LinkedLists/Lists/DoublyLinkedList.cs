using System.Collections.Generic;
using SimCorp.LinkedLists.Nodes;

namespace SimCorp.LinkedLists.Lists
{
    /// <summary>
    ///     Provides linked list's functionality with doubly linked nodes.
    /// </summary>
    /// <typeparam name="T">Any value or reference type.</typeparam>
    public class DoublyLinkedList<T> : LinkedList<T, DoublyLinkedListNode<T>>
    {
        private DoublyLinkedListNode<T>? LastNode => FirstNode?.Previous;

        protected DoublyLinkedList(IEqualityComparer<T>? comparer = null) : base(comparer) { }

        /// <summary>
        ///     Creates instance of list.
        /// </summary>
        /// <param name="comparer">Custom comparer for checking values' equality. If not provided, the default one will be used.</param>
        public static DoublyLinkedList<T> Create(IEqualityComparer<T>? comparer = null) => new DoublyLinkedList<T>(comparer);

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override void RemoveNotSingleNode(DoublyLinkedListNode<T> node)
        {
            DoublyLinkedListNode<T> previousNode = node.Previous;
            DoublyLinkedListNode<T> nextNode = node.Next;
            previousNode.Next = nextNode;
            nextNode.Previous = previousNode;

            if (node == FirstNode) FirstNode = nextNode;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override DoublyLinkedListNode<T> AddSingleValue(T value) => FirstNode = DoublyLinkedListNode<T>.FromValueAsSelfReferenced(value);

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override DoublyLinkedListNode<T> AddValueToEndOfNotEmptyList(T value)
        {
            DoublyLinkedListNode<T> node = DoublyLinkedListNode<T>.FromValue(value, FirstNode!);
            LastNode!.Next = node;
            FirstNode!.Previous = node;

            return node;
        }
    }
}