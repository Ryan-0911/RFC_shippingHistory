using RFC_shippingHistory.Struct;
using RFC_shippingHistory.Struct.Interface;
using RFC_shippingHistory.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory.RFC
{
    public class Z_SUMEEKO_001_LAA : ISapLogin
    {
        public SapCommonProcess sapCommonProcess { get; set; }
        List<IT_HEAD> list_IT_HEAD { get; set; }
        public Z_SUMEEKO_001_LAA()
        {
            sapCommonProcess = new SapCommonProcess("Z_SUMEEKO_001_LAA");
            sapCommonProcess.Invoke();
            list_IT_HEAD = new StructElement<IT_HEAD>(sapCommonProcess.GetTable("IT_HEAD")).ToList;
        }   

    }
}
