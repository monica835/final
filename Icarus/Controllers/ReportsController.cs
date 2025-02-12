﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Icarus.Models;
using Rotativa;


namespace Icarus.Controllers
{
    public class ReportsController : Controller
    {
        private ICARUSDBEntities db = new ICARUSDBEntities();

        // GET: Report
        [Route("Reports/")]
        public ActionResult Index()
        {
            if (Session["Username"] != null) {
                return View();
            }

            return RedirectToAction("Login","Login");
        }

        public ActionResult AssertionSummary(DateTime? datefrom, DateTime? dateto)
        {
            if (Session["Username"] != null)
            {
                if (datefrom != null && dateto == null)
                {
                    return View("_AssertionSummary", db.vrptAssertionSummaries.ToList().Where(x => x.AssertionDate >= datefrom).ToList());
                }
                else {
                    Response.Write("<script>console.log('TESTING in ELSE ASSERTION')</script>");
                    return View("_AssertionSummary", db.vrptAssertionSummaries.ToList().Where(x => x.AssertionDate >= datefrom && x.AssertionDate <= dateto).ToList());
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public ActionResult FileToPdf(DateTime? datefrom, DateTime? dateto)
        {
            if (Session["Username"] != null)
            {
                if (datefrom != null && dateto == null)
                {
                    Response.Write("<script>alert("+datefrom+");</script>");
                    var report = new ActionAsPdf("AssertionSummary", new { datefrom, dateto })
                    {
                        //FileName = Server.MapPath("~/Content/Relato.pdf"),
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageSize = Rotativa.Options.Size.A4
                    };
                    return report;
                    
                }
                else
                {
                    Response.Write("<script>console.log('TESTING in ELSE ASSERTION GET')</script>");
                    System.Diagnostics.Debug.WriteLine("Testing Reading");
                    var report = new ActionAsPdf("AssertionSummary", new { datefrom, dateto })
                    {
                        //FileName = Server.MapPath("~/Content/Relato.pdf"),
                        PageOrientation = Rotativa.Options.Orientation.Landscape,
                        PageSize = Rotativa.Options.Size.A4
                    };
                    return report;
                }
            }
            return RedirectToAction("Login", "Login");
        }
    }
}