using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMe.BuildingBlocks.App.Tests
{
    [TestClass]
    public class MergeWithTests
    {
        [TestMethod]
        public void GivenEmptyTargetList_AllItemsShouldBeAdded()
        {
            var target = new List<Item>();
            var source = new List<Item>() { new Item(0, "a"), new Item(0, "b") };

            target.MergeWith(source, x => x.Id, x => x.Id,
                             onAdd: x => target.Add(x),
                             onUpdate: (x, y) => { x.Id = y.Id; x.Name = y.Name; },
                             onDelete: x => target.Remove(x));


            bool areSequencesEqual = target.SequenceEqual(source);
            Assert.AreEqual(true, areSequencesEqual);
        }

        [TestMethod]
        public void GivenEmptySourceList_AllItemsShouldBeRemovedFromTarget()
        {
            var target = new List<Item>() { new Item(1, "a"), new Item(2, "b") };
            var source = new List<Item>() { };

            target.MergeWith(source, x => x.Id, x => x.Id,
                             onAdd: x => target.Add(x),
                             onUpdate: (x, y) => { x.Id = y.Id; x.Name = y.Name; },
                             onDelete: x => target.Remove(x));

            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void GivenItemsWithTheSameId_NamesShouldBeUpdated()
        {
            var target = new List<Item>() { new Item(1, "a"), new Item(2, "b") };
            var source = new List<Item>() { new Item(1, "c"), new Item(2, "d") };

            target.MergeWith(source, x => x.Id, x => x.Id,
                             onAdd: x => target.Add(x),
                             onUpdate: (x, y) => { x.Id = y.Id; x.Name = y.Name; },
                             onDelete: x => target.Remove(x));

            bool areSequencesEqual = target.SequenceEqual(source);
            Assert.AreEqual(true, areSequencesEqual);
        }

        [TestMethod]
        public void FullUseCaseIsSuccessful()
        {
            var target = new List<Item>() { new Item(1, "a"), new Item(2, "b") };
            var source = new List<Item>() { new Item(2, "c"), new Item(0, "d") };

            target.MergeWith(source, x => x.Id, x => x.Id,
                             onAdd: x => target.Add(x),
                             onUpdate: (x, y) => { x.Id = y.Id; x.Name = y.Name; },
                             onDelete: x => target.Remove(x));

            bool areSequencesEqual = target.SequenceEqual(source);
            Assert.AreEqual(true, areSequencesEqual);
        }


        private class Item : System.IEquatable<Item>
        {
            public long Id { get; set; }
            public string Name { get; set; }

            public Item(long id, string name)
            {
                Id = id;
                Name = name;
            }

            public bool Equals(Item other)
            {
                return this.Id == other.Id && this.Name == other.Name;
            }
        }
    }
}
