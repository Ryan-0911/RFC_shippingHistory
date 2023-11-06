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
using System.Collections;
using System.Globalization;

namespace RFC_shippingHistory
{
    internal partial class Form1 : Form
    {
        /// <summary>
        /// 從 Plex Excel 讀出
        /// </summary>
        List<ShippingInfo> listPlex = new List<ShippingInfo>();

        /// <summary>
        /// 系統所選的批次庫存 
        /// </summary>
        List<ShippingInfo> listSystemSelect = new List<ShippingInfo>();

        /// <summary>
        /// 單筆檢視 
        /// </summary>
        List<DataTable> listDtOneView = new List<DataTable>();

        /// <summary>
        /// 編輯畫面的當前頁數
        /// </summary>
        int currentPage = 1;

        /// <summary>
        /// ComboBox【銷貨交項-客戶料號/客戶地址】
        /// </summary>
        List<string> listDropDown = new List<string>();

        /// <summary>
        /// 是否確認更新
        /// </summary>
        bool editConfirmed = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 啟用動態時間
            timer1.Start();

            // 隱藏錯誤圖示&訊息
            pictureBoxError.Visible = false;
            lblError.Text = "";
        }


        /// <summary>
        /// 每一次匯入Plex Excel前，先清空相關集合
        /// </summary>
        private void ClearList()
        {
            listPlex.Clear();
            listSystemSelect.Clear();
            listDtOneView.Clear();
            listDropDown.Clear();
            comboSearch.Items.Clear();
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
        /// 下拉選擇客戶物料號碼與客戶地址後，查詢庫存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconSearch_Click(object sender, EventArgs e)
        {
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (comboSearch.SelectedIndex >= 0)
            {
                DataTable foundDataTable = listDtOneView.FirstOrDefault(table => table.TableName == comboSearch.Text.ToString());
                if (foundDataTable != null)
                {
                    currentPage = listDtOneView.IndexOf(foundDataTable) + 1;
                    refreshPage();
                }
            }

        }

        /// <summary>
        /// 修改系統選好的庫存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconEdit_Click(object sender, EventArgs e)
        {
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult = MessageBox.Show("確定要更新嗎?", "更新確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DialogResult == DialogResult.Yes)
            {
                // 確認Sap選用庫存等於Plex出貨數
                float total = SapInventorySelected();
                if (total < Convert.ToSingle(dgvOne.Rows[2].Cells[2].Value))
                {
                    MessageBox.Show("Sap選用庫存不足Plex出貨數", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // 將 dgvOne 的值更新到 listSystemSelect 
                MessageBox.Show("更新成功", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editConfirmed = true;
                listSystemSelect[currentPage - 1].inventory.Clear();
                foreach (DataGridViewRow row in dgvOne.Rows)
                {
                    Inventory iv = new Inventory();
                    iv.BatchNo = row.Cells["批號"].Value.ToString();
                    iv.BatchAmount = Convert.ToSingle(row.Cells["Sap可用庫存"].Value);
                    iv.BatchTaken = Convert.ToSingle(row.Cells["Sap庫存取用量"].Value);
                    iv.SD = row.Cells["銷售文件號碼"].Value.ToString();
                    iv.SD_date = Convert.ToDateTime(row.Cells["銷售文件日期"].Value.ToString());
                    iv.NetUnitPrice = Convert.ToDecimal(row.Cells["Sap單位淨價"].Value);
                    iv.Currency = row.Cells["幣別"].Value.ToString();
                    listSystemSelect[currentPage - 1].inventory.Add(iv);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 第一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconFirst_Click(object sender, EventArgs e)
        {
            // 預防未匯入 Plex Excel
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 確認當頁 listDtOneView 的每筆庫存選用量是否跟 listSystemSelect 一樣，不一樣代表有更新
            bool edit = editOrNot();
            if (listDtOneView.Count > 0 && edit == true && editConfirmed == false)
            {
                DialogResult r = MessageBox.Show("您所選用的庫存將捨棄，確定離開嗎?", "操作說明", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // 若捨棄，就要將當頁 dtOneView 的庫存選用量回復成原來的值
                if (r == DialogResult.Yes)
                {
                    int j = 0;
                    foreach (Inventory iv in listSystemSelect[currentPage - 1].inventory)
                    {
                        listDtOneView[currentPage - 1].Rows[j]["Sap庫存取用量"] = iv.BatchTaken;
                        j++;
                    }
                }
                else
                {
                    return;
                }
            }

            // 更新頁數 & 重整頁面
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
            // 預防未匯入 Plex Excel
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 確認當頁 listDtOneView 的每筆庫存選用量是否跟 listSystemSelect 一樣，不一樣代表有更新
            bool edit = editOrNot();
            if (listDtOneView.Count > 0 && edit == true && editConfirmed == false)
            {
                DialogResult r = MessageBox.Show("您所選用的庫存將捨棄，確定離開嗎?", "操作說明", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // 若捨棄，就要將當頁 dtOneView 的庫存選用量回復成原來的值
                if (r == DialogResult.Yes)
                {
                    int j = 0;
                    foreach (Inventory iv in listSystemSelect[currentPage - 1].inventory)
                    {
                        listDtOneView[currentPage - 1].Rows[j]["Sap庫存取用量"] = iv.BatchTaken;
                        j++;
                    }
                }
                else
                {
                    return;
                }
            }

            // 更新頁數 & 重整頁面
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
            // 預防未匯入 Plex Excel
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 確認當頁 listDtOneView 的每筆庫存選用量是否跟 listSystemSelect 一樣，不一樣代表有更新
            bool edit = editOrNot();
            if (listDtOneView.Count > 0 && edit == true && editConfirmed == false)
            {
                DialogResult r = MessageBox.Show("您所選用的庫存將捨棄，確定離開嗎?", "操作說明", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // 若捨棄，就要將當頁 dtOneView 的庫存選用量回復成原來的值
                if (r == DialogResult.Yes)
                {
                    int j = 0;
                    foreach (Inventory iv in listSystemSelect[currentPage - 1].inventory)
                    {
                        listDtOneView[currentPage - 1].Rows[j]["Sap庫存取用量"] = iv.BatchTaken;
                        j++;
                    }
                }
                else
                {
                    return;
                }
            }

            // 更新頁數 & 重整頁面
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
            // 預防未匯入 Plex Excel
            if (listDtOneView.Count == 0)
            {
                MessageBox.Show("請先匯入Plex Excel!", "操作說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 確認當頁 listDtOneView 的每筆庫存選用量是否跟 listSystemSelect 一樣，不一樣代表有更新
            bool edit = editOrNot();
            if (listDtOneView.Count > 0 && edit == true && editConfirmed == false)
            {
                DialogResult r = MessageBox.Show("您所選用的庫存將捨棄，確定離開嗎?", "操作說明", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // 若捨棄，就要將當頁 dtOneView 的庫存選用量回復成原來的值
                if (r == DialogResult.Yes)
                {
                    int j = 0;
                    foreach (Inventory iv in listSystemSelect[currentPage - 1].inventory)
                    {
                        listDtOneView[currentPage - 1].Rows[j]["Sap庫存取用量"] = iv.BatchTaken;
                        j++;
                    }
                }
                else
                {
                    return;
                }
            }

            // 更新頁數 & 重整頁面
            currentPage = listDtOneView.Count;
            refreshPage();
        }


        private bool editOrNot()
        {
            // 確認當頁 listDtOneView 的每筆庫存選用量是否跟 listSystemSelect 一樣，不一樣代表有更新
            bool edit = false;
            int i = 0;
            foreach (Inventory iv in listSystemSelect[currentPage - 1].inventory)
            {
                if (iv.BatchTaken != Convert.ToSingle(listDtOneView[currentPage - 1].Rows[i]["Sap庫存取用量"]))
                {
                    edit = true;
                    break;
                }
                i++;
            }
            return edit;
        }

        private float SapInventorySelected()
        {
            float total = 0; // 用來儲存所選庫存加總
            foreach (DataGridViewRow row in dgvOne.Rows)
            {
                if (row.Cells[0].Value != null && Single.TryParse(row.Cells[0].Value.ToString(), out float cellValue))
                {
                    total += cellValue;
                }
            }
            float t = total;
            string str = t.ToString("N4");
            float.TryParse(str, out total);
            return total;
        }

        /// <summary>
        /// 重整編輯頁面
        /// </summary>
        private void refreshPage()
        {
            // 綁定 DataTable 至 DataGridView
            lib.Control.BindDataTableToDataGridView(dgvOne, listDtOneView[currentPage - 1]);

            // 更新下拉選單、頁數
            lib.Control.SetComboBoxText(comboSearch, $"{listSystemSelect[currentPage - 1].ShipperNo.ToString()}-{listSystemSelect[currentPage - 1].Customer}/{listSystemSelect[currentPage - 1].CPartNo}");
            lib.Control.SetLblText(lblPage, $"第{currentPage}頁/共{listDtOneView.Count}頁");

            // lbl 錯誤原因
            if (listSystemSelect[currentPage - 1].ok == false)
            {
                lib.Control.SetLblText(lblError, $"{listSystemSelect[currentPage - 1].E_MESSAGE_rfc1}{listSystemSelect[currentPage - 1].E_MESSAGE_rfc2}");
                lib.Control.PictureBoxVisibleOrNot(pictureBoxError, true);
            }
            else
            {
                lib.Control.SetLblText(lblError, "");
                lib.Control.PictureBoxVisibleOrNot(pictureBoxError, false);
            }
        }

        /// <summary>
        /// 將 listSystemSelect 加入至 listDtOneView (索引值對應)
        /// </summary>
        private void LoadListDtOneView()
        {
            foreach (ShippingInfo s in listSystemSelect)
            {
                string key = $"{s.ShipperNo}-{s.Customer}/{s.CPartNo}";

                DataTable dt = new DataTable(key); // Use key as the table name
                dt.Columns.Add("Sap庫存取用量");
                dt.Columns.Add("Sap可用庫存");
                dt.Columns.Add("Plex出貨量");
                dt.Columns.Add("單位");
                dt.Columns.Add("批號");
                dt.Columns.Add("銷售文件號碼");
                dt.Columns.Add("銷售文件日期");
                dt.Columns.Add("料號");
                dt.Columns.Add("Sap單位淨價");
                dt.Columns.Add("Plex單位淨價");
                dt.Columns.Add("幣別");


                foreach (Inventory iv in s.inventory)
                {
                    DataRow dr = dt.NewRow();
                    dr["Sap庫存取用量"] = iv.BatchTaken;
                    dr["Sap可用庫存"] = iv.BatchAmount;
                    dr["Plex出貨量"] = s.Quantity;
                    dr["單位"] = "MPC";
                    dr["批號"] = iv.BatchNo;
                    dr["銷售文件號碼"] = iv.SD;
                    dr["銷售文件日期"] = iv.SD_date;
                    dr["料號"] = s.PartNo;
                    dr["Sap單位淨價"] = iv.NetUnitPrice;
                    dr["Plex單位淨價"] = s.NetUnitPrice;
                    dr["幣別"] = iv.Currency;
                    dt.Rows.Add(dr);
                }
                listDtOneView.Add(dt);
            }
            refreshPage();
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
        /// 將 .xls、.xlsx、.csv 格式文件存入資料庫中
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

                //int lastRow = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                //for (int row = lastRow; row >= 1; row--)
                //{
                //    Excel.Range cell = (Excel.Range)xlWorksheet.Cells[row, 2];
                //    if (string.IsNullOrEmpty(cell.Value2?.ToString()))
                //    {
                //        Excel.Range entireRow = cell.EntireRow;
                //        entireRow.Delete(Excel.XlDirection.xlUp);
                //    }
                //}

                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                lib.Control.ShowLog(tbLog, $"開始將Excel[路徑: {path}]存入資料庫 \r\n");
                lib.Control.ShowLog(tbLog, $"[工作表數: {z.ToString()}/ 列數: {rowCount.ToString()}/ 行數: {colCount.ToString()}] \r\n");
                lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

                if (system == "Plex")
                {
                    for (int i = 2; i <= rowCount; i++)
                    {
                        if (xlRange.Cells[i, 2].Value2 == null)
                        {
                            continue;
                        }

                        ShippingInfo shippingInfo = new ShippingInfo();
                        for (int j = 1; j <= colCount; j++)
                        {
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            {
                                switch (j)
                                {
                                    case 2:
                                        shippingInfo.ShipperNo = xlRange.Cells[i, 2].Value2.ToString();
                                        break;
                                    case 3:
                                        string[] dateF = {"MM/dd/yy", "MM/d/yy"};
                                        shippingInfo.ShipDate = xlRange.Cells[i, 3].Text.ToString().Substring(2);
                                        // 解析原始日期
                                        if (DateTime.TryParseExact(shippingInfo.ShipDate, dateF, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                        {
                                            // 將日期格式化為 "yyyy/MM/dd"
                                            string formattedDate = parsedDate.ToString("yyyy/MM/dd");
                                            shippingInfo.ShipDate = formattedDate;
                                        }
                                        break;
                                    case 7:
                                        shippingInfo.CPartNo = xlRange.Cells[i, 7].Value2.ToString();
                                        break;
                                    case 8:
                                        shippingInfo.Customer = xlRange.Cells[i, 8].Value2.ToString().Trim().ToUpper();
                                        break;
                                    case 9:
                                        shippingInfo.Quantity = Convert.ToSingle(xlRange.Cells[i, 9].Value2) / 1000;
                                        break;
                                    case 10:
                                        shippingInfo.NetUnitPrice = Convert.ToDecimal(xlRange.Cells[i, 12].Value2) / Convert.ToDecimal(xlRange.Cells[i, 9].Value2);
                                        break;
                                }
                                Save2DB(fileid, GetFieldName(i, j), xlRange.Cells[i, j].Value2.ToString(), z, system);
                            }
                        }
                        listDropDown.Add($"{xlRange.Cells[i, 2].Value2.ToString()}-{shippingInfo.Customer = xlRange.Cells[i, 8].Value2.ToString().Trim().ToUpper()}/{xlRange.Cells[i, 7].Value2.ToString()}");
                        listPlex.Add(shippingInfo);
                        lib.Control.ShowPgbar(pgBar, rowCount, i);
                    }
                    // 將listPlex內容複製給listSystemSelect
                    listSystemSelect = new List<ShippingInfo>(listPlex);

                    // ComboBox 值初始化
                    listDropDown.Distinct().OrderBy(s => s).ToList();
                    foreach (string s in listDropDown)
                    {
                        lib.Control.AddComboBoxItem(comboSearch, s);
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
                // 進度條跑滿
                lib.Control.ShowPgbar(pgBar, rowCount, rowCount);

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
            lib.Control.ShowPgbar(pgBar, 0, 0);
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

        /// <summary>
        /// 需要繪製儲存格時發生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvOne_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
                {
                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else
            {
                e.AdvancedBorderStyle.Top = dgvOne.AdvancedCellBorderStyle.Top;
            }
        }

        /// <summary>
        /// 確認DataGridView前列與該列的值是否一樣
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        bool IsTheSameCellValue(int column, int row)
        {
            DataGridViewCell cell1 = dgvOne[column, row];
            DataGridViewCell cell2 = dgvOne[column, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }
            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        /// <summary>
        /// 設定要顯示的儲存格內容時發生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvOne_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)
                return;
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }

        /// <summary>
        /// 正在驗證儲存格時發生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvOne_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // 驗證Sap庫存取用量
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // 驗證; 只能輸入數字
                if (!Regex.IsMatch(e.FormattedValue.ToString(), @"^[0-9]*(\.[0-9]*)?$"))
                {
                    e.Cancel = true;
                    //dgvOne.Rows[e.RowIndex].ErrorText = "只能輸入數字和小數點"; // 顯示錯誤提示
                    MessageBox.Show("只能輸入數字和小數點", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // 將值還原為原始值
                    dgvOne.CancelEdit();
                    return;
                }

                // 驗證: 不可超出Sap可用庫存
                if (Convert.ToSingle(e.FormattedValue.ToString()) > Convert.ToSingle(dgvOne.Rows[e.RowIndex].Cells[1].Value))
                {
                    e.Cancel = true;
                    //dgvOne.Rows[e.RowIndex].ErrorText = "不可超出Sap可用庫存"; // 顯示錯誤提示
                    MessageBox.Show("不可超過Sap可用庫存", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // 將值還原為原始值
                    dgvOne.CancelEdit();
                    dgvOne[e.ColumnIndex, e.RowIndex].Value = listDtOneView[currentPage - 1].Rows[e.RowIndex][e.ColumnIndex].ToString();
                    return;
                }

                // 驗證: 不可超出Plex出貨數
                float total = 0; // 用來存儲所選庫存加總
                foreach (DataGridViewRow row in dgvOne.Rows)
                {
                    if (row.Cells[e.ColumnIndex].Value != null && Single.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out float cellValue))
                    {
                        if (row.Index == e.RowIndex)
                        {
                            total += Convert.ToSingle(e.FormattedValue.ToString());
                        }
                        else
                        {
                            total += cellValue;
                        }
                    }
                    float t = total;
                    string str = t.ToString("N4");
                    float.TryParse(str, out total);
                }
                if (total > Convert.ToSingle(dgvOne.Rows[e.RowIndex].Cells[2].Value))
                {
                    e.Cancel = true;
                    //dgvOne.Rows[e.RowIndex].ErrorText = "不可超出Plex出貨數"; // 顯示錯誤提示
                    MessageBox.Show("不可超過Plex出貨數", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // 將值還原為原始值
                    dgvOne.CancelEdit();
                    dgvOne[e.ColumnIndex, e.RowIndex].Value = 0;  /*listDtOneView[currentPage - 1].Rows[e.RowIndex][e.ColumnIndex].ToString();    */
                    return;
                }
            }
        }

        private void dgvOne_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // 除了庫存選用量欄位可以編輯，其他欄位都禁止編輯
            if (e.ColumnIndex != 0)
            {
                e.Cancel = true;
            }

            // Sap單位淨價與Plex單位淨價差距超過0.003美元就無法編輯
            Decimal differ;
            if (Convert.ToDecimal(listDtOneView[currentPage - 1].Rows[e.RowIndex]["Sap單位淨價"]) > listSystemSelect[currentPage - 1].NetUnitPrice)
            {
                differ = Convert.ToDecimal(listDtOneView[currentPage - 1].Rows[e.RowIndex]["Sap單位淨價"]) - listSystemSelect[currentPage - 1].NetUnitPrice;
            }
            else
            {
                differ = listSystemSelect[currentPage - 1].NetUnitPrice - Convert.ToDecimal(listDtOneView[currentPage - 1].Rows[e.RowIndex]["Sap單位淨價"]);
            }

            if (differ >= 0.003M)
            {
                MessageBox.Show("Plex與Sap差距大於0.003美元!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }

            // 若Sap庫存數量不足一律不可編輯
            if (listSystemSelect[currentPage - 1].E_MESSAGE_rfc2 != null)
            {
                MessageBox.Show("Sap庫存可用量不足!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }

        }
    }
}