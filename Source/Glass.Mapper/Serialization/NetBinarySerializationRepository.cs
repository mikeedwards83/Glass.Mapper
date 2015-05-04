using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Glass.Mapper.Serialization
{
    public class NetBinarySerializationManager : ISerializationManager
    {
        public byte[] Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    binaryFormatter.Serialize(memoryStream, value);
                    byte[] objectDataAsStream = memoryStream.ToArray();
                    return objectDataAsStream;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public object Deserialize(byte[] stream)
        {
            if (stream == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                object result = binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        public virtual bool CanSerialize(object value)
        {
            Type objectType = value.GetType();

            if (objectType.IsValueType)
            {
                return false;
            }

            if (objectType == typeof (string))
            {
                return false;
            }

            if (objectType.IsClass && !objectType.IsSerializable)
            {
                return false;
            }

            return true;
        }

        public virtual bool CanDeserialize(object value)
        {
            return value.GetType() == typeof(byte[]);
        }
    }
}
