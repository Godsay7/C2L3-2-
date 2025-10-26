using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace DAL
{
    public class Student : Person
    {
        [Key(4)] 
        public int Course { get; set; }

        [Key(5)] 
        public string StudentId { get; set; }
    }
}
