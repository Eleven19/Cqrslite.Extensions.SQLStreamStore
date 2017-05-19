using System;

namespace CQRSlite.Extensions.SQLStreamStore
{
    public interface ISerializationFacade
    {
        //TODO: Consider whether or not we need to provide functionality for specifying what format it is searlizing to
        // for now we are assuming JSON
        string Serialize<T>(T value);

        string Serialize(object value, Type type);
        T Deserialize<T>(string value);
        object Deserialize(string value, Type type);
    }
}