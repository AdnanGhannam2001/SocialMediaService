using System.Text.Json;
using System.Text.Json.Serialization;
using PR2.Shared.Common;

namespace SocialMediaService.WebApi.JsonConverters;

public sealed class ExceptionBaseConverter : JsonConverter<ExceptionBase>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(ExceptionBase).IsAssignableFrom(typeToConvert);
    }

    public override void Write(Utf8JsonWriter writer, ExceptionBase? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            return;
        }
        
        writer.WriteStartObject();
            writer.WritePropertyName(nameof(ExceptionBase.PropertyName));
            writer.WriteStringValue(value.PropertyName);

            writer.WritePropertyName(nameof(ExceptionBase.ErrorCode));
            writer.WriteStringValue(value.ErrorCode);

            writer.WritePropertyName(nameof(ExceptionBase.Message));
            writer.WriteStringValue(value.Message);
        writer.WriteEndObject();
    }

    // Would not be used
    public override ExceptionBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}