using System;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class ActionEqualityFixture
    {
        [Test]
        public void ActionsAreEqual()
        {
            Action<IDependencyRegistrar> action1 = x => x.RegisterTransient<ITestInterface, Concrete1>();
            Action<IDependencyRegistrar> action2 = x => x.RegisterTransient<ITestInterface, Concrete1>();
            Assert.AreEqual(action2, action1);
        }
    }

    public interface ITestInterface
    {
        
    }

    public class Concrete1 : ITestInterface
    {
        
    }
}
