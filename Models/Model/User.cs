using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public class User : IdentityUser
    {
        public User()
        {
            Employee = new HashSet<Employee>();
        }

        public ICollection<Employee> Employee { get; set; }
    }
}
