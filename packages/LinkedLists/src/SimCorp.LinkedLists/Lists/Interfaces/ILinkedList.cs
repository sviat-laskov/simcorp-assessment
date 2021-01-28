using System.Collections.Generic;
using SimCorp.LinkedLists.Nodes;

namespace SimCorp.LinkedLists.Lists.Interfaces
{
    /// <summary>
    ///     Interface for all types of linked lists.
    /// </summary>
    /// <typeparam name="T">Any value or reference type.</typeparam>
    /// <typeparam name="TNode"><see cref="SinglyLinkedListNode{T}" /> or <see cref="DoublyLinkedListNode{T}" />.</typeparam>
    public interface ILinkedList<T, out TNode> : IEnumerable<T>
        where TNode : Nodes.LinkedListNode<T>
    {
        /// <summary>
        ///     Count of values in the list.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     List's values' array.
        /// </summary>
        T[] Values { get; }

        /// <summary>
        ///     Adds value to end of the list.
        /// </summary>
        /// <param name="value">Value of specified type.</param>
        /// <returns>Added node.</returns>
        TNode AddToEnd(T value);

        /// <summary>
        ///     Gets first node, which value equals provided value.
        /// </summary>
        /// <param name="value">Value of specified type.</param>
        /// <returns>Specified node or null if node does not exist.</returns>
        TNode? GetNodeOrDefaultByValue(T value);

        /// <summary>
        ///     Removes first node, which value equals provided value.
        /// </summary>
        /// <param name="value">Value of specified type.</param>
        /// <returns>'True' if node is removed, or 'False' if node with specified values is not found.</returns>
        bool TryRemove(T value);
    }
}