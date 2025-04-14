using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using events_managments.Data;
using events_managments.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace events_managments.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Event Event { get; set; }

        // Property to capture the access code input from the user.
        [BindProperty]
        public string InputAccessCode { get; set; }

        // Flag to indicate if the access code is verified.
        public bool CodeVerified { get; set; } = false;

        // Optional error message if access code verification fails.
        public string ErrorMessage { get; set; }

        // Property to hold the current user's registration (if any)
        public Registration UserRegistration { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (Event == null)
            {
                return NotFound();
            }
            if (!Event.IsPrivate)
            {
                CodeVerified = true;
            }
            // If user is logged in, fetch their registration
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                UserRegistration = await _context.Registrations
                    .FirstOrDefaultAsync(r => r.EventId == id && r.RegistrantEmail == userEmail);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostVerifyAsync(int id)
        {
            Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (Event == null)
            {
                return NotFound();
            }
            if (!Event.IsPrivate)
            {
                CodeVerified = true;
                return Page();
            }
            if (InputAccessCode == Event.AccessCode)
            {
                CodeVerified = true;
            }
            else
            {
                CodeVerified = false;
                ErrorMessage = "Incorrect access code. Please try again.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostToggleRSVPAsync(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Identity/LoginAndRegister");
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.EventId == id && r.RegistrantEmail == userEmail);

            if (registration == null)
            {
                TempData["Error"] = "You must register for the event before RSVPing.";
                return RedirectToPage();
            }

            registration.IsAttending = !registration.IsAttending;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = id });
        }
    }
}
