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
using Microsoft.Office.Core;

namespace RFC_shippingHistory
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 從 Plex Excel 讀出，用於 interate 處理資料
        /// </summary>
        List<ShippingInfo> listShippingInfo = new List<ShippingInfo>();

        /// <summary>
        /// 所有可選的批次庫存 
        /// </summary>
        List<ShippingInfo> listAllOptions = new List<ShippingInfo>();

        /// <summary>
        /// 系統所選的批次庫存 
        /// </summary>
        List<ShippingInfo> listSystemSelect = new List<ShippingInfo>();

        /// <summary>
        /// 使用者所選的批次庫存 
        /// </summary>
        List<ShippingInfo> listUserSelect = new List<ShippingInfo>();

        /// <summary>
        /// 單筆檢視 (以CAddrCode & CPartNo 為分類基準)
        /// </summary>
        List<DataTable> listDtOneView = new List<DataTable>();

        /// <summary>
        /// 全筆檢視
        /// </summary>
        DataTable dtAllView = new DataTable();


        /// <summary>
        /// 用來存放最終出貨單 (1. 匯出 Excel、2. 寫進 SAP 出貨單) 
        /// </summary>
        DataTable dtResult = new DataTable();

        /// <summary>
        /// 編輯畫面的當前頁數
        /// </summary>
        int currentPage = 1;

        /// <summary>
        /// 客戶料號/客戶地址搜尋 ComboBox
        /// </summary>
        List<string> listCPartNoAddrCode = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 啟用動態時間
            timer1.Start();

            // 要匯出的 DataTable (使用者所選庫存)
            dtResult.Columns.Add("銷售文件");
            dtResult.Columns.Add("收貨方");
            dtResult.Columns.Add("實際發貨日期");
            dtResult.Columns.Add("物料");
            dtResult.Columns.Add("客戶料號");
            dtResult.Columns.Add("客戶地址");
            dtResult.Columns.Add("交貨數量");
            dtResult.Columns.Add("單位");
            dtResult.Columns.Add("批次");
            dtResult.Columns.Add("儲存地點");
            dtResult.Columns.Add("說明");

            // 可選庫存清單
            DataGridViewTextBoxColumn dgvCol = new DataGridViewTextBoxColumn();
            dgvCol.HeaderText = "庫存取用量";
            dgvCol.Name = "庫存取用量";
            dgvOne.Columns.Insert(0, dgvCol);
            dgvOne.Columns["庫存取用量"].ReadOnly = false;
            dgvOne.Columns["庫存取用量"].Visible = false;
            dgvOne.Visible = false;
        }

        /// <summary>
        /// 從資料夾匯入 Plex Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconFolder_Click(object sender, EventArgs e)
        {
            ClearList();

            ImportFolder();

            // Step 1: 取得 CustomerCode
            getCustomerCode();

            // Step 2: 取得某客戶某物料的批次庫存狀況
            dealWithBatch();

            // 以客戶物料、客戶地址將listAllOptions分成多頁 (一頁等於一個DataTable)
            LoadListDtOneView();

            // 將進度條歸零
            lib.Control.ShowPgbar(pgBar, 0, 0);
        }

        /// <summary>
        /// 從檔案匯入 Plex Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconFile_Click(object sender, EventArgs e)
        {
            ClearList();

            ImportFile();

            // Step 1: 取得 CustomerCode
            getCustomerCode();

            // Step 2: 取得某客戶某物料的批次庫存狀況
            dealWithBatch();

            foreach (ShippingInfo s in listSystemSelect)
            {
                Console.WriteLine($"客戶物料號碼:{s.CPartNo}、銷售文件:{s.ShipperNo}、客戶地址:{s.CustomerAddressCode}、出貨數量:{s.Quantity}、實際發貨日期:{s.ShipDate}、資料取得成功:{s.ok}、收貨方【RFC1】:{s.CustomerCode}、物料號碼【RFC2】:{s.PartNo}、批次號碼【RFC2】:{s.BatchNo}、未限制使用庫存【RFC2】:{s.BatchAmount}、儲存地點【RFC2】:{s.Repository}、儲存地點說明【RFC2】:{s.RepositoryDesc}、Sales & Distribution 文件【RFC2】:{s.SD}");
            }

            LoadListDtOneView();
            LoadDtAllView();

            // 將進度條歸零
            lib.Control.ShowPgbar(pgBar, 0, 0);
        }

        /// <summary>
        /// 每一次匯入Plex Excel前，先清空相關list
        /// </summary>
        private void ClearList()
        {
            listShippingInfo.Clear();
            listAllOptions.Clear();
            listDtOneView.Clear();
            listCPartNoAddrCode.Clear();
        }

        /// <summary>
        /// 清除訊息視窗內容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconClear_Click(object sender, EventArgs e)
        {
            tbLog.Text = "";
        }

        /// <summary>
        /// 檢視模式切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconViewMode_Click(object sender, EventArgs e)
        {
            if (lblViewMode.Text == "全筆編輯")
            {
                iconFirst.Visible = true;
                iconPrevious.Visible = true;
                iconNext.Visible = true;
                iconLast.Visible = true;
                lblPage.Visible = true;
                comboSearch.Visible = true;
                iconSearch.Visible = true;
                lblFilter.Visible = true;
                lblViewMode.Text = "單筆編輯";
                dgvAll.Visible = false;
                dgvOne.Visible = true;
            }
            else
            {
                iconFirst.Visible = false;
                iconPrevious.Visible = false;
                iconNext.Visible = false;
                iconLast.Visible = false;
                lblPage.Visible = false;
                comboSearch.Visible = false;
                iconSearch.Visible = false;
                lblFilter.Visible = false;
                lblViewMode.Text = "全筆編輯";
                dgvAll.Visible = true;
                dgvOne.Visible = false;
            }
        }

        /// <summary>
        /// 下拉選擇客戶物料號碼與客戶地址後，查詢庫存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconSearch_Click(object sender, EventArgs e)
        {
            if (listAllOptions.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (comboSearch.SelectedIndex >= 0)
            {
                foreach (DataTable dt in listDtOneView.AsEnumerable())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if ($"{dr["客戶料號"].ToString()}+{dr["客戶地址"].ToString()}" == comboSearch.Text.ToString())
                        {
                            currentPage = listDtOneView.IndexOf(dt) + 1;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                   
                }
            }
            refreshPage();
        }

        /// <summary>
        /// 將dtResult匯出成Excel，並存入ShippingHistory資料庫中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconExport_Click(object sender, EventArgs e)
        {
            var AllOptionsOrderedByShipperNo = from all in listAllOptions orderby all.ShipperNo select all;
            foreach (ShippingInfo s in AllOptionsOrderedByShipperNo)
            {
                DataRow dr = dtResult.NewRow();
                dr["客戶料號"] = s.CPartNo;
                dr["客戶地址"] = s.CustomerAddressCode;
                dr["銷售文件"] = s.SD;
                dr["收貨方"] = s.CustomerCode;
                dr["實際發貨日期"] = s.ShipDate;
                dr["物料"] = s.PartNo;
                dr["交貨數量"] = s.BatchAmount;
                dr["單位"] = "MPC";
                dr["批次"] = s.BatchNo;
                dr["儲存地點"] = s.Repository;
                dr["說明"] = s.RepositoryDesc;
                dtResult.Rows.Add(dr);
            }
            
            if (dtResult.Rows.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtResult);
                    string directoryPath = dialog.SelectedPath;
                    string filePath = ExportDataSetToExcel(ds, directoryPath);
                    ExcelProcessAll(filePath, "Sap");
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



        /// <summary>
        /// 第一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconFirst_Click(object sender, EventArgs e)
        {
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = 1;
            refreshPage();
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconPrevious_Click(object sender, EventArgs e)
        {
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = currentPage > 1 ? --currentPage : 1;
            refreshPage();
        }

        /// <summary>
        /// 下一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconNext_Click(object sender, EventArgs e)
        {
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = currentPage < listDtOneView.Count ? ++currentPage : listDtOneView.Count;
            refreshPage();
        }

        /// <summary>
        /// 最後一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconLast_Click(object sender, EventArgs e)
        {
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            currentPage = listDtOneView.Count;
            refreshPage();
        }

        /// <summary>
        /// 重整編輯頁面
        /// </summary>
        private void refreshPage()
        {
            dgvOne.DataSource = listDtOneView[currentPage - 1];
            dgvOne.Visible = true;
            dgvOne.Columns["客戶地址"].Visible = false;
            dgvOne.Columns["客戶料號"].Visible = false;
            dgvOne.Columns["庫存取用量"].Visible = true;

            foreach (DataRow dr in listDtOneView[currentPage - 1].Rows)
            {
                comboSearch.Text = $"{dr["客戶料號"].ToString()}+{dr["客戶地址"]}";             
                break;
            }
            lblPage.Text = $"第{currentPage}頁/共{listDtOneView.Count}頁";
        }

        /// <summary>
        /// 將listAllOptions以客戶料號、客戶地址分成多張DataTable，並加到listDtOneView
        /// </summary>
        private void LoadListDtOneView()
        {
            Dictionary<string, DataTable> dataTableDictionary = new Dictionary<string, DataTable>();

            foreach (var s in listAllOptions)
            {
                string key = $"{s.CPartNo}_{s.CustomerAddressCode}";

                if (!dataTableDictionary.ContainsKey(key))
                {
                    // Create a new DataTable with a unique name based on the key
                    DataTable dt = new DataTable(key); // Use key as the table name
                    dt.Columns.Add("客戶料號");
                    dt.Columns.Add("客戶地址");
                    dt.Columns.Add("銷售文件");
                    dt.Columns.Add("收貨方");
                    dt.Columns.Add("實際發貨日期");
                    dt.Columns.Add("物料");
                    dt.Columns.Add("可用庫存");
                    dt.Columns.Add("單位");
                    dt.Columns.Add("批次");
                    dt.Columns.Add("儲存地點");
                    dt.Columns.Add("說明");

                    DataRow dr = dt.NewRow();
                    dr["客戶料號"] = s.CPartNo;
                    dr["客戶地址"] = s.CustomerAddressCode;
                    dr["銷售文件"] = s.SD;
                    dr["收貨方"] = s.CustomerCode;
                    dr["實際發貨日期"] = s.ShipDate;
                    dr["物料"] = s.PartNo;
                    dr["可用庫存"] = s.BatchAmount;
                    dr["單位"] = "MPC";
                    dr["批次"] = s.BatchNo;
                    dr["儲存地點"] = s.Repository;
                    dr["說明"] = s.RepositoryDesc;
                    dt.Rows.Add(dr);

                    dataTableDictionary[key] = dt;
                    listDtOneView.Add(dt);
                }
                else
                {
                    // 如果字典中已存在匹配的DataTable，將該列添加到現有的DataTable中
                    DataRow dr = dataTableDictionary[key].NewRow();
                    //dr["客戶料號"] = s.CPartNo;
                    //dr["客戶地址"] = s.CustomerAddressCode;
                    dr["銷售文件"] = s.SD;
                    dr["收貨方"] = s.CustomerCode;
                    dr["實際發貨日期"] = s.ShipDate;
                    dr["物料"] = s.PartNo;
                    dr["可用庫存"] = s.BatchAmount;
                    dr["單位"] = "MPC";
                    dr["批次"] = s.BatchNo;
                    dr["儲存地點"] = s.Repository;
                    dr["說明"] = s.RepositoryDesc;
                    dataTableDictionary[key].Rows.Add(dr);
                }
            }
            lblPage.Text = $"第{currentPage}頁/共{listDtOneView.Count}頁";
            refreshPage();
        }

        /// <summary>
        /// 從listAllOptions載入dtAllView
        /// </summary>
        private void LoadDtAllView()
        {
            dtAllView.Columns.Add("客戶料號");
            dtAllView.Columns.Add("客戶地址");
            dtAllView.Columns.Add("銷售文件");
            dtAllView.Columns.Add("收貨方");
            dtAllView.Columns.Add("實際發貨日期");
            dtAllView.Columns.Add("物料");
            dtAllView.Columns.Add("可用庫存");
            dtAllView.Columns.Add("單位");
            dtAllView.Columns.Add("批次");
            dtAllView.Columns.Add("儲存地點");
            dtAllView.Columns.Add("說明");

            foreach (ShippingInfo s in listAllOptions)
            {
                DataRow dr = dtAllView.NewRow();
                dr["客戶料號"] = s.CPartNo;
                dr["客戶地址"] = s.CustomerAddressCode;
                dr["銷售文件"] = s.SD;
                dr["收貨方"] = s.CustomerCode;
                dr["實際發貨日期"] = s.ShipDate;
                dr["物料"] = s.PartNo;
                dr["可用庫存"] = s.BatchAmount;
                dr["單位"] = "MPC";
                dr["批次"] = s.BatchNo;
                dr["儲存地點"] = s.Repository;
                dr["說明"] = s.RepositoryDesc;
                dtAllView.Rows.Add(dr);
            }
            dgvAll.DataSource = dtAllView;
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
        /// 匯入Plex Excel檔案並將之存入ShippingHistory資料庫
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
                    ExcelProcessAll(xlsfile, "Plex");
                }
            }
        }

        /// <summary>
        /// 匯入資料夾
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
        /// 將資料夾中的所有Plex Excel檔案存入ShippingHistory資料庫
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
                    ExcelProcessAll(file, "Plex");
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
        /// 將 .xls、.xlsx、.csv 格式文件存入資料庫中、將Excel填入listShippingHistory&listCPartNoAddrCode
        /// </summary>
        /// <param name="path"></param>
        protected void ExcelProcessAll(string path, string system)
        {
            string fileid = GetFileID(path, system);
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
            for (int z = 1; z <= xlWorkbook.Sheets.Count; z++)
            {
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[z];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                // 將進度條歸零
                lib.Control.ShowPgbar(pgBar, 0, 0);
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
                            if (system == "Plex")
                            {
                                switch (j)
                                {
                                    case 1:
                                        shippingInfo.CPartNo = xlRange.Cells[i, 2].Value2.ToString();
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
                            }
                            Save2DB(fileid, GetFieldName(i, j), xlRange.Cells[i, j].Value2.ToString(), z, system);
                        }
                    }
                    if (system == "Plex")
                    {
                        listCPartNoAddrCode.Add($"{xlRange.Cells[i, 2].Value2.ToString()}+{shippingInfo.CustomerAddressCode = xlRange.Cells[i, 6].Value2.ToString().Trim().ToUpper()}");
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

            // ComboBox 值初始化
            listCPartNoAddrCode.Distinct().OrderBy(s => s).ToList();
            foreach(string s in listCPartNoAddrCode)
            {
                comboSearch.Items.Add(s);
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
        /// 從 XXXExcelFile 和 XXXExcelData 表刪除符合傳入路徑的所有資料列，接著重新填入 XXXExcelFile 表的所有欄位，回傳 ef_id (外來鍵)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string GetFileID(string path, string system)
        {
            string guid = string.Empty;
            string sql = $"select * from {system}ExcelFile where path = '{path}'";
            DataTable dt = lib.DB.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                sql = $"delete from {system}ExcelFile where path = '{path}'; delete from {system}ExcelData where ef_id='{dt.Rows[0]["ef_id"]}';";
                lib.DB.ExecuteNoParams(sql);
                //MessageBox.Show(lib.DB.ExecuteNoParams(sql));
            }
            guid = Guid.NewGuid().ToString();
            sql = $"insert into {system}ExcelFile(ef_id,path,imp_date) values('{guid}', '{path}', getdate());";
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
        /// 存入 XXXexcelData 資料表
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="sheet"></param>
        /// <param name="system"></param>
        protected void Save2DB(string fileid, string field, string value, int sheet, string system)
        {
            string sql = $"insert into {system}ExcelData(ed_id,ef_id,value,cell,sheet) values(newid(),@ef_id,@value,@cell,@sheet)";

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
            // 清空進度條
            int total = listShippingInfo.Count;
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

                foreach (ShippingInfo s in listShippingInfo)
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
            int total = listShippingInfo.Count;
            int i = 0;
            lib.Control.ShowPgbar(pgBar, 0, 0);

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

                foreach (ShippingInfo s in listShippingInfo)
                {
                    // 進度條
                    lib.Control.ShowPgbar(pgBar, total, i);

                    listAllInventory.Clear();

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
                            listAllInventory.Add(sh);
                        }

                        // 系統挑選的批次庫存 (先進先出)-----------------------------------------------------------------------------------------------------------------------------
                        // Plex 出貨數量
                        float quantity = s.Quantity;

                        // 將所查到的庫存從DataTable轉成list【系統挑選的選項(先進先出)】
                        foreach (DataRow dr in dataTable.AsEnumerable())
                        {
                            // 將Plex出貨數減掉Sap批次庫存
                            quantity -= (Convert.ToSingle(dr["LABST"]) * 1000);
                            // 若Plex出貨數還是大於零
                           
                            if (quantity >= 0)
                                
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
                                listSystemInventory.Add(sh);
                            }
                            else
                            {
                                break;
                            }
                        }
                        listAllOptions.AddRange(listAllInventory);
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

        /// <summary>
        /// 將 DataSet 匯出成 Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private string ExportDataSetToExcel(DataSet ds, string strPath)
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

                    //// 若「物料號碼」的值與上筆一樣就合併儲存格
                    //if (m > 0 && dtbl.Rows[m]["銷售文件"].ToString() == dtbl.Rows[m - 1]["銷售文件"].ToString())
                    //{
                    //    excelApp.DisplayAlerts = false;
                    //    excelWorkSheet.get_Range("A" + inRow.ToString(), "A" + (inRow - 1).ToString()).Merge();
                    //}
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
            return strPath;
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