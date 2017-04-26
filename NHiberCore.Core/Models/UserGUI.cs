using System;
using System.Collections.Generic;
using System.Text;

namespace NHiberCore.Core.Models
{
    public class UserGUI
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }

        public UserGUI(int id, string firstName, string lastName, string roomNumber)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Number = roomNumber;
        }
    }
}
