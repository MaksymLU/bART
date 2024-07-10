using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using bART_Test.Models;
using bART_Test.Data;
using System.ComponentModel.DataAnnotations;

namespace bART_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly MyDbContext _context;

        public IncidentController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody] IncidentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = await _context.Accounts
                .Include(a => a.Contacts)
                .FirstOrDefaultAsync(a => a.Name == request.AccountName);

            if (account == null)
                return NotFound("Account not found");

            Contact? contact = account.Contacts.FirstOrDefault(c => c.Email == request.ContactEmail);
            if (contact == null)
            {
                contact = new Contact
                {
                    FirstName = request.ContactFirstName,
                    LastName = request.ContactLastName,
                    Email = request.ContactEmail,
                    Account = account
                };
                _context.Contacts.Add(contact);
            }
            else
            {
                contact.FirstName = request.ContactFirstName;
                contact.LastName = request.ContactLastName;
            }

            var incident = new Incident
            {
                IncidentName = Guid.NewGuid().ToString(),
                Description = request.IncidentDescription,
                Account = account
            };
            _context.Incidents.Add(incident);

            await _context.SaveChangesAsync();

            return Ok(new { IncidentName = incident.IncidentName });
        }
    }

    public class IncidentRequest
    {
        [Required]
        public string? AccountName { get; set; }

        [Required]
        public string? ContactFirstName { get; set; }

        [Required]
        public string? ContactLastName { get; set; }

        [Required]
        [EmailAddress]
        public string? ContactEmail { get; set; }

        [Required]
        public string? IncidentDescription { get; set; }
    }
}
