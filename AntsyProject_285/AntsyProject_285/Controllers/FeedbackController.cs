using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AntsyProject_285.Critique;
using AntsyProject_285.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AntsyProject_285.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly DataContext dataContext;
        public FeedbackController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        private static Expression<Func<Feedback, FeedbackDTO>> MappingOfEntityDTO()
        {
            return x => new FeedbackDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Comments = x.Comments,
            };
        }

        [HttpGet]
        public IEnumerable<FeedbackDTO> ReturnAll()
        {
            return dataContext.Set<Feedback>().Select(MappingOfEntityDTO()).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<FeedbackDTO> GetById(int id)
        {
            var data = dataContext.Set<Feedback>().Where(x => x.Id == id).Select(MappingOfEntityDTO()).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }
            return data;
        }

        [HttpPost]
        public ActionResult<FeedbackDTO> Create(FeedbackDTO target)
        {
            var data = dataContext.Set<Feedback>().Add(new Feedback
            {
                FullName = target.FullName,
                Email = target.Email,
                Comments = target.Comments,
            });

            dataContext.SaveChanges();
            //Return 200
            return Ok(new FeedbackDTO
            {
                FullName = target.FullName,
                Email = target.Email,
                Comments = target.Comments,
            });
        }

        [HttpPut("{id}")]
        public ActionResult<FeedbackDTO> Update(int id, FeedbackDTO target)
        {
            var data = dataContext.Set<Feedback>().FirstOrDefault(x => x.Id == id);
            data.FullName = target.FullName;
            data.Email = target.Email;
            data.Comments = target.Comments;

            dataContext.SaveChanges();
            //Return 200
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<FeedbackDTO> Delete(int id)
        {
            var data = dataContext.Set<Feedback>().FirstOrDefault(x => x.Id == id);
            dataContext.Set<Feedback>().Remove(data);
            //Return 200
            return Ok();
        }
    }
}