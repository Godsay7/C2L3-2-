using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class StudentService
    {
        private readonly EntityContext<Student> _context;
        private List<Student> _students; // Кеш даних у пам'яті

        public StudentService(IDataProvider<Student> provider, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");
            }

            _context = new EntityContext<Student>(provider, filePath);

            try
            {
                _students = _context.LoadData().ToList();
            }
            catch (Exception ex)
            {
                // "Добре": Обробка винятків
                // Загортаємо виняток з DAL у власний
                throw new DataServiceException($"Failed to load student data: {ex.Message}", ex);
            }
        }

        // Метод для додавання (отримує прості типи з PL)
        public void AddStudent(string lastName, string firstName, int course, string studentId, DateTime birthDate)
        {
            // Валідація бізнес-логіки
            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
                throw new StudentValidationException("First name and last name are required.");
            if (course < 1 || course > 6)
                throw new StudentValidationException("Course must be between 1 and 6.");
            if (_students.Any(s => s.StudentId == studentId))
                throw new StudentValidationException($"Student with ID {studentId} already exists.");

            // "Відмінно": BLL створює DAL-сутність
            var newStudent = new Student
            {
                LastName = lastName,
                FirstName = firstName,
                Course = course,
                StudentId = studentId,
                BirthDate = birthDate
            };

            _students.Add(newStudent);

            // Збереження даних
            try
            {
                _context.SaveData(_students);
            }
            catch (Exception ex)
            {
                // Якщо зберегти не вдалося, відкочуємо зміни в кеші
                _students.Remove(newStudent);
                throw new DataServiceException($"Failed to save student data: {ex.Message}", ex);
            }
        }

        // Метод для отримання DTO
        public IEnumerable<StudentDTO> GetAllStudents()
        {
            // "Відмінно": BLL мапить DAL-сутність у BLL-DTO
            return _students.Select(s => new StudentDTO
            {
                FullName = $"{s.LastName} {s.FirstName}",
                Course = s.Course,
                StudentId = s.StudentId,
                BirthDate = s.BirthDate
            }).ToList();
        }

        // --- Завдання з варіанту ---
        public (int Count, List<StudentDTO> Students) GetSecondYearsBornInWinter()
        {
            int[] winterMonths = { 12, 1, 2 }; // Грудень, Січень, Лютий

            var query = _students
                .Where(s => s.Course == 2 && winterMonths.Contains(s.BirthDate.Month))
                .ToList(); // Матеріалізуємо запит

            // Мапимо в DTO
            var dtoList = query.Select(s => new StudentDTO
            {
                FullName = $"{s.LastName} {s.FirstName}",
                Course = s.Course,
                StudentId = s.StudentId,
                BirthDate = s.BirthDate
            }).ToList();

            return (dtoList.Count, dtoList);
        }
    }
}
