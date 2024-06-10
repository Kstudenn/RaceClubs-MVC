using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public Address? Address { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public ICollection<Club>? Clubs { get; set; }
        public ICollection<Race>? Races { get; set; }
    }
}
