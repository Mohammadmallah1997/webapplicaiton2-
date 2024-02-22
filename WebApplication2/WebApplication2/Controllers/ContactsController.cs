using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Controllers.Models;
using ContactsAPI.Data;
using WebApplication2.Controllers.Models;


namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;



        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task <IActionResult> GetAllContacts()
        {
             return Ok(dbContext.Contacts.ToList());

           
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact == null) {
                return NotFound("please be sure about the id ");  
            }
            return Ok(contact);
        }
        
        [HttpPost]
        public async Task <IActionResult> AddContact(AddcontactRequset addContactRequset)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequset.Address,
                Email = addContactRequset.Email,
                FullName = addContactRequset.FullName,
                PhoneNumber = addContactRequset.PhoneNumber
            };
           await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);

        }
        [HttpPut]
        [Route("{id:guid}")]
        

        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest )
        {
          var contact= await dbContext.Contacts.FindAsync(id);
            

            if(contact != null) 
            { 
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;
                contact.FullName = updateContactRequest.FullName;
                contact.PhoneNumber = updateContactRequest.PhoneNumber;
              await  dbContext.SaveChangesAsync();

                return Ok(contact);
            
            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id) 

        {
           var contact = await dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                dbContext.Remove(contact);
                dbContext.SaveChanges();
                return Ok("Deleted");
            }
            return NotFound();
        }

    }
}
