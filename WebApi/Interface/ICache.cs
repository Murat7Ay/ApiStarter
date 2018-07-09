using System;

namespace WebApi.Interface
{
    internal interface ICache
    {
        object GetValue(string key);
        bool Add(string key, object value, DateTimeOffset absExpiration);
        void Delete(string key);
        void Set(string key, object value, DateTimeOffset absExpiration);
    }
}