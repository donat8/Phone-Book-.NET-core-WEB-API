using AdressBook.Helpers;
using AdressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdressBook.Repositories
{
    public interface IContactRepository
    {
        //get all contacts
        Task<PagedList<Contact>> Get(UrlQueryParameters urlQueryParameters);

        //get contact by id
        Task<Contact> Get(int id);

        //Create a contact
        Task<Contact> Create(Contact contact);

        //Update a contact
        Task Update(Contact contact);

        //delete a contact
        Task Delete(int id);

        Task<Contact> FindName(string name);

        Task<Contact> FindAddress(string address);

    }
}
