using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HireATutor.Models;
using HireATutor.Utilities;
using System.Net.Http;

namespace HireATutor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gig(int id)
        {
            Gig gig = new Gig();
            List<Timeframe> timeframes = new List<Timeframe>();

            System.Diagnostics.Debug.WriteLine("Calling API now...");
            HttpResponseMessage response = ApiUtilities.PerformApiCall<Gig>(id);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                HttpResponseMessage response1 = ApiUtilities.PerformApiCall<Timeframe>();

                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var timeframeTask = response1.Content.ReadAsAsync(timeframes.GetType());
                    timeframes = (List<Timeframe>)timeframeTask.Result;

                    ViewBag.Timeframes = timeframes;

                    var gigTask = response.Content.ReadAsAsync(gig.GetType());
                    gig = (Gig)gigTask.Result;

                    return View("GigDetails", gig);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult MySchedules()
        {
            List<Schedule> schedules = new List<Schedule>();

            System.Diagnostics.Debug.WriteLine("Calling API now...");
            HttpResponseMessage response = ApiUtilities.PerformApiCall<Schedule>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var gigTask = response.Content.ReadAsAsync(schedules.GetType());
                schedules = (List<Schedule>)gigTask.Result;

                return View("Schedules", schedules);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult AddGig()
        {
            List<Category> categories = new List<Category>();
            HttpResponseMessage response = ApiUtilities.PerformApiCall<Category>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var gigTask = response.Content.ReadAsAsync(categories.GetType());
                categories = (List<Category>)gigTask.Result;

                var categoriesSelect = categories
                .Select(x =>
                        new SelectListItem
                        {
                            Value = x.catId.ToString(),
                            Text = x.catName
                        });

                var selectList = new SelectList(categoriesSelect, "Value", "Text");

                ViewBag.Categories = categories;
                ViewBag.Title = "Create new Gig";

                return View("PersistGig");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error...");
                return HttpNotFound();
            }
        }

        public ActionResult MyGigs()
        {
            Instructor instructor = new Instructor();

            List<Gig> gigs = new List<Gig>();
            HttpResponseMessage response = ApiUtilities.PerformApiCall<Instructor>(0);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var gigTask = response.Content.ReadAsAsync(instructor.GetType());
                instructor = (Instructor)gigTask.Result;
                return View(instructor.Gigs);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error...");
                return HttpNotFound();
            }
        }

        public ActionResult Categories()
        {
            List<Category> categories = new List<Category>();
            HttpResponseMessage response = ApiUtilities.PerformApiCall<Category>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var gigTask = response.Content.ReadAsAsync(categories.GetType());
                categories = (List<Category>)gigTask.Result;
                return View(categories);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error...");
                return HttpNotFound();
            }
        }

        public ActionResult Category(int id)
        {
            Category category = new Category();
            HttpResponseMessage response = ApiUtilities.PerformApiCall<Category>(id);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var gigTask = response.Content.ReadAsAsync(category.GetType());
                category = (Category)gigTask.Result;
                return View("CategoryDetails", category);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error...");
                return HttpNotFound();
            }
        }
    }
}