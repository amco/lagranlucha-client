using Newtonsoft.Json;
using JsonApiSerializer;
using CFLFramework.Data;

namespace CFLFramework.API
{
    public class WebRequestResponse
    {
        #region FIELDS

        private const int ErrorsResponseCodeStart = 400;
        private const int HostErrorResponseCode = 0;
        private const string HostErrorMessage = "Could not connect to host";
        private const string ToStringFormat = "Response code: {0}\nRaw response: {1}\nError: {2}";

        private JsonApiSerializerSettings settings = null;

        #endregion

        #region PROPERTIES

        public long Code { get; private set; } = 0;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Undefined;
        public Errors Errors { get; private set; } = null;
        public string RawResponse { get; private set; } = "";
        public bool Succeeded { get { return Errors == null; } }

        #endregion

        #region CONSTRUCTORS

        public WebRequestResponse(IWebRequest response)
        {
            Code = response.ResponseCode;
            RawResponse = response.RawResponse;

            try
            {
                StatusCode = (HttpStatusCode)this.Code;
                if (Code == HostErrorResponseCode)
                    Errors = new Errors(HostErrorMessage);
                else if (Code >= ErrorsResponseCodeStart)
                    JsonConvert.PopulateObject(RawResponse, Errors = new Errors());
            }
            catch
            {
                StatusCode = HttpStatusCode.Undefined;
            }

            settings = new JsonApiSerializerSettings();
            settings.Converters.Add(new GenericConverter());
        }

        #endregion

        #region BEHAVIORS

        public override string ToString()
        {
            return string.Format(ToStringFormat, Code, RawResponse, Errors);
        }

        public T Response<T>()
        {
            return JsonConvert.DeserializeObject<T>(RawResponse, settings);
        }

        public T[] ResponseArray<T>()
        {
            return JsonConvert.DeserializeObject<T[]>(RawResponse, settings);
        }

        public void PopulateObject<T>(T jsonObject)
        {
            JsonConvert.PopulateObject(RawResponse, jsonObject);
        }

        #endregion
    }
}
