using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Glass.Sandbox.ObjectCreation
{

    [TestFixture]
    public class ExpressionBuilderFixture
    {
        [Test]
        public void BasicBuild()
        {
            ExpressionBuilder builder = new ExpressionBuilder(typeof(Stub));
            var mapper1 = new DataMapper();
            var mapper2 = new DataMapper2();
            var propertyInfo1 = typeof(Stub).GetProperty("Property1");
            var propertyInfo2 = typeof(Stub).GetProperty("Property2");

            builder.AddMemberBinding(propertyInfo1, mapper1);
            builder.AddMemberBinding(propertyInfo2, mapper2);
            var func = builder.Build();

            var args1 = new Args();
            args1.Value = "Some value";
            var result1 = func(args1) as Stub;

            var args2 = new Args();
            args2.Value = "Some value other";
            var result2 = func(args2) as Stub;

            Assert.AreEqual(args1.Value, result1.Property1);
            Assert.AreEqual("Hello World", result1.Property2);
            Assert.AreEqual(args2.Value, result2.Property1);

        }

    }

    public class ExpressionBuilder
    {
        private readonly Type _type;

        private List<MemberBinding> _bindings;
        private ParameterExpression _argsExpression;


        public ExpressionBuilder(Type type)
        {
            _type = type;
            _bindings = new List<MemberBinding>();
            _argsExpression = Expression.Parameter(typeof (Args), "args");
        }


        public void AddMemberBinding(PropertyInfo propertyInfo, DataMapper  target)
        {
            var methodInfo = target.GetType().GetMethod("DoWork");
            var callExpression = Expression.Call(Expression.Constant(target), methodInfo, _argsExpression);
            var memberBinding = Expression.Bind(propertyInfo, callExpression);
            _bindings.Add(memberBinding);

        }

        public MemberInitExpression CreateConstructor()
        {
            var constructorInfo = _type.GetConstructors()[0];
            var exNew = Expression.New(constructorInfo);
            return Expression.MemberInit(exNew, _bindings);
        }

        public Func<Args, object> Build()
        {
            return Expression.Lambda<Func<Args, object>>(
                CreateConstructor(), _argsExpression
                ).Compile();
        }
    }


   

    public class DataMapper
    {

        public virtual string DoWork(Args args)
        {
            return args.Value;
        }
    }

    public class DataMapper2 :DataMapper
    {

        public virtual string DoWork(Args args)
        {
            return "hello world";
        }
    }

    public class Args
    {
        public string Value { get; set; }
    }

    public class Stub
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
