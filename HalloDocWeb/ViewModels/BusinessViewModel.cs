using System.ComponentModel.DataAnnotations;

namespace HalloDocWeb.ViewModels
{
    public class BusinessViewModel
    {
        [Required]
        public string firstName { get; set; } = null!;
        [Required]
        public string? lastName { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Casenumber { get; set; }
        public string? Symptoms { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string? LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Street { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        public string? Room { get; set; }

    }
}
