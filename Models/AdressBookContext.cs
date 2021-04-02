using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdressBook.Models
{
    public class AdressBookContext : DbContext
    {
        public AdressBookContext(DbContextOptions<AdressBookContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
