using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory
{
    internal static class SapLogin
    {
        public static void SapConnect(out RfcDestination dest, out RfcRepository repo) 
        {
            RfcConfigParameters args = new RfcConfigParameters();
            args.Add(RfcConfigParameters.Name, "");
            args.Add(RfcConfigParameters.AppServerHost, "");
            args.Add(RfcConfigParameters.SystemNumber, "");
            args.Add(RfcConfigParameters.SystemID, "");
            args.Add(RfcConfigParameters.User, "");
            args.Add(RfcConfigParameters.Password, "");
            args.Add(RfcConfigParameters.Client, "");
            args.Add(RfcConfigParameters.Language, "ZF");

           dest = RfcDestinationManager.GetDestination(args); 
           repo = dest.Repository;
        }
    }
}
