using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class ExpenceType
    {
        public ExpenceType()
        {
            Expence = new HashSet<Expence>();
        }

        public int ExpenceTypeId { get; set; }
        public string ExpenceType1 { get; set; }

        public virtual ICollection<Expence> Expence { get; set; }
    }
}
