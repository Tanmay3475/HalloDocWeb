


using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HalloDocWeb.ViewModels
{
    public class ConciergeViewModel
    {
        [Required]
        public string Conciergename { get; set; } = null!;
        [Required]
        public string firstName { get; set; } = null!;
        [Required]
        public string? lastName { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
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
        public string Street { get; set; }=null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string State { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        public string? Room { get; set; }
       
    }
}
