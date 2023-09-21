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
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace RFC_shippingHistory
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// </summary>
        List<ShippingInfo> listShippingInfo = new List<ShippingInfo>();

        /// <summary>
        /// 最終結果 (1. 轉成 Excel 2. 寫進 Sap 出貨單) 
        /// </summary>
        DataTable finalResult = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            lib.Control.ShowLog(tbLog, "連線至SAP......\n");

            try
            {
                RfcDestination rfcDestination = lib.SAP.GetDestination();
                // 調用 RFC 函数
                RfcRepository rfcRepository = rfcDestination.Repository;
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("Z_SUMEEKO_006_PNGETSTK");

                // 設置 RFC 函数的輸入參數
                rfcFunction.SetValue("I_PN_MATNR", "11601723");
                rfcFunction.SetValue("I_ADDNAME2", "GENERAL MOTORS LLC");

                lib.Control.ShowLog(tbLog, "連線中......\r\n");

                // 執行 RFC 函数
                rfcFunction.Invoke(rfcDestination);

                //  RFC 函数獲取输出参数
                string dataRCode = rfcFunction.GetString("E_RCODE");

                if (dataRCode.Equals("S"))
                {
                    lib.Control.ShowLog(tbLog, "連線成功!\r\n");
                    IRfcTable rfcTable = rfcFunction.GetTable("ET_MSKA");
                    DataTable result = lib.SAP.ConvertRfcTableToDataTable(rfcTable);
                    dgvTest.DataSource = result;
                }
                else
                {
                    lib.Control.ShowLog(tbLog, "連線失敗! \r\n");
                    string dataEx = rfcFunction.GetString("E_MESSAGE");
                    lib.Control.ShowLog(tbLog, dataEx + "\r\n");
                }
                lib.Control.ShowLog(tbLog, "..........................................\r\n");
                lib.Control.ShowLog(tbLog, "完成....\r\n");
            }
            catch (RfcCommunicationException ex)
            {
                lib.Control.ShowLog(tbLog, "RfcCommunicationException\r\n");
                lib.Control.ShowLog(tbLog, ex.Message.ToString() + "....\r\n");
            }
            catch (RfcLogonException ex)
            {
                lib.Control.ShowLog(tbLog, ".................RfcLogonException.........................\r\n");
                lib.Control.ShowLog(tbLog, ex.Message.ToString() + "....\r\n");
            }
            catch (RfcAbapRuntimeException ex)
            {
                lib.Control.ShowLog(tbLog, ".................RfcAbapRuntimeException.........................\r\n");
                lib.Control.ShowLog(tbLog, ex.Message.ToString() + "....\r\n");
            }
            catch (RfcAbapBaseException ex)
            {
                lib.Control.ShowLog(tbLog, ".................RfcAbapBaseException.........................\r\n");
                lib.Control.ShowLog(tbLog, ex.Message.ToString() + "....\r\n");
            }
            catch (Exception ex)
            {
                lib.Control.ShowLog(tbLog, ".................exception.........................\r\n");
                lib.Control.ShowLog(tbLog, ex.Message.ToString() + "....\r\n");
            }
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
            dealWithBatch();
        }

        private void iconExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("銷售文件類型");
            dt.Columns.Add("買方");
            dt.Columns.Add("客戶參考(PN碼)");
            dt.Columns.Add("銷售文件");
            dt.Columns.Add("物料VBAP");
            dt.Columns.Add("客戶物料");
            dt.Columns.Add("物料MARD");
            dt.Columns.Add("未限制");
            dt.Columns.Add("移轉中");
            dt.Columns.Add("品質檢驗");
            dt.Columns.Add("限制使用庫存");
            dt.Columns.Add("已凍結");
            dt.Columns.Add("物料MSEG");
            dt.Columns.Add("倉庫地點");
            dt.Columns.Add("批次");
            dt.Columns.Add("物料尺寸");
            dt.Columns.Add("物料說明");
            dt.Columns.Add("儲存地點");
            dt.Columns.Add("儲存位置的說明");

            dt.Rows.Add(null, "1GMNAO", null, "SH58242", "AASZZ8M0666NXC003XFP01", "11546396", "AASZZ8M0666NXC003XFP01", 200, 5, null, null, null, "AASZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58242", "AASZZ8M0666NXC003XFP01", "11546396", "AASZZ8M0666NXC003XFP01", 35, 5, null, null, null, "AASZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58242", "BBSZZ8M0666NXC003XFP01", "11546397", "BBSZZ8M0666NXC003XFP01", 77, 5, null, null, null, "BBSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58242", "BBSZZ8M0666NXC003XFP01", "11546397", "BBSZZ8M0666NXC003XFP01", 32, 5, null, null, null, "BBSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58242", "CCSZZ8M0666NXC003XFP01", "11546398", "CCSZZ8M0666NXC003XFP01", 24, 5, null, null, null, "CCSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58243", "DDSZZ8M0666NXC003XFP01", "11546399", "DDSZZ8M0666NXC003XFP01", 15, 5, null, null, null, "DDSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58243", "DDSZZ8M0666NXC003XFP01", "11546399", "DDSZZ8M0666NXC003XFP01", 18, 5, null, null, null, "DDSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58243", "AASZZ8M0666NXC003XFP01", "11546396", "AASZZ8M0666NXC003XFP01", 20, 5, null, null, null, "AASZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58243", "EESZZ8M0666NXC003XFP01", "11546311", "EESZZ8M0666NXC003XFP01", 56, 5, null, null, null, "EESZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58243", "EESZZ8M0666NXC003XFP01", "11546311", "EESZZ8M0666NXC003XFP01", 114, 5, null, null, null, "EESZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58243", "EESZZ8M0666NXC003XFP01", "11546311", "EESZZ8M0666NXC003XFP01", 46, 5, null, null, null, "EESZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58245", "RRSZZ8M0666NXC003XFP01", "11546311", "RRSZZ8M0666NXC003XFP01", 23, 5, null, null, null, "RRSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58245", "RRSZZ8M0666NXC003XFP01", "11546311", "RRSZZ8M0666NXC003XFP01", 24, 5, null, null, null, "RRSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");
            dt.Rows.Add(null, "1GMNAO", null, "SH58245", "RRSZZ8M0666NXC003XFP01", "11546311", "RRSZZ8M0666NXC003XFP01", 12, 5, null, null, null, "RRSZZ8M0666NXC003XFP01", "UF01", "Z00011741", null, null, null, "美國成品倉");

            dgvTest.DataSource = dt;

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ExportDataSetToExcel(ds, dialog.SelectedPath);
                }
            }
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
                                    shippingInfo.PartNo = xlRange.Cells[i, 1].Value2.ToString();
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

                // 依照 ShipperNo 欄位排序
                IEnumerable<ShippingInfo> result = from s in listShippingInfo orderby s.ShipperNo select s;


                //foreach (ShippingInfo shippingInfo in result)
                //{
                //    Console.WriteLine($"料號: {shippingInfo.PartNo}、出貨號碼: {shippingInfo.ShipperNo}、客戶地址: {shippingInfo.CustomerAddressCode}、數量: {shippingInfo.Quantity}");
                //}

                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
            }
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
        /// 呼叫 RFC【Z_SUMEEKO_001_LAA】，獲取使用者料號
        /// </summary>
        /// <returns></returns>
        private void getCustomerCode()
        {
            RfcDestination dest = lib.SAP.GetDestination();
            IRfcFunction func = dest.Repository.CreateFunction("");
            foreach (ShippingInfo s in listShippingInfo)
            {

                func.SetValue("", s.CustomerAddressCode);
                func.Invoke(dest);
                IRfcTable rfcTable = func.GetTable("");
                DataTable result = lib.SAP.ConvertRfcTableToDataTable(rfcTable);
                foreach (DataRow row in result.Rows)
                {
                    s.CustomerCode = row["CustomerCode"].ToString();
                }
            }
        }

        /// <summary>
        /// 處理批次
        /// </summary>
        /// <returns></returns>
        private void dealWithBatch()
        {


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
            strPath += @"\ShippingHistory" + Guid.NewGuid().ToString("D") + ".xlsx";
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
                        excelWorkSheet.Cells[inRow, inColumn] = dtbl.Rows[m].ItemArray[n].ToString();  // DataRow.ItemArray 透過陣列取得或設定這個資料列的所有值。
                        if (m % 2 == 0)
                            excelWorkSheet.get_Range("A" + inRow.ToString(), charTotalCol + inRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.FromHtml("#C5CBD3");
                    }
                }

                // Excel Header
                Excel.Range cellRang = excelWorkSheet.get_Range("A1", $"{charTotalCol}3");
                cellRang.Merge(false);
                cellRang.Interior.Color = Color.White;
                cellRang.Font.Color = Color.Gray;
                cellRang.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // xlHAlignLeft 置左、xlHAlignRight 置右
                cellRang.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;  // xlVAlignTop 置上、xlVAlignBottom 置下
                cellRang.Font.Size = 26;
                excelWorkSheet.Cells[1, 1] = "Shpping History (PLEX to SAP)";

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