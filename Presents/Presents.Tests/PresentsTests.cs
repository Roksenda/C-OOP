using System;

namespace Presents.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class PresentsTests
    {
        private Bag bag;
    
        [SetUp]

        public void SetUp()
        { 
            bag = new Bag();
        }
        [Test]

        public void ConstructorShouldInitializeCollection()
        {
            Assert.AreEqual(0, bag.GetPresents().Count);
        }

        [Test]

        public void CreateShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>((() => bag.Create(null)));
        }

        [Test]

        public void CreateShouldThrowExceptionForDuplicate()
        {
            Present present = new Present("toy", 100);
            bag.Create(present);
            Assert.Throws<InvalidOperationException>((() => bag.Create(present)));
        }

        [Test]

        public void CreateShouldAddPresent()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Create(present1);

            Assert.AreEqual(2, bag.GetPresents().Count);
        }

        [Test]

        public void RemoveShouldRemove()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Create(present1);
            bag.Remove(present1);

            Assert.AreEqual(1, bag.GetPresents().Count);
        }

        public void RemoveShouldReturnFalse()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Remove(present1);

            Assert.IsFalse(bag.Remove(present1));
        }
        public void RemoveShouldReturnTrue()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Create(present1);
            Assert.IsTrue(bag.Remove(present1));
        }

        [Test]

        public void GetPresentShouldFindPresent()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Create(present1);

            Assert.AreSame(present, bag.GetPresent(present.Name));
        }

        [Test]

        public void GetPresentShouldReturnNullIfPresentNotFound()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Create(present1);

            Assert.AreEqual(null, bag.GetPresent("Pesho"));
        }
        [Test]

        public void GetPresentWithMagic()
        {
            Present present = new Present("toy", 100);
            Present present1 = new Present("pipi", 101);
            bag.Create(present);
            bag.Create(present1);

            Assert.AreSame(present, bag.GetPresentWithLeastMagic());
        }
    }
}
