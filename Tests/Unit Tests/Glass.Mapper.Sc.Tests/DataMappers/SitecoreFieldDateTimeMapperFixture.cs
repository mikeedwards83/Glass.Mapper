/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Tests.DataMappers
{
    [TestFixture]
    public class SitecoreFieldDateTimeMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidDate_ReturnsDateTime()
        {
            //Assign
            string fieldValue = "20120101T010101";
            DateTime expected = new DateTime(2012, 01, 01, 01, 01, 01);
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();

          
            //Act
            var result = (DateTime) mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsValidIsoDate_ReturnsDateTime()
        {
            //Assign 
            string fieldValue = "20121101T230000";
            DateTime expected = new DateTime(2012, 11, 01, 23, 00, 00);
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();


            //Act
            var result = (DateTime)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

#if (SC80 || SC81)

        [Test]
        public void SitecoreTimeZoneDemo()
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("be-NL");
            const string isoDateString = "20151013T220000Z";
            const string serverDateString = "20120901T010101";

            var serverDateTime = Sitecore.DateUtil.IsoDateToServerTimeIsoDate(isoDateString);
            var utcDate = Sitecore.DateUtil.IsoDateToUtcIsoDate(isoDateString);

            Console.WriteLine(Sitecore.DateUtil.IsoDateToDateTime(utcDate));
            var isoDate = Sitecore.DateUtil.IsoDateToDateTime(isoDateString);
            Console.WriteLine(isoDate);
            Console.WriteLine(Sitecore.DateUtil.ToServerTime(isoDate));
            Console.WriteLine(Sitecore.DateUtil.IsoDateToDateTime(serverDateTime));

            // go the other way

            Console.WriteLine(Sitecore.DateUtil.ToIsoDate(DateTime.Now));
            Console.WriteLine(Sitecore.DateUtil.ToIsoDate(DateTime.UtcNow));
            Console.WriteLine(Sitecore.DateUtil.ToIsoDate(DateTime.Now, false, true));

            var serverDate = Sitecore.DateUtil.IsoDateToDateTime(serverDateString);
            Console.WriteLine(serverDate);
            Console.WriteLine(Sitecore.DateUtil.ToServerTime(serverDate));

            Console.WriteLine(Sitecore.DateUtil.IsoDateToDateTime(serverDateString, DateTime.MinValue, false));
            var crappyDate = Sitecore.DateUtil.IsoDateToDateTime(serverDateString, DateTime.MinValue, true);
            Console.WriteLine(crappyDate);
            Console.WriteLine(Sitecore.DateUtil.ToServerTime(serverDate));
            Console.WriteLine(Sitecore.DateUtil.IsoDateToDateTime(isoDateString, DateTime.MinValue, true));

        }

        [Test]
        [Ignore("POC only")]
        public void ConvertTimeToServerTime_using_date_lots()
        {
            const string isoDateString = "20151013T220000Z";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var isoDate = Sitecore.DateUtil.IsoDateToDateTime(isoDateString);
                Sitecore.DateUtil.ToServerTime(isoDate);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
        }

        [Test]
        [Ignore("POC only")]
        public void ConvertTimeToServerTime_checking_z_lots()
        {
            const string isoDateString = "20151013T220000Z";
            const string serverDateString = "20120101T010101";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var currentString = i%2 == 1 ? isoDateString : serverDateString;
                DateTime isoDate = Sitecore.DateUtil.IsoDateToDateTime(currentString);
                if (currentString.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
                {
                    Sitecore.DateUtil.ToServerTime(isoDate);
                }
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [Test]
        [Ignore("POC only")]
        public void ConvertTimeToServerTime_not_checking_z_lots()
        {
            const string isoDateString = "20151013T220000Z";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var isoDate = Sitecore.DateUtil.IsoDateToDateTime(isoDateString);
                Sitecore.DateUtil.ToServerTime(isoDate);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [Test]
        [Ignore("POC only")]
        public void ConvertTimeToServerTime_using_string_lots()
        {
            const string isoDateString = "20151013T220000Z";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var serverDateTime = Sitecore.DateUtil.IsoDateToServerTimeIsoDate(isoDateString);
                Sitecore.DateUtil.IsoDateToDateTime(serverDateTime);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
        }

#endif

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_DateTimePassed_SetsFieldValue()
        {
            //Assign
#if (SC75)
            string expected = "20120101T010101";
#else
            string expected = "20120101T010101Z";
#endif
            DateTime objectValue = new DateTime(2012, 01, 01, 01, 01, 01);
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();

            item.Editing.BeginEdit();
            //Act
          
                mapper.SetField(field, objectValue, null, null);
           
        

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof (NotSupportedException))]
        public void SetField_NonDateTimePassed_ExceptionThrown()
        {
            //Assign
            string expected = "20120101T010101Z";
            int objectValue = 4;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();

            item.Editing.BeginEdit();

            //Act

            mapper.SetField(field, objectValue, null, null);



            //Assert
        }

#endregion
    }
}




