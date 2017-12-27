using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HireATutor.Filters;
using HireATutor.Models;
using Microsoft.AspNet.Identity;

namespace HireATutor.Controllers.api
{
    public class InstructorsController : ApiController
    {
        // GET api/<controller>
        GigContext gigContext = GigContext.Create();

        [Authorize(Roles = "Instructor")]
        public Instructor Get()
        {
            string id = User.Identity.GetUserId();
            ApplicationDbContext appDbContext = new ApplicationDbContext();
            
            Instructor instructor = new Instructor();
            instructor.Gigs = gigContext.Gigs.Include("Packages").Include("Category").Where(x => x.instructor_Id == id).ToList();

            return instructor;
        }
    }
}