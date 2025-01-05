using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext dbContext;

        public RegionsController(NzWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionDomainModelList = dbContext.Regions.ToList();
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
        public IActionResult GetById([FromRoute] Guid Id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == Id);
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
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
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

            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();
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
        public IActionResult Update([FromRoute] Guid Id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            Region? regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == Id);
            if (regionDomainModel == null)
                return NotFound();
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            dbContext.SaveChanges();
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
        public IActionResult Delete([FromRoute] Guid Id)
        {
            Region? regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == Id);
            if (regionDomainModel == null)
                return NotFound();
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();
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
