using RFC_shippingHistory.Struct.Interface;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory.Table
{
    internal class IT_HEAD : IStructBasic<IT_HEAD>
    {
        public string ID { get; set; }
        public string NAME { get; set; }

        public IT_HEAD GetT(IRfcStructure rfcStructure)
        {
            return new IT_HEAD()
            {
                ID = rfcStructure.GetString("ID"),
                NAME = rfcStructure.GetString("Name"),
            };
        }
    }
}

