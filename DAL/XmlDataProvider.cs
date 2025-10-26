using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public class XmlDataProvider<T> : IDataProvider<T> where T : class
    {
        public void Serialize(IEnumerable<T> data, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public IEnumerable<T> Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (TextReader reader = new StreamReader(filePath))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }
    }
}
