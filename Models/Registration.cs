using System;
using System.ComponentModel.DataAnnotations;

namespace events_managments.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        public string RegistrantName { get; set; }

        [Required, EmailAddress]
        public string RegistrantEmail { get; set; }

        // Foreign key to Event
        public int EventId { get; set; }
        public Event Event { get; set; }

        // Date/time of registration
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // New property for RSVP status
        public bool IsAttending { get; set; } = false;
    }
}
