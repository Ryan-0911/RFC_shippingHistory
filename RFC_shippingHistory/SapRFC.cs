using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC_shippingHistory
{
    internal partial class Form1
    {
        /// <summary>
        /// 呼叫 RFC【Z_SUMEEKO_001_LAA】，獲取使用者代號 (CustomerCode)
        /// </summary>
        /// <returns></returns>
        private void getCustomerCode()
        {
            // 清空進度條
            int total = listPlex.Count;
            int i = 0;
            pgBar.Value = 0;

            try
            {
                string FunctionName = "Z_SUMEEKO_005_PLEXTOBP";
                lib.Control.ShowLog(tbLog, "開始對應Plex客戶......\r\n");

                RfcDestination rfcDestination = lib.SAP.GetDestination();
                RfcRepository rfcRepository = rfcDestination.Repository;
                // 調用 RFC 函数
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                foreach (ShippingInfo s in listSystemSelect)
                {
                    // 增加進度條
                    lib.Control.ShowPgbar(pgBar, total, i);

                    // 輸入 Customer 參數
                    rfcFunction.SetValue("I_NAME2", s.Customer.Trim().ToUpper());
                    rfcFunction.SetValue("I_TYPE", "X");

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    // 從 RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, $"查詢成功，已對應【{s.Customer}】\r\n");
                        string customerCode = rfcFunction.GetString("E_BPNO");
                        s.CustomerCode = customerCode; // 收貨方【RFC1】
                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, $"無法對應【{s.Customer}】: {rfcFunction.GetString("E_MESSAGE")}\r\n");
                        s.CustomerCode = null;
                        s.ok = false;
                        s.E_MESSAGE_rfc1 = rfcFunction.GetString("E_MESSAGE");
                    }
                    i++;
                    lib.Control.ShowLog(tbLog, "完成! \r\n");
                    lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
                }
            }
            catch (RfcCommunicationException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcCommunicationException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            }
            catch (RfcLogonException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcLogonException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            }
            catch (RfcAbapRuntimeException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcAbapRuntimeException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            catch (RfcAbapBaseException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcAbapBaseException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            catch (Exception ex)
            {
                lib.Control.ShowLog(tbLog, $"Exception: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
        }

        /// <summary>
        /// 呼叫RFC【Z_SUMEEKO_006_PNGETSTK】，取回批次與庫存
        /// </summary>
        /// <returns></returns>
        private void dealWithBatch()
        {
            // 清空進度條
            int total = listPlex.Count;
            int i = 0;
            pgBar.Value = 0;

            try
            {
                string FunctionName = "Z_SUMEEKO_006_PNGETSTK";
                lib.Control.ShowLog(tbLog, "開始查詢庫存......\r\n");

                RfcDestination rfcDestination = lib.SAP.GetDestination();
                // 調用 RFC 函数
                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                foreach (ShippingInfo s in listSystemSelect)
                {
                    // 增加進度條
                    lib.Control.ShowPgbar(pgBar, total, i);

                    // 設置 RFC 函数的輸入參數  (客戶物料號碼、客戶代碼)
                    rfcFunction.SetValue("I_PN_MATNR", s.CPartNo);
                    rfcFunction.SetValue("I_ADDNAME2", s.Customer);
                    rfcFunction.SetValue("I_STYPE", "X");

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    //  RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, $"執行成功，取出【{s.CPartNo}_{s.Customer}】的批次庫存 \r\n");
                        IRfcTable rfcTable = rfcFunction.GetTable("ET_ZSDT012");
                        DataTable dataTable = lib.SAP.ConvertRfcTableToDataTable(rfcTable);

                        // Condition 1: 預防取出的KBETR(金額)跟KPEIN(條件定價單位)為0
                        bool priceZero = dataTable.AsEnumerable().Any(row => Convert.ToDecimal(row.Field<string>("KPEIN")) == 0);
                        if (priceZero == true)
                        {
                            s.ok = false; // 兩個RFC的資料是否都有查詢到【made】
                            s.E_MESSAGE_rfc2 = "價格主檔尚未維護";

                            // 將所查到的批次庫存從DataTable轉成list
                            foreach (DataRow dr in dataTable.AsEnumerable())
                            {
                                s.PartNo = dr["MATNR"].ToString(); // 物料號碼【RFC2】
                                s.Repository = dr["LGORT"].ToString(); // 儲存地點【RFC2】
                                s.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明【RFC2】

                                // 庫存狀態物件
                                Inventory iv = new Inventory();
                                iv.NetUnitPrice = 0; // 金額
                                iv.Currency = dr["KONWA"].ToString(); // 幣別
                                iv.MPC = Convert.ToInt32(dr["KPEIN"]); // 條件定價單位
                                iv.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                iv.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                iv.BatchTaken = 0;  // 取用的庫存
                                iv.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                iv.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                s.inventory.Add(iv);
                            }
                            rfcTable.Clear();

                            // 增加進度變數，遍歷下筆listShippingInfo元素
                            i++;
                            lib.Control.ShowLog(tbLog, "完成! \r\n");
                            lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
                            continue;
                        }

                            // Condition 2: Sap庫存總和不足Plex的出貨數****************************************************************************************
                            // Condition 2-1: 若Sap庫存本身總和不足Plex的出貨數
                            Single totalInventoryWithoutPrice = dataTable.AsEnumerable()
                                       .Sum(row => Convert.ToSingle(row.Field<string>("LABST")));

                        if (totalInventoryWithoutPrice < s.Quantity)
                        {
                            s.ok = false; // 兩個RFC的資料是否都有查詢到【made】
                            s.E_MESSAGE_rfc2 = "庫存數量不足";
                            
                            // 將所查到的批次庫存從DataTable轉成list
                            foreach (DataRow dr in dataTable.AsEnumerable())
                            {
                                s.PartNo = dr["MATNR"].ToString(); // 物料號碼【RFC2】
                                s.Repository = dr["LGORT"].ToString(); // 儲存地點【RFC2】
                                s.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明【RFC2】

                                // 庫存狀態物件
                                Inventory iv = new Inventory(); 
                                iv.NetUnitPrice = Convert.ToDecimal(dr["KBETR"]) / Convert.ToInt32(dr["KPEIN"]) / 1000; // 金額
                                iv.Currency = dr["KONWA"].ToString(); // 幣別
                                iv.MPC = Convert.ToInt32(dr["KPEIN"]); // 條件定價單位
                                iv.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                iv.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                iv.BatchTaken = 0;  // 取用的庫存
                                iv.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                iv.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                s.inventory.Add(iv);
                            }
                            rfcTable.Clear();

                            // 增加進度變數，遍歷下筆listShippingInfo元素
                            i++;
                            lib.Control.ShowLog(tbLog, "完成! \r\n");
                            lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
                            continue;
                        }

                        // Condition 2-2: 加入單位淨價條件才導致庫存不足
                        Single totalInventoryWithPrice = dataTable.AsEnumerable()
                                        .Where(row => Math.Abs((Convert.ToDecimal(row.Field<string>("KBETR")) / Convert.ToDecimal(row.Field<string>("KPEIN")) / 1000) - s.NetUnitPrice) < 0.003M)
                                        .Sum(row => Convert.ToSingle(row.Field<string>("LABST")));

                        if (totalInventoryWithPrice < s.Quantity)
                        {
                            s.PartNo = null; // 物料號碼【RFC2】
                            s.Repository = null; // 儲存地點【RFC2】
                            s.RepositoryDesc = null; // 儲存地點說明【RFC2】
                            s.ok = false; // 兩個RFC的資料是否都有查詢到【made】
                            s.E_MESSAGE_rfc2 = "庫存數量不足(單位淨價問題)";

                            // 將所查到的批次庫存從DataTable轉成list
                            foreach (DataRow dr in dataTable.AsEnumerable())
                            {
                                s.PartNo = dr["MATNR"].ToString(); // 物料號碼【RFC2】
                                s.Repository = dr["LGORT"].ToString(); // 儲存地點【RFC2】
                                s.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明【RFC2】

                                // 庫存狀態物件
                                Inventory iv = new Inventory(); // 庫存狀態物件
                                iv.NetUnitPrice = Convert.ToDecimal(dr["KBETR"]) / Convert.ToInt32(dr["KPEIN"]) / 1000; // 金額
                                iv.Currency = dr["KONWA"].ToString(); // 幣別
                                iv.MPC = Convert.ToInt32(dr["KPEIN"]); // 條件定價單位
                                iv.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                iv.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                iv.BatchTaken = 0;  // 取用的庫存
                                iv.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                iv.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                s.inventory.Add(iv);
                            }
                            rfcTable.Clear();

                            // 增加進度變數，遍歷下筆listShippingInfo元素
                            i++;
                            lib.Control.ShowLog(tbLog, "完成! \r\n");
                            lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
                            continue;
                        }

                        // Condition 3: Sap庫存總和足夠Plex的出貨數****************************************************************************************

                        // Sap庫存累加額
                        float SapInventory = 0;
                        // datatable 第幾列
                        int cnt = 0;
                        // Sap庫存累加額是否已超過Plex出貨數
                        bool above = false;

                        // 將所查到的批次庫存從DataTable轉成list
                        foreach (DataRow dr in dataTable.AsEnumerable())
                        {
                            // 先設定共同的屬性
                            s.PartNo = dr["MATNR"].ToString(); // 物料號碼【RFC2】
                            s.Repository = dr["LGORT"].ToString(); // 儲存地點【RFC2】
                            s.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明【RFC2】
                            cnt++;

                            // 計算Plex單位淨跟Sap單位淨額的差距
                            Decimal SapNetUnitPrice = Convert.ToDecimal(dr["KBETR"]) / Convert.ToInt32(dr["KPEIN"]) / 1000;
                            Decimal differ;
                            if (SapNetUnitPrice > s.NetUnitPrice)
                            {
                                differ = SapNetUnitPrice - s.NetUnitPrice;
                            }
                            else
                            {
                                differ = s.NetUnitPrice - SapNetUnitPrice;
                            }

                            // 若 (Plex單位淨價跟Sap單位淨價差距大於0.003美元) 或 (Sap庫存累加額已超過Plex出貨數)
                            if (differ > 0.003M || above == true)
                            {
                                Inventory iv = new Inventory(); // 庫存狀態物件
                                iv.NetUnitPrice = SapNetUnitPrice; // Sap單位淨價
                                iv.Currency = dr["KONWA"].ToString(); // 幣別
                                iv.MPC = Convert.ToInt32(dr["KPEIN"]); // 條件定價單位
                                iv.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                iv.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                iv.BatchTaken = 0;  // 取用的庫存
                                iv.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                iv.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                s.inventory.Add(iv);
                                continue;
                            }

                            // 逐列累加Sap批次庫存
                            SapInventory += Convert.ToSingle(dr["LABST"]);
                            // 若累加數量大於等於s.Quantity
                            if (SapInventory >= s.Quantity)
                            {
                                Inventory iv = new Inventory(); // 庫存狀態物件
                                iv.NetUnitPrice = SapNetUnitPrice; // Sap單位淨價
                                iv.Currency = dr["KONWA"].ToString(); // 幣別
                                iv.MPC = Convert.ToInt32(dr["KPEIN"]); // 條件定價單位
                                iv.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                iv.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                // 若第一筆就大於等於s.Quantity
                                if (cnt == 1)
                                {
                                    iv.BatchTaken = s.Quantity;
                                }
                                else
                                {
                                    float bt = s.Quantity - (SapInventory - Convert.ToSingle(dr["LABST"]));
                                    string str = bt.ToString("N4");
                                    float.TryParse(str, out bt);
                                    iv.BatchTaken = bt;
                                }
                                iv.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                iv.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                s.inventory.Add(iv);
                                // 設定Sap庫存累加額已超過Plex出貨數
                                above = true;
                            }
                            else
                            {
                                Inventory iv = new Inventory(); // 庫存狀態物件
                                iv.NetUnitPrice = SapNetUnitPrice; // Sap單位淨價
                                iv.Currency = dr["KONWA"].ToString(); // 幣別
                                iv.MPC = Convert.ToInt32(dr["KPEIN"]); // 條件定價單位
                                iv.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                iv.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                iv.BatchTaken = Convert.ToSingle(dr["LABST"]);  // 取用的庫存
                                iv.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                iv.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                s.inventory.Add(iv);
                            }
                        }
                        rfcTable.Clear();
                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, $"查無【{s.CPartNo}_{s.Customer}】的批次庫存: {rfcFunction.GetString("E_MESSAGE")} \r\n");

                        // RFC2
                        s.PartNo = null; // 物料號碼
                        s.Repository = null; // 儲存地點
                        s.RepositoryDesc = null; // 儲存地點說明
                        s.ok = false;
                        s.E_MESSAGE_rfc2 = rfcFunction.GetString("E_MESSAGE");
                    }
                    i++;
                    lib.Control.ShowLog(tbLog, "完成! \r\n");
                    lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
                }
            }
            catch (RfcCommunicationException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcCommunicationException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            }
            catch (RfcLogonException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcLogonException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            }
            catch (RfcAbapRuntimeException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcAbapRuntimeException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            catch (RfcAbapBaseException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcAbapBaseException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            catch (Exception ex)
            {
                lib.Control.ShowLog(tbLog, $"Exception: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
        }

        /// <summary>
        /// 呼叫【Z_SUMEEKO_008_SHIPMENTPOST】，寫入出貨單
        /// </summary>
        private void WriteShippingHistory()
        {
            // 清空進度條
            int i = 0;
            pgBar.Value = 0;

            try
            {
                string FunctionName = "Z_SUMEEKO_008_SHIPMENTPOST";
                lib.Control.ShowLog(tbLog, "開始寫入出貨單......\r\n");

                RfcDestination rfcDestination = lib.SAP.GetDestination();
                RfcRepository rfcRepository = rfcDestination.Repository;
                // 調用 RFC 函数
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                // 篩選出要寫入的資料 (RFC1與RFC2都要查詢到)
                List<ShippingInfo> ResultSapSuccessOrderedByShipperNo = (from exp in listSystemSelect
                                                                         where exp.ok == true 
                                                                         orderby exp.ShipperNo 
                                                                         select exp).ToList();

                // 設置 RFC 參數
                IRfcTable rfcTable;
                foreach (ShippingInfo sh in ResultSapSuccessOrderedByShipperNo)
                {
                    lib.Control.ShowPgbar(pgBar, ResultSapSuccessOrderedByShipperNo.Count, i);

                    rfcTable = rfcFunction.GetTable("ET_ZSDT014");

                    foreach (Inventory iv in sh.inventory)
                    {
                        if (iv.BatchTaken <= 0) { continue; }
                        rfcTable.Insert();
                        rfcTable.CurrentRow.SetValue("MANDT", ""); // 用戶端 
                        rfcTable.CurrentRow.SetValue("CHARG", iv.BatchNo); // 批次號碼
                        rfcTable.CurrentRow.SetValue("MATNR", sh.PartNo); // 物料號碼
                        rfcTable.CurrentRow.SetValue("LGMNG", iv.BatchTaken); // 評價的未限制使用庫存
                    }

                    // 如果當前不是最後一筆ShippingInfo
                    if (ResultSapSuccessOrderedByShipperNo.IndexOf(sh) != ResultSapSuccessOrderedByShipperNo.Count - 1)
                    {
                        // 如果與下一筆 ShippingInfo 的銷項交貨一樣
                        if (ResultSapSuccessOrderedByShipperNo[ResultSapSuccessOrderedByShipperNo.IndexOf(sh) + 1].ShipperNo == sh.ShipperNo)
                        {
                            continue;
                        }
                    }

                    //DataTable dt = lib.SAP.ConvertRfcTableToDataTable(rfcTable);

                    // 傳入參數，table類型
                    rfcFunction.SetValue("ET_ZSDT014", rfcTable);

                    // 傳入參數，字符串類型 
                    rfcFunction.SetValue("I_VBELN", sh.ShipperNo);

                    // 傳入參數，字符串類型 
                    rfcFunction.SetValue("I_POST", "X");

                    // 傳入參數，字符串類型 
                    rfcFunction.SetValue("I_WADAT_IST", Convert.ToDateTime(sh.ShipDate));

                    rfcFunction.SetParameterActive(0, true);

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    // 從 RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, $"成功寫入【{sh.ShipperNo}】銷貨交項 \r\n");
                        listSystemSelect[listSystemSelect.IndexOf(sh)].E_MESSAGE_rfc3 = "寫入成功!";

                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, $"無法寫入【{sh.ShipperNo}】銷貨交項: {rfcFunction.GetString("E_MESSAGE")} \r\n");
                        listSystemSelect[listSystemSelect.IndexOf(sh)].E_MESSAGE_rfc3 = rfcFunction.GetString("E_MESSAGE");
                    }
                    i++;
                    lib.Control.ShowLog(tbLog, "完成! \r\n");
                    lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

                    // 清空table參數
                    rfcTable.Clear();
                }
            }
            catch (RfcCommunicationException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcCommunicationException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            }
            catch (RfcLogonException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcLogonException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            }
            catch (RfcAbapRuntimeException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcAbapRuntimeException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            catch (RfcAbapBaseException ex)
            {
                lib.Control.ShowLog(tbLog, $"RfcAbapBaseException: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            catch (Exception ex)
            {
                lib.Control.ShowLog(tbLog, $"Exception: {ex.Message.ToString()} \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
        }
    }
}