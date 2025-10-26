using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using MessagePack; 


namespace DAL
{
    [MessagePackObject]
    [Union(0, typeof(Student))]
    [Union(1, typeof(Painter))]
    [Union(2, typeof(Farmer))]
    public abstract class Person
    {
        [MessagePack.Key(0)]
        public string LastName { get; set; }

        [MessagePack.Key(1)]
        public string FirstName { get; set; }

        [MessagePack.Key(2)]
        public DateTime BirthDate { get; set; }

        [MessagePack.Key(3)]
        public bool CanSwim { get; set; }
    }
}
