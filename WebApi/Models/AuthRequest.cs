using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class AuthRequest
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}