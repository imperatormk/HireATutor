using HireATutor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Net.Http.Formatting;

namespace HireATutor.Controllers.api
{
    public class SchedulesController : ApiController
    {
        ScheduleContext scheduleContext = ScheduleContext.Create();

        [Authorize]
        public IEnumerable<Schedule> Get(string id)
        {
            IEnumerable<Schedule> list = null;

            list = scheduleContext.Schedules.Include("Gig.Instructor").Where(x => x.gig.instructor_Id == id).ToList();
            return list;
        }

        [Authorize]
        public IEnumerable<Schedule> Get() // MySchedules
        {
            IEnumerable<Schedule> list = null;
            string id = User.Identity.GetUserId();

            if (User.IsInRole("Student"))
            {
                list = scheduleContext.Schedules.Include("Gig.Instructor").Include("Timeframe").Where(x => x.studentId == id).ToList();
            } else if (User.IsInRole("Instructor"))
            {
                list = scheduleContext.Schedules.Include("Gig").Include("Student").Include("Timeframe").Where(x => x.gig.instructor_Id == id).ToList();
            }
            
            return list;
        }

        // POST api/<controller>
        [Authorize(Roles = "Student")]
        public HttpResponseMessage Post([FromBody]Schedule schedule)
        {
            string id = User.Identity.GetUserId();
            schedule.studentId = id;

            scheduleContext.Schedules.Add(schedule);
            HttpResponseMessage response = new HttpResponseMessage();

            if (scheduleContext.SaveChanges() > 0)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Content = new ObjectContent(typeof(Schedule), schedule, new JsonMediaTypeFormatter());
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
