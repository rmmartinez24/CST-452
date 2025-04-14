using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using events_managments.Data;
using events_managments.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace events_managments.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Event NewEvent { get; set; }

        // File upload is accepted as a method parameter, not bound to a property.
        public async Task<IActionResult> OnPostAsync(IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Process the image file if provided
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                NewEvent.ImagePath = "/images/" + uniqueFileName;
            }

            _context.Events.Add(NewEvent);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
