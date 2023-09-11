using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory.Struct
{
    internal class SapCommonProcess
    {
        public IRfcFunction IRfcUnitTransfer { get; private set; }
        public RfcDestination dest { get; private set; }
        public RfcRepository repo { get; private set; }
        public SapCommonProcess(string strCreateFunction)
        {
            SapLogin.SapConnect(out RfcDestination dest, out RfcRepository repo);
            IRfcUnitTransfer = repo.CreateFunction(strCreateFunction);
            this.dest = dest;
            this.repo = repo;
        }

        public IRfcTable GetTable(string strTableName)
        {
            return IRfcUnitTransfer.GetTable(strTableName);
        }

        public void SetValue(string strFirst, string strSec)
        {
            IRfcUnitTransfer.SetValue(strFirst, strSec);
        }

        public IRfcTable GetTable(string strTableName)
        {
            return IRfcUnitTransfer.GetTable(strTableName);
        }

        public IRfcTable GetTable(string strTableName)
        {
            return IRfcUnitTransfer.GetTable(strTableName);
        }

        public IRfcTable GetTable(string strTableName)
        {
            return IRfcUnitTransfer.GetTable(strTableName);
        }

        public voiud Invoke(string strTableName)
        {
            return IRfcUnitTransfer.GetTable(strTableName);
        }
    }
}
