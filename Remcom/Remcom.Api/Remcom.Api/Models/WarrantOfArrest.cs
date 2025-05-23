using System;

namespace Remcom.Api.Models
{
    public class WarrantOfArrest
    {
        public int Id { get; set; }
        public required string WoaNumber { get; set; }
        public required string OffenceDetails { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal AmountDue { get; set; }
        public required string Status { get; set; } // e.g. Outstanding, Executed
        public required string LinkedDriverId { get; set; }
        public required string LinkedVehicleReg { get; set; }
        public DateTime? ExecutedAt { get; set; }
        public int? ExecutedByUserId { get; set; }
        public string? ExecutionComment { get; set; }
    }
}
