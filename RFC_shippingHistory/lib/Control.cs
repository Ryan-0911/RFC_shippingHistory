using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFC_shippingHistory.lib
{
    internal class Control
    {
        delegate void logHandler(TextBox tb, string text);
        delegate void pgbarHandler(ProgressBar pgbar, int total, int current);

        public static void ShowLog(TextBox tb, string text)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                logHandler ph = new logHandler(ShowLog);
                tb.Invoke(ph, tb, text);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                //tb.Text = text + tb.Text;
                tb.AppendText(text);
            }
        }
        public static void ShowPgbar(ProgressBar pgbar, int total, int current)
        {
            if (pgbar.InvokeRequired)
            {
                pgbarHandler ph = new pgbarHandler(ShowPgbar);
                pgbar.Invoke(ph, pgbar, total, current);
            }
            else
            {
                pgbar.Maximum = total;
                pgbar.Value = current;
            }
        }

        public static Object dNull(Object toCatch, string default_value = "")
        {
            if (System.Convert.IsDBNull(toCatch) || Equals(toCatch, ""))
            {
                return default_value;
            }
            else
            {
                return toCatch;
            }
        }

        public static void SaveTestLog(string r)
        {
            r = "xx";
        }

        //'-----------------------------------
        //'函式名:StrRigth
        //'目的:字串取右邊
        //'傳入:
        //'	    s:原字串
        //'	    length:共幾位
        //'-----------------------------------       
        public static string StrRigth(string s, int length)
        {
            return s.Substring(s.Length - length);
        }

        public static string Add0(int innum, int digits)
        {
            string rr = innum.ToString();
            for (int i = 0; i < digits - innum.ToString().Length; i++)
            {
                rr = "0" + rr;
            }
            return rr;
        }

        //'-----------------------------------
        //'判斷DBNull,
        //'傳入:
        //'     toCatch:要判斷的data item
        //'     default_value:若為DBNull要取代的值
        //'-----------------------------------       
        public static Object DNull(Object toCatch, string default_value = "")
        {
            if (System.Convert.IsDBNull(toCatch) || Equals(toCatch, ""))
            {
                return default_value;
            }
            else
            {
                return toCatch;
            }
        }

        public static double DNull2double(Object toCatch, double default_value = 0)
        {
            if (System.Convert.IsDBNull(toCatch) || Equals(toCatch, null) || Equals(toCatch, ""))
            {
                return default_value;
            }
            else
            {
                string tmps = toCatch.ToString();
                if (IsNumeric(tmps))
                {
                    double tmpd = Convert.ToDouble(tmps);
                    return tmpd;
                }
                else
                {
                    return default_value;
                }
            }
        }

        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}