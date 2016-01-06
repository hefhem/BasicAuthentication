using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebForTraining.Database;

namespace WebForTraining.Models
{
    public class Login
    {
        public Tuple<ClsReturnValues, string, List<ClsUserDisplay>> authenticateUser(string UserName, string Password, string DeviceType, string DeviceName, string Browser)
        {
            string encryptedPassword = Security.Encrypt(Password);
            string token = "";
            List<ClsUserDisplay> ud = new List<ClsUserDisplay>();
            ClsReturnValues result = new ClsReturnValues();
            ClsUsers userGroup = new ClsUsers();
            using (tdoEntities db = new tdoEntities())
            {
                result = db.uspUserAuthentication(UserName, encryptedPassword, DeviceType, DeviceName, Browser).FirstOrDefault();
                userGroup = db.uspGetUsers().Where(p => p.userID == result.ID).FirstOrDefault();

                if (result.IsSuccess == true)
                {

                    ud = db.uspGetUserDisplay(result.ID).ToList<ClsUserDisplay>();
                    // Token issuer
                    TokenIssuer issuer = new TokenIssuer();
                    // A client of the relying party app gets the token
                    token = issuer.GetToken(result, ud.First().userGroupID);

                }


            }
            return new Tuple<ClsReturnValues, string, List<ClsUserDisplay>>(result, token, ud);
        }

        #region usersession, activesession and password
        public static ClsReturnValues setUserSessions(ClsUserSessions item)
        {
            ClsReturnValues obj = new ClsReturnValues();
            try
            {
                using (var db = new tdoEntities())
                {
                    //uspAddEditUserSessions(Guid? sessionID, int? userID, bool? isActive, string deviceType, string deviceName, string browser, int? companyID);
                    obj = db.uspAddEditUserSessions(item.sessionID, item.userID, item.isActive, item.deviceType, item.deviceName, item.browser).FirstOrDefault();
                }
            }
            catch (Exception ex) { obj.Response = ex.Message; obj.IsSuccess = false; obj.ID = 0; }
            return obj;
        }

        public static List<ClsUserSessions> getUserSessions()
        {
            List<ClsUserSessions> lst = new List<ClsUserSessions>();
            try
            {
                using (var db = new tdoEntities()) { lst = db.uspGetUserSessions().ToList<ClsUserSessions>(); }
            }
            catch { } return lst;

        }

        public static ClsReturnValues delUserSession(Guid sessionID, int userID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                try
                {
                    lst = db.uspDelUserSession(sessionID, userID).FirstOrDefault();
                }
                catch { }
            }
            return lst;
        }
        public static ClsReturnValues setUserSessionHistory(ClsUserSessionHistory item)
        {
            ClsReturnValues obj = new ClsReturnValues();
            try
            {
                using (var db = new tdoEntities())
                {
                    obj = db.uspAddEditUserSessionHistory(item.sessionID, item.userID, item.logoutDate, item.isActive, item.deviceType, item.deviceName, item.browser).FirstOrDefault();
                }
            }
            catch (Exception ex) { obj.Response = ex.Message; obj.IsSuccess = false; obj.ID = 0; }
            return obj;
        }

        public static List<ClsUserSessionHistory> getUserSessionHistory()
        {
            List<ClsUserSessionHistory> lst = new List<ClsUserSessionHistory>();
            try
            {
                using (var db = new tdoEntities()) { lst = db.uspGetUserSessionHistory().ToList<ClsUserSessionHistory>(); }
            }
            catch { } return lst;

        }

        //public static ClsReturnValues setPasswordPolicy(ClsPasswordPolicy item)
        //{
        //    ClsReturnValues obj = new ClsReturnValues();
        //    try
        //    {
        //        using (var db = new EnPeopleEntities())
        //        {

        //            obj = db.uspAddEditPasswordPolicy(item.passwordPolicyID, item.passwordResetMediumID, item.maxFailedAttempts, item.idleTime, item.minPasswordLength, item.includeSpecialChar, item.includeAlphanumeric, item.passwordValidity, item.useADAuthentication, item.passwordHistoryValue, item.useHistoryDays, item.createdByID, item.companyID, item.validFrom, item.validTo).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex) { obj.Response = ex.Message; obj.IsSuccess = false; obj.ID = 0; }
        //    return obj;
        //}

        //public static List<ClsPasswordPolicy> getPasswordPolicy(int companyID)
        //{
        //    List<ClsPasswordPolicy> lst = new List<ClsPasswordPolicy>();
        //    try
        //    {
        //        using (var db = new EnPeopleEntities()) { lst = db.uspGetPasswordPolicy(companyID).ToList<ClsPasswordPolicy>(); }
        //    }
        //    catch { } return lst;

        //}

        //public static List<ClsPasswordResetMedium> getPasswordResetMedium()
        //{
        //    List<ClsPasswordResetMedium> lst = new List<ClsPasswordResetMedium>();
        //    try
        //    {
        //        using (var db = new EnPeopleEntities()) { lst = db.uspGetPasswordResetMedium().ToList<ClsPasswordResetMedium>(); }
        //    }
        //    catch { } return lst;

        //}

        public static List<ClsUserSessions> getActiveSessions()
        {
            List<ClsUserSessions> lst = new List<ClsUserSessions>();
            try
            {
                using (var db = new tdoEntities()) { lst = db.uspGetUserSessions().ToList<ClsUserSessions>(); }
            }
            catch { } return lst;

        }

        public static ClsReturnValues delActiveSessions(Guid sessionID, int userID)
        {
            ClsReturnValues lst = new ClsReturnValues();
            using (var db = new tdoEntities())
            {
                lst = db.uspDelUserSession(sessionID, userID).FirstOrDefault();
            }
            return lst;
        }

        #endregion
    }
}