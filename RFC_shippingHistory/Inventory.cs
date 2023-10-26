using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory
{
    internal class Inventory
    {
        /// <summary>
        /// 批次號碼【RFC2】
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 未限制使用庫存【RFC2】
        /// </summary>
        public float BatchAmount { get; set; }

        /// <summary>
        /// 取用的庫存【RFC2】
        /// </summary>
        public float BatchTaken { get; set; }

        /// <summary>
        /// 銷售與配銷文件號碼 【RFC2】
        /// </summary>
        public string SD { get; set; }

        /// <summary>
        /// 銷售與配銷文件日期【RFC2】
        /// </summary>
        public DateTime? SD_date { get; set; }

        /// <summary>
        /// 淨額【RFC2】
        /// </summary>
        public decimal NetUnitPrice { get; set; }

        /// <summary>
        /// 幣別【RFC2】
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 條件定價單位
        /// </summary>
        public int MPC { get; set; }
    }
}
