using Microsoft.AspNetCore.Identity;

namespace ShawkanyDb.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string ImagePath { get; set; }

    }
}
