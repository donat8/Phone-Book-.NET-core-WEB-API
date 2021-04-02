using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdressBook.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Address cannot contain more than 100 characters")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public List<string> TelephoneNumbers { get; set; }
      
    }
}
