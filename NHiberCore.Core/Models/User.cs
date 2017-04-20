using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NHiberCore.Core.Models
{
    public class User
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public Room Room { get; set; }
    }
}
