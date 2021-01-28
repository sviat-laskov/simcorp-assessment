namespace SimCorp.LinkedLists.Nodes
{
    public class DoublyLinkedListNode<T> : LinkedListNode<T>
    {
        private DoublyLinkedListNode<T> _previous;

        public DoublyLinkedListNode<T> Previous
        {
            get => GetPropertyIfNodeIsNotMarkedAsRemoved(_previous);
            internal set => _previous = value;
        }

        public new DoublyLinkedListNode<T> Next
        {
            get => (DoublyLinkedListNode<T>) base.Next!;
            set => base.Next = value;
        }

        private DoublyLinkedListNode(T value, DoublyLinkedListNode<T>? next = null) : base(value, next) => _previous = next?.Previous ?? this;

        internal static DoublyLinkedListNode<T> FromValue(T value, DoublyLinkedListNode<T> next) => new DoublyLinkedListNode<T>(value, next);

        internal static DoublyLinkedListNode<T> FromValueAsSelfReferenced(T value) => new DoublyLinkedListNode<T>(value);
    }
}