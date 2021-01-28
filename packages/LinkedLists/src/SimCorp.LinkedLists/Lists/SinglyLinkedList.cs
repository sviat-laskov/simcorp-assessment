using System.Collections.Generic;
using SimCorp.LinkedLists.Nodes;

namespace SimCorp.LinkedLists.Lists
{
    /// <summary>
    ///     Provides linked list's functionality with singly linked nodes.
    /// </summary>
    /// <typeparam name="T">Any value or reference type.</typeparam>
    public class SinglyLinkedList<T> : LinkedList<T, SinglyLinkedListNode<T>>
    {
        /// <summary>
        ///     Last node's <see cref="SinglyLinkedListNode{T}.Next" /> property is pointed at the first node and is needed to add
        ///     new nodes to the end of list.
        /// </summary>
        protected SinglyLinkedListNode<T>? LastNode { get; set; } // Is needed cause of AC about adding new items to the end of list.

        protected SinglyLinkedList(IEqualityComparer<T>? comparer = null) : base(comparer) { }

        /// <summary>
        ///     Creates instance of list.
        /// </summary>
        /// <param name="comparer">Custom comparer for checking values' equality. If not provided, the default one will be used.</param>
        public static SinglyLinkedList<T> Create(IEqualityComparer<T>? comparer = null) => new SinglyLinkedList<T>(comparer);

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override void RemoveNotSingleNode(SinglyLinkedListNode<T> node)
        {
            if (node == FirstNode)
            {
                FirstNode = FirstNode.Next;
                LastNode!.Next = FirstNode;

                return;
            }

            SinglyLinkedListNode<T> previousNode = GetNodeBefore(node);
            previousNode.Next = node.Next;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override SinglyLinkedListNode<T> AddSingleValue(T value) => FirstNode = LastNode = SinglyLinkedListNode<T>.FromValueAsSelfReferenced(value);

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override SinglyLinkedListNode<T> AddValueToEndOfNotEmptyList(T value)
        {
            SinglyLinkedListNode<T> node = SinglyLinkedListNode<T>.FromValue(value, FirstNode!);
            LastNode!.Next = node;
            LastNode = node;

            return node;
        }

        /// <param name="node">Not first node in the list.</param>
        private SinglyLinkedListNode<T> GetNodeBefore(SinglyLinkedListNode<T> node)
        {
            SinglyLinkedListNode<T> currentNode = FirstNode!;
            while (currentNode.Next != node) currentNode = currentNode.Next!;
            return currentNode;
        }
    }
}