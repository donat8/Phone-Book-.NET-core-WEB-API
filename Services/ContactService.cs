//using AdressBook.DTOs;
//using AdressBook.Extensions;
//using AdressBook.Interfaces;
//using AdressBook.Models;
//using AdressBook.Repositories;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AdressBook.Services
//{
//    public class ContactService : IContactService
//    {
//        private readonly IContactRepository _contactRepo;

//        public ContactService(IContactRepository contactRepo)
//        {
//            _contactRepo = contactRepo;
//        }

//        public async Task<GetContactResponseDto> AddAsync(GetContactResponseDto contactDto)
//        {
//            if (contactDto == null)
//                throw new ArgumentNullException(nameof(contactDto));

//            if (await _contactRepo.FindAddress(contactDto.Address) != null)
//                throw new Exception("This address already exists!");
//            if (await _contactRepo.FindName(contactDto.Name) != null)
//                throw new Exception("This name already exists!");

//            var contact = new Contact(contactDto);
//            await _contactRepo.Create(contact);
//            //???????
//            return contactDto;
//        }

//        public async Task DeleteContactAsync(Guid id)
//        {
//            var contact = await _contactRepo.Get(id);
//        }

//        public async Task<GetContactResponseDto> GetByIdAsync(Guid id)
//        {
//            if (id == Guid.Empty)
//                throw new ArgumentNullException("No contact id");

//            var contact = await _contactRepo.Get(id);
            
//            return new GetContactResponseDto {
//                Id = contact.ContactId,
//                Name = contact.Name,
//                DateOfBirth = contact.DateOfBirth,
//                Address = contact.Address,
//                TelephoneNumbers = contact.TelephoneNumbers.ToList()
//            };
//        }

//        public async Task<GetContactListResponseDto> GetByPageAsync(int limit, int page, CancellationToken cancellationToken)
//        {
//            var contacts = await _contactRepo.Contacts
//                           .AsNoTracking()
//                           .OrderBy(p => p.Name)
//                           .PaginateAsync(page, limit, cancellationToken);

//            return new GetContactListResponseDto
//            {

//                // PRILAGODITI
//                CurrentPage = contacts.CurrentPage,
//                TotalPages = contacts.TotalPages,
//                TotalItems = contacts.TotalItems,
//                Items = contacts.Items.Select(p => new GetContactResponseDto
//                {
//                    Id = p.ContactId,
//                    Name = p.Name,
//                    DateOfBirth = p.DateOfBirth,
//                    Address=p.Address,
//                    TelephoneNumbers=p.TelephoneNumbers.ToList()
//                }).ToList()
//            };
//        }

//        public async Task UpdateContactAsync(GetContactResponseDto contactDto)
//        {
//            if (contactDto == null)
//                throw new ArgumentNullException(nameof(contactDto));

//            var contact = new Contact(contactDto);
//            await _contactRepo.Update(contact);
//        }
//    }
//}
