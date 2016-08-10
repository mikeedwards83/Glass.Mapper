using System;
using System.Threading;
using Glass.Mapper.Caching;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.Caching
{
    [TestFixture]
    public class NetMemoryCacheManagerTestFixture
    {
        [Test]
        public void Can_get_item_from_cache_and_expires_absolute()
        {
            // Arrange
            const string cacheKey = "TestAbsoluteKey";
            NetMemoryCacheManager memoryCacheManager = new NetMemoryCacheManager {AbsoluteExpiry = 2};

            // Act
            TestCacheStub testCacheClass = new TestCacheStub();

            memoryCacheManager.AddOrUpdate(cacheKey, testCacheClass);

            // Assert
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));
            Thread.Sleep(new TimeSpan(0,0,0,1));
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 1));
            Assert.IsNull(memoryCacheManager.Get<TestCacheStub>(cacheKey));
        }

        [Test]
        public void Can_get_item_from_cache_and_expires_sliding()
        {
            // Arrange
            const string cacheKey = "TestSlidingKey";
            NetMemoryCacheManager memoryCacheManager = new NetMemoryCacheManager {SlidingExpiry = 2};

            // Act
            TestCacheStub testCacheClass = new TestCacheStub();
            memoryCacheManager.AddOrUpdate(cacheKey, testCacheClass);

            // Assert
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 1));
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 1));
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 3));
            Assert.IsNull(memoryCacheManager.Get<TestCacheStub>(cacheKey));
        }

        [Test]
        public void Can_get_integer_from_cache_and_expires_sliding()
        {
            // Arrange
            const string cacheKey = "TestSlidingKey";
            NetMemoryCacheManager memoryCacheManager = new NetMemoryCacheManager { SlidingExpiry = 2 };

            // Act
            memoryCacheManager.AddOrUpdate(cacheKey, 1);

            // Assert
            Assert.AreEqual(1, memoryCacheManager.GetValue<int>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 1));
            Assert.AreEqual(1, memoryCacheManager.GetValue<int>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 1));
            Assert.AreEqual(1, memoryCacheManager.GetValue<int>(cacheKey));
            Thread.Sleep(new TimeSpan(0, 0, 0, 3));
            Assert.AreEqual(0, memoryCacheManager.GetValue<int>(cacheKey));
        }

        [Test]
        public void Cache_destroyed_and_recreated()
        {
            // Arrange
            const string cacheKey = "TestDestroyedCache";
            NetMemoryCacheManager memoryCacheManager = new NetMemoryCacheManager {AbsoluteExpiry = 200};

            // Act
            TestCacheStub testCacheClass = new TestCacheStub();
            memoryCacheManager.AddOrUpdate(cacheKey, testCacheClass);

            // Assert
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));

            memoryCacheManager.ClearCache();
            Assert.IsNull(memoryCacheManager.Get<TestCacheStub>(cacheKey));

            memoryCacheManager.AddOrUpdate(cacheKey, testCacheClass);
            Assert.AreEqual(testCacheClass, memoryCacheManager.Get<TestCacheStub>(cacheKey));
        }
    }

    internal class TestCacheStub
    {
        
    }
}
