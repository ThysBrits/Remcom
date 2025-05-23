using System;

namespace Remcom.Api.Models
{
    public class ReconciliationFile
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
        public int UploadedByUserId { get; set; }
        public required string Status { get; set; } // e.g. Processed, Pending
    }
}
