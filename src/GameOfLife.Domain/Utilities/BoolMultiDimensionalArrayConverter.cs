using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameOfLife.Domain.Utilities
{
    public class BoolMultiDimensionalArrayConverter : JsonConverter<bool[,]>
    {
        public override bool[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<List<List<bool>>>(ref reader, options);
            if (list == null || list.Count == 0)
            {
                return new bool[0, 0];
            }
            
            int rows = list.Count;
            int cols = list[0].Count;
            bool[,] result = new bool[rows, cols];
            
            for (int i = 0; i < rows; i++)
            {
                if (list[i].Count != cols)
                    throw new JsonException("All rows must have the same number of columns.");

                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = list[i][j];
                }
            }
            
            return result;
        }

        public override void Write(Utf8JsonWriter writer, bool[,] value, JsonSerializerOptions options)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);
            
            writer.WriteStartArray();
            for (int i = 0; i < rows; i++)
            {
                writer.WriteStartArray();
                for (int j = 0; j < cols; j++)
                {
                    writer.WriteBooleanValue(value[i, j]);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}