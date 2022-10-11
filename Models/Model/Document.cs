using System;
using System.Collections.Generic;

namespace ShawkanyDb.Models.Model
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public string DocumentDetails { get; set; }
        public string DocumentPicture { get; set; }
    }
}
