using System;

namespace SimCorp.LinkedLists.Nodes
{
    public abstract class LinkedListNode<T>
    {
        private readonly T _value;
        private LinkedListNode<T> _next;
        internal bool IsRemoved;

        public T Value => GetPropertyIfNodeIsNotMarkedAsRemoved(_value);

        public LinkedListNode<T> Next
        {
            get => GetPropertyIfNodeIsNotMarkedAsRemoved(_next);
            protected set => _next = value;
        }

        internal LinkedListNode(T value, LinkedListNode<T>? next = null)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _next = next ?? this;
        }

        public override string ToString() => Value!.ToString();

        protected TProperty GetPropertyIfNodeIsNotMarkedAsRemoved<TProperty>(TProperty property) => !IsRemoved ? property : throw new ObjectDisposedException("This linked list's node is removed.");
    }
}