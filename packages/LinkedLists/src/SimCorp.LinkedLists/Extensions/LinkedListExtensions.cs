using System.Collections.Generic;
using System.Linq;
using SimCorp.LinkedLists.Lists.Interfaces;

namespace SimCorp.LinkedLists.Extensions
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<TNode> AddRangeToEnd<T, TNode>(this ILinkedList<T, TNode> list, IEnumerable<T> values)
            where TNode : Nodes.LinkedListNode<T> => values.Select(list.AddToEnd);
    }
}