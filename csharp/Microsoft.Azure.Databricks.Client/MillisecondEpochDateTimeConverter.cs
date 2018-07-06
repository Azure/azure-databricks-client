using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class MillisecondEpochDateTimeConverter : DateTimeConverterBase
    {
        internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ms;

            switch (value)
            {
                case DateTime dateTime:
                    ms = (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
                    break;
                case DateTimeOffset dateTimeOffset:
                    ms = (long)(dateTimeOffset.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
                    break;
                default:
                    throw new JsonSerializationException("Expected date object value.");
            }

            if (ms < 0)
            {
                throw new JsonSerializationException("Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970.");
            }

            writer.WriteValue(ms);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var time = UnixEpoch.AddMilliseconds((long)reader.Value);

            if (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?))
            {
                return new DateTimeOffset(time, TimeSpan.Zero);
            }

            return time;
        }
    }
}
