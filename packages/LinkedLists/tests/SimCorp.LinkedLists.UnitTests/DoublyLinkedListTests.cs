using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimCorp.LinkedLists.Extensions;
using SimCorp.LinkedLists.Lists;
using SimCorp.LinkedLists.Nodes;

namespace SimCorp.LinkedLists.UnitTests
{
    [TestClass]
    public class DoublyLinkedListTests
    {
        private const string SampleValue = "value";

        private readonly IFixture _fixture = new Fixture();

        private DoublyLinkedList<string> _list = DoublyLinkedList<string>.Create();

        private string[] ManyStrings => _fixture.CreateMany<string>(count: 100).ToArray();

        [TestInitialize]
        public void CreateList() => _list = DoublyLinkedList<string>.Create();

        [TestMethod]
        public void IsEmptyPropertyIsTrueWhenListIsEmpty() => _list.IsEmpty.Should().BeTrue();

        [TestMethod]
        public void IsEmptyPropertyIsTrueWhenSingleNodeIsRemoved()
        {
            // Arrange
            _ = _list.AddToEnd(SampleValue);
            _list.TryRemove(SampleValue);

            // Act
            bool actualResult = _list.IsEmpty;

            // Assert
            actualResult
                .Should().BeTrue();
        }

        [TestMethod]
        public void CountPropertyReturnsNodesCount()
        {
            // Arrange
            string[] values = ManyStrings;
            _ = _list.AddRangeToEnd(values).ToArray();
            int expectedCount = values.Length;

            // Act
            int actualCount = _list.Count;

            // Assert
            actualCount
                .Should().Be(expectedCount);
        }

        [TestMethod]
        public void CountPropertyReturnsNodesCountAfterNodeRemoval()
        {
            // Arrange
            string[] values = ManyStrings;
            _ = _list.AddRangeToEnd(values).ToArray();
            _list.TryRemove(values.Last());
            int expectedCount = values.Length - 1;

            // Act
            int actualCount = _list.Count;

            // Assert
            actualCount
                .Should().Be(expectedCount);
        }

        [TestMethod]
        [DataRow(SampleValue)]
        public void SingleNodeNextAndPreviousPropertiesPointsAtSingleNode(string value)
        {
            // Act
            DoublyLinkedListNode<string> node = _list.AddToEnd(value);

            // Assert
            node.Next
                .Should().Be(node);
            node.Previous
                .Should().Be(node);
        }

        [TestMethod]
        public void AddingNullValueThrows() => _list.Invoking(list => list.AddToEnd(value: null!))
            .Should().ThrowExactly<ArgumentNullException>();

        [TestMethod]
        public void NodesCanBeGotByValues()
        {
            // Arrange
            string[] expectedValues = ManyStrings;
            _ = _list.AddRangeToEnd(expectedValues).ToArray();

            // Act
            IEnumerable<DoublyLinkedListNode<string>> nodes = expectedValues.Select(value => _list.GetNodeOrDefaultByValue(value)!).ToArray();

            // Assert
            IEnumerable<string> actualValues = nodes.Select(node => node.Value);
            actualValues
                .Should().Equal(expectedValues);
        }

        [TestMethod]
        [DataRow(SampleValue)]
        public void FirstNodeFromMultipleWithSameValuesIsGotByValue(string value)
        {
            // Arrange
            DoublyLinkedListNode<string> expectedFirstNode = _list.AddToEnd(value);
            DoublyLinkedListNode<string> expectedSecondNode = _list.AddToEnd(value);

            // Act
            DoublyLinkedListNode<string> actualFirstNode = _list.GetNodeOrDefaultByValue(value)!;
            bool firstNodeRemovalResult = _list.TryRemove(value);
            DoublyLinkedListNode<string> actualSecondNode = _list.GetNodeOrDefaultByValue(value)!;

            // Assert
            actualFirstNode
                .Should().Be(expectedFirstNode);
            firstNodeRemovalResult
                .Should().BeTrue();
            actualSecondNode
                .Should().Be(expectedSecondNode);
        }

        [TestMethod]
        [DataRow(SampleValue)]
        public void RemovedNodeCantBeAccessed(string value)
        {
            // Arrange
            DoublyLinkedListNode<string> node = _list.AddToEnd(value);
            _list.TryRemove(value);
            var funcsThrowing = new List<Func<DoublyLinkedListNode<string>, object>>
            {
                node => node.Value,
                node => node.Next,
                node => node.Previous,
                node => node.ToString()
            };

            // Assert
            funcsThrowing.ForEach(func => func.Invoking(func => func(node))
                .Should().ThrowExactly<ObjectDisposedException>());
        }

        [TestMethod]
        public void NodesNextPropertyPointsAtNextNodeAndPreviousPropertyPointsAtPreviousNode()
        {
            // Arrange
            DoublyLinkedListNode<string>[] nodes = _list.AddRangeToEnd(ManyStrings).ToArray();

            int firstNodeCount = 1;
            int nodesCountWithoutFirstAndLastElement = _list.Count - 2;
            List<(DoublyLinkedListNode<string> previousNode, DoublyLinkedListNode<string> node, DoublyLinkedListNode<string> nextNode)> neighboringNodesWithoutLast = nodes
                .Skip(firstNodeCount)
                .Take(nodesCountWithoutFirstAndLastElement)
                .Select((node, nodeIndex) =>
                {
                    nodeIndex += firstNodeCount;

                    DoublyLinkedListNode<string> previousNode = nodes[nodeIndex - 1];
                    DoublyLinkedListNode<string> nextNode = nodes[nodeIndex + 1];
                    return (previousNode, node, nextNode);
                })
                .ToList();

            // Assert
            neighboringNodesWithoutLast.ForEach(neighboringNodes =>
            {
                neighboringNodes.node.Previous
                    .Should().Be(neighboringNodes.previousNode);
                neighboringNodes.node.Next
                    .Should().Be(neighboringNodes.nextNode);
            });
            nodes.First().Previous
                .Should().Be(nodes.Last());
            nodes.Last().Next
                .Should().Be(nodes.First());
        }

        [TestMethod]
        public void ValuesCanBeGotViaValuesProperty()
        {
            // Arrange
            string[] expectedValues = ManyStrings;
            _ = _list.AddRangeToEnd(expectedValues).ToArray();

            // Act
            string[] actualValues = _list.Values;

            // Assert
            actualValues
                .Should().Equal(expectedValues);
        }

        [TestMethod]
        public void ValuesCanBeGotViaEnumerator()
        {
            // Arrange
            string[] expectedValues = ManyStrings;
            _ = _list.AddRangeToEnd(expectedValues).ToArray();

            // Act
            string[] actualValues = _list.ToArray();

            // Assert
            actualValues
                .Should().Equal(expectedValues);
        }

        [TestMethod]
        [DataRow(SampleValue)]
        public void EnumeratorThrowsWhenListIsUpdated(string value)
        {
            // Arrange
            _list.AddToEnd(value);
            using IEnumerator<string> listEnumerator = _list.GetEnumerator();

            // Act
            _list.AddToEnd(value);

            // Assert
            listEnumerator.Invoking(enumerator => enumerator.MoveNext())
                .Should().ThrowExactly<InvalidOperationException>();
        }
    }
}