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
            lib.Control.ShowPgbar(pgBar, 0, 0);

            try
            {
                string FunctionName = "Z_SUMEEKO_005_PLEXTOBP";
                lib.Control.ShowLog(tbLog, "開始對應客戶地址......\r\n");

                RfcDestination rfcDestination = lib.SAP.GetDestination();
                RfcRepository rfcRepository = rfcDestination.Repository;
                // 調用 RFC 函数
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                foreach (ShippingInfo s in listPlex)
                {
                    lib.Control.ShowPgbar(pgBar, total, i);

                    //輸入 CustomerAddressCode 參數
                    rfcFunction.SetValue("I_NAME2", s.CustomerAddressCode.Trim().ToUpper());
                    //rfcFunction.SetValue("I_NAME2", "DURA AUTOMOTIVE");

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    // 從 RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, $"查詢成功，已對應【{s.CustomerAddressCode}】\r\n");
                        string customerCode = rfcFunction.GetString("E_BPNO");
                        // 將「收貨方」存入 ShippingInfo model
                        s.CustomerCode = customerCode;
                        //Console.WriteLine(customerCode);
                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, $"查詢失敗，無法對應【{s.CustomerAddressCode}】\r\n");
                        // 若查不到CustomerCode，就將CustomerCode設為null
                        s.CustomerCode = null;
                        s.ok = false;
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
        /// 處理批次
        /// </summary>
        /// <returns></returns>
        private void dealWithBatch()
        {
            // 清空進度條
            int total = listPlex.Count;
            int i = 0;
            pgBar.Value = 0;

            // 用來存放查詢到的所有庫存
            List<ShippingInfo> listAllInventory = new List<ShippingInfo>();

            // 用來存放系統所選的庫存
            List<ShippingInfo> listSystemInventory = new List<ShippingInfo>();

            try
            {
                string FunctionName = "Z_SUMEEKO_006_PNGETSTK";
                lib.Control.ShowLog(tbLog, "開始查詢庫存......\r\n");

                RfcDestination rfcDestination = lib.SAP.GetDestination();
                // 調用 RFC 函数
                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                foreach (ShippingInfo s in listPlex)
                {
                    // 進度條
                    lib.Control.ShowPgbar(pgBar, total, i);

                    // 清空前筆批次庫存
                    listAllInventory.Clear();
                    listSystemInventory.Clear();

                    // 設置 RFC 函数的輸入參數  (客戶物料號碼、客戶地址)
                    rfcFunction.SetValue("I_PN_MATNR", s.CPartNo);
                    rfcFunction.SetValue("I_ADDNAME2", s.CustomerAddressCode);
                    //rfcFunction.SetValue("I_PN_MATNR", "20227041");
                    //rfcFunction.SetValue("I_ADDNAME2", "DURA AUTOMOTIVE");

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    //  RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, $"執行成功，取出【{s.CPartNo}_{s.CustomerAddressCode}】的批次庫存 \r\n");
                        IRfcTable rfcTable = rfcFunction.GetTable("ET_ZSDT012");
                        DataTable dataTable = lib.SAP.ConvertRfcTableToDataTable(rfcTable);

                        // 所有的批次庫存-----------------------------------------------------------------------------------------------------------------------------
                        foreach (DataRow dr in dataTable.AsEnumerable())
                        {
                            ShippingInfo sh = new ShippingInfo();
                            sh.CPartNo = s.CPartNo;
                            sh.ShipperNo = s.ShipperNo;
                            sh.CustomerAddressCode = s.CustomerAddressCode;
                            sh.Quantity = s.Quantity;
                            sh.ShipDate = s.ShipDate;
                            sh.ok = s.ok;
                            // RFC1
                            sh.CustomerCode = s.CustomerCode;
                            // RFC2
                            sh.PartNo = dr["MATNR"].ToString(); // 物料號碼
                            sh.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                            sh.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                            sh.Repository = dr["LGORT"].ToString(); // 儲存地點
                            sh.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明
                            sh.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                            sh.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                            listAllInventory.Add(sh);
                        }
                        listAllOptions.AddRange(listAllInventory);


                        // 系統挑選的批次庫存 (先進先出)-----------------------------------------------------------------------------------------------------------------------------

                        // 先確認Sap未限制使用庫存總數是否大於等於Plex出貨數
                        //Console.WriteLine(dataTable.Columns["LABST"].DataType);
                        Single totalInventory = dataTable.AsEnumerable()
                                       .Sum(row => Convert.ToSingle(row.Field<string>("LABST")));
                        // 若不足Plex的出貨數，直接視為RFC資料不齊全
                        if (totalInventory < s.Quantity)
                        {
                            ShippingInfo sh = new ShippingInfo();
                            sh.CPartNo = s.CPartNo;
                            sh.ShipperNo = s.ShipperNo;
                            sh.CustomerAddressCode = s.CustomerAddressCode;
                            sh.Quantity = s.Quantity;
                            sh.ShipDate = s.ShipDate;
                            // RFC1
                            sh.CustomerCode = s.CustomerCode;
                            // RFC2
                            sh.PartNo = null; // 物料號碼
                            sh.BatchNo = null; // 批次號碼
                            sh.BatchAmount = 0; // 未限制使用庫存
                            sh.Repository = null; // 儲存地點
                            sh.RepositoryDesc = null; // 儲存地點說明
                            sh.SD = null; // 銷售與配銷文件號碼
                            sh.SD_date = null; // 銷售與配銷文件日期
                            sh.ok = false;
                            listSystemSelect.Add(sh);

                            // 遍歷下筆listShippingInfo元素
                            i++;
                            lib.Control.ShowLog(tbLog, "完成! \r\n");
                            lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
                            continue;
                        }

                        // Plex 出貨數量
                        float SapInventory = 0;

                        // 將所查到的批次庫存從DataTable轉成list
                        foreach (DataRow dr in dataTable.AsEnumerable())
                        {
                            // 逐列累加Sap批次庫存
                            SapInventory += Convert.ToSingle(dr["LABST"]);

                            // 若累加數量超過s.Quantity，將該列加入至listSystemInventory並跳出foreach
                            if (SapInventory >= s.Quantity)
                            {
                                ShippingInfo sh = new ShippingInfo();
                                sh.CPartNo = s.CPartNo;
                                sh.ShipperNo = s.ShipperNo;
                                sh.CustomerAddressCode = s.CustomerAddressCode;
                                sh.Quantity = s.Quantity;
                                sh.ShipDate = s.ShipDate;
                                sh.ok = s.ok;
                                // RFC1
                                sh.CustomerCode = s.CustomerCode;
                                // RFC2
                                sh.PartNo = dr["MATNR"].ToString(); // 物料號碼
                                sh.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                sh.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                sh.Repository = dr["LGORT"].ToString(); // 儲存地點
                                sh.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明
                                sh.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                sh.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                listSystemInventory.Add(sh);
                                break;
                            }
                            else
                            {
                                ShippingInfo sh = new ShippingInfo();
                                sh.CPartNo = s.CPartNo;
                                sh.ShipperNo = s.ShipperNo;
                                sh.CustomerAddressCode = s.CustomerAddressCode;
                                sh.Quantity = s.Quantity;
                                sh.ShipDate = s.ShipDate;
                                sh.ok = s.ok;
                                // RFC1
                                sh.CustomerCode = s.CustomerCode;
                                // RFC2
                                sh.PartNo = dr["MATNR"].ToString(); // 物料號碼
                                sh.BatchNo = dr["CHARG"].ToString(); // 批次號碼
                                sh.BatchAmount = Convert.ToSingle(dr["LABST"]); // 未限制使用庫存
                                sh.Repository = dr["LGORT"].ToString(); // 儲存地點
                                sh.RepositoryDesc = dr["LGOBE"].ToString(); // 儲存地點說明
                                sh.SD = dr["VBELN"].ToString(); // 銷售與配銷文件號碼
                                sh.SD_date = Convert.ToDateTime(dr["ERSDA"]); // 銷售與配銷文件號碼日期
                                listSystemInventory.Add(sh);
                            }
                        }
                        listSystemSelect.AddRange(listSystemInventory);
                        rfcTable.Clear();
                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, $"執行失敗，查不到【{s.CPartNo}_{s.CustomerAddressCode}】的批次庫存 \r\n");
                        ShippingInfo sh = new ShippingInfo();
                        sh.CPartNo = s.CPartNo;
                        sh.ShipperNo = s.ShipperNo;
                        sh.CustomerAddressCode = s.CustomerAddressCode;
                        sh.Quantity = s.Quantity;
                        sh.ShipDate = s.ShipDate;
                        // RFC1
                        sh.CustomerCode = s.CustomerCode;
                        // RFC2
                        sh.PartNo = null; // 物料號碼
                        sh.BatchNo = null; // 批次號碼
                        sh.BatchAmount = 0; // 未限制使用庫存
                        sh.Repository = null; // 儲存地點
                        sh.RepositoryDesc = null; // 儲存地點說明
                        sh.SD = null; // 銷售與配銷文件號碼
                        sh.ok = false;
                        listAllOptions.Add(sh);
                        listSystemSelect.Add(sh);
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
    }
}