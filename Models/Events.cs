using System;
using System.ComponentModel.DataAnnotations;

namespace events_managments.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public bool IsPrivate { get; set; }
        public string? AccessCode { get; set; }
        public string? ImagePath { get; set; }
    }
}
