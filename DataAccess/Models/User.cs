using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
