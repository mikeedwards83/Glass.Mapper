using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Glass.Mapper.Serialization;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Serialization
{
    [TestFixture]
    public class NetBinarySerializationFixture
    {
        [Test]
        public void Can_serialize_and_deserialize_serializable_object()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var serializable = new SerializableClass {Name = "Test"};

            // Act
            byte[] serialized = manager.Serialize(serializable);
            var result = manager.Deserialize(serialized) as SerializableClass;

            // Assert
            Assert.AreNotEqual(serializable, result);
            Assert.AreEqual("Test", result.Name);
            Assert.AreEqual(serializable.Name, result.Name);
        }

        [Test]
        public void Serializable_object_can_serialize_returns_true()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var serializable = new SerializableClass { Name = "Test" };

            // Act
            var result = manager.CanSerialize(serializable);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Serializable_object_can_deserialize_returns_true()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var serializable = new SerializableClass { Name = "Test" };
            var serialized = manager.Serialize(serializable);

            // Act
            var result = manager.CanDeserialize(serialized);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Cannot_serialize_and_deserialize_serializable_object()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var serializable = new NonSerializableTestClass { Name = "Test" };

            // Act
            byte[] serialized = manager.Serialize(serializable);

            // Assert
            Assert.IsNull(serialized);
        }

        [Test]
        public void Non_serializable_object_can_serialize_returns_false()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var serializable = new NonSerializableTestClass { Name = "Test" };

            // Act
            var result = manager.CanSerialize(serializable);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Non_serializable_object_can_serialize_speed()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var serializable = new NonSerializableTestClass { Name = "Test" };

            // Act

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                var result = manager.CanSerialize(serializable);
            }
            sw.Stop();
            Console.Write(sw.ElapsedMilliseconds);

            // Assert
        }

        [Test]
        public void Value_type_can_serialize_returns_false()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            const int serializable = 12;

            // Act
            var result = manager.CanSerialize(serializable);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void String_can_serialize_returns_false()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            const string serializable = "fred";

            // Act
            var result = manager.CanSerialize(serializable);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Interface_can_be_serialized()
        {
            // Assign
            var manager = new NetBinarySerializationManager();
            var builder = new PersistentProxyBuilder();
            var generator = new ProxyGenerator(builder);
            object proxy = generator.CreateInterfaceProxyWithoutTarget(typeof(IStubInterface), new Type[] { },
                                                new StandardInterceptor());

            // Act
            byte[] serialized = manager.Serialize(proxy);

            IStubInterface deserialized = manager.Deserialize(serialized) as IStubInterface;
            
            // Assert
            Assert.IsNotNull(deserialized);
        }

        public interface IStubInterface
        {
             
        }

        public class NonSerializableTestClass
        {
            public string Name { get; set; }
        }

        [Serializable]
        public class SerializableClass
        {
            public string Name { get; set; }
        }
    }
}
