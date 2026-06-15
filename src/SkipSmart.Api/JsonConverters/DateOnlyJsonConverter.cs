using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkipSmart.Api.JsonConverters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly> {
    private readonly string _format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString();
        return DateOnly.ParseExact(value!, _format, null);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString(_format));
    }
}