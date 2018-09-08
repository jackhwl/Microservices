using System.Collections.Generic;
using AspNetSecurity_m2.Models;
using AspNetSecurity_m2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSecurity_m2.Controllers.API
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
