using Glass.Mapper.Serialization;

namespace Glass.Mapper.Caching
{
    public abstract class AbstractSerializationCacheManager : AbstractCacheManager
    {
        protected ISerializationManager SerializationManager { get; private set; }

        protected AbstractSerializationCacheManager(ISerializationManager serializationManager)
        {
            SerializationManager = serializationManager;
        }

        protected virtual object GetSerializingValue(object value)
        {
            object valueToUse = value;
            if (SerializationManager.CanSerialize(value))
            {
                valueToUse = SerializationManager.Serialize(value);
            }
            return valueToUse;
        }

        protected virtual object GetDeserializingValue(object value)
        {
            byte[] valueToUse = value as byte[];
            if (valueToUse != null && SerializationManager.CanDeserialize(valueToUse))
            {
                return SerializationManager.Deserialize(valueToUse);
            }
            
            return value;
        }
    }
}
