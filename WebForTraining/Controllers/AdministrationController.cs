using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebForTraining.Database;
using WebForTraining.Models;

namespace WebForTraining.Controllers {
    public class AdministrationController : Controller {
        // GET: Administration
        private string GetSession() { return Session["SessionID"].ToString(); }
        private int GetID() { return int.Parse(Session["UserID"].ToString()); }
        private string GetUserName() { return Session["Username"].ToString(); }


        private bool CheckSession() {
            if (Session["SessionID"] == null) { return false; } else {
                var ActiveSession = Login.getUserSessions()
                    .Where(p => p.isActive && p.sessionID == Guid.Parse(Session["SessionID"].ToString())).FirstOrDefault();
                if (ActiveSession == null) { return false; } else
                    return true;
            }
        }
        private bool CheckResetSession() {
            if (Session["ResetID"] == null) {
                return false;
            } else {
                return true;
            }
        }
        public ActionResult Index() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult Dashboard() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult Setup() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getUserGroupDisplay() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getUserDisplay() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AddEditUserGroup() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AddEditUser() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult changePassword() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult resetPassword() {
            if (!CheckResetSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getAccessLevel() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AddEditMenus() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getMenusDisplay() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AddEditMenuItems() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getMenuItemsDisplay() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AddEditForms() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getFormsDisplay() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AddEditMenuIcons() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getMenuIconsDisplay() {
            if (!CheckSession()) {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
       
        public JsonResult setUsersGroup(string userGroupID, string groupName, string description) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (int.Parse(userGroupID) == 0 && !addableForms.Contains("UsersGroup")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            } else if (int.Parse(userGroupID) != 0 && !editableForms.Contains("UsersGroup")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to edit records." });
            }

            if (userGroupID == "") { userGroupID = "0"; }

            Guid Session = new Guid(GetSession()); //do not hard code session ID and createdbyID
            int _id = 0;
            try { _id = int.Parse(userGroupID.Trim()); } catch { }
            ClsUserGroups obj = new ClsUserGroups() {
                userGroupID = _id,
                groupName = groupName,
                description = description,
                createdByID = GetID(),
                sessionID = Session
            };
            ClsReturnValues k = Administration.setUsersGroup(obj, Session);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }
        [HttpPost]
        public JsonResult setUsers(string userID, string userGroupID, string userName, string Password, int isLocked, int resetPassword) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (int.Parse(userID) == 0 && !addableForms.Contains("Users")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            } else if (int.Parse(userID) != 0 && !editableForms.Contains("Users")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to edit records." });
            }

            Guid Session = new Guid(GetSession());
            if (Password == "") Password = " ";
            int _id = 0; try { _id = int.Parse(userID.Trim()); } catch { }
            int _grIid = 0; try { _grIid = int.Parse(userGroupID.Trim()); } catch { }
            bool Locked = false; bool reset = false;
            if (isLocked == 1) Locked = true; if (resetPassword == 1) reset = true;
            ClsUsers obj = new ClsUsers() {
                userID = _id,
                userGroupID = _grIid,
                userName = userName.Trim(),
                resetPassword = reset,
                password = Password,
                isLocked = Locked,
                createdByID = GetID(),
                theme = "Default",
                sessionID = Session
            };
            ClsReturnValues k = Administration.setUsers(obj);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }
        public JsonResult deleteUsersGroup(string ids) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> deleteableForms = Restriction.GetDeletableForms(userDisplay);

            if (!deleteableForms.Contains("UsersGroup")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to delete records here" });
            }

            string[] id_s = ids.Trim().Split(',');
            string message = "";
            List<ClsReturnValues> obj = new List<ClsReturnValues>();
            foreach (var id in id_s) {
                int _id = 0; try { _id = int.Parse(id.Trim()); } catch { }
                if (_id > 0)
                    obj.Add(Administration.delUsersGroup(_id));
            }

            bool isSuccess = obj.Count(p => p.IsSuccess == false) > 0 ? false : true;
            if (obj.Count(p => p.IsSuccess == true) > 1) {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " records deleted";
            } else {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " record deleted";
            }

            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = message });
        }
        public JsonResult deleteUser(string ids) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> deleteableForms = Restriction.GetDeletableForms(userDisplay);

            if (!deleteableForms.Contains("Users")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to delete records here" });
            }

            string[] id_s = ids.Trim().Split(',');
            string message = "";
            List<ClsReturnValues> obj = new List<ClsReturnValues>();
            foreach (var id in id_s) {
                int _id = 0; try { _id = int.Parse(id.Trim()); } catch { }
                if (_id > 0)
                    obj.Add(Administration.delUsers(_id));
            }

            bool isSuccess = obj.Count(p => p.IsSuccess == false) > 0 ? false : true;
            if (obj.Count(p => p.IsSuccess == true) > 1) {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " records deleted";
            } else {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " record deleted";
            }

            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = message });
        }
        public JsonResult setChangePassword(string userID, string oldPassword, string newPassword, string confirmNewPassword) {
            if (newPassword != confirmNewPassword) {
                return Json(new { id = 0, isSuccess = false, msg = "Confirm password not correct." });
            }
            int id = int.Parse(userID);

            ClsReturnValues k = Administration.changePassword(id, oldPassword, newPassword);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }
        public JsonResult setResetPassword(string userID, string newPassword, string confirmNewPassword) {
            if (newPassword != confirmNewPassword) {
                return Json(new { id = 0, isSuccess = false, msg = "Confirm password not correct." });
            }
            int id = int.Parse(userID);

            ClsReturnValues k = Administration.resetPassword(id, newPassword);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }
        public JsonResult setAccessLevel(string accessLevelID, string userGroupID, string formID, string canAdd, string canView, string canEdit, string canDelete, string canApprove) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (!addableForms.Contains("AccessLevels") && GetUserName() != "Admin") {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            }

            Guid Session = new Guid(GetSession());
            List<ClsReturnValues> returnObjs = new List<ClsReturnValues>();

            var accessLevelID_ = StringToArray.PutInList(StringToArray.seperateCommaValues(accessLevelID, ','));
            var userGroupID_ = int.Parse(userGroupID);
            var formID_ = StringToArray.PutInList(StringToArray.seperateCommaValues(formID, ','));
            var canAdd_ = StringToArray.PutInList(StringToArray.seperateCommaValues(canAdd, ','));
            var canView_ = StringToArray.PutInList(StringToArray.seperateCommaValues(canView, ','));
            var canEdit_ = StringToArray.PutInList(StringToArray.seperateCommaValues(canEdit, ','));
            var canDelete_ = StringToArray.PutInList(StringToArray.seperateCommaValues(canDelete, ','));
            var canApprove_ = StringToArray.PutInList(StringToArray.seperateCommaValues(canApprove, ','));

            for (int i = 0; i < accessLevelID_.Count; i++) {
                if (formID_[i] == "") continue;
                ClsAccessLevels obj = new ClsAccessLevels() {
                    accessLevelID = int.Parse(accessLevelID_[i]),
                    formID = int.Parse(formID_[i]),
                    canAdd = bool.Parse(canAdd_[i]),
                    canView = bool.Parse(canView_[i]),
                    canEdit = bool.Parse(canEdit_[i]),
                    canDelete = bool.Parse(canDelete_[i]),
                    canApprove = bool.Parse(canApprove_[i]),
                    userGroupID = userGroupID_,
                    createdByID = GetID(),
                    sessionID = Session
                };

                returnObjs.Add(Administration.setAccessLevel(obj));
            }
            bool isSuccess = returnObjs.Count(p => p.IsSuccess == false) > 0 ? false : true;
            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = returnObjs.Count(p => p.IsSuccess == true).ToString() });
        }

       public JsonResult setMenus(string menuID, string menuName, string menuDesc, string menuRanking) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (int.Parse(menuID) == 0 && !addableForms.Contains("Menus")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            } else if (int.Parse(menuID) != 0 && !editableForms.Contains("Menus")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to edit records." });
            }

            if (menuID == "") { menuID = "0"; }

            Guid Session = new Guid(GetSession()); //do not hard code session ID and createdbyID
            int _id = 0;
            try { _id = int.Parse(menuID.Trim()); } catch { }
            ClsMenus obj = new ClsMenus() {
                menuID = _id,
                menuName = menuName,
                menuDesc = menuDesc,
                menuRanking = int.Parse(menuRanking),
                createdByID = GetID(),
                sessionID = Session
            };
            ClsReturnValues k = Administration.setMenus(obj);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }

        public JsonResult deleteMenus(string ids) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> deleteableForms = Restriction.GetDeletableForms(userDisplay);

            if (!deleteableForms.Contains("Menus")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to delete records here" });
            }
            string[] id_s = ids.Trim().Split(',');
            string message = "";
            List<ClsReturnValues> obj = new List<ClsReturnValues>();
            foreach (var id in id_s) {
                int _id = 0; try { _id = int.Parse(id.Trim()); } catch { }
                if (_id > 0)
                    obj.Add(Administration.delMenus(_id));
            }

            bool isSuccess = obj.Count(p => p.IsSuccess == false) > 0 ? false : true;
            if (obj.Count(p => p.IsSuccess == true) > 1) {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " records deleted";
            } else {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " record deleted";
            }

            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = message });
        }

        public JsonResult setMenuItems(string menuItemID, string menuID, string menuItemName, string menuItemDescription, string menuItemRanking) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (int.Parse(menuItemID) == 0 && !addableForms.Contains("MenuItems")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            } else if (int.Parse(menuItemID) != 0 && !editableForms.Contains("MenuItems")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to edit records." });
            }

            if (menuItemID == "") { menuItemID = "0"; }

            Guid Session = new Guid(GetSession()); //do not hard code session ID and createdbyID
            int _id = 0;
            try { _id = int.Parse(menuItemID.Trim()); } catch { }
            ClsMenuItems obj = new ClsMenuItems() {
                menuItemID = _id,
                menuID = int.Parse(menuID),
                menuItemName = menuItemName,
                menuItemDescription = menuItemDescription,
                menuItemRanking = int.Parse(menuItemRanking),
                createdByID = GetID(),
                sessionID = Session
            };
            ClsReturnValues k = Administration.setMenuItems(obj);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }

        public JsonResult deleteMenuItems(string ids) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> deleteableForms = Restriction.GetDeletableForms(userDisplay);

            if (!deleteableForms.Contains("MenuItems")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to delete records here" });
            }
            string[] id_s = ids.Trim().Split(',');
            string message = "";
            List<ClsReturnValues> obj = new List<ClsReturnValues>();
            foreach (var id in id_s) {
                int _id = 0; try { _id = int.Parse(id.Trim()); } catch { }
                if (_id > 0)
                    obj.Add(Administration.delMenuItems(_id));
            }

            bool isSuccess = obj.Count(p => p.IsSuccess == false) > 0 ? false : true;
            if (obj.Count(p => p.IsSuccess == true) > 1) {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " records deleted";
            } else {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " record deleted";
            }

            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = message });
        }
        public JsonResult setMenuIcons(string menuIconID, string menuID, string menuIconName) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (int.Parse(menuIconID) == 0 && !addableForms.Contains("MenuIcons")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            } else if (int.Parse(menuIconID) != 0 && !editableForms.Contains("MenuIcons")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to edit records." });
            }

            if (menuIconID == "") { menuIconID = "0"; }

            Guid Session = new Guid(GetSession()); //do not hard code session ID and createdbyID
            int _id = 0;
            try { _id = int.Parse(menuIconID.Trim()); } catch { }
            ClsMenuIcons obj = new ClsMenuIcons() {
                menuIconID = _id,
                menuID = int.Parse(menuID),
                menuIconName = menuIconName
            };
            ClsReturnValues k = Administration.setMenuIcons(obj);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }

        public JsonResult deleteMenuIcons(string ids) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> deleteableForms = Restriction.GetDeletableForms(userDisplay);

            if (!deleteableForms.Contains("MenuIcons")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to delete records here" });
            }

            string[] id_s = ids.Trim().Split(',');
            string message = "";
            List<ClsReturnValues> obj = new List<ClsReturnValues>();
            foreach (var id in id_s) {
                int _id = 0; try { _id = int.Parse(id.Trim()); } catch { }
                if (_id > 0)
                    obj.Add(Administration.delMenuIcons(_id));
            }

            bool isSuccess = obj.Count(p => p.IsSuccess == false) > 0 ? false : true;
            if (obj.Count(p => p.IsSuccess == true) > 1) {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " records deleted";
            } else {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " record deleted";
            }

            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = message });
        }
        public JsonResult setForms(string formID, string menuItemID, string formName, string formDescription) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> editableForms = Restriction.GetEditableForms(userDisplay);
            List<string> addableForms = Restriction.GetAddableForms(userDisplay);

            if (int.Parse(formID) == 0 && !addableForms.Contains("Forms")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to add new records." });
            } else if (int.Parse(formID) != 0 && !editableForms.Contains("Forms")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to edit records." });
            }

            if (formID == "") { formID = "0"; }

            Guid Session = new Guid(GetSession()); //do not hard code session ID and createdbyID
            int _id = 0;
            try { _id = int.Parse(formID.Trim()); } catch { }
            ClsForms obj = new ClsForms() {
                formID = _id,
                menuItemID = int.Parse(menuItemID),
                formName = formName,
                formDescription = formDescription,
                createdByID = GetID(),
                sessionID = Session
            };
            ClsReturnValues k = Administration.setForms(obj);
            return Json(new { id = k.ID, isSuccess = k.IsSuccess ?? false ? 1 : 0, msg = k.Response });
        }

        public JsonResult deleteForms(string ids) {
            List<ClsUserDisplay> userDisplay = new List<ClsUserDisplay>();
            using (tdoEntities db = new tdoEntities()) {
                userDisplay = db.uspGetUserDisplay(GetID()).ToList<ClsUserDisplay>();
            }
            List<string> deleteableForms = Restriction.GetDeletableForms(userDisplay);

            if (!deleteableForms.Contains("Forms")) {
                return Json(new { id = 0, isSuccess = false, msg = "You are not allowed to delete records here" });
            }


            string[] id_s = ids.Trim().Split(',');
            string message = "";
            List<ClsReturnValues> obj = new List<ClsReturnValues>();
            foreach (var id in id_s) {
                int _id = 0; try { _id = int.Parse(id.Trim()); } catch { }
                if (_id > 0)
                    obj.Add(Administration.delForms(_id));
            }

            bool isSuccess = obj.Count(p => p.IsSuccess == false) > 0 ? false : true;
            if (obj.Count(p => p.IsSuccess == true) > 1) {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " records deleted";
            } else {
                message = obj.Count(p => p.IsSuccess == true).ToString() + " record deleted";
            }

            return Json(new { id = isSuccess ? 1 : 0, isSuccess = isSuccess ? 1 : 0, msg = message });
        }
    }
}