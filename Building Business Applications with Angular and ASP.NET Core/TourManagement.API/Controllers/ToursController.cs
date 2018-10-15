using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [HttpPost]
        [RequestHeaderMatchesMediaType("Accept", new [] {"application/vnd.marvin.tourforcreation+json"})]
        public async Task<IActionResult> AddTour([FromBody] TourForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of the DTO happens here

            return await AddSpecificTour(tour);
        }  

        [HttpPost]
        [RequestHeaderMatchesMediaType("Accept", new [] {"application/vnd.marvin.tourwithmanagerforcreation+json"})]
        public async Task<IActionResult> AddTourWithManager([FromBody] TourWithManagerForCreation tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }

            // validation of the DTO happens here

            return await AddSpecificTour(tour);
        }

        private async Task<IActionResult> GetSpecificTour<T>(Guid tourId) where T : class
        {
            var tourFromRepo = await _tourManagementRepository.GetTour(tourId);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            var tour = Mapper.Map<T>(tourFromRepo);

            return Ok(tour);
        }

        private async Task<IActionResult> AddSpecificTour<T>(T tour) where T : class
        {
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

    }
}
