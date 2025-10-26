using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EntityContext<T> where T : class
    {
        private readonly IDataProvider<T> _provider;
        private readonly string _filePath;

        public EntityContext(IDataProvider<T> provider, string filePath)
        {
            _provider = provider;
            _filePath = filePath;
        }

        public IEnumerable<T> LoadData()
        {
            try
            {
                return _provider.Deserialize(_filePath);
            }
            catch (FileNotFoundException)
            {
                return new List<T>(); // OK, якщо файлу ще немає
            }
            catch (DirectoryNotFoundException)
            {
                // Спробуємо створити директорію
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                return new List<T>();
            }
            catch (Exception ex)
            {
                // "Відмінно": BLL має обробити цей виняток
                // Ми загортаємо виняток DAL у специфічний для BLL
                // (Хоча тут це можна зробити і в BLL)
                // Для простоти, BLL буде обробляти стандартні винятки
                throw; // Передаємо виняток на рівень BLL
            }
        }

        public void SaveData(IEnumerable<T> data)
        {
            try
            {
                _provider.Serialize(data, _filePath);
            }
            catch (Exception ex)
            {
                // Передаємо виняток на рівень BLL
                throw;
            }
        }
    }
}
