﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
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
            ViewBag.IsInProgress = false;
            return View();
        }

        public JsonResult SimulationStart()
        {
            var solverData = GetSolverData();
            var now = DateTime.UtcNow;
            var inputData = solverData.ArchName(now.ToBinary().ToString());
            var inputFs = solverData.MakeFS();
            SharpZipUtils.CompressFolder(inputFs, inputData, null);
            Directory.Delete(inputFs, true);

            var job = new Job
            {
                Id = Guid.NewGuid(),
                Started = now,
                Owner = User.Identity.Name,
                Name = StandardCases.CfdCase,
                State = JobState.Started,
                InputData = inputData // or may be just allocate memory for zip data ?
            };

            new JobsController().AddJobManually(job);

            SetCaseJob(job.Id);
            return Json(job.Id);
        }

        public JsonResult SimulationStop()
        {
            return Json("Simulation stopped");
        }

        public static string CreateJobPath(Job job)
        {
            Contract.Requires(job != null);
            Contract.Requires(job.Id != Guid.Empty);
            Contract.Requires(job.Owner != null);
            Contract.Requires(job.Owner.Trim().Length > 0);
            Contract.Requires(job.Owner.Trim().Length == job.Owner.Length); // No pre- and post- spaces

            string path = WebConfigurationManager.AppSettings["UsersDir"];
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, job.Owner);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, job.Id.ToString());
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
    }
}