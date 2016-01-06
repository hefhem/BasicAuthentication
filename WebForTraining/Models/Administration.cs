using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebForTraining.Database;
using System.Threading;
using System.Security.Claims;
using System.Security.Permissions;

namespace WebForTraining.Models
{
    public class Administration
    {
        #region UsersGroup
        public static ClsReturnValues setUsersGroup(ClsUserGroups obj, Guid SessionID)
        {
            ClsReturnValues lst = new ClsReturnValues();

            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditUserGroups(obj.userGroupID, obj.groupName, obj.description ?? "", obj.createdByID, SessionID).FirstOrDefault();
            }
            return lst;
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public static List<ClsUserGroups> getUsersGroup()
        {
            List<ClsUserGroups> lst = new List<ClsUserGroups>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetUserGroups().ToList<ClsUserGroups>();
            }
            return lst;
        }

        public static ClsReturnValues delUsersGroup(int usersGroupId)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelUsergroup(usersGroupId).FirstOrDefault();
            }
            return lst;
        }

        #endregion

        #region Users

        public static ClsReturnValues setUsers(ClsUsers obj)
        {
            //password encryption happens here
            obj.password = Security.Encrypt(obj.password);

            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditUsers(obj.userID, obj.userGroupID, obj.userName, obj.password, obj.password, obj.passwordCanExpire, obj.passwordExpiryDate, obj.isLocked, obj.loginAttempts, obj.lastLoginDate, obj.theme, obj.resetPassword, obj.createdByID, obj.sessionID).FirstOrDefault();
            }
            return lst;
        }

        public static List<ClsUsers> getUsers()
        {
            List<ClsUsers> lst = new List<ClsUsers>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetUsers().ToList<ClsUsers>();
            }
            return lst;
        }

        public static ClsReturnValues delUsers(int userID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelUsers(userID).FirstOrDefault();
            }
            return lst;
        }

        #endregion users

        #region Change Password
public static ClsReturnValues changePassword(int userID, string oldPassword, string newPassword)
        {
            newPassword = Security.Encrypt(newPassword);
            oldPassword = Security.Encrypt(oldPassword);
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspChangePassword(userID, oldPassword, newPassword).FirstOrDefault();
            }
            return lst;
        }
        public static ClsReturnValues resetPassword(int userID, string newPassword)
        {
            newPassword = Security.Encrypt(newPassword);
            ClsUsers U = Administration.getUsers().Where(p => p.userID == userID).ToList().FirstOrDefault();
            string oldPassword = U.password;
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspChangePassword(userID, oldPassword, newPassword).FirstOrDefault();
            }
            return lst;
        }
        #endregion

        #region Access Level
        public static ClsReturnValues setAccessLevel(ClsAccessLevels obj)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditAccessLevels(obj.accessLevelID,obj.userGroupID,obj.formID,obj.canAdd,obj.canView,obj.canEdit,obj.canDelete,obj.canApprove,obj.createdByID,obj.sessionID).FirstOrDefault();
            }
            return lst;
        }
        public static List<ClsAccessLevels> getAccessLevel(int accessLevelID, int userGroupID, int formID)
        {
            List<ClsAccessLevels> lst = new List<ClsAccessLevels>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetAccessLevels(accessLevelID, userGroupID, formID).ToList<ClsAccessLevels>();
            }
            return lst;
        }
        #endregion

        #region Menu, Menu Item, Forms, Icons
        public static ClsReturnValues setMenus(ClsMenus obj)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditMenus(obj.menuID,obj.menuName,obj.menuDesc, obj.createdByID,obj.menuRanking, obj.sessionID).FirstOrDefault();
            }
            return lst;
        }
        public static List<ClsMenus> getMenus(int menuID)
        {
            List<ClsMenus> lst = new List<ClsMenus>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetMenus(menuID).ToList<ClsMenus>();
            }
            return lst;
        }
        public static ClsReturnValues delMenus(int menuID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelMenus(menuID).FirstOrDefault();
            }
            return lst;
        }
        public static ClsReturnValues setMenuItems(ClsMenuItems obj)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditMenuItems(obj.menuItemID,obj.menuID,obj.menuItemName, obj.menuItemDescription, obj.menuItemRanking, obj.createdByID, obj.sessionID).FirstOrDefault();
            }
            return lst;
        }
        public static List<ClsMenuItems> getMenuItems(int menuItemID)
        {
            List<ClsMenuItems> lst = new List<ClsMenuItems>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetMenuItems(menuItemID).ToList<ClsMenuItems>();
            }
            return lst;
        }
        public static ClsReturnValues delMenuItems(int menuItemID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelMenuItems(menuItemID).FirstOrDefault();
            }
            return lst;
        }
        public static ClsReturnValues setMenuIcons(ClsMenuIcons obj)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditMenuIcons(obj.menuIconID, obj.menuID, obj.menuIconName).FirstOrDefault();
            }
            return lst;
        }
        public static List<ClsMenuIcons> getMenuIcons(int menuIconID)
        {
            List<ClsMenuIcons> lst = new List<ClsMenuIcons>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetMenuIcons(menuIconID).ToList<ClsMenuIcons>();
            }
            return lst;
        }
        public static ClsReturnValues delMenuIcons(int menuIconID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelMenuIcons(menuIconID).FirstOrDefault();
            }
            return lst;
        }
        public static ClsReturnValues setForms(ClsForms obj)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {

                lst = db.uspAddEditForms(obj.formID,obj.menuItemID,obj.formName,obj.formDescription,obj.createdByID, obj.sessionID).FirstOrDefault();
            }
            return lst;
        }
        public static List<ClsForms> getForms(int formID)
        {
            List<ClsForms> lst = new List<ClsForms>();
            using (var db = new tdoEntities())
            {
                lst = db.uspGetForms(formID).ToList<ClsForms>();
            }
            return lst;
        }
        public static ClsReturnValues delForms(int formID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelForms(formID).FirstOrDefault();
            }
            return lst;
        }
        #endregion
    }
}