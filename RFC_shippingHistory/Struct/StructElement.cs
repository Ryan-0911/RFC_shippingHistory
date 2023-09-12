using RFC_shippingHistory.Struct.Interface;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory.Struct
{
    // 將 SAP RFC 表格轉換為指定類型 T 的 List　
    public class StructElement<T> where T : IStructBasic<T>, new() // T必須實現IStructBasic<T>介面且有預設(無參數)建構子
    {
        public List<T> ToList { get; set; }
        public StructElement(IRfcTable _1stRfcTable) 
        { 
            T t = new T();
            ToList = new List<T>();
            foreach(var item in _1stRfcTable)
            {
                ToList.Add(t.GetT(item));
            }
        }
    }
}
