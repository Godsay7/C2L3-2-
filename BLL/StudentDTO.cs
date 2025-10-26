using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class StudentDTO
    {
        public string FullName { get; set; }
        public int Course { get; set; }
        public string StudentId { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
