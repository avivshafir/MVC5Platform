﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVC5Platform.Models;

namespace MVC5Platform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return View(HttpContext.Application["events"]);
            return View(GetTimeStamps());
        }

        private List<string> GetTimeStamps()
        {
            return new List<string>
            {
                string.Format("App timestamp: {0}",
                    HttpContext.Application["app_timestamp"]),
                string.Format("Request timestamp: {0}", Session["request_timestamp"]),
            };
        }


        [HttpPost]
        public ActionResult Index(Color color)
        {
            Color? oldColor = Session["color"] as Color?;
            if (oldColor != null)
            {
                Votes.ChangeVote(color, (Color) oldColor);
            }
            else
            {
                Votes.RecordVote(color);
            }
            ViewBag.SelectedColor = Session["color"] = color;
            //return View(HttpContext.Application["events"]);
            return View(GetTimeStamps());
        }

        public ActionResult Modules()
        {
            var modules = HttpContext.ApplicationInstance.Modules;
            Tuple<string, string>[] data =
                modules.AllKeys
                    .Select(x => new Tuple<string, string>(
                        x.StartsWith("__Dynamic") ? x.Split('_', ',')[3] : x,
                        modules[x].GetType().Name))
                    .OrderBy(x => x.Item1)
                    .ToArray();
            return View(data);
        }
    }
}