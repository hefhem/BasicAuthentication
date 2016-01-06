using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebForTraining.Database;
using WebForTraining.Models;

namespace WebForTraining.Models
{
    public class Restriction

    {
        public static List<string> GetAddableForms(List<ClsUserDisplay> userDisplay)
        {
            List<string> addableForms = new List<string>();
            var forms = userDisplay.Where(p => p.canAdd).Select(e => new { e.formID, e.formName, e.canAdd, e.canDelete, e.canEdit, e.canView, e.canApprove });
            foreach (var item in forms)
            {
                if (item.canAdd)
                    addableForms.Add(item.formName);
            }
            return addableForms;
        }
        public static List<string> GetEditableForms(List<ClsUserDisplay> userDisplay)
        {
            List<string> editableForms = new List<string>();
            var forms = userDisplay.Where(p => p.canEdit).Select(e => new { e.formID, e.formName, e.canAdd, e.canDelete, e.canEdit, e.canView, e.canApprove });
            foreach (var item in forms)
            {
                if (item.canEdit)
                    editableForms.Add(item.formName);
            }
            return editableForms;
        }

        public static List<string> GetDeletableForms(List<ClsUserDisplay> userDisplay)
        {
            List<string> deleteableForms = new List<string>();
            var forms = userDisplay.Where(p => p.canDelete).Select(e => new { e.formID, e.formName, e.canAdd, e.canDelete, e.canEdit, e.canView, e.canApprove });
            foreach (var item in forms)
            {
                if (item.canDelete)
                    deleteableForms.Add(item.formName);
            }
            return deleteableForms;
        }

        public static List<string> GetViewableForms(List<ClsUserDisplay> userDisplay)
        {
            List<string> viewableForms = new List<string>();
            var forms = userDisplay.Where(p => p.canView).Select(e => new { e.formID, e.formName, e.canAdd, e.canDelete, e.canEdit, e.canView, e.canApprove });
            foreach (var item in forms)
            {
                viewableForms.Add(item.formName);
            }
            return viewableForms;
        }
        public static List<string> GetApprovableForms(List<ClsUserDisplay> userDisplay)
        {
            List<string> approvableForms = new List<string>();
            var forms = userDisplay.Where(p => p.canApprove).Select(e => new { e.formID, e.formName, e.canAdd, e.canDelete, e.canEdit, e.canView, e.canApprove });
            foreach (var item in forms)
            {
                if (item.canApprove)
                    approvableForms.Add(item.formName);
            }
            return approvableForms;
        }

    }
}

//List<ClsUserDisplay> userDisplay = returnedTuple.Item2;

//            List<string> editableForms = Restriction.GetEditableForms(userDisplay);

//            if (editableForms.Contains("Formula Detail"))
//            {
//                return EnPeopleBAL.Payroll.setFormulaDetail(FormulaDetail);
//            }
//            else return new ClsReturnValues() { ID = 0, IsSuccess = false, Response = "You are not allowed to create or edit records here" };