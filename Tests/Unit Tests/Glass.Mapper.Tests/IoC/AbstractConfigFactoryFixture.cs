using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.IoC;
using NUnit.Framework;

namespace Glass.Mapper.Tests.IoC
{
    [TestFixture]
    public class AbstractConfigFactoryFixture
    {
        [Test]
        public void InsertBefore_InsertBeforeSpecifiedType()
        {
            //Arrange
            var factory = new StubAbstractConfigFactory();
            factory.Add(() => new Task1());
            factory.Add(()=>new Task2());

            //Act
            factory.InsertBefore<Task2, Task3>(()=> new Task3());

            //Assert

            var types = factory.Types.ToArray();
            Assert.AreEqual(typeof(Task1), types[0]);
            Assert.AreEqual(typeof(Task3), types[1]);
            Assert.AreEqual(typeof(Task2), types[2]);
        }

        [Test]
        public void InsertAfter_InsertAfterSpecifiedType()
        {
            //Arrange
            var factory = new StubAbstractConfigFactory();
            factory.Add(() => new Task1());
            factory.Add(() => new Task2());

            //Act
            factory.InsertAfter<Task1, Task3>(() => new Task3());

            //Assert
            var types = factory.Types.ToArray();
            Assert.AreEqual(typeof(Task1), types[0]);
            Assert.AreEqual(typeof(Task3), types[1]);
            Assert.AreEqual(typeof(Task2), types[2]);
        }

        [Test]
        public void Replace_ReplaceSpecifiedType()
        {
            //Arrange
            var factory = new StubAbstractConfigFactory();
            factory.Add(() => new Task1());
            factory.Add(() => new Task2());

            //Act
            factory.Replace<Task1, Task3>(() => new Task3());

            //Assert
            var types = factory.Types.ToArray();
            Assert.AreEqual(typeof(Task3), types[0]);
            Assert.AreEqual(typeof(Task2), types[1]);
        }

        [Test]
        public void Remove_RemoveSpecifiedType()
        {
            //Arrange
            var factory = new StubAbstractConfigFactory();
            factory.Add(() => new Task1());
            factory.Add(() => new Task2());

            //Act
            factory.Remove<Task1>();

            //Assert
            var types = factory.Types.ToArray();
            Assert.AreEqual(typeof(Task2), types[0]);
        }

        #region Stubs


        public class StubAbstractConfigFactory : AbstractConfigFactory<ITask>
        {
            public new IEnumerable<Type> Types
            {
                get { return base.TypeGenerators.Select(x=>x.Type); }
            }
        }

        public interface ITask {}
        public class Task1 : ITask { }
        public class Task2 : ITask { }
        public class Task3 : ITask { }

        #endregion

    }
}
