using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProBus.Models
{
    public class CustomerModel
    {
        public int CustomerKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDay { get; set; }
        public string Gender { get; set; } 
        public string EmailAddress { get; set; }
    }

    public class CustomerV1Model
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public int Phone { get; set; }
    }
}
