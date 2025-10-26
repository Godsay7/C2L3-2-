using System;
using MessagePack;

namespace DAL
{
    public class Farmer : Person
    {

        [Key(4)] 
        public string FarmLocation { get; set; }
    }
}
