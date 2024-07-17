using ApiExamples.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiExamples.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
