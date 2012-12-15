﻿using System.Web.Mvc;
using EmitMapper;
using Offwind.OpenFoam.Models.Fields;
using Offwind.OpenFoam.Sintef;
using Offwind.OpenFoam.Sintef.BoundaryFields;
using Offwind.WebApp.Areas.CFD.Models.BoundaryConditions;

namespace Offwind.WebApp.Areas.CFD.Controllers
{
    public class BoundaryConditionsController : __BaseCfdController
    {
        public BoundaryConditionsController()
        {
            SectionTitle = "Boundary Conditions";
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FieldK()
        {
            ShortTitle = "k";
            var m = new VFieldK();
            SolverData sd = GetSolverData();
            ObjectMapperManager.DefaultInstance.GetMapper<FieldK, VFieldK>().Map(sd.FieldK, m);
            return View(m);
        }


        [ActionName("FieldK")]
        [HttpPost]
        public ActionResult FieldKSave(VFieldK m)
        {
            ShortTitle = "k";
            SolverData sd = GetSolverData();
            ObjectMapperManager.DefaultInstance.GetMapper<VFieldK, FieldK>().Map(m, sd.FieldK);
            SetSolverData(sd);
            if (Request.IsAjaxRequest()) return Json("OK");
            return View(m);
        }

        public ActionResult FieldEpsilon()
        {
            ShortTitle = "epsilon";
            var m = new VFieldEpsilon();
            SolverData sd = GetSolverData();
            //ObjectMapperManager.DefaultInstance.GetMapper<VFieldEpsilon, BoundaryField>().Map(m, sd.FieldEpsilon);
            return View(m);
        }

        [ActionName("FieldEpsilon")]
        [HttpPost]
        public ActionResult FieldEpsilonSave(VFieldEpsilon m)
        {
            ShortTitle = "epsilon";
            SolverData sd = GetSolverData();
            //Mapper.Map(m, sd.FieldEpsilon);
            SetSolverData(sd);
            if (Request.IsAjaxRequest()) return Json("OK");
            return View(m);
        }

        public ActionResult FieldP()
        {
            ShortTitle = "p";
            var m = new VFieldP();
            SolverData sd = GetSolverData();
            //Mapper.Map(sd.FieldP, m);
            return View(m);
        }

        [ActionName("FieldP")]
        [HttpPost]
        public ActionResult FieldPSave(VFieldP m)
        {
            ShortTitle = "p";
            SolverData sd = GetSolverData();
            //Mapper.Map(m, sd.FieldP);
            SetSolverData(sd);
            if (Request.IsAjaxRequest()) return Json("OK");
            return View(m);
        }

        public ActionResult FieldR()
        {
            ShortTitle = "R";
            var m = new VFieldR();
            SolverData sd = GetSolverData();
            //Mapper.Map(sd.FieldR, m);
            return View(m);
        }

        [ActionName("FieldR")]
        [HttpPost]
        public ActionResult FieldRSave(VFieldR m)
        {
            ShortTitle = "R";
            SolverData sd = GetSolverData();
            //Mapper.Map(m, sd.FieldR);
            SetSolverData(sd);
            if (Request.IsAjaxRequest()) return Json("OK");
            return View(m);
        }

        public ActionResult FieldU()
        {
            ShortTitle = "U";
            var m = new VFieldU();
            SolverData sd = GetSolverData();
            //Mapper.Map(sd.FieldU, m);
            return View(m);
        }

        [ActionName("FieldU")]
        [HttpPost]
        public ActionResult FieldUSave(VFieldU m)
        {
            ShortTitle = "U";
            SolverData sd = GetSolverData();
            //Mapper.Map(m, sd.FieldU);
            SetSolverData(sd);
            if (Request.IsAjaxRequest()) return Json("OK");
            return View(m);
        }
    }
}