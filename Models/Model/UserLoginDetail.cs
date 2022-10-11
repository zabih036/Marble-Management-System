using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class UserLoginDetail
    {
        public int DetailId { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string UserId { get; set; }
    }
}
