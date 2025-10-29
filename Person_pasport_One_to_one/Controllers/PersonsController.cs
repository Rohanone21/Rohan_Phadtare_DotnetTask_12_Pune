using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Person_pasport_One_to_one.Data;
using Person_pasport_One_to_one.Models;

namespace Person_pasport_One_to_one.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PersonsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.people.Include(p => p.Passport).ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Person>> GetPeopleById(int id)
        {
            var persons=await _context.people.Include(p=>p.Passport).FirstOrDefaultAsync(s=>s.PersonId==id);

            if(persons==null)
            {
                return NotFound();
            }
            return persons;
        }
        [HttpPost]

        public async Task<ActionResult<Person>> PostPerson(Person person)
        {

            _context.people.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPeopleById", new { id = person.PersonId }, person);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> UpdatePerson(int id, [FromBody] Person updatedPerson)
        {
         


            if (id != updatedPerson.PersonId)
            {
                return BadRequest("ID not Present");
            }


            var existing=await _context.people.Include(s=>s.Passport).FirstOrDefaultAsync(s=>s.PersonId==id);
            if (existing == null)
            {
                return BadRequest("Person not Present");
            }
            existing.PersonName=updatedPerson.PersonName;

            if (updatedPerson.Passport != null)
            {
                if (existing.Passport != null)
                {
                    existing.Passport.PassportNumber = updatedPerson.Passport.PassportNumber;
                    existing.Passport.IssueDate = updatedPerson.Passport.IssueDate;
                    existing.Passport.ExpiryDate = updatedPerson.Passport.ExpiryDate;
                }
                else
                {
                    existing.Passport = new Passport
                    {
                        PersonId = id,
                        PassportNumber = updatedPerson.Passport.PassportNumber,
                        IssueDate = updatedPerson.Passport.IssueDate,
                        ExpiryDate = updatedPerson.Passport.ExpiryDate
                    };
                }


            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok(existing);

        }

        private bool PersonExists(int id)
        {
            

            return _context.people.Any(p=>p.PersonId==id);
        }

        [HttpDelete]
        public async Task<ActionResult<Person>> DeletePerson(int id)
        {
            var persons = await _context.people.Include(p => p.Passport).FirstOrDefaultAsync(p => p.PersonId == id);
            if (persons == null)
            {
                return BadRequest();
            }
            if (persons.Passport != null)
            {
                _context.passport.Remove(persons.Passport); 
            }
            _context.people.Remove(persons);
            await _context.SaveChangesAsync();
            return Ok(persons);
        }

        
    }
}
