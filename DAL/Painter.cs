using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace DAL
{
    public class Painter : Person
    {

        [Key(4)] 
        public string Style { get; set; }

    }
}
