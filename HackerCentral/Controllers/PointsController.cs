﻿using HackerCentral.Accessors;
using HackerCentral.Models;
using HackerCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HackerCentral.Controllers
{
    [Authorize]
    public class PointsController : Controller
    {
        public ActionResult Index(string message = "")
        {
            var pa = new PointAccessor();
            var model = new PointsViewModel()
            {
                points = pa.GetAllPoints(),
                point = new Point()
                {
                    id = 0,
                    summary = "",
                    full_text = "",
                    category = 0
                }
            };

            ViewBag.Message = message;
            return View("Index", model);
        }

        public ActionResult Create(Point p)
        {
            var pa = new PointAccessor();
            if (pa.CreatePoint(p))
                return Index("Point successfully created");
            else
                return Index("Point not successfully created");
        }

        public ActionResult Destroy(long id)
        {
            var pa = new PointAccessor();
            if (pa.DestroyPoint(id))
                return Index("Point successfully deleted");
            else
                return Index("Point did not delete successfully");
        }

        public ActionResult Edit(Point p)
        {
            var pa = new PointAccessor();
            if (pa.UpdatePoint(p))
                return Index("Point successfully updated");
            else
                return Index("Point was not successfully updated");
        }

        public PartialViewResult EditForm(long id)
        {
            var pa = new PointAccessor();
            return PartialView("_EditPoint", pa.GetPoint(id));
        }
    }


}
