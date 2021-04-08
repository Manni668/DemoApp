using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
