using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sympli.Project.Tracking.Domains.Responses
{
    public class Response<T>
    {
        [JsonPropertyName("Data")]
        public T Data { get; set; }

        [JsonPropertyName("StatusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }

        public Response()
        {
        }

        public Response(ModelStateDictionary modelState)
        {
            Dictionary<string, string> dictionary =
                modelState.Where<KeyValuePair<string, ModelStateEntry>>((KeyValuePair<string, ModelStateEntry> x) => x.Value.Errors.Count > 0)
                .ToDictionary((KeyValuePair<string, ModelStateEntry> keyValuePair) => keyValuePair.Key, (KeyValuePair<string, ModelStateEntry> keyValuePair) => keyValuePair.Value.Errors.Select((ModelError e) => e.ErrorMessage).First());
            Message = string.Join('\n', dictionary.Values);
            StatusCode = HttpStatusCode.BadRequest;
            Data = (T)Convert.ChangeType(dictionary, typeof(T));
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}