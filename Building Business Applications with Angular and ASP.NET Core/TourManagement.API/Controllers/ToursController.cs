using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Primitives;
using TourManagement.API.Dtos;
using TourManagement.API.Helpers;
using TourManagement.API.Services;

namespace TourManagement.API.Controllers
{
    [Route("api/tours")]
    public class ToursController : Controller
    {
        private readonly ITourManagementRepository _tourManagementRepository;

        public ToursController(ITourManagementRepository tourManagementRepository)
        {
            _tourManagementRepository = tourManagementRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTours()
        {
            var toursFromRepo = await _tourManagementRepository.GetTours();

            var tours = Mapper.Map<IEnumerable<Tour>>(toursFromRepo);
            return Ok(tours);
        }


        //[HttpGet("{tourId}", Name = "GetTour")]
        //public async Task<IActionResult> GetTour(Guid tourId)
        //{
        //    var tourFromRepo = await _tourManagementRepository.GetTour(tourId);

        //    if (tourFromRepo == null)
        //    {
        //        return BadRequest();
        //    }

        //    var tour = Mapper.Map<Tour>(tourFromRepo);

        //    return Ok(tour);
        //}        

        [HttpGet("{tourId}")]
        public async Task<IActionResult> GetDefaultTour(Guid tourId)
        {
            if (Request.Headers.TryGetValue("Accept", out StringValues values))
            {
                Debug.WriteLine($"Accept header(s): {string.Join(",", values)}");
            }
            return await GetSpecificTour<Tour>(tourId);
        }  

        [HttpGet("{tourId}", Name="GetTour")]
        [RequestHeaderMatchesMediaType("Accept", new [] {"application/vnd.marvin.tour+json"})]
        public async Task<IActionResult> GetTour(Guid tourId)
        {
            return await GetSpecificTour<Tour>(tourId);
        }  

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new [] {"application/vnd.marvin.tourwithestimatedprofits+json"})]
        public async Task<IActionResult> GetTourWithEstimatedProfits(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfits>(tourId);
        }  

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new [] {"application/vnd.marvin.tourwithshows+json"})]
        public async Task<IActionResult> GetTourWithShows(Guid tourId)
        {
            return await GetSpecificTour<TourWithShows>(tourId, true);
        }

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new [] {"application/vnd.marvin.tourwithestimatedprofitsandshows+json"})]
        public async Task<IActionResult> GetTourWithEstimatedProfitsAndShows(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfitsAndShows>(tourId, true);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new [] {"application/json", "application/vnd.marvin.tourforcreation+json"})]
        public async Task<IActionResult> AddTour([FromBody] TourForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of the DTO happens here
            if (!(tour.StartDate < tour.EndDate))
            {
                ModelState.AddModelError(nameof(tour), "A tour must start before it can end.");
            }

            return await AddSpecificTour(tour);
        }  

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new [] {"application/vnd.marvin.tourwithmanagerforcreation+json"})]
        public async Task<IActionResult> AddTourWithManager([FromBody] TourWithManagerForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of the DTO happens here

            return await AddSpecificTour(tour);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new [] {"application/vnd.marvin.tourwithshowsforcreation+json"})]
        public async Task<IActionResult> AddTourWithShows([FromBody] TourWithShowsForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of the DTO happens here

            return await AddSpecificTour(tour);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new [] {"application/vnd.marvin.tourwithmanagerandshowsforcreation+json"})]
        public async Task<IActionResult> AddTourWithManagerAndShows([FromBody] TourWithManagerAndShowsForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of the DTO happens here

            return await AddSpecificTour(tour);
        }

        private async Task<IActionResult> GetSpecificTour<T>(Guid tourId, bool includeShows = false) where T : class
        {
            var tourFromRepo = await _tourManagementRepository.GetTour(tourId, includeShows);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            var tour = Mapper.Map<T>(tourFromRepo);

            return Ok(tour);
        }

        private async Task<IActionResult> AddSpecificTour<T>(T tour) where T : class
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var tourEntity = Mapper.Map<Entities.Tour>(tour);

            if (tourEntity.ManagerId == Guid.Empty)
            {
                tourEntity.ManagerId = new Guid("fec0a4d6-5830-4eb8-8024-272bd5d6d2bb");
            }

            await _tourManagementRepository.AddTour(tourEntity);

            if (!await _tourManagementRepository.SaveAsync())
            {
                throw new Exception("Adding a tour failed on save.");
            }

            var tourToReturn = Mapper.Map<Tour>(tourEntity);

            return CreatedAtRoute("GetTour", new {tourId = tourToReturn.TourId}, tourToReturn);
        }

        [HttpPatch("{tourId}")]
        public async Task<IActionResult> PartiallyUpdateTour(Guid tourId,
            [FromBody] JsonPatchDocument<TourForUpdate> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var tourFromRepo = await _tourManagementRepository.GetTour(tourId);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            var tourToPatch = Mapper.Map<TourForUpdate>(tourFromRepo);
            jsonPatchDocument.ApplyTo(tourToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(tourToPatch))
            {
                return BadRequest();
            }

            Mapper.Map(tourToPatch, tourFromRepo);

            await _tourManagementRepository.UpdateTour(tourFromRepo);

            if (!await _tourManagementRepository.SaveAsync())
            {
                throw new Exception("Updating a tour failed on save.");
            }

            return NoContent();
        }
    }
}
