using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace RFC_shippingHistory
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 先將 Plex Excel 加入 ShipperInfo Model 欄位，後續再透過呼叫 RFC 將 ShipperInfo Model 剩餘欄位補齊
        /// </summary>
        List<ShippingInfo> listShippingInfo = new List<ShippingInfo>();

        /// <summary>
        /// 無法湊齊批次的 CPartNo (匯出的 Excel 以紅色背景顯示)
        /// </summary>
        List<string> listCPartNo = new List<string>();

        /// <summary>
        /// 存放編輯頁面
        /// </summary>
        DataTable dtEdit = new DataTable();

        /// <summary>
        /// 存放最終出貨單 (1. 匯出 Excel、2. 寫進 SAP 出貨單) 
        /// </summary>
        DataTable dtResult = new DataTable();

        /// <summary>
        /// 編輯畫面的當前頁數
        /// </summary>
        int currentPage = 1;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            dtEdit.Columns.Add("銷項交貨");
            dtEdit.Columns.Add("收貨方");
            dtEdit.Columns.Add("實際發貨日期");
            dtEdit.Columns.Add("物料");
            dtEdit.Columns.Add("客戶物料號碼");
            dtEdit.Columns.Add("交貨數量");
            dtEdit.Columns.Add("單位");
            dtEdit.Columns.Add("批次");
            dtEdit.Columns.Add("儲存地點");
            dtEdit.Columns.Add("說明");

            // Test: 批次狀況---------------------------------------------------------------------------------------
            // 建立假資料 (呼叫 RFC2 回傳的結果)
            //DataTable dtBatch = new DataTable();
            //dtBatch.Columns.Add("MANDT"); // 用戶端
            //dtBatch.Columns.Add("VBELN"); // 銷售與配銷文件號碼
            //dtBatch.Columns.Add("MATNR"); // 物料號碼
            //dtBatch.Columns.Add("AUART"); // 銷售文件類型
            //dtBatch.Columns.Add("KUNNR"); // 客戶號碼
            //dtBatch.Columns.Add("BSTNK"); // 客戶參考
            //dtBatch.Columns.Add("KDMAT"); // 客戶物料
            //dtBatch.Columns.Add("LGORT"); // 儲存地點
            //dtBatch.Columns.Add("MEINH"); // 作業的計量單位
            //dtBatch.Columns.Add("LABST"); // 評價的未限制使用庫存
            //dtBatch.Columns.Add("UMLME"); // 移轉中庫存（工廠到工廠）
            //dtBatch.Columns.Add("INSME"); // 品質檢驗中的庫存
            //dtBatch.Columns.Add("EINME"); // 所有限制批次的總計庫存
            //dtBatch.Columns.Add("SPEME"); // 凍結庫存
            //dtBatch.Columns.Add("RETME"); // 凍結庫存退貨
            //dtBatch.Columns.Add("CHARG"); // 批次號碼
            //dtBatch.Columns.Add("GROES"); // Dimensions
            //dtBatch.Columns.Add("MAKTX"); // 物料說明
            //dtBatch.Columns.Add("LGOBE"); // 儲存地點說明

            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");
            //dtBatch.Rows.Add("330", "SD12345", "DFVSDVDVFGVFB", null, "1GMNAO", null, "15032594", "UF01", "MPC", 0.6000, 0.000, 0.000, 0.000, 0.000, "Z00001043", null, "梅花孔六角輪緣頭", "美國成品倉");

            //dgvTest.DataSource = dtBatch;
        }

        private void iconFolder_Click(object sender, EventArgs e)
        {
            ImportFolder();
        }

        private void iconFile_Click(object sender, EventArgs e)
        {
            ImportFile();
        }

        private void iconClear_Click(object sender, EventArgs e)
        {
            tbLog.Text = "";
        }

        private void iconWrite2SAP_Click(object sender, EventArgs e)
        {
            // Step 1: 取得 CustomerCode
            getCustomerCode();

            // Step 2: 取得某出貨單中某物料的批次狀況、顯示在DataGridViewe供使用者檢視
            dealWithBatch();

            // Step 3: 寫進 SAP VL01N 出貨單

        }

        /// <summary>
        /// 點擊匯出Excel按鍵的觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconExport_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtResult);
                    string directoryPath = dialog.SelectedPath;
                    ExportDataSetToExcel(ds, directoryPath);
                }
            }
        }

        /// <summary>
        /// 修改系統選好的批號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconEdit_Click(object sender, EventArgs e)
        {

        }

        private void icobFirst_Click(object sender, EventArgs e)
        {
            if (listShippingInfo.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = 1;
            refreshPage();
        }

        private void iconPrevous_Click(object sender, EventArgs e)
        {
            if (listShippingInfo.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = currentPage > 1 ? --currentPage : 1;
            refreshPage();
        }

        private void iconNext_Click(object sender, EventArgs e)
        {
            if (listShippingInfo.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = currentPage < listShippingInfo.Count ? ++currentPage : listShippingInfo.Count;
            refreshPage();
        }

        private void iconLast_Click(object sender, EventArgs e)
        {
            if (listShippingInfo.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = listShippingInfo.Count;
            refreshPage();
        }

        /// <summary>
        /// 重整編輯頁面
        /// </summary>
        private void refreshPage()
        {
            dtEdit.Clear();
            // 客戶物料號碼一樣的要顯示在同一頁上
            while (currentPage - 1 > 1 && listShippingInfo[currentPage - 1] == listShippingInfo[currentPage - 2] )
            {
                DataRow dr = dtEdit.NewRow();
                dr["銷項交貨"] = listShippingInfo[currentPage - 1].ShipperNo;
                dr["收貨方"] = listShippingInfo[currentPage - 1].CustomerCode;
                dr["實際發貨日期"] = listShippingInfo[currentPage - 1].ShipDate;
                dr["物料"] = listShippingInfo[currentPage - 1].PartNo;
                dr["客戶物料號碼"] = listShippingInfo[currentPage - 1].CPartNo;
                dr["交貨數量"] = listShippingInfo[currentPage - 1].BatchAmount;
                dr["單位"] = "MPC";
                dr["批次"] = listShippingInfo[currentPage - 1].BatchNo;
                dr["儲存地點"] = listShippingInfo[currentPage - 1].Repository;
                dr["說明"] = listShippingInfo[currentPage - 1].RepositoryDesc;
                dtEdit.Rows.Add(dr);
            }
          
            dgvResult.DataSource = dtEdit;
            lblPage.Text = $"第{currentPage}頁/共{listShippingInfo.Count}頁";
        }

        /// <summary>
        /// 動態時間與日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
        }


        /// <summary>
        /// 匯入檔案
        /// </summary>
        protected void ImportFile()
        {
            OpenFileDialog dialog = new OpenFileDialog(); //建立檔案選擇視窗
            dialog.Title = "選擇要匯入的excel檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "xls Files(*.xls; *.xlsx; *.csv)| *.xls; *.xlsx; *.csv";
            string msg = "";
            string xlsfile = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                xlsfile = dialog.FileName;
                msg = $"確定要將 [{xlsfile}] 匯入資料庫嗎?";
                DialogResult ans = MessageBox.Show(msg, "Check Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (ans == DialogResult.OK)
                {
                    ExcelProcessAll(xlsfile);
                }
            }
        }

        /// <summary>
        /// 匯入資料夾中的所有檔案
        /// </summary>
        protected void ImportFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string directoryPath = dialog.SelectedPath;
                    GetFileList(directoryPath);
                }
            }
        }

        /// <summary>
        /// 將資料夾中的所有文件存入資料庫
        /// </summary>
        /// <param name="folderpath"></param>
        protected void GetFileList(string folderpath)
        {
            // 列出所有檔案
            string[] files = Directory.GetFiles(folderpath);
            foreach (string file in files)
            {
                // 只有 .xls、.xlsx、.csv 格式文件才會處理
                if (Path.GetExtension(file).Equals(".xls") || Path.GetExtension(file).Equals(".xlsx") || Path.GetExtension(file).Equals(".csv"))
                {
                    ExcelProcessAll(file);
                    Console.WriteLine(file);
                }
            }

            // 列出所有子目錄
            string[] directories = Directory.GetDirectories(folderpath);
            if (directories.Length > 0)
            {
                foreach (string directory in directories)
                {
                    lib.Control.ShowLog(tbLog, $"目錄: [{folderpath}] \r\n 子目錄: [{directory}] \r\n");
                    GetFileList(directory);
                }
            }
        }

        /// <summary>
        /// 將 .xls、.xlsx、.csv 格式文件存入資料庫中
        /// </summary>
        /// <param name="path"></param>
        protected void ExcelProcessAll(string path)
        {
            string fileid = GetFileID(path);
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
            for (int z = 1; z <= xlWorkbook.Sheets.Count; z++)
            {
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[z];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                lib.Control.ShowPgbar(pgBar, rowCount, 0);
                lib.Control.ShowLog(tbLog, $"找到Excel文件! 開始讀取 [路徑: {path}] \r\n");

                lib.Control.ShowLog(tbLog, $"[工作表數: {z.ToString()}/ 列數: {rowCount.ToString()}/ 行數: {colCount.ToString()}] \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

                for (int i = 2; i <= rowCount; i++)
                {
                    ShippingInfo shippingInfo = new ShippingInfo();
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            switch (j)
                            {
                                case 1:
                                    shippingInfo.CPartNo = xlRange.Cells[i, 1].Value2.ToString();
                                    break;
                                case 3:
                                    shippingInfo.ShipDate = xlRange.Cells[i, 3].Value2.ToString();
                                    break;
                                case 4:
                                    shippingInfo.ShipperNo = xlRange.Cells[i, 4].Value2.ToString();
                                    break;
                                case 6:
                                    shippingInfo.CustomerAddressCode = xlRange.Cells[i, 6].Value2.ToString().Trim().ToUpper();
                                    break;
                                case 7:
                                    shippingInfo.Quantity = Convert.ToInt32(xlRange.Cells[i, 7].Value2);
                                    break;
                            }
                            Save2DB(fileid, GetFieldName(i, j), xlRange.Cells[i, j].Value2.ToString(), z);
                        }
                    }
                    listShippingInfo.Add(shippingInfo);
                    lib.Control.ShowPgbar(pgBar, rowCount, i);
                }

                // 清除資源
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
            }

            // 依照 ShipperNo 欄位排序
            IEnumerable<ShippingInfo> result = from s in listShippingInfo orderby s.ShipperNo select s;
            foreach (ShippingInfo shippingInfo in result)
            {
                Console.WriteLine($"料號: {shippingInfo.CPartNo}、出貨號碼: {shippingInfo.ShipperNo}、客戶地址: {shippingInfo.CustomerAddressCode}、數量: {shippingInfo.Quantity}、出貨日期: {shippingInfo.ShipDate}");
            }
            refreshPage();
            lblPage.Text = $"第1頁/共{result.Count()}頁";

            MessageBox.Show("匯入資料庫完成!", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pgBar.Value = 0;
            xlWorkbook.Close(true);
            Marshal.ReleaseComObject(xlWorkbook);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
            Marshal.FinalReleaseComObject(xlApp);
        }

        /// <summary>
        /// 從 excelFile 和 excelData 表刪除符合傳入路徑的所有資料列，接著重新填入 excelFile 表的所有欄位，回傳 ef_id (外來鍵)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string GetFileID(string path)
        {
            string guid = string.Empty;
            string sql = $"select * from excelFile where path = '{path}'";
            DataTable dt = lib.DB.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                sql = $"delete from excelFile where path = '{path}'; delete from excelData where ef_id='{dt.Rows[0]["ef_id"]}';";
                lib.DB.ExecuteNoParams(sql);
                //MessageBox.Show(lib.DB.ExecuteNoParams(sql));
            }
            guid = Guid.NewGuid().ToString();
            sql = $"insert into excelFile(ef_id,path,imp_date) values('{guid}', '{path}', getdate());";
            lib.DB.ExecuteNoParams(sql);
            dt.Dispose();
            return guid;
        }

        /// <summary>
        /// 將列行轉換成儲存格名稱
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string GetFieldName(int row, int col)
        {
            col--;
            string r = "A" + row.ToString();
            if (col < 0) { throw new Exception("col發生錯誤"); }
            List<string> chars = new List<string>();
            do
            {
                if (chars.Count > 0) col--;
                chars.Insert(0, ((char)(col % 26 + (int)'A')).ToString());
                col = (int)((col - col % 26) / 26);
            } while (col > 0);
            r = String.Join(string.Empty, chars.ToArray()) + row.ToString();
            return r;
        }

        /// <summary>
        /// 存入 excel_data 資料表
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="sheet"></param>
        protected void Save2DB(string fileid, string field, string value, int sheet)
        {
            string sql = "insert into excelData(ed_id,ef_id,value,cell,sheet) values(newid(),@ef_id,@value,@cell,@sheet)";

            using (SqlConnection con = lib.DB.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ef_id", fileid);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@cell", field);
                cmd.Parameters.AddWithValue("@sheet", sheet);
                int rowsAffected = cmd.ExecuteNonQuery();
                cmd.Cancel();
            }
        }

        /// <summary>
        /// 呼叫 RFC【Z_SUMEEKO_001_LAA】，獲取使用者代號 (CustomerCode)
        /// </summary>
        /// <returns></returns>
        private void getCustomerCode()
        {
            try
            {
                string FunctionName = "Z_SUMEEKO_005_PLEXTOBP";
                lib.Control.ShowLog(tbLog, "連線至SAP中......\r\n");

                RfcDestination rfcDestination = lib.SAP.GetDestination();
                RfcRepository rfcRepository = rfcDestination.Repository;
                // 調用 RFC 函数
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                foreach (ShippingInfo s in listShippingInfo)
                {
                    //輸入 CustomerAddressCode 參數
                    //rfcFunction.SetValue("I_NAME2", s.CustomerAddressCode.Trim().ToUpper());
                    rfcFunction.SetValue("I_NAME2", "GENERAL MOTORS LLC");

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    // 從 RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, "連線成功，取出收貨方...... \r\n");
                        string customerCode = rfcFunction.GetString("E_BPNO");
                        // 將「收貨方」存入 ShippingInfo model
                        s.CustomerCode = customerCode;
                        Console.WriteLine(customerCode);
                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, "連線失敗! \r\n");
                        string dataEx = rfcFunction.GetString("E_MESSAGE");
                        lib.Control.ShowLog(tbLog, dataEx + "\r\n");
                    }
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
            try
            {
                string FunctionName = "Z_SUMEEKO_006_PNGETSTK";
                lib.Control.ShowLog(tbLog, "連線至SAP中......\r\n");
                RfcDestination rfcDestination = lib.SAP.GetDestination();

                // 調用 RFC 函数
                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction(FunctionName);

                foreach (ShippingInfo s in listShippingInfo)
                {
                    // 設置 RFC 函数的輸入參數  (客戶物料號碼、客戶地址)
                    //rfcFunction.SetValue("I_PN_MATNR", s.CPartNo);
                    //rfcFunction.SetValue("I_ADDNAME2", s.CustomerAddressCode);
                    rfcFunction.SetValue("I_PN_MATNR", "20227041");
                    rfcFunction.SetValue("I_ADDNAME2", "DURA AUTOMOTIVE");

                    // 執行 RFC 函数
                    rfcFunction.Invoke(rfcDestination);

                    //  RFC 函数獲取輸出參數
                    string dataRCode = rfcFunction.GetString("E_RCODE");

                    if (dataRCode.Equals("S"))
                    {
                        lib.Control.ShowLog(tbLog, "連線成功，取出批次狀況...... \r\n");
                        IRfcTable rfcTable = rfcFunction.GetTable("ET_ZSDT012");

                        // 情況一: 查不到任何批次庫存
                        if (rfcTable == null)
                        {
                            // 將這筆 ShippingInfo 的 CPartNo 加入 list
                            listCPartNo.Add(s.CPartNo);
                            Console.WriteLine($"查不到【{s.CPartNo}】的批次庫存");
                            return;
                        }

                        // 查詢 "LABST" (評價的未限制使用庫存) 欄位值大於等於 ShippingInfo.Quantity 的紀錄
                        DataTable result = lib.SAP.ConvertRfcTableToDataTable(rfcTable);
                        dgvEdit.DataSource = result;

                        DataRow query = (from batch in result.AsEnumerable()
                                         where Convert.ToInt32(batch.Field<int>("LABST") * 100) >= s.Quantity
                                         select batch).FirstOrDefault();

                        // 情況二: 找到 "LABST" (評價的未限制使用庫存) 欄位值大於等於 ShippingInfo.Quantity 的紀錄
                        if (query != null)
                        {
                            query["MATNR"] = s.PartNo; // 物料號碼
                            query["LABST"] = s.BatchAmount; // 未限制使用庫存
                            query["CHARG"] = s.BatchNo; // 批次號碼
                            query["LABST"] = s.BatchAmount; // 未限制使用庫存
                            query["LGORT"] = s.Repository; // 儲存地點
                            query["LGOBE"] = s.RepositoryDesc;// 儲存地點說明
                            query["VBELN"] = s.SD; // 銷售與配銷文件號碼
                            MessageBox.Show($"找到可用庫存: 批號為{s.BatchNo}");
                        }
                        // 情況三: 找到 "LABST" (評價的未限制使用庫存) 欄位值大於等於 ShippingInfo.Quantity 的紀錄



                    }
                    else
                    {
                        lib.Control.ShowLog(tbLog, "連線失敗! \r\n");
                        string dataEx = rfcFunction.GetString("E_MESSAGE");
                        lib.Control.ShowLog(tbLog, dataEx + "\r\n");
                    }
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
        /// 將 DataSet 匯出成 Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="strPath"></param>
        private void ExportDataSetToExcel(DataSet ds, string strPath)
        {
            int inHeaderLength = 3, inColumn = 0, inRow = 0;
            System.Reflection.Missing Default = System.Reflection.Missing.Value;
            // Create Excel File
            strPath += @"\ShippingHistory-" + Guid.NewGuid().ToString("D") + ".xlsx";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(1);
            foreach (DataTable dtbl in ds.Tables)
            {

                string charTotalCol = ConvertToColumnName(dtbl.Columns.Count);
                // Create Excel WorkSheet
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add(Default, excelWorkBook.Sheets[excelWorkBook.Sheets.Count], 1, Default);
                excelWorkSheet.Name = dtbl.TableName; //Name worksheet

                // Write Column Name
                for (int i = 0; i < dtbl.Columns.Count; i++)
                    excelWorkSheet.Cells[inHeaderLength + 1, i + 1] = dtbl.Columns[i].ColumnName.ToUpper();

                // Write Rows
                for (int m = 0; m < dtbl.Rows.Count; m++)
                {
                    for (int n = 0; n < dtbl.Columns.Count; n++)
                    {
                        inColumn = n + 1;
                        inRow = inHeaderLength + 2 + m;
                        excelWorkSheet.Cells[inRow, inColumn] = dtbl.Rows[m][n].ToString();
                    }

                    // 若「銷項交貨」的值與上筆一樣就合併儲存格
                    if (m > 0 && dtbl.Rows[m]["銷項交貨"].ToString() == dtbl.Rows[m - 1]["銷項交貨"].ToString())
                    {
                        excelApp.DisplayAlerts = false;
                        excelWorkSheet.get_Range("A" + inRow.ToString(), "A" + (inRow - 1).ToString()).Merge();
                    }
                }
                excelApp.DisplayAlerts = true;

                // Excel Header
                Excel.Range cellRang = excelWorkSheet.get_Range("A1", $"{charTotalCol}3");
                cellRang.Merge(false);
                cellRang.Interior.Color = Color.White;
                cellRang.Font.Color = Color.Gray;
                cellRang.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // xlHAlignLeft 置左、xlHAlignRight 置右
                cellRang.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;  // xlVAlignTop 置上、xlVAlignBottom 置下
                cellRang.Font.Size = 26;
                excelWorkSheet.Cells[1, 1] = "Shipping History (PLEX to SAP)";

                // Style table column names
                cellRang = excelWorkSheet.get_Range("A4", $"{charTotalCol}4");
                cellRang.Font.Bold = true;
                cellRang.Font.Color = ColorTranslator.ToOle(Color.White);
                cellRang.Interior.Color = ColorTranslator.FromHtml("#465775");
                excelWorkSheet.get_Range("F4").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //// Formate price column
                //excelWorkSheet.get_Range("F5").EntireColumn.NumberFormat = "0.00";
                // Auto fit columns
                excelWorkSheet.Columns.AutoFit();
            }

            // Delete First Page
            excelApp.DisplayAlerts = false;
            Excel.Worksheet lastWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets[1];
            lastWorkSheet.Delete();
            excelApp.DisplayAlerts = true;

            // Set Defualt Page
            (excelWorkBook.Sheets[1] as Excel._Worksheet).Activate();

            excelWorkBook.SaveAs(strPath, Default, Default, Default, false, Default, Excel.XlSaveAsAccessMode.xlNoChange, Default, Default, Default, Default, Default);
            excelWorkBook.Close();
            excelApp.Quit();

            MessageBox.Show($"成功匯出Excel至【{strPath}】!", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 將行索引轉換成字母
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string ConvertToColumnName(int columnIndex)
        {
            if (columnIndex <= 0)
            {
                throw new ArgumentException("行索引必須大於0!", nameof(columnIndex));
            }

            string columnName = "";

            while (columnIndex > 0)
            {
                int remainder = (columnIndex - 1) % 26;
                char digit = (char)('A' + remainder);
                columnName = digit + columnName;
                columnIndex = (columnIndex - 1) / 26;
            }
            return columnName;
        }
    }
}