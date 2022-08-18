using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace VSSystem.Extensions.Hosting.Extenstions
{
    static partial class SessionExtension
    {
        public static void Set(this ISession session, string key, object obj)
        {

            try
            {
                string jsonObj = JsonConvert.SerializeObject(obj);
                session.SetString(key, jsonObj);
            }
            catch { }
        }
        public static T Get<T>(this ISession session, string key)
        {

            try
            {
                string jsonObj = session.GetString(key);
                if (!string.IsNullOrWhiteSpace(jsonObj))
                {
                    T result = JsonConvert.DeserializeObject<T>(jsonObj);
                    return result;
                }
            }
            catch { }
            return default;
        }
    }

}