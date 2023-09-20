using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory.lib
{
    internal class SAP
    {
        // RFC 連線配置
        public static RfcConfigParameters GetConfig()
        {
            RfcConfigParameters rfcConfigParameters = new RfcConfigParameters
            {
                { RfcConfigParameters.Name, "DEV" },
                { RfcConfigParameters.AppServerHost, "10.10.1.193" },
                { RfcConfigParameters.SystemNumber, "00" },
                { RfcConfigParameters.SystemID, "DS4" },
                { RfcConfigParameters.User, "S2239002" },
                { RfcConfigParameters.Password, "S2239002" },
                { RfcConfigParameters.Client, "110" },
                { RfcConfigParameters.Language, "ZF" }
            };
            return rfcConfigParameters;
        }

        // 獲取 RfcDestination -> 管理與 SAP 連線的相關工作
        public static RfcDestination GetDestination()
        {
            RfcConfigParameters configParams = GetConfig();
            RfcDestination dest = RfcDestinationManager.GetDestination(configParams);

            return dest;
        }

        // 將 RfcTable 轉成 DataTable
        public static DataTable ConvertRfcTableToDataTable(IRfcTable rfcTable)
        {
            DataTable dataTable = new DataTable();

            // DataTable column definition
            for (int i = 0; i < rfcTable.ElementCount; i++) // ElementCount 行數
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(i); 
                dataTable.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }

            // DataTable rows
            foreach (IRfcStructure currentRow in rfcTable) // for (int rowIdx = 0; rowIdx < rfcTable.RowCount; rowIdx++) RowCount 列數
            {
                DataRow newRow = dataTable.NewRow();
                for (int i = 0; i < currentRow.ElementCount; i++)
                {
                    // 等同於 newRow[i] = currentRow.GetString(i);
                    newRow[rfcTable.GetElementMetadata(i).Name] = currentRow.GetString(rfcTable.GetElementMetadata(i).Name);
                }
                dataTable.Rows.Add(newRow);
            }
            return dataTable;
        }

        // 建立 IRfcStructure 
        public static IRfcStructure CreateRfcStructure(RfcDestination destination, string structureName, Dictionary<string, object> fields)
        {
            IRfcStructure structure = destination.Repository.GetStructureMetadata(structureName).CreateStructure();

            foreach (var field in fields)
            {
                structure.SetValue(field.Key, field.Value);
            }
            return structure;
        }

        // 將 DataTable 轉成 RfcTable
        public static IRfcTable ConvertDataTableToRfcTable(DataTable dataTable, RfcDestination destination, string rfcTableName, string rfcName)
        {
            IRfcFunction function = destination.Repository.CreateFunction(rfcName);
            IRfcTable rfcTable = function.GetTable(rfcTableName);

            // 將 DataTable 的行添加到 RFC 表中
            foreach (DataColumn column in dataTable.Columns)
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(column.ColumnName);
                if (metadata == null)
                {
                    throw new InvalidOperationException($"Column '{column.ColumnName}' not found in RFC table '{rfcTableName}'.");
                }
            }

            // 將DataTable的列添加到RFC表中
            foreach (DataRow dataRow in dataTable.Rows)
            {
                IRfcStructure rfcRow = rfcTable.Metadata.LineType.CreateStructure();

                foreach (DataColumn column in dataTable.Columns)
                {
                    rfcRow.SetValue(column.ColumnName, dataRow[column]);
                }

                rfcTable.Insert(rfcRow);
            }
            return rfcTable;
        }

        // 將 RFC 數據類型轉換為 .NET 數據類型
        private static Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                case RfcDataType.TIME:
                case RfcDataType.UTCLONG:
                case RfcDataType.UTCSECOND:
                case RfcDataType.UTCMINUTE:
                case RfcDataType.DECF16:
                case RfcDataType.DECF34:
                    return typeof(DateTime);

                case RfcDataType.BCD:
                case RfcDataType.FLOAT:
                    return typeof(decimal);

                case RfcDataType.INT1:
                case RfcDataType.INT2:
                case RfcDataType.INT4:
                    return typeof(int);

                case RfcDataType.INT8:
                    return typeof(long);

                case RfcDataType.CHAR:
                case RfcDataType.STRING:
                case RfcDataType.XSTRING:
                    return typeof(string);

                case RfcDataType.NUM:
                    return typeof(decimal);

                case RfcDataType.BYTE:
                    return typeof(byte[]);

                default:
                    return typeof(string);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------



























    }
}
