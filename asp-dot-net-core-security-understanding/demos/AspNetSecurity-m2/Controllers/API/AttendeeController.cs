using AspNetSecurity_m2.Models;
using AspNetSecurity_m2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSecurity_m2.Controllers.API
{
    [Route("api/[controller]")]
    public class AttendeeController: Controller
    {
        private readonly AttendeeRepo repo;

        public AttendeeController(AttendeeRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public AttendeeModel Get(int id)
        {
            return repo.GetById(id);
        }

        [HttpPost]
        public IActionResult Post(int conferenceId, string name)
        {
            var attendee = repo.Add(new AttendeeModel {ConferenceId = conferenceId, Name = name});
            return new CreatedAtActionResult("Get", "Attendee", new {id = attendee.Id}, attendee);
        }
    }
}
