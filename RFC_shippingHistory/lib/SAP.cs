﻿using SAP.Middleware.Connector;
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
        /// <summary>
        /// RFC 連線配置
        /// </summary>
        /// <returns></returns>
        public static RfcConfigParameters GetConfig()
        {
            RfcConfigParameters rfcConfigParameters = new RfcConfigParameters
            {
                { RfcConfigParameters.Name, "dev" },
                { RfcConfigParameters.AppServerHost, "172.16.2.23" },
                { RfcConfigParameters.SystemNumber, "00" },
                { RfcConfigParameters.SystemID, "DS4" },
                { RfcConfigParameters.User, "SALES_OEM" },
                { RfcConfigParameters.Password, "Su2254@oem" },
                { RfcConfigParameters.Client, "330" },
                { RfcConfigParameters.Language, "ZF" }

                //{ RfcConfigParameters.Name, "dev" },
                //{ RfcConfigParameters.AppServerHost, "172.16.2.166" },
                //{ RfcConfigParameters.SystemNumber, "00" },
                //{ RfcConfigParameters.SystemID, "PS4" },
                //{ RfcConfigParameters.User, "SALES_OEM" },
                //{ RfcConfigParameters.Password, "Su2254@oem" },
                //{ RfcConfigParameters.Client, "800" },
                //{ RfcConfigParameters.Language, "ZF" }
            };
            return rfcConfigParameters;
        }

        /// <summary>
        /// 獲取 RfcDestination (管理與 SAP 連線的相關工作)
        /// </summary>
        /// <returns></returns>
        public static RfcDestination GetDestination()
        {
            RfcConfigParameters configParams = GetConfig();
            RfcDestination dest = RfcDestinationManager.GetDestination(configParams);

            return dest;
        }

        /// <summary>
        /// 將 RfcTable 轉成 DataTable
        /// </summary>
        /// <param name="rfcTable"></param>
        /// <returns></returns>
        public static DataTable ConvertRfcTableToDataTable(IRfcTable rfcTable)
        {
            DataTable dataTable = new DataTable();

            // DataTable column definition
            for (int i = 0; i < rfcTable.ElementCount; i++) // ElementCount 行數
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(i); 
                dataTable.Columns.Add(metadata.Name); // ,GetDataType(metadata.DataType)
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

        /// <summary>
        /// 將 DataTable 轉成 RfcTable
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="destination"></param>
        /// <param name="rfcTableName"></param>
        /// <param name="rfcName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// 將 RFC 數據類型轉換為 .NET 數據類型
        /// </summary>
        /// <param name="rfcDataType"></param>
        /// <returns></returns>
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
    }
}
