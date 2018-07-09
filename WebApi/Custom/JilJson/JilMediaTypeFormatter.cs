using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jil;

namespace WebApi.Custom.JilJson
{
    public class JilMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly MediaTypeHeaderValue _applicationJsonMediaType = new MediaTypeHeaderValue("application/json");
        private static readonly MediaTypeHeaderValue _textJsonMediaType = new MediaTypeHeaderValue("text/json");
        private static readonly Task<bool> _done = Task.FromResult(true);

        private static readonly Options _options;

        static JilMediaTypeFormatter()
        {
            _options = new Options(excludeNulls: false, includeInherited: true,
                dateFormat: DateTimeFormat.ISO8601);
        }

        public JilMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(_applicationJsonMediaType);
            SupportedMediaTypes.Add(_textJsonMediaType);

            SupportedEncodings.Add(new UTF8Encoding(false, true));
            SupportedEncodings.Add(new UnicodeEncoding(false, true, true));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(this.DeserializeFromStream(type, readStream));
        }


        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    MethodInfo method =
                        typeof(JSON).GetMethod("Deserialize", new Type[] {typeof(TextReader), typeof(Options)});
                    MethodInfo generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[] {reader, _options});
                }
            }
            catch (TargetException e)
            {
                return null;
            }
            catch (ArgumentException e)
            {
                return null;
            }
            catch (TargetInvocationException e)
            {
                return null;
            }
            catch (TargetParameterCountException e)
            {
                return null;
            }
            catch (MethodAccessException e)
            {
                return null;
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
            catch (NotSupportedException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value, streamWriter, _options);
            streamWriter.Flush();
            return Task.FromResult(writeStream);
        }
    }
}