using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AntsyProject_285.Data;
using AntsyProject_285.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AntsyProject_285.Controllers
{
    [Route("api/publicinfo")]
    [ApiController]
    public class ProfileInfoController : ControllerBase
    {
        private readonly DataContext dataContext;
        public ProfileInfoController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        private static Expression<Func<PublicInfo, PublicInfoDTO>> MappingOfEntityToDTO()
        {
            return x => new PublicInfoDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Service = x.Service,
                Description = x.Description,
            };
        }

        [HttpGet]
        public IEnumerable<PublicInfoDTO> ReturnAll()
        {
            return dataContext.Set<PublicInfo>().Select(MappingOfEntityToDTO()).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<PublicInfoDTO> GetById(int id)
        {
            var data = dataContext.Set<PublicInfo>().Where(x => x.Id == id).Select(MappingOfEntityToDTO()).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }
            return data;
        }

        [HttpPost]
        public ActionResult<PublicInfoDTO> Create(PublicInfoDTO target)
        {
            var data = dataContext.Set<PublicInfo>().Add(new PublicInfo
            {
                FullName = target.FullName,
                Email = target.Email,
                PhoneNumber = target.PhoneNumber,
                Service = target.Service,
                Description = target.Description,
            });

            dataContext.SaveChanges();
            //return 200
            return Ok(new PublicInfoDTO
            {
                FullName = target.FullName,
                Email = target.Email,
                PhoneNumber = target.PhoneNumber,
                Service = target.Service,
                Description = target.Description,
            });
        }

        [HttpPut("{id}")]
        public ActionResult<PublicInfoDTO> Update(int id, PublicInfoDTO target)
        {
            var data = dataContext.Set<PublicInfo>().FirstOrDefault(x => x.Id == id);
            data.FullName = target.FullName;
            data.Email = target.Email;
            data.PhoneNumber = target.PhoneNumber;
            data.Service = target.Service;
            data.Description = target.Description;

            dataContext.SaveChanges();
            //return 200
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<PublicInfoDTO> Delete(int id)
        {
            var data = dataContext.Set<PublicInfo>().FirstOrDefault(x => x.Id == id);
            dataContext.Set<PublicInfo>().Remove(data);
            //Return 200
            return Ok();
        }
    }
}