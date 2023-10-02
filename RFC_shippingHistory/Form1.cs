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
using System.Reflection.Emit;

namespace RFC_shippingHistory
{
    internal partial class Form1 : Form
    {
        /// <summary>
        /// 從 Plex Excel 讀出
        /// </summary>
        List<ShippingInfo> listPlex = new List<ShippingInfo>();

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

            // 要匯出的 DataTable 
            dtResult.Columns.Add("銷項交貨");
            dtResult.Columns.Add("物料");
            dtResult.Columns.Add("客戶料號");
            dtResult.Columns.Add("銷售文件號碼");
            dtResult.Columns.Add("銷售文件日期");
            dtResult.Columns.Add("客戶地址");
            dtResult.Columns.Add("收貨方");
            dtResult.Columns.Add("實際發貨日期");
            dtResult.Columns.Add("庫存數量");
            dtResult.Columns.Add("實際出貨數量");
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

            // 先隱藏dgvAll
            dgvAll.Visible = false;
        }


        /// <summary>
        /// 每一次匯入Plex Excel前，先清空相關集合
        /// </summary>
        private void ClearList()
        {
            listPlex.Clear();
            listAllOptions.Clear();
            listSystemSelect.Clear();
            listUserSelect.Clear();
            listDtOneView.Clear();
            listCPartNoAddrCode.Clear();
            dtAllView.Clear();
            dtResult.Clear();
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
                    dt.Columns.Add("銷售文件號碼");
                    dt.Columns.Add("銷售文件日期");
                    dt.Columns.Add("收貨方");
                    dt.Columns.Add("物料");
                    dt.Columns.Add("可用庫存");
                    dt.Columns.Add("單位");
                    dt.Columns.Add("批次");
                    dt.Columns.Add("儲存地點");
                    dt.Columns.Add("說明");

                    DataRow dr = dt.NewRow();
                    dr["客戶料號"] = s.CPartNo;
                    dr["客戶地址"] = s.CustomerAddressCode;
                    dr["銷售文件號碼"] = s.SD;
                    dr["銷售文件日期"] = s.SD_date;
                    dr["收貨方"] = s.CustomerCode;
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
                    dr["客戶料號"] = s.CPartNo;
                    dr["客戶地址"] = s.CustomerAddressCode;
                    dr["銷售文件號碼"] = s.SD;
                    dr["銷售文件日期"] = s.SD_date;
                    dr["收貨方"] = s.CustomerCode;
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
            dtAllView.Columns.Add("銷售文件號碼");
            dtAllView.Columns.Add("銷售文件日期");
            dtAllView.Columns.Add("收貨方");
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
                dr["銷售文件號碼"] = s.SD;
                dr["銷售文件日期"] = s.SD_date;
                dr["收貨方"] = s.CustomerCode;
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
                lib.Control.ShowLog(tbLog, $"找到Excel，開始將[路徑: {path}]存入資料庫 \r\n");
                lib.Control.ShowLog(tbLog, $"[工作表數: {z.ToString()}/ 列數: {rowCount.ToString()}/ 行數: {colCount.ToString()}] \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

                if (system == "Plex")
                {
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
                                        shippingInfo.Quantity = Convert.ToInt32(xlRange.Cells[i, 7].Value2) / 1000;
                                        break;
                                }

                                Save2DB(fileid, GetFieldName(i, j), xlRange.Cells[i, j].Value2.ToString(), z, system);
                            }
                        }
                        listCPartNoAddrCode.Add($"{xlRange.Cells[i, 2].Value2.ToString()}+{shippingInfo.CustomerAddressCode = xlRange.Cells[i, 6].Value2.ToString().Trim().ToUpper()}");
                        listPlex.Add(shippingInfo);
                        lib.Control.ShowPgbar(pgBar, rowCount, i);
                    }

                    // ComboBox 值初始化
                    listCPartNoAddrCode.Distinct().OrderBy(s => s).ToList();
                    foreach (string s in listCPartNoAddrCode)
                    {
                        comboSearch.Items.Add(s);
                    }
                }
                else
                {
                    for (int i = 5; i <= rowCount; i++)
                    {
                        for (int j = 1; j <= colCount; j++)
                        {
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            {

                                Save2DB(fileid, GetFieldName(i, j), xlRange.Cells[i, j].Value2.ToString(), z, system);
                            }
                        }
                        lib.Control.ShowPgbar(pgBar, rowCount, i);
                    }
                }

                // 清除資源
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
            }

            if (system == "Plex")
            {
                MessageBox.Show($"成功匯入Plex Excel", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lib.Control.ShowLog(tbLog, $"成功匯入Plex Excel \r\n");
                //lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
            else
            {
                MessageBox.Show($"成功匯出Sap Excel", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lib.Control.ShowLog(tbLog, $"成功匯出Sap Excel \r\n");
                //lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");
            }
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