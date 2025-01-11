using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        public RegionsController(NzWalksDbContext dbContext, IRegionRepository _regionRepository)
        {
            regionRepository = _regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionDomainModelList = await regionRepository.GetAllAsync();
            List<RegionDto> regionDto = new();
            if (regionDomainModelList.Count > 0 && regionDomainModelList != null)
            {
                foreach (var region in regionDomainModelList)
                {
                    regionDto.Add(new RegionDto
                    {
                        Id = region.Id,
                        Code = region.Code,
                        Name = region.Name,
                        RegionImageUrl = region.RegionImageUrl
                    });
                }
            }
            return Ok(regionDto);
        }

        [HttpGet]
        [Route("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var regionDomainModel = await regionRepository.GetByIdAsync(Id);
            if (regionDomainModel == null)
                return NotFound();
            RegionDto regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (addRegionRequestDto == null)
                return BadRequest();

            // Converting DTO model to Domain model
            Region regionDomainModel = new()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            // Mapping Domain model back to Dto
            RegionDto regionDto = new()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            Region? regionDomainModel = new()
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };
            if (regionDomainModel == null)
                return NotFound();

            regionDomainModel = await regionRepository.UpdateAsync(Id, regionDomainModel);
            if (regionDomainModel == null)
                return NotFound();

            RegionDto regionDto = new()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            Region? regionDomainModel = await regionRepository.DeleteAsync(Id);
            if (regionDomainModel == null)
                return NotFound();

            RegionDto regionDto = new()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}