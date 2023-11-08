using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceShop.Dto;
using SpaceShop.Interfaces;
using SpaceShop.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace SpaceShop.Controllers
{
    [Authorize]
    public class CityController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CityController(IUnitOfWork uow, IMapper mapper)
        {
            //throw new UnauthorizedAccessException();
            this.uow = uow;
            this.mapper = mapper;
        }

        // Get all the cities : GET api/city/GetCities

        [AllowAnonymous]
        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await uow.CityRepository.GetCitiesAsync();

            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);

            //var citiesDto = from c in cities
            //                select new CityDto()
            //                {
            //                    Id = c.Id,
            //                    Name = c.Name,
            //                };

            return Ok(citiesDto);
        }

        // Add a city : POST api/city/add

        [HttpPost("add")]
        public async Task<IActionResult> AddCity(CityDto citydto)
        {
            var city = mapper.Map<City>(citydto);
            city.LastUpdatedOn = DateTime.Now;
            city.LastUpdateedBy = 1;

            uow.CityRepository.AddCity(city);
            await uow.SaveAsync();

            return StatusCode(201);
        }

        // Update a city: PUT api/city/update/2     ** Updates the complete object

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityDto citydto)
        {
            if (id != citydto.Id)
                return BadRequest("Update Not Allowed.");

            var cityFromDb = await uow.CityRepository.FindCity(id);

            if (cityFromDb == null)
                return BadRequest("Update Not Allowed.");

            cityFromDb.LastUpdateedBy = 1;
            cityFromDb.LastUpdatedOn = DateTime.Now;

            mapper.Map(citydto, cityFromDb);
            await uow.SaveAsync();

            return StatusCode(200);

        }

        // Update a city: PUT api/city/update/2     ** Updates only Name component of the city 

        [HttpPut("updateCityName/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityNameUpdateDto citydto)
        {
            var cityFromDb = await uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdateedBy = 1;
            cityFromDb.LastUpdatedOn = DateTime.Now;

            mapper.Map(citydto, cityFromDb);
            await uow.SaveAsync();

            return StatusCode(200);
        }


        // Update only a component of a city: PATCH api/city/update/2

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> cityDtoPatch)
        {
            var cityFromDb = await uow.CityRepository.FindCity(id);
            cityFromDb.LastUpdateedBy = 1;
            cityFromDb.LastUpdatedOn = DateTime.Now;

            cityDtoPatch.ApplyTo(cityFromDb, ModelState);
            await uow.SaveAsync();

            return StatusCode(200);
        }

        // Delete a city: DELETE api/city/delete/4

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            uow.CityRepository.DeleteCity(id);
            await uow.SaveAsync();

            return Ok(id);
        }
    }
}
