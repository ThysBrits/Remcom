using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remcom.Api.Data;
using Remcom.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Remcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticesController : ControllerBase
    {
        private readonly RemcomDbContext _context;
        public NoticesController(RemcomDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? idNumber, [FromQuery] string? vehicleReg)
        {
            if (string.IsNullOrWhiteSpace(idNumber) && string.IsNullOrWhiteSpace(vehicleReg))
                return BadRequest("Please provide an ID Number or Vehicle Registration Number.");

            // Search Notices
            var noticeQuery = _context.Notices.AsQueryable();
            if (!string.IsNullOrWhiteSpace(idNumber))
                noticeQuery = noticeQuery.Where(n => n.LinkedDriverId == idNumber);
            if (!string.IsNullOrWhiteSpace(vehicleReg))
                noticeQuery = noticeQuery.Where(n => n.LinkedVehicleReg == vehicleReg);
            var noticeResults = await noticeQuery.ToListAsync();
            var noticeDtos = noticeResults.Select(n => new {
                Source = "Notice",
                Id = n.Id,
                NoticeNumber = n.NoticeNumber,
                OffenceDetails = n.OffenceDetails,
                DateIssued = n.DateIssued,
                AmountDue = n.AmountDue,
                Status = n.Status,
                LinkedDriverId = n.LinkedDriverId,
                LinkedVehicleReg = n.LinkedVehicleReg
            });

            // Search Warrants of Arrest
            var warrantQuery = _context.WarrantsOfArrest.AsQueryable();
            if (!string.IsNullOrWhiteSpace(idNumber))
                warrantQuery = warrantQuery.Where(w => w.LinkedDriverId == idNumber);
            if (!string.IsNullOrWhiteSpace(vehicleReg))
                warrantQuery = warrantQuery.Where(w => w.LinkedVehicleReg == vehicleReg);
            var warrantResults = await warrantQuery.ToListAsync();
            var warrantDtos = warrantResults.Select(w => new {
                Source = "WoA",
                Id = w.Id,
                WoaNumber = w.WoaNumber,
                OffenceDetails = w.OffenceDetails,
                DateIssued = w.DateIssued,
                AmountDue = w.AmountDue,
                Status = w.Status,
                LinkedDriverId = w.LinkedDriverId,
                LinkedVehicleReg = w.LinkedVehicleReg
            });

            // Combine and return
            var combined = noticeDtos.Cast<object>().Concat(warrantDtos.Cast<object>()).ToList();
            return Ok(combined);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notice notice)
        {
            if (notice == null)
                return BadRequest("Notice data is required.");

            _context.Notices.Add(notice);
            await _context.SaveChangesAsync();
            return Ok(notice);
        }
    }
}
