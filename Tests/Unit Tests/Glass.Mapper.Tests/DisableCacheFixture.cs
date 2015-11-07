using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class DisableCacheFixture
    {
        [Test]
        public void DisableCache_SetDisableFlag()
        {
            Assert.AreEqual(CacheSetting.Enabled, DisableCache.Current);

            var disabler = new DisableCache();

            Assert.AreEqual(CacheSetting.Disabled, DisableCache.Current);

            disabler.Dispose();

            Assert.AreEqual(CacheSetting.Enabled, DisableCache.Current);
        }

        [Test]
        public void DisableCacheNest_SetDisableFlagTwice()
        {
            Assert.AreEqual(CacheSetting.Enabled, DisableCache.Current);

            var disabler1 = new DisableCache();
            var disabler2 = new DisableCache();

            Assert.AreEqual(CacheSetting.Disabled, DisableCache.Current);

            disabler1.Dispose();

            Assert.AreEqual(CacheSetting.Disabled, DisableCache.Current);

            disabler2.Dispose();

            Assert.AreEqual(CacheSetting.Enabled, DisableCache.Current);
        }
    }
}
