using NUnit.Framework;

namespace Glass.Sandbox.ObjectCreation
{
    [TestFixture]
    public class NewObjectCreationTests : ObjectCreationBase
    {
        [Test]
        public void CreateNewObject()
        {
            // Assign
            
            // Act
            TestObject(x => new StubClass() {Property1 = x.Property1, Property2 = x.Property2});

            // Assert
        }

        [Test]
        public void CreateNewObjectLots()
        {
            // Assign

            // Act
            TestObjectLots(x => new StubClass { Property1 = x.Property1, Property2 = x.Property2 });

            // Assert
        }
    }
}
