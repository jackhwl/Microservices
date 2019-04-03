using System.Collections.Generic;
using AspNetSecurity_NoSecurity.Models;
using AspNetSecurity_NoSecurity.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSecurity_NoSecurity.Controllers.API
{
    [Route("api/[controller]")]
    public class ProposalController
    {
        private readonly ProposalRepo repo;

        public ProposalController(ProposalRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IEnumerable<ProposalModel> Get(int conferenceId)
        {
            return repo.GetAllApprovedForConference(conferenceId);
        }
    }
}
