﻿using HackerCentral.Models;
using HackerCentral.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HackerCentral.Controllers
{
    /// <summary>
    /// Show information stored by the tracking system
    /// </summary>
    public class TrackingController : TrackedController
    {
        [HttpGet]
        public ActionResult Touch()
        {
            using (SimpleContext dbContext = new HackerCentralContext(this))
            {
                dbContext.Messages.Add(new Message());

                return View();
            }
        }

        [HttpGet]
        public ActionResult Data(bool isLimited = true, int numToShow = 10)
        {
            using(SimpleContext dbContext = new HackerCentralContext(this))
            {
                List<ActionTrack> actionTracks;

                if(isLimited)
                {
                    actionTracks = dbContext.ActionTracks
                        .OrderByDescending(a => a.TimeStamp)
                        .Include(a => a.SaveTracks.Select(s => s.EntityTracks))
                        .Include(a => a.SaveTracks.Select(s => s.UserEntityTrack))
                        .Include(a => a.SaveTracks.Select(s => s.FieldTracks.Select(f => f.Entity)))
                        .Take(numToShow).ToList();
                }
                else
                {
                    actionTracks = dbContext.ActionTracks
                        .OrderByDescending(a => a.TimeStamp)
                        .Include(a => a.SaveTracks.Select(s => s.EntityTracks))
                        .Include(a => a.SaveTracks.Select(s => s.UserEntityTrack))
                        .Include(a => a.SaveTracks.Select(s => s.FieldTracks.Select(f => f.Entity)))
                        .ToList();
                }

                var model = new TrackingViewModel()
                {
                    IsLimited = isLimited,
                    NumActionsShown = actionTracks.Count(),
                    ActionTracks = new List<ActionTrackViewModel>(actionTracks.Count)
                };

                foreach (var actionTrack in actionTracks)
                {
                    model.ActionTracks.Add(new ActionTrackViewModel(actionTrack));
                }

                return View(model);
            }
        }
    }
}
