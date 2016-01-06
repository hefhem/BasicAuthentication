using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForTraining.Models
{
    public class StringToArray
    {

        public static List<string> PutInList(IEnumerable<string> array)
        {
            List<string> buffer = new List<string>();

            foreach (var item in array)
            {
                buffer.Add(item);
            }
            return buffer;
        }
        public static DateTime ConvertToDate(string strDate)  //"yyyy-mm-dd"
        {
            strDate = strDate.ToLower().Trim();
            string[] months = { "", "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };
            string[] mths = { "", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            for (int m = 1; m <= 12; m++)
            {
                strDate = strDate.Replace(months[m], m.ToString()); strDate = strDate.Replace(mths[m], m.ToString());
            }
            try
            {
                if (strDate.Contains(" ") || strDate.Contains(":") || strDate.ToLower().Contains("am") || strDate.ToLower().Contains("pm"))
                    strDate = strDate.Split(' ')[0].Trim();
            }
            catch { }

            strDate = strDate.Replace("  ", "-").Replace(' ', '-').Replace('/', '-').Replace('\\', '-').Replace(',', '-').Replace(';', '-')
                .Replace('_', '-').Replace('.', '-');
            string[] yrMthDay = strDate.Split('-');
            DateTime _date = new DateTime(1900, 1, 1); int yr = 0, mth = 0, day = 0;
            if (strDate.Contains('-'))
            {
                try
                {
                    string _yr = yrMthDay[0].Trim(), _mth = yrMthDay[1].Trim(), _day = yrMthDay[2].Trim();
                    if (_yr.Length == 4)
                    { //yyyy-mm-dd                   
                        yr = int.Parse(yrMthDay[0].Trim()); mth = int.Parse(yrMthDay[1].Trim()); day = int.Parse(yrMthDay[2].Trim());
                        if (mth > 12) { int _m = mth; mth = day; day = _m; }
                    }
                    else
                    { //dd-mm-yyyy                   
                        yr = int.Parse(yrMthDay[2].Trim()); mth = int.Parse(yrMthDay[1].Trim()); day = int.Parse(yrMthDay[0].Trim());
                        if (mth > 12) { int _m = mth; mth = day; day = _m; }
                    }
                    _date = new DateTime(yr, mth, day);
                }
                catch { return DateTime.Parse(strDate); }
            }
            else
            {
                DateTime dt = DateTime.Now;
                try { dt = DateTime.Parse(strDate); }
                catch { } _date = new DateTime(dt.Year, dt.Month, dt.Day);
            }
            return _date;
        }
        public static List<DateTime> PutInDateList(IEnumerable<string> array)
        {
            List<DateTime> buffer = new List<DateTime>();

            foreach (var item in array)
            {

                var dateComponent = seperateCommaValues(item, '/').ToList();
                string day = dateComponent[0];
                string month = dateComponent[1];
                string year = dateComponent[2];
                buffer.Add(ConvertToDate(day + "/" + month + "/" + year));
                //buffer.Add(DateTime.Parse(month + "/" + day + "/" + year));
                //buffer.Add(temp);
            }
            return buffer;
        }

        public static IEnumerable<string> seperateCommaValues(string values, char delimiter)
        {
            return values.Trim().Split(delimiter);
        }

    }
}