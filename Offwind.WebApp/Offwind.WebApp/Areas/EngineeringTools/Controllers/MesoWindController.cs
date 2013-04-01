﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using NUnit.Framework;
using Offwind.WebApp.Areas.EngineeringTools.Models.MesoWind;
using Offwind.WebApp.Models;
using Offwind.WebApp.Models.Account;
using log4net;

namespace Offwind.WebApp.Areas.EngineeringTools.Controllers
{
    [Authorize(Roles = SystemRole.RegularUser)]
    public class MesoWindController : _BaseController
    {
        private const string CurrentFile = "CurrentFile";
        private ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly DbSettings Settings = new DbSettings() { startLat = 0, showAll = ShowAll.yes, distance = 100 };

        public MesoWindController()
        {
            _currentGroup = "Meso Wind";
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Overview | Mesoscale Wind Characteristics | Offwind";
            return View(new VWebPage());
        }

        public ActionResult Database()
        {
            ViewBag.Title = "Database | Mesoscale Wind Characteristics | Offwind";
            ViewBag.TotalCount = _ctx.SmallMesoscaleTabFiles.Count();
            ViewBag.FnlCount = _ctx.SmallMesoscaleTabFiles.Count(t => t.DatabaseId == (int)DbType.FNL);
            ViewBag.MerraCount = _ctx.SmallMesoscaleTabFiles.Count(t => t.DatabaseId == (int)DbType.MERRA);
            return View(Settings);
        }

        [ActionName("Database")]
        [HttpPost]
        public ActionResult ApplySettings(DbSettings model)
        {
            Settings.DbType = model.DbType;
            Settings.showAll = model.showAll;
            Settings.startLat = model.startLat;
            Settings.startLng = model.startLng;
            Settings.distance = model.distance;
            return View(Settings);
        }

        public ActionResult CurrentData()
        {
            ViewBag.Title = "Current Data | Mesoscale Wind Characteristics - Offwind";
            return View(new VWebPage());
        }
        
        public ActionResult VelocityFreq()
        {
            ViewBag.Title = "Histogram | Mesoscale Wind Characteristics | Offwind";
            var m = new VWebPageSimpleObject<List<HPoint>>();
            if (Session[CurrentFile] == null)
            {
                m.SimpleObject = new List<HPoint>();
                return View(m);
            }

            var file = (string)Session[CurrentFile];
            string DbDir = WebConfigurationManager.AppSettings["MesoWindTabDir" + Settings.DbType];
            var imported = ImportFile(DbDir, file);
            m.SimpleObject = imported.VelocityFreq;
            return View(m);
        }

        public ActionResult WindRose()
        {
            ViewBag.Title = "Wind Roses | Mesoscale Wind Characteristics | Offwind";
            var model = new VWindRose();
            if (Session[CurrentFile] == null)
            {
                return View(model);
            }

            var file = (string)Session[CurrentFile];
            string DbDir = WebConfigurationManager.AppSettings["MesoWindTabDir" + Settings.DbType];
            var imported = ImportFile(DbDir, file);

            for (var i = 0; i < imported.FreqByDirs.Count; i++)
            {
                var freq = imported.FreqByDirs[i];
                var dir = i * 360 / imported.NDirs;
                model.FreqByDirs.Add(new HPoint(dir, 0, freq));
            }

            for (var i = 0; i < imported.MeanVelocityPerDir.Count; i++)
            {
                var vel = imported.MeanVelocityPerDir[i];
                var dir = i * 360/imported.NDirs;
                model.MeanVelocityPerDir.Add(new HPoint(dir, vel, 0));
            }

            return View(model);
        }

        public ActionResult Results()
        {
            return View();
        }

        public JsonResult Import(string file)
        {
            Session[CurrentFile] = file;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CurrentDataJson(int sEcho)
        {
            if (Session[CurrentFile] == null)
            {
                _log.WarnFormat("CurrentFile is not set");
                var dataEmpty = new { sEcho = sEcho, iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new List<decimal[]>() };
                return Json(dataEmpty, JsonRequestBehavior.AllowGet);
            }

            var file = (string)Session[CurrentFile];
            string DbDir = WebConfigurationManager.AppSettings["MesoWindTabDir" + Settings.DbType];
            var imported = ImportFile(DbDir, file);
            
            var final = new List<string[]>();
            var n = imported.NDirs + 1;

            var freqs = new string[n];
            freqs[0] = "Frequencies";
            for (var i = 0; i < imported.FreqByDirs.Count; i++)
            {
                freqs[i + 1] = imported.FreqByDirs[i].ToString();
            }
            final.Add(freqs);

            for (var bIndex = 0; bIndex < imported.FreqByBins.Count; bIndex++)
            {
                var bin = imported.FreqByBins[bIndex];
                var binWith13 = new string[n];
                binWith13[0] = (bIndex + 1).ToString();
                for (var i = 0; i < bin.Length; i++)
                {
                    binWith13[i + 1] = bin[i].ToString();
                }
                final.Add(binWith13);
            }

            var mean = new string[n];
            mean[0] = "Mean Vel.";
            for (var i = 0; i < imported.MeanVelocityPerDir.Count; i++)
            {
                mean[i + 1] = imported.MeanVelocityPerDir[i].ToString();
            }
            final.Add(mean);

            var data = new { sEcho = sEcho, iTotalRecords = imported.NBins, iTotalDisplayRecords = imported.NBins, aaData = final };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private VDataImport ImportFile(string dir, string fileName)
        {
            var model = new VDataImport();
            var path = System.IO.Path.Combine(dir, fileName);
            _log.InfoFormat("Importing file: {0}", path);
            using (var f = new StreamReader(path))
            {
                var lineN = 0;
                while (!f.EndOfStream)
                {
                    var line = f.ReadLine();
                    lineN++;
                    switch (lineN)
                    {
                        case 1:
                            break;
                        case 2:
                            var line2 = line.Trim().Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            _log.InfoFormat("NBins parsing: [{0}]", line);
                            model.NBins = ParseInt(line2[2]);
                            _log.InfoFormat("NBins: {0}", model.NBins);
                            break;
                        case 3:
                            var line3 = line.Trim().Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            model.NDirs = ParseInt(line3[0]);
                            _log.InfoFormat("NDirs: {0}", model.NDirs);
                            break;
                        case 4:
                            var line4 = line.Trim().Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            foreach (var s in line4)
                            {
                                model.FreqByDirs.Add(ParseDecimal(s));
                            }
                            break;
                        default:
                            var line5N = line.Trim().Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Debug.Assert(line5N.Length == model.NDirs + 1); // 1st cell contains bin number
                            var tmp = new decimal[model.NDirs];
                            for (var i = 0; i < model.NDirs; i++)
                            {
                                tmp[i] = ParseDecimal(line5N[i + 1]);
                            }
                            model.FreqByBins.Add(tmp);
                            break;
                    }
                }
            }
            _log.Info("File import complete. Calculating...");

            // MeanVelocityPerDir
            model.MeanVelocityPerDir.AddRange(new decimal[model.NDirs]);
            for (var binIdx = 0; binIdx < model.NBins; binIdx++)
                for (var dirIdx = 0; dirIdx < model.NDirs; dirIdx++)
                {
                    var velocity = binIdx + 1;
                    model.MeanVelocityPerDir[dirIdx] += (decimal)(velocity * (double)model.FreqByBins[binIdx][dirIdx] / 1000);
                }

            // Velocity frequencies
            model.VelocityFreq.Clear();
            for (int binIdx = 0; binIdx < model.NBins; binIdx++)
            {
                decimal freq = 0;
                for (int dirIdx = 0; dirIdx < model.NDirs; dirIdx++)
                {
                    freq += model.FreqByBins[binIdx][dirIdx] / 1000 * model.FreqByDirs[dirIdx];
                }
                model.VelocityFreq.Add(new HPoint(binIdx, 0, freq));
            }
            _log.Info("Import&Calculations complete.");

            return model;
        }

        public JsonResult GetDatabasePoints(int sEcho, int iDisplayLength, int iDisplayStart)
        {
            List<object[]> goodPoints;
            if (Settings.showAll == ShowAll.no)
            {
                var allowedDistance = Settings.distance * 1000;
                goodPoints = GetFiltered(Settings.startLat, Settings.startLng, allowedDistance)
                    .Select(MapDatabaseItem)
                    .ToList();
            }
            else
            {
                goodPoints = _ctx.SmallMesoscaleTabFiles
                    .Select(MapDatabaseItem)
                    .ToList();
            }
            var filtered = goodPoints
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToArray();

            var data =
                new
                {
                    sEcho,
                    iTotalRecords = goodPoints.Count,
                    iTotalDisplayRecords = goodPoints.Count,
                    aaData = filtered
                };
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllData()
        {
            if (Settings.showAll == ShowAll.yes)
                return Json(_ctx.SmallMesoscaleTabFiles.Select(MapDatabaseItem).ToArray(), JsonRequestBehavior.AllowGet);

            var filtered = GetFiltered(Settings.startLat, Settings.startLng, Settings.distance)
                .Select(MapDatabaseItem)
                .ToArray();
            return Json(filtered, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SmallMesoscaleTabFile> GetFiltered(decimal lat, decimal lng, decimal allowedDistance)
        {
            var dbType = (int) Settings.DbType;

            foreach (var item in _ctx.SmallMesoscaleTabFiles.Where(t => t.DatabaseId == dbType))
            {
                var sCoord = new GeoCoordinate((double)item.Latitude, (double)item.Longitude);
                var eCoord = new GeoCoordinate((double)lat, (double)lng);

                var distance = sCoord.GetDistanceTo(eCoord);
                if (distance > (double)allowedDistance) continue;
                yield return item;
            }
        }

        private static object[] MapDatabaseItem(SmallMesoscaleTabFile x)
        {
            var db = x.DatabaseId == (int) DbType.FNL ? "FNL" : "MERRA";
            return new object[] { x.Id, x.Latitude, x.Longitude, db};
        }

        private int ParseInt(string input)
        {
            int ir;
            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out ir))
                return ir;
            decimal dr;
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out dr))
                return Convert.ToInt32(dr);
            _log.ErrorFormat("[ParseInt] Unable to parse '{0}'", input);
            return 0;
        }

        private decimal ParseDecimal(string input)
        {
            decimal dr;
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out dr))
                return dr;
            _log.ErrorFormat("[ParseDecimal] Unable to parse '{0}'", input);
            return 0;
        }
    }
}
