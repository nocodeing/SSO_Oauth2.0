using System.Web.Script.Serialization;

namespace CommonTools
{
    public static class SerializerHelper
    {
        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public static string Serializer(this object obj)
        {
            return _serializer.Serialize(obj);
        }

        public static T Deserialize<T>(this string json)
        {
            return _serializer.Deserialize<T>(json);
        }
    }
}
