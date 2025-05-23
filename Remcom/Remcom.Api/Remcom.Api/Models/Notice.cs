using System;

namespace Remcom.Api.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public required string NoticeNumber { get; set; }
        public required string OffenceDetails { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal AmountDue { get; set; }
        public required string Status { get; set; } // e.g. Outstanding, Paid
        public required string LinkedDriverId { get; set; } // South African Driver ID
        public required string LinkedVehicleReg { get; set; } // Vehicle Registration
    }
}
