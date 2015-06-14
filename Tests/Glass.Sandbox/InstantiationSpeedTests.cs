using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Glass.Sandbox.ObjectCreation;

namespace Glass.Sandbox
{
    [TestFixture]
    public class InstantiationSpeedTests
    {
        [Test]
        public void CreateObject()
        {
            //Arrange
            var stopWatch1 = new Stopwatch();
            var stopWatch2 = new Stopwatch();
            var stopWatch3 = new Stopwatch();
            var stopWatch4 = new Stopwatch();
            var stopWatch5 = new Stopwatch();
            var stopWatch6 = new Stopwatch();
            var stopWatch7 = new Stopwatch();

            var type = typeof (Stub);
            var lamda = LamdaMethod(typeof(Stub));
            var jitted = LamdaMethod(typeof (Stub));
            var constructor = type.GetConstructors()[0];

            var il = IlMethod(typeof(Stub));
            var builder = new ExpressionBuilder(type);
            var builderFunc = builder.Build();

            //Act

            stopWatch1.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj1 = new Stub();
            }
            stopWatch1.Stop();

            stopWatch2.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj2 =Activator.CreateInstance<Stub>();
            }
            stopWatch2.Stop();

            stopWatch3.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj3 = lamda();
            }
            stopWatch3.Stop();

            stopWatch4.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj4 = il.Invoke(null,null);
            }
            stopWatch4.Stop();

            stopWatch5.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj5 = jitted();
            }
            stopWatch5.Stop();


            stopWatch6.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj6 = FormatterServices.GetUninitializedObject(type);
                constructor.Invoke(obj6, null);
            }
            stopWatch6.Stop();

            stopWatch7.Start();
            for (int i = 0; i < 50000; i++)
            {
                var obj7 = builderFunc(null);
            }
            stopWatch7.Stop();


            //Assert
            Console.WriteLine("Raw: {0} Activator: {1} Lambda: {2} IL: {3} Jit: {4} Constructor: {5}, Builder: {6}", stopWatch1.ElapsedTicks, stopWatch2.ElapsedTicks, stopWatch3.ElapsedTicks, stopWatch4.ElapsedTicks, stopWatch5.ElapsedTicks, stopWatch6.ElapsedTicks, stopWatch7.ElapsedTicks);

        }



        private DynamicMethod IlMethod(Type type)
        {
            var method = new DynamicMethod("", typeof (object), Type.EmptyTypes);
            var il = method.GetILGenerator();


            var ctor = type.GetConstructor(Type.EmptyTypes);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            return method;
        }

        static Func<object> LamdaMethod(Type type)
        {
            return Expression.Lambda<Func<object>>(
                Expression.Convert(Expression.New(type), typeof(object)))
                .Compile();
        }
        public class Stub { }

        public Func<object> Jitted(Type type)
        {
            Func<object> func = Expression.Lambda<Func<object>>(
                Expression.Convert(Expression.New(type), typeof (object))).Compile();

            JitCompile(func);
            return func;
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void JitCompile(Delegate del)
        {
            RuntimeHelpers.PrepareDelegate(del);
        }
    }
}
