using System;

namespace Remcom.Api.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public required string Action { get; set; }
        public required string Details { get; set; }
    }
}
