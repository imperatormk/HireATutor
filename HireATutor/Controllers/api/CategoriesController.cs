using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HireATutor.Filters;
using HireATutor.Models;

namespace HireATutor.Controllers.api
{
    public class CategoriesController : ApiController
    {
        // GET api/<controller>
        GigContext gigContext = GigContext.Create();

        public IEnumerable<Category> Get()
        {
            
            return gigContext.Categories.ToList();
        }

        // GET api/<controller>/5
        public Category Get(int id)
        {
            return gigContext.Categories.Include("Gigs.Instructor").Include("Gigs.Packages").Where(c => c.catId == id).ToList().First();
        }

        // POST api/<controller>
        [Authorize(Roles = "Instructor")]
        public HttpStatusCode Post([FromBody]Category category)
        {
            gigContext.Categories.Add(category);

            if (gigContext.SaveChanges() == 1)
            {
                return HttpStatusCode.Created;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        // PUT api/<controller>/5
        [Authorize(Roles = "Instructor")]
        public HttpStatusCode Put(int id, [FromBody]Category category)
        {
            if (gigContext.Categories.Where(c => c.catId == id).ToList().Count == 1)
            {
                Category catOld = gigContext.Categories.Where(c => c.catId == id).ToList().First();
                catOld = category;
                if (gigContext.SaveChanges() == 1)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = "Instructor")]
        public HttpStatusCode Delete(int id)
        {
            if (gigContext.Categories.Where(c => c.catId == id).ToList().Count == 1)
            {
                gigContext.Categories.Remove(gigContext.Categories.Where(c => c.catId == id).ToList().First());
                if (gigContext.SaveChanges() == 1)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }
    }
}