using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdressBook.Hubs;
using AdressBook.Models;
using AdressBook.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace AdressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;

        private readonly IHubContext<ContactsHub> _signalrHub;
 
        public ContactsController(IContactRepository repository, IHubContext<ContactsHub> signalrHub)
        {
            _repository = repository;
            _signalrHub = signalrHub;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts(
            [FromQuery] Models.UrlQueryParameters urlQueryParameters)
        {
            var contacts = await _repository.Get(urlQueryParameters);
            var metadata = new
            {
                contacts.TotalCount,
                contacts.PageSize,
                contacts.CurrentPage,
                contacts.HasNext,
                contacts.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            var contact= await _repository.Get(id);

            if (contact == null)
                return NotFound("No contact with given id is found");

            return Ok(contact);
        }

        //CREATE
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
            if (contact == null)
                return BadRequest("No contact specified");
            if (await _repository.FindAddress(contact.Address) != null)
                return BadRequest("This address already exists!");
            if (await _repository.FindName(contact.Name) != null)
                return BadRequest("This name already exists!");
            if (await _repository.Get(contact.ContactId) != null)
                return BadRequest("This id already exists, try again");
            await _repository.Create(contact);
            await _signalrHub.Clients.All.SendAsync("LoadContacts");
            return Ok(contact);
        }

        //UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Contact contact)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
            if (id != contact.ContactId)
                return BadRequest("Wrong id for updating");
            if (await _repository.FindAddress(contact.Address) != null)
                return BadRequest("This address already exists!");
            if (await _repository.FindName(contact.Name) != null)
                return BadRequest("This name already exists!");

            await _repository.Update(contact);
            await _signalrHub.Clients.All.SendAsync("LoadContacts");
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contactToDelete = await _repository.Get(id);
            if (contactToDelete == null)
                return NotFound("Id given for deletion is not found");

            await _repository.Delete(id);
            await _signalrHub.Clients.All.SendAsync("LoadContacts");
            return Ok("Contact is deleted");
        }

        //public BadRequestObjectResult CheckNameAndAddress(Contact contact)
        //{
        //    if ( _repository.FindAddress(contact.Address) != null)
        //        return BadRequest("This address already exists!");
        //    if ( _repository.FindName(contact.Name) != null)
        //        return BadRequest("This name already exists!");
        //    return null;
        //}
    }
}
