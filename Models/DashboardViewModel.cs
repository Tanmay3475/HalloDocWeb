using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Models
{
    public class DashboardViewModel
    {

        public User? User { get; set; }
        public List<Request>? Requests { get; set; }
        public string? Aspuser { get; set; }
        public string Confirmation { get; set; }
        public List<Requestwisefile>? requestwisefiles { get; set; }

        enum statusName
        {
            january,
            Unassigned,
            Cancelled,
            MdEnRoute,
            MdOnSite,
            Closed,
            Clear,
            Unpaid
        }
        public int requestid { get; set; }
        public string findStatus(int status)
        {
            string sName = ((statusName)status).ToString();
            return sName;
        }
        public List<IFormFile> Filepath { get; set; }
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
        public string? Relationname { get; set; }
    }
}
