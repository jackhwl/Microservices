using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController: Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet("{cityid}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);
                var pointsOfInterestForCityResults =
                    Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);
                return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
        
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{cityid}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);
            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityid}/pointsofinterest")]
        public IActionResult CreatePointsOfInterest(int cityid, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null) return BadRequest();
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
            if (city == null) return NotFound();

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            //201
            return CreatedAtRoute("GetPointOfInterest", new
                { cityId = cityid, id = finalPointOfInterest.Id}, finalPointOfInterest);
        }

        [HttpPut("{cityid}/pointsofinterest/{id}")]
        public IActionResult UpdatePointsOfInterest(int cityid, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null) return BadRequest();
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null) return NotFound();

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent(); //204
        }

        [HttpPatch("{cityid}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityid, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
            if (city == null) return NotFound();
            
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null) return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent(); //204
        }

        [HttpDelete("{cityid}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityid, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null) return NotFound();

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            return NoContent();
        }

    }
}
