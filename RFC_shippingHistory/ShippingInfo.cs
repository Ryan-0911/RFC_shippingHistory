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
        /// 料號
        /// </summary>
        public string PartNo { get; set; }
        /// <summary>
        /// 出貨號碼
        /// </summary>
        public string ShipperNo { get; set; }

        /// <summary>
        /// 客戶地址
        /// </summary>
        public string CustomerAddressCode { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; } 
    }
}
