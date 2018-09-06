using System.Collections.Generic;
using AspNetSecurity_m1.Models;
using AspNetSecurity_m1.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSecurity_m1.Controllers.API
{
    [EnableCors("AllowBankCom")]
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
