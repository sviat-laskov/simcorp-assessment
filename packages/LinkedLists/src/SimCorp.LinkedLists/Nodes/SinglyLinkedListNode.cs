namespace SimCorp.LinkedLists.Nodes
{
    public class SinglyLinkedListNode<T> : LinkedListNode<T>
    {
        public new SinglyLinkedListNode<T> Next
        {
            get => (SinglyLinkedListNode<T>) base.Next;
            set => base.Next = value;
        }

        private SinglyLinkedListNode(T value, SinglyLinkedListNode<T>? next = null) : base(value, next) { }

        internal static SinglyLinkedListNode<T> FromValue(T value, SinglyLinkedListNode<T> next) => new SinglyLinkedListNode<T>(value, next);

        internal static SinglyLinkedListNode<T> FromValueAsSelfReferenced(T value) => new SinglyLinkedListNode<T>(value);
    }
}