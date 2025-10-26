using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace DAL
{
    public class BinaryDataProvider<T> : IDataProvider<T> where T : class
    {
        private readonly MessagePackSerializerOptions _options;

        public BinaryDataProvider()
        {
            // Використовуємо ContractlessStandardResolver, щоб не вимагати атрибути
            // на всіх класах (хоча ми їх додали). LZ4 - для стиснення.
            _options = MessagePack.Resolvers.ContractlessStandardResolver.Options
                .WithCompression(MessagePackCompression.Lz4BlockArray);
        }

        public void Serialize(IEnumerable<T> data, string filePath)
        {
            byte[] bytes = MessagePackSerializer.Serialize(data, _options);
            File.WriteAllBytes(filePath, bytes);
        }

        public IEnumerable<T> Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            byte[] bytes = File.ReadAllBytes(filePath);
            return MessagePackSerializer.Deserialize<List<T>>(bytes, _options);
        }
    }
}
