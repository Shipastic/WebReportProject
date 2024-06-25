using System.Text.Json;

namespace DAPManSWebReports.API.Services.JsonHelper
{
    public static class JsonValidator
    {
        public static bool IsValidJson(string strInput, out string validationError)
        {
            validationError = null;
            try
            {
                JsonDocument.Parse(strInput);
                return true;
            }
            catch (JsonException ex)
            {
                validationError = $"Invalid JSON: {ex.Message}";
                return false;
            }
        }
        public static bool TryDeserialize<T>(string json, out T result, out string deserializationError)
        {
            deserializationError = null;
            result = default;

            try
            {
                result = JsonSerializer.Deserialize<T>(json);
                if (result == null)
                {
                    deserializationError = "Entity is null.";
                    return false;
                }
            }
            catch (JsonException ex)
            {
                deserializationError = $"Deserialization error: {ex.Message}";
                return false;
            }
            if (result == null)
            {
                deserializationError = "Entity is null.";
                return false;
            }
            return true;
        }
    }
}
