﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Serialization;
using Offwind.OpenFoam.Sintef;
using Offwind.WebApp.Controllers;
using Offwind.WebApp.Infrastructure;
using Offwind.WebApp.Models;
using Offwind.WebApp.Models.Jobs;

namespace Offwind.WebApp.Areas.CFD.Controllers
{
    public class ProcessingController : __BaseCfdController
    {
        public ProcessingController()
        {
            rootPath = "C:\\work\\temp"; // just for tests, take value from Web.config
            SectionTitle = "Processing";
        }

        public ActionResult Settings()
        {
            ShortTitle = "Settings";
            return View();
        }

        public ActionResult Simulation()
        {
            ShortTitle = "Simulation";
            var dCase = GetCase();
            var caseHasJob = dCase.CurrentJobId != null;
            bool jobActive = false;
            if (caseHasJob)
            {
                var dJob = ctx.DJobs.FirstOrDefault(dj => dj.Id == dCase.CurrentJobId);
                jobActive = (dJob != null) && (dJob.State != JobState.Idle.ToString());
            }
            ViewBag.IsInProgress = jobActive;
            return View();
        }

        public FileResult SimulationPreview()
        {
            var dCase = GetCase();
            var jobZip = CreateJobPath(User.Identity.Name, dCase.Id);
            var jobPath = jobZip.Replace(".zip", "");
            var solverData = GetSolverData();
            solverData.MakeJobFS(jobPath);
            SharpZipUtils.CompressFolder(jobPath, jobZip, null);

            return File(jobZip, "application/octet-stream", "preview.zip");
        }

        public JsonResult SimulationStart()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Started = DateTime.UtcNow,
                Owner = User.Identity.Name,
                Name = StandardCases.CfdCase,
                State = JobState.Started,                
            };

            var jobZip = CreateJobPath(job.Owner, job.Id);
            var jobPath = jobZip.Replace(".zip", "");
            
            var solverData = GetSolverData();            
            solverData.MakeJobFS(jobPath);
            SharpZipUtils.CompressFolder(jobPath, jobZip, null);

            new JobsController().AddJobManually(job);

            SetCaseJob(job.Id);
            return Json(job.Id);
        }

        public JsonResult SimulationStop()
        {
            var dCase = GetCase();
            if (dCase.CurrentJobId == null) return Json("Already idle");
            new JobsController().SetJobFinished(dCase.CurrentJobId.Value);
            return Json("Simulation stopped");
        }

        public static string CreateJobPath(string owner, Guid id)
        {
            string path = WebConfigurationManager.AppSettings["UsersDir"];
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, owner);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, id.ToString());
            path += ".zip";
            return path;
        }

        public static string CreateTestJobPath()
        {
            string path = WebConfigurationManager.AppSettings["UsersDir"];
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, "test");
            path += ".zip";
            return path;
        }

        public ActionResult History()
        {
            ShortTitle = "History";
            var jobs = ctx.DJobs
                .Where(dj => dj.Owner == User.Identity.Name && dj.Name == StandardCases.CfdCase)
                .OrderByDescending(dj => dj.Started);
            return View(jobs);
        }

        public ActionResult ClearHistory()
        {
            var state = JobState.Idle.ToString();
            var jobs = ctx.DJobs.Where(dj =>
                dj.Owner == User.Identity.Name
                && dj.Name == StandardCases.CfdCase
                && dj.State == state
                );
            foreach (var dJob in jobs)
            {
                ctx.DeleteObject(dJob);
            }
            ctx.SaveChanges();
            return RedirectToAction("History");
        }

        public ActionResult ClearAllJobs()
        {
            var jobs = ctx.DJobs.Where(dj =>
                dj.Owner == User.Identity.Name
                && dj.Name == StandardCases.CfdCase
                );
            foreach (var dJob in jobs)
            {
                ctx.DeleteObject(dJob);
            }
            ctx.SaveChanges();
            return RedirectToAction("History");
        }
    }
}