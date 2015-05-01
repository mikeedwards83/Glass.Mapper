using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    [TestFixture]
    public class InterfaceSerializationFixture
    {
        private CreateInterfaceTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new CreateInterfaceTask();
        }

        [Test]
        [Ignore("This is a failing test showing deserialization issues.")]
        public void CanSerializeAndDeserializeGeneratedInterface()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType = typeof(IStubInterface);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            //Act
            _task.Execute(args);
            byte[] serialized = Serialize(args.Result);
            /*SerializationInfo info = new SerializationInfo(typeof(IStubInterface),);
            StreamingContext context = new StreamingContext();
            ProxyObjectReference pr = new ProxyObjectReference(info, new StreamingContext() )*/
            IStubInterface deserialized = Deserialize<IStubInterface>(serialized);


            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.IsAborted);
            Assert.IsTrue(args.Result is IStubInterface);
            Assert.IsFalse(args.Result.GetType() == typeof(IStubInterface));
        }


        private static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        private static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        public interface IStubInterface : ISerializable
        {

        }
    }
}
