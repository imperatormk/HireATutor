using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using HireATutor.Filters;
using HireATutor.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace HireATutor.Controllers.api
{
    public class GigsController : ApiController
    {
        // GET api/<controller>
        GigContext gigContext = GigContext.Create();

        public IEnumerable<Gig> Get()
        {

            return gigContext.Gigs.Include("Packages").Include("Category").Include("Instructor").ToList();
        }

        // GET api/<controller>/5
        public Gig Get(int id)
        {
            List<Gig> gigs =  gigContext.Gigs.Include("Packages").Include("Category").Include("Instructor").Where(c => c.gigId == id).ToList();
            Gig gig = new Gig();

            if (gigs.Count() > 0)
            {
                gig = gigs.First();
            }

            return gig;
        }

        // POST api/<controller>
        [Authorize(Roles = "Instructor")]
        public HttpResponseMessage Post([FromBody]Gig gig)
        {
            string id = User.Identity.GetUserId();
            gig.instructor_Id = id;
            gig.active = true;

            System.Diagnostics.Debug.WriteLine("Package A desc: " + gig.PackageA.packageDesc);

            gigContext.Gigs.Add(gig);
            HttpResponseMessage response = new HttpResponseMessage();

            if (gigContext.SaveChanges() > 0)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Content = new ObjectContent(typeof(Gig), gig, new JsonMediaTypeFormatter());
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        // PUT api/<controller>/5
        [Authorize(Roles = "Instructor")]
        public HttpResponseMessage Put(int id, [FromBody]Gig gig)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            if (gigContext.Gigs.Where(c => c.gigId == id).ToList().Count == 1)
            {
                Gig gigOld = gigContext.Gigs.Where(c => c.gigId == id).ToList().First();
                gigOld = gig;


                if (gigContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.Created;
                    response.Content = new ObjectContent(typeof(Gig), gig, new JsonMediaTypeFormatter());
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = "Instructor")]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            if (gigContext.Gigs.Where(c => c.gigId == id).ToList().Count == 1)
            {
                gigContext.Gigs.Remove(gigContext.Gigs.Where(c => c.gigId == id).ToList().First());

                if (gigContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.Created;
                    response.Content = new ObjectContent(typeof(int), id, new JsonMediaTypeFormatter());
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                 response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }
    }
}