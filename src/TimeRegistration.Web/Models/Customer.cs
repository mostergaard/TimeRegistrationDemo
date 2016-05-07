using System.Collections.Generic;

namespace TimeRegistration.Web.Models
{
    public class Customer
    {
        public string Name { get; set; }

        public IList<string> Projects { get; set; }
    }
}
