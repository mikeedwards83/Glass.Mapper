using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class UtilitiesFixture
    {

        #region Method - CreateConstructorDelegates

        [Test]
        public void CreateConstructorDelegates_NoParameters_CreatesSingleConstructor()
        {

        }

        #region Stubs

        public class StubNoParameters
        {
            public StubNoParameters()
            {
                
            }
        }

        public class StubOneParameter
        {
            public StubOneParameter(string param1)
            {

            }
        }

        public class StubTwoParameters
        {
            public StubTwoParameters(string param1, string param2)
            {

            }
        }

        public class StubThreeParameters
        {
            public StubThreeParameters(string param1, string param2, string param3)
            {

            }
        }

        public class StubFourParameters
        {
            public StubFourParameters(string param1, string param2, string param3, string param4)
            {

            }
        }

        #endregion

        #endregion



    }
}
