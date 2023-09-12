using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory.Struct
{
    public class SapCommonProcess
    {
        public IRfcFunction IRfcUnitTransfer { get; private set; }
        public RfcDestination dest { get; private set; }
        public RfcRepository repo { get; private set; }
        public SapCommonProcess(string strCreateFunction)
        {
            SapLogin.SapConnect(out RfcDestination dest, out RfcRepository repo); // 參數設置
            IRfcUnitTransfer = repo.CreateFunction(strCreateFunction); // 獲取 RFC
            this.dest = dest;
            this.repo = repo;
        }

        public IRfcTable GetTable(string strTableName) // 獲取 table 結構
        {
            return IRfcUnitTransfer.GetTable(strTableName);
        }

        public void SetValue(string strFirst, string strSec)  // 傳入參數給 RFC (key + value)
        {
            IRfcUnitTransfer.SetValue(strFirst, strSec);
        }

        public string GetValue(string strKey) // 用 key 獲取返回值 value
        {
            string output = IRfcUnitTransfer.GetValue(strKey).ToString();
            return output;
        }


        public string GetString(string strGetString)  
        {
            string result = IRfcUnitTransfer.GetString(strGetString);
            return result;
        }

        public void Invoke()  //執行 RFC
        {
            IRfcUnitTransfer.Invoke(dest);
        }
    }
}