using System.IO;
using Jil;

namespace WebApi.Custom.JilJson
{
    public static class JilHelper
    {

        private static readonly Options JilOptipons =
            new Options(excludeNulls: false, serializationNameFormat: SerializationNameFormat.Verbatim,
                unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsLocal, includeInherited: true,
                dateFormat: DateTimeFormat.ISO8601);

        public static string Serialize<T>(T obj, Options options = null)

        {
            if (obj == null)
            {
                return null;
            }

            using (var output = new StringWriter())
            {
                JSON.Serialize(
                    obj,
                    output,
                    options ?? JilOptipons
                );
                return output.ToString();
            }
        }

        public static T Deserialize<T>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return default(T);
            }
            using (var input = new StringReader(jsonString))
            {
                var result = JSON.Deserialize<T>(input);
                return result;
            }
        }

        public static string SerializeDynamic(object obj, Options options = null)
        {
            if (obj == null)
            {
                return null;
            }

            using (var output = new StringWriter())
            {
                JSON.SerializeDynamic(
                    obj,
                    output,
                    options ?? JilOptipons
                );
                return output.ToString();
            }
        }

        public static dynamic DeserializeDynamic(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return null;
            }
            using (var input = new StringReader(jsonString))
            {
                var result = JSON.DeserializeDynamic(input);
                return result;
            }
        }
    }
}