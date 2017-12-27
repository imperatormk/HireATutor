using HireATutor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HireATutor.Controllers.api
{
    public class TimeframesController : ApiController
    {
        ScheduleContext scheduleContext = ScheduleContext.Create();

        public IEnumerable<Timeframe> Get()
        {
            return scheduleContext.Timeframes.ToList();
        }
    }
}
