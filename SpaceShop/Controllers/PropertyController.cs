using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceShop.Dto;
using SpaceShop.Interfaces;

namespace SpaceShop.Controllers
{ 
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public PropertyController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("type/{sellRent}")]
        public async Task<IActionResult> GetPropertyList(int sellRent)
        {
            var properties = await uow.PropertyRepository.GetPropertiesAsync(sellRent);

            var propertyListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties);

            return Ok(propertyListDto);
        }
    }
}
