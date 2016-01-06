using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebForTraining.Database;
using WebForTraining.Models;

namespace WebForTraining.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["SessionID"] != null) { 
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Guid session = Guid.Parse(Session["SessionID"].ToString());
            int userid = int.Parse(Session["UserID"].ToString());
            ClsReturnValues returned = Login.delUserSession(session, userid);
            if (returned.IsSuccess == true)
            {
                Session.RemoveAll();
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult Validate(string UserName, string Password)
        {

            string DeviceType = "";
            string DeviceName = "";
            string Browser = "";
            int userid = 0;
            //Session["employeeID"] = EmployeeID;
            ClsUsers U = Administration.getUsers().Where(p => p.userName == UserName).ToList().FirstOrDefault();

            if (U != null)
            {
                if (U.resetPassword == true)
                {
                    Session["ResetUsername"] = U.userName;
                    Session["ResetID"] = U.userID;
                    return Json(new { id = U.userID, isSuccess = 0, msg = "PRYP" });
                }
            }

            Login rs = new Login();

            Tuple<ClsReturnValues, string, List<ClsUserDisplay>> result = rs.authenticateUser(UserName, Password, DeviceType, DeviceName, Browser);
            
            if (result.Item1.IsSuccess == true)
            {
                Session["MenusMenuItemsRaw"] = result.Item3;
                Session["SessionID"] = result.Item1.Response;
                Session["Token"] = result.Item2;
                Session["Username"] = UserName;
                Session["UserID"] = result.Item1.ID;
                userid = (int)result.Item1.ID;
                ClsUsers u = Administration.getUsers().Where(f => f.userID == userid).FirstOrDefault();
            }
            return Json(new { id = result.Item1.ID, isSuccess = result.Item1.IsSuccess, msg = result.Item1.Response });

        }
    }
}