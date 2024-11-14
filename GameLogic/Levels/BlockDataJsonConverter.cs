using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlocksConsole.GameLogic.Levels
{
    internal class BlockDataJsonConverter : JsonConverter<BlockData>
    {
        public override BlockData ReadJson(
            JsonReader reader,
            Type objectType,
            BlockData existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            var array = JArray.Load(reader);
            int x = array[0].Value<int>();
            int y = array[1].Value<int>();
            return new BlockData(x, y);
        }

        public override void WriteJson(
            JsonWriter writer,
            BlockData value,
            JsonSerializer serializer
        )
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteEndArray();
        }
    }
}
