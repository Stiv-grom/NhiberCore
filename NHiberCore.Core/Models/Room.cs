using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NHiberCore.Core.Models
{
    public class Room
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Number { get; set; }
        public int Floor { get; set; }
        public bool HasTV { get; set; }
        public bool HasPhone { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
