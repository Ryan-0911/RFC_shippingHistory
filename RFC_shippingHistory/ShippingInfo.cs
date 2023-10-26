using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory
{
    internal class ShippingInfo
    {
        /// <summary>
        /// 客戶料號【Plex】
        /// </summary>
        public string CPartNo { get; set; }

        /// <summary>
        /// 銷售文件【Plex】
        /// </summary>
        public string ShipperNo { get; set; }

        /// <summary>
        /// 客戶地址【Plex】
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 出貨數量【Plex】
        /// </summary>
        public float Quantity { get; set; }

        /// <summary>
        /// 實際發貨日期【Plex】
        /// </summary>
        public string ShipDate { get; set; }

        /// <summary>
        /// 淨額【Plex】
        /// </summary>
        public decimal NetUnitPrice { get; set; }

        /// <summary>
        /// 兩個RFC的資料是否都有查詢到【made】
        /// </summary>
        public bool ok { get; set; } = true;

        /// <summary>
        /// 收貨方【RFC1】
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// RFC1錯誤訊息【RFC1】
        /// </summary>
        public string E_MESSAGE_rfc1 { get; set; }

        /// <summary>
        /// 物料號碼【RFC2】
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 庫存狀態【made】
        /// </summary>
        public List<Inventory> inventory { get; set; } = new List<Inventory>();

        /// <summary>
        /// 儲存地點【RFC2】
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// 儲存地點說明【RFC2】
        /// </summary>
        public string RepositoryDesc { get; set; }


        /// <summary>
        /// RFC2錯誤訊息【RFC2】
        /// </summary>
        public string E_MESSAGE_rfc2 { get; set; }

        /// <summary>
        /// RFC3錯誤訊息【RFC3】
        /// </summary>
        public string E_MESSAGE_rfc3 { get; set; }
    }
}
