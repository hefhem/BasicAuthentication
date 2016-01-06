using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebForTraining.Models;
using WebForTraining.Database;

namespace WebForTraining.Controllers
{
    public class HomeController : Controller
    {
        private string GetSession() { return Session["SessionID"].ToString(); }
        private int GetID() { return int.Parse(Session["UserID"].ToString()); }
        private int GetCompanyID() { return int.Parse(Session["CompanyID"].ToString()); }


        private bool CheckSession()
        {
            if (Session["SessionID"] == null) { return false; }
            else
            {
                var ActiveSession = Login.getUserSessions()
                    .Where(p => p.isActive && p.sessionID == Guid.Parse(Session["SessionID"].ToString())).FirstOrDefault();
                if (ActiveSession == null) { return false; }
                else
                    return true;
            }
        }
        public ActionResult Index()
        {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login"); 
            }
            return View();
        }
    }
}