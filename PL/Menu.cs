using System.Globalization;
using BLL;
using DAL;

namespace PL
{
    public class Menu
    {
        private StudentService _studentService;

        public void MainMenu()
        {
            Console.WriteLine("Ласкаво просимо до Університетської системи!");

            // "Відмінно": Отримання типу серіалізації та імені файлу від користувача
            string providerType = GetProviderType();
            string fileName = GetFileName(providerType);

            try
            {
                InitializeServices(providerType, fileName);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nКритична помилка під час ініціалізації сервісів: {ex.Message}");
                Console.WriteLine("Подальша робота неможлива. Натисніть Enter для виходу.");
                Console.ResetColor();
                Console.ReadLine();
                return;
            }

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Головне Меню ---");
                Console.WriteLine("1. Додати нового студента");
                Console.WriteLine("2. Показати всіх студентів");
                Console.WriteLine("3. **Звіт: Студенти 2-го курсу, народжені взимку**");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddNewStudent();
                        break;
                    case "2":
                        ShowAllStudents();
                        break;
                    case "3":
                        RunWinterReport();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private string GetProviderType()
        {
            while (true)
            {
                Console.WriteLine("\nОберіть тип сховища (серіалізації):");
                Console.WriteLine("1. JSON (рекомендовано)");
                Console.WriteLine("2. XML");
                Console.WriteLine("3. MessagePack ");
                Console.Write("Ваш вибір: ");
                switch (Console.ReadLine())
                {
                    case "1": return "json";
                    case "2": return "xml";
                    case "3": return "msgpack";
                    default: Console.WriteLine("Невірний вибір."); break;
                }
            }
        }

        private string GetFileName(string type)
        {
            Console.Write($"\nВведіть ім'я файлу (напр., 'students.{type}'): ");
            string file = Console.ReadLine();
            string fileName = $@"D:\Project\Files\{file}";
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = $"default_students.{type}";
                Console.WriteLine($"Використовується ім'я за замовчуванням: {fileName}");
            }
            return fileName;
        }

        private void InitializeServices(string type, string fileName)
        {
            // "Відмінно": PL вирішує, який IDataProvider використати
            IDataProvider<Student> studentProvider;

            switch (type.ToLower())
            {
                case "xml":
                    studentProvider = new XmlDataProvider<Student>();
                    break;
                case "msgpack": 
                    studentProvider = new BinaryDataProvider<Student>(); // Змінено
                    break;
                default:
                    studentProvider = new JsonDataProvider<Student>();
                    break;
            }

            // "Відмінно": Передача провайдера та шляху з PL до BLL
            _studentService = new StudentService(studentProvider, fileName);
            Console.WriteLine($"Сервіс студентів ініціалізовано з файлом '{fileName}' ({type}).");
        }

        // "Добре": Обробка виняткових ситуацій відбувається тут (на PL),
        // а не в BLL, де вони генеруються.
        private void AddNewStudent()
        {
            try
            {
                Console.WriteLine("\n--- Додавання студента ---");
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine();

                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine();

                Console.Write("Курс (1-6): ");
                int course = int.Parse(Console.ReadLine()); // Може кинути FormatException

                Console.Write("Номер студ. квитка: ");
                string studentId = Console.ReadLine();

                Console.Write("Дата народження (YYYY MM DD): ");
                DateTime birthDate = DateTime.ParseExact(Console.ReadLine(), "yyyy MM dd", CultureInfo.InvariantCulture);

                _studentService.AddStudent(lastName, firstName, course, studentId, birthDate);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Студента успішно додано та збережено!");
                Console.ResetColor();
            }
            catch (StudentValidationException ex) // Власний виняток
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Помилка валідації: {ex.Message}");
                Console.ResetColor();
            }
            catch (DataServiceException ex) // Власний виняток
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка збереження даних: {ex.Message}");
                Console.ResetColor();
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Помилка формату. Будь ласка, введіть дату (YYYY MM DD) або курс (1-6) коректно.");
                Console.ResetColor();
            }
            catch (Exception ex) // Інші непередбачені помилки
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Сталася непередбачена помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void ShowAllStudents()
        {
            Console.WriteLine("\n--- Список усіх студентів ---");
            var students = _studentService.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("Список студентів порожній.");
                return;
            }

            foreach (var s in students)
            {
                // PL використовує DTO з BLL
                Console.WriteLine($"  {s.FullName}, Курс: {s.Course}, ID: {s.StudentId}, ДН: {s.BirthDate:yyyy-MM-dd}");
            }
        }

        private void RunWinterReport()
        {
            Console.WriteLine("\n--- Звіт: 2-й курс, народжені взимку ---");
            var (count, students) = _studentService.GetSecondYearsBornInWinter();

            if (count == 0)
            {
                Console.WriteLine("Такі студенти не знайдені.");
                return;
            }

            Console.WriteLine($"Знайдено студентів: {count}");
            foreach (var s in students)
            {
                Console.WriteLine($"  {s.FullName} (ID: {s.StudentId}), ДН: {s.BirthDate:yyyy-MM-dd}");
            }
        }
    }
}
