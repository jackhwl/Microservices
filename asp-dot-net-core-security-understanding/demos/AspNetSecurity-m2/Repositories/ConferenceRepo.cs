using System;
using System.Collections.Generic;
using System.Linq;
using AspNetSecurity_m2.Models;
using Microsoft.AspNetCore.DataProtection;

namespace AspNetSecurity_m2.Repositories
{
    public class ConferenceRepo
    {
        private readonly List<ConferenceModel> conferences = new List<ConferenceModel>();
        private readonly IDataProtector protector;
        public ConferenceRepo(IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants)
        {
            this.protector = protectionProvider.CreateProtector(purposeStringConstants.ConferenceIdQueryString);
            conferences.Add(new ConferenceModel { Id = 1, Name = "NDC", EncryptedId = protector.Protect("1"), Location = "Oslo", Start = new DateTime(2017, 6, 12) });
            conferences.Add(new ConferenceModel { Id = 2, Name = "IT/DevConnections", EncryptedId = protector.Protect("2"), Location = "San Francisco", Start = new DateTime(2017, 10, 18) });
        }

        public IEnumerable<ConferenceModel> GetAll()
        {
            return conferences;
        }

        public ConferenceModel GetById(int id)
        {
            return conferences.First(c => c.Id == id);
        }

        public void Add(ConferenceModel model)
        {
            model.Id = conferences.Max(c => c.Id) + 1;
            model.EncryptedId = protector.Protect(model.Id.ToString());
            conferences.Add(model);
        }
    }
}
