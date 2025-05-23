using System;

namespace Remcom.Api.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int? NoticeId { get; set; }
        public int? WarrantOfArrestId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaidAt { get; set; }
        public int PaidByUserId { get; set; }
        public string? Comment { get; set; }
    }
}
