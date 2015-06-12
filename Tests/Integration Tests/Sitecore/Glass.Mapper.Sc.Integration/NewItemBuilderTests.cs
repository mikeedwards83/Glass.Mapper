using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class NewItemBuilderTests
    {
        [SetUp]
        public void Setup()
        {
            _db = Factory.GetDatabase("master");
            _id = new Guid("{59784F74-F830-4BCD-B1F0-1A08616EF726}");
            var item = _db.GetItem(new ID(_id));
        }

        private Database _db;
        private Guid _id;

        [Test]
        public void BasicMap()
        {
            var item = _db.GetItem(new ID(_id));

            var result = Map<StubClass>(item);
            Assert.AreEqual("hello world", result.Field);
            Assert.AreEqual(_id, result.Id);
        }

        [Test]
        public void DoMappingLots()
        {
            var item = _db.GetItem(new ID(_id));
            var resultWarmup = Map<StubClass>(item).Field;

            Stopwatch sitecoreStopwatch = new Stopwatch();
            sitecoreStopwatch.Start();
            for (var i = 0; i < 200000; i++)
            {
                var result = item["Field"];
                var id = item.ID.ToGuid();
            }

            sitecoreStopwatch.Stop();
            var sitecoreTotalTicks = sitecoreStopwatch.ElapsedTicks;


            Stopwatch glassStopwatch = new Stopwatch();
            glassStopwatch.Start();
            for (var i = 0; i < 200000; i++)
            {
                var result = Map<StubClass>(item);
                var field = result.Field;
                var id = result.Id;
            }
            glassStopwatch.Stop();
            var glassTotalTicks = glassStopwatch.ElapsedTicks;

            double total = (double)glassTotalTicks / (double)sitecoreTotalTicks;
            Console.WriteLine("Ratio: {0}", total);
            Console.WriteLine(sitecoreTotalTicks);
            Console.WriteLine(glassTotalTicks);
        }

        [Test]
        public void DoMappingLots2()
        {
            var item = _db.GetItem(new ID(_id));
            var resultWarmup = MapMore<StubClassWithLotsOfProperties>(item).Field1;

            Stopwatch sitecoreStopwatch = new Stopwatch();
            sitecoreStopwatch.Start();
            string lastSitecoreResult = null;
            for (var i = 0; i < 200000; i++)
            {
                var result = item["Field"];
                var result1 = item["Field"];
                var result2 = item["Field"];
                var result3 = item["Field"];
                var result4 = item["Field"];
                var result5 = item["Field"];
                var result6 = item["Field"];
                var result7 = item["Field"];
                var result8 = item["Field"];
                var result9 = item["Field"];
                var result10 = item["Field"];
                var result11 = item["Field"];
                var result12 = item["Field"];
                var result13 = item["Field"];
                var result14 = item["Field"];
                var result15 = item["Field"];
                var result16 = item["Field"];
                var result17 = item["Field"];
                var result18 = item["Field"];
                lastSitecoreResult = item["Field"];
                var id = item.ID.ToGuid();
                var url = LinkManager.GetItemUrl(item);
            }

            sitecoreStopwatch.Stop();
            var sitecoreTotalTicks = sitecoreStopwatch.ElapsedTicks;


            Stopwatch glassStopwatch = new Stopwatch();
            glassStopwatch.Start();
            string lastGlassResult = null;
            for (var i = 0; i < 200000; i++)
            {
                var result = MapMore<StubClassWithLotsOfProperties>(item);
                var result1 = result.Field1;
                var result2 = result.Field2;
                var result3 = result.Field3;
                var result4 = result.Field4;
                var result5 = result.Field5;
                var result6 = result.Field6;
                var result7 = result.Field7;
                var result8 = result.Field8;
                var result9 = result.Field9;
                var result10 = result.Field10;
                var result11 = result.Field11;
                var result12 = result.Field12;
                var result13 = result.Field13;
                var result14 = result.Field14;
                var result15 = result.Field15;
                var result16 = result.Field16;
                var result17 = result.Field17;
                var result18 = result.Field18;
                var result19 = result.Field19;
                lastGlassResult = result.Field20;
                var id = result.Id;
            }
            glassStopwatch.Stop();
            var glassTotalTicks = glassStopwatch.ElapsedTicks;

            double total = (double)glassTotalTicks / (double)sitecoreTotalTicks;
            Console.WriteLine("Ratio: {0}", total);
            Console.WriteLine(sitecoreTotalTicks);
            Console.WriteLine(glassTotalTicks);
            Assert.AreEqual(lastSitecoreResult, lastGlassResult);
        }

        public StubClass Map<T>(Item item)
        {
            // Assign
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            Type type = typeof(T);

                var fieldDataBuilder = new SitecoreFieldStringDataBuilder(item)
                {
                    PropertyInfo = type.GetProperty("Field"),
                    FieldName = "Field"
                };

                memberBindings.Add(fieldDataBuilder.GetMemberBinding());

                var fieldDataBuilder2 = new SitecoreIdDataBuilder(item)
                {
                    PropertyInfo = type.GetProperty("Id"),
                    FieldName = "Field"
                };
                memberBindings.Add(fieldDataBuilder2.GetMemberBinding());

            // Act

            return BuildObject<StubClass>(GetConstructorInfo(type), memberBindings);

            // Assert
        }

        public StubClassWithLotsOfProperties MapMore<T>(Item item)
        {
            // Assign
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            Type type = typeof(T);

            for (var i = 1; i < 21; i++)
            {
                var fieldDataBuilder = new SitecoreFieldStringDataBuilder(item)
                {
                    PropertyInfo = type.GetProperty(String.Format("Field{0}", i)),
                    FieldName = "Field"
                };

                memberBindings.Add(fieldDataBuilder.GetMemberBinding());
            }

            var fieldDataBuilder2 = new SitecoreIdDataBuilder(item)
            {
                PropertyInfo = type.GetProperty("Id"),
                FieldName = "Field"
            };

            memberBindings.Add(fieldDataBuilder2.GetMemberBinding());

            var fieldDataBuilder3 = new SitecoreUrlDataBuilder(item)
            {
                PropertyInfo = type.GetProperty("Url"),
                FieldName = "Url"
            };

            memberBindings.Add(fieldDataBuilder3.GetMemberBinding());
            // Act

            return BuildObject<StubClassWithLotsOfProperties>(GetConstructorInfo(type), memberBindings);

            // Assert
        }

        private ConstructorInfo GetConstructorInfo(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            return constructors.FirstOrDefault();
        }

        private Dictionary<Type, object> compiledFuncs = new Dictionary<Type, object>(); 

        public T BuildObject<T>(ConstructorInfo constructor, List<MemberBinding> membersBindings)
        {
            var type = typeof (T);
            if (compiledFuncs.ContainsKey(type))
            {
                var compiledFunc = compiledFuncs[type] as Func<T>;
                return compiledFunc();
            }

            ParameterInfo[] paramsInfo = constructor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            var argsExp = new Expression[paramsInfo.Length];

            // Create a typed expression with each arg from the parameter array
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            NewExpression newExp = Expression.New(constructor, argsExp);
            Expression testExpression = Expression.MemberInit(
                newExp,
                membersBindings);


            //create a lambda with the New Expression as the body and our param object[] as arg
            var func = Expression.Lambda<Func<T>>(testExpression).Compile();
            compiledFuncs.Add(type, func);

            // return the compiled activator
            return func();
        }

    }

    public class StubClass
    {
        [SitecoreField(Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field { get; set; }

        [SitecoreId]
        public virtual Guid Id { get; set; }
    }

    [SitecoreType]
    public class StubClassWithLotsOfProperties
    {
        public string Url { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field1 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field2 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field3 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field4 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field5 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field6 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field7 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field8 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field9 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field10 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field11 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field12 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field13 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field14 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field15 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field16 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field17 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field18 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field19 { get; set; }

        [SitecoreField("Field", Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string Field20 { get; set; }


        [SitecoreId]
        public virtual Guid Id { get; set; }
    }

    public class SitecoreFieldStringDataBuilder : DataBuilder<string>
    {
        private readonly Item item;

        public SitecoreFieldStringDataBuilder(Item item)
        {
            this.item = item;
        }

        public override string GetValue()
        {
            return item[FieldName];
        }
    }

    public class SitecoreIdDataBuilder : DataBuilder<Guid>
    {
        private readonly Item item;

        public SitecoreIdDataBuilder(Item item)
        {
            this.item = item;
        }

        public override Guid GetValue()
        {
            return item.ID.ToGuid();
        }
    }

    public class SitecoreUrlDataBuilder : DataBuilder<string>
    {
        private readonly Item item;

        public SitecoreUrlDataBuilder(Item item)
        {
            this.item = item;
        }

        public override string GetValue()
        {
            return LinkManager.GetItemUrl(item);
        }
    }

    public abstract class DataBuilder<T>
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string FieldName { get; set; }

        public MemberBinding GetMemberBinding()
        {
            return Expression.Bind(PropertyInfo,
                Expression.Constant(GetValue()));
        }

        public abstract T GetValue();
    }
}
