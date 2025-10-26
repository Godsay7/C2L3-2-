using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class JsonDataProvider<T> : IDataProvider<T> where T : class
    {
        public void Serialize(IEnumerable<T> data, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, jsonString);
        }

        public IEnumerable<T> Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>(); // Повернути порожній список, якщо файлу немає

            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
    }
}
