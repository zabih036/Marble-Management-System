using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Hdealer
    {
        public Hdealer()
        {
            Hdealler = new HashSet<Hdealler>();
        }

        public int HdealerId { get; set; }
        public string Hdealer1 { get; set; }
        public string Mobile { get; set; }

        public virtual ICollection<Hdealler> Hdealler { get; set; }
    }
}
