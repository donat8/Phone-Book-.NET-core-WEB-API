using AdressBook.Helpers;
using AdressBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdressBook.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AdressBookContext _context;
        public ContactRepository(AdressBookContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Contact> Create(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
           
            return contact;
        }

        public async Task Delete(int id)
        {
            var contactToDelete = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contactToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Contact> Get(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }
    
        public async Task<PagedList<Contact>> Get(UrlQueryParameters urlQueryParameters)
        {
            var source= _context.Contacts.AsQueryable();
            return PagedList<Contact>.ToPagedList(source.OrderBy(c=>c.Name), urlQueryParameters.PageNumber, urlQueryParameters.PageSize);
        }

        public async Task Update(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Contact> FindAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            return await _context.Contacts.FirstOrDefaultAsync(c => c.Address==address);
        }

        public async Task<Contact> FindName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
     
            return await _context.Contacts.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
