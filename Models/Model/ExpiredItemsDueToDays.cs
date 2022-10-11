using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class ExpiredItemsDueToDays
    {
        public int Id { get; set; }
        public int? Days { get; set; }
    }
}
