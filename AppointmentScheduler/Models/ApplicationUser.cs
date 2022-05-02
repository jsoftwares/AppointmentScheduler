using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Models
{
    [Required]
    public class ApplicationUser : IdentityUser
    {
    }
}
