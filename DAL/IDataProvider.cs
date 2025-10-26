using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDataProvider<T> where T : class
    {
        void Serialize(IEnumerable<T> data, string filePath);
        IEnumerable<T> Deserialize(string filePath);
    }
}
