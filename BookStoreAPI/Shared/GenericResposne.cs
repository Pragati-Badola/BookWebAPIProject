using System.Net;

namespace BookStoreAPI.Shared
{
        public class GenericResponse<T>
        {
            private T? _data;
            private T? _error;
            public GenericResponse()
            {
            }
            public HttpStatusCode StatusCode { get; set; }
            public string Status => StatusCode.ToString();
            public T? Data
            {
                get
                {
                    return _data;
                }
                set
                {
                    _data = value;
                    if (_data is null)
                        _data = _data.NullToEmpty();
                }
            }
            public T? Error
            {
                get
                {
                    return _error;
                }
                set
                {
                    _error = value;
                    if (_error is null)
                        _error = _error.NullToEmpty();
                }
            }
        }

        public class DefaultResponse : GenericResponse<string>
        {
            public DefaultResponse()
            {
                Data = "";
                Error = "";
            }

            public DefaultResponse(HttpStatusCode statusCode, string message)
            {
                this.StatusCode = statusCode;
            }

            public DefaultResponse(HttpStatusCode statusCode, string[] messages)
            {
                this.StatusCode = statusCode;
            }
        }

        public static class ResponseHelper
        {
            public static TObject NullToEmpty<TObject>(this TObject obj)
            {
                var propList = obj!.GetType().GetProperties().Where(x => x.PropertyType == typeof(string) && x.GetValue(obj, null) is null);
                foreach (var p in propList)
                {
                    p.SetValue(obj, string.Empty);
                }
                propList = obj.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime?) && x.GetValue(obj, null) is null);
                foreach (var p in propList)
                {
                    p.SetValue(obj, new DateTime(1, 1, 1));
                }
                return obj;
            }
        }

}
