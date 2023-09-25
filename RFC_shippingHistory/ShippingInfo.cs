using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory
{
    internal class ShippingInfo
    {
        /// <summary>
        /// 客戶物料號碼
        /// </summary>
        public string CPartNo { get; set; }

        /// <summary>
        /// 物料號碼
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 銷項交貨
        /// </summary>
        public string ShipperNo { get; set; }

        /// <summary>
        /// 客戶地址
        /// </summary>
        public string CustomerAddressCode { get; set; }

        /// <summary>
        /// 出貨數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 實際發貨日期
        /// </summary>
        public string ShipDate { get; set; }

        /// <summary>
        /// 收貨方【RFC1】
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 批次號碼 【RFC2】
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 未限制使用庫存 【RFC2】
        /// </summary>
        public int BatchAmount { get; set; }

        /// <summary>
        /// 儲存地點 【RFC2】
        /// </summary>
        public string Repository{ get; set; }

        /// <summary>
        /// 儲存地點說明 【RFC2】
        /// </summary>
        public string RepositoryDesc { get; set; }

        /// <summary>
        /// Sales & Distribution 文件 【RFC2】
        /// </summary>
        public string SD { get; set; }
    }
}
