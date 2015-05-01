using System.Collections.Generic;
using Glass.Mapper.Caching;
using Glass.Mapper.Serialization;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Caching
{
    [TestFixture]
    public class CachingTestFixture
    {
        [Test]
        public void Serializable_objects_add_correctly()
        {
            // Assign
            const string value = "value";
            ISerializationManager manager = Substitute.For<ISerializationManager>();
            manager.CanSerialize(Arg.Any<object>()).Returns(true);
            var cache = new SerializableDictionaryCache(manager);

            // Act
            cache.AddOrUpdate("key", value);

            // Assert
            manager.Received(1).CanSerialize(value);
            manager.Received(1).Serialize(value);
        }

        [Test]
        public void Non_serializable_objects_add_correctly()
        {
            // Assign
            const string value = "value";
            ISerializationManager manager = Substitute.For<ISerializationManager>();
            manager.CanSerialize(Arg.Any<object>()).Returns(false);
            var cache = new SerializableDictionaryCache(manager);

            // Act
            cache.AddOrUpdate("key", value);

            // Assert
            manager.Received(1).CanSerialize(value);
            manager.Received(0).Serialize(value);
        }

        [Test]
        public void Serializable_objects_return_correctly()
        {
            // Assign
            const string value = "value";
            ISerializationManager manager = Substitute.For<ISerializationManager>();
            manager.CanSerialize(Arg.Any<object>()).Returns(true);
            manager.CanDeserialize(Arg.Any<object>()).Returns(true);
            ISerializationManager actualSerializationManager = new NetBinarySerializationManager();
            var serializedValue = actualSerializationManager.Serialize(value);
            manager.Serialize(value).Returns(serializedValue);
            manager.Deserialize(serializedValue).Returns(actualSerializationManager.Deserialize(serializedValue));
            var cache = new SerializableDictionaryCache(manager);

            // Act
            cache.AddOrUpdate("key", value);
            var result = cache.Get("key");

            // Assert
            Assert.AreEqual(result, value);
            manager.Received(1).CanSerialize(value);
            manager.Received(1).CanDeserialize(serializedValue);
            manager.Received(1).Serialize(value);
            manager.Received(1).Deserialize(serializedValue);
        }

        [Test]
        public void Non_serializable_objects_return_correctly()
        {
            // Assign
            const string value = "value";
            ISerializationManager manager = Substitute.For<ISerializationManager>();
            manager.CanSerialize(Arg.Any<object>()).Returns(false);
            manager.CanDeserialize(Arg.Any<object>()).Returns(false);
            var cache = new SerializableDictionaryCache(manager);

            // Act
            cache.AddOrUpdate("key", value);
            var result = cache.Get("key");

            // Assert
            Assert.AreEqual(result, value);
            manager.Received(1).CanSerialize(value);
            manager.Received(0).CanDeserialize(value);
            manager.Received(0).Serialize(value);
            manager.Received(0).Deserialize(Arg.Any<byte[]>());
        }

        public class SerializableDictionaryCache : AbstractSerializationCacheManager
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            public SerializableDictionaryCache(ISerializationManager serializationManager) : base(serializationManager)
            {
            }

            public override void ClearCache()
            {
                dictionary = new Dictionary<string, object>();
            }

            public override void AddOrUpdate(string key, object value)
            {
                object valueToUse = GetSerializingValue(value);

                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, valueToUse);
                }
                else
                {
                    dictionary[key] = valueToUse;
                }
            }

            public override object Get(string key)
            {
                if (dictionary.ContainsKey(key))
                {
                    object value = dictionary[key];
                    return GetDeserializingValue(value);
                }

                return null;
            }

            public override bool Contains(string key)
            {
                return dictionary.ContainsKey(key);
            }
        }

        public class DictionaryCache : AbstractCacheManager
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>(); 

            public override void ClearCache()
            {
                dictionary = new Dictionary<string, object>();
            }

            public override void AddOrUpdate(string key, object value)
            {
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, value);
                }
                else
                {
                    dictionary[key] = value;
                }
            }

            public override object Get(string key)
            {
                return dictionary.ContainsKey(key) ? dictionary[key] : null;
            }

            public override bool Contains(string key)
            {
                return dictionary.ContainsKey(key);
            }
        }
    }
}
