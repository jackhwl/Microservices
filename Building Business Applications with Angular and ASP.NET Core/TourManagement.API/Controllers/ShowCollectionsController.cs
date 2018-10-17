using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TourManagement.API.Dtos;
using TourManagement.API.Helpers;
using TourManagement.API.Services;

namespace TourManagement.API.Controllers
{
    [Route("api/tours/{tourId}/showcollections")]
    public class ShowCollectionsController : Controller
    {
        private readonly ITourManagementRepository _tourManagementRepository;
        public ShowCollectionsController(ITourManagementRepository tourManagementRepository)
        {
            _tourManagementRepository = tourManagementRepository;
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new [] {"application/json", "application/vnd.marvin.showcollectionforcreation+json"})]
        public async Task<IActionResult> CreateShowCollection(Guid tourId,
            [FromBody] IEnumerable<ShowForCreation> showCollection)
        {
            if (showCollection == null)
            {
                return BadRequest();
            }

            if (!await _tourManagementRepository.TourExists(tourId))
            {
                return NotFound();
            }

            var showEntities = Mapper.Map<IEnumerable<Entities.Show>>(showCollection);
            foreach (var show in showEntities)
            {
                await _tourManagementRepository.AddShow(tourId, show);
            }

            if (!await _tourManagementRepository.SaveAsync())
            {
                throw new Exception("Adding a collection of shows failed on save.");
            }

            return Ok();
        }
    }
}
