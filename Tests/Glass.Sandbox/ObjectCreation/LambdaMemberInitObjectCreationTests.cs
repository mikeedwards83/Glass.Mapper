using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace Glass.Sandbox.ObjectCreation
{
    [TestFixture]
    public class LambdaMemberInitObjectCreationTests : ObjectCreationBase
    {
        private readonly Dictionary<Type, object> compiledFuncs = new Dictionary<Type, object>(); 

        [Test]
        public void CreateNewObject()
        {
            // Assign

            // Act
            TestObject(GetStubClass);

            // Assert
        }

        [Test]
        public void CreateNewObjectLots()
        {
            // Assign

            // Act
            TestObjectLots(GetStubClass);

            // Assert
        }

        protected StubClass GetStubClass(SourceItem item)
        {
            var destinationType = typeof (StubClass);
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            PropertyInfo propertyInfo = destinationType.GetProperty("Property1");

            ParameterExpression instanceParameter = Expression.Parameter(typeof (StubClass), "instance");
            ParameterExpression valueParameter = Expression.Parameter(typeof (Func<string>), "value");


            ParameterExpression instanceParameter2 = Expression.Parameter(typeof (StubClass), "instance");
            var propertyExpression2 = Expression.Property(instanceParameter2, propertyInfo);

            Expression<Action<StubClass, Func<string>>> lambda = Expression.Lambda<Action<StubClass, Func<String>>>(
                Expression.Assign(
                    Expression.Property(instanceParameter, propertyInfo), valueParameter
                    ));



            Action<StubClass, Func<string>> action = lambda.Compile();


            //Action<SourceItem> dataMapper = (i) =>
            //{
            //    var r = i.Property1;
            //    action()
            //};


           


            //memberBindings.Add(Expression.Bind(info2, ));
            //ConstructorInfo[] constructors = destinationType.GetConstructors();
            //var constructorInfo = constructors.FirstOrDefault();
            //var objectToUse =  BuildObject<StubClass>(constructorInfo, memberBindings);
            //action(item, objectToUse);
            return null;

        }

        public T BuildObject<T>(ConstructorInfo constructor, List<MemberBinding> membersBindings)
        {
            var type = typeof(T);
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
}
