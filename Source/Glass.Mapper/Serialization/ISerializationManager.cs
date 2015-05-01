namespace Glass.Mapper.Serialization
{
    public interface ISerializationManager
    {
        byte[] Serialize(object value);

        object Deserialize(byte[] stream);

        bool CanSerialize(object value);

        bool CanDeserialize(object value);
    }
}
