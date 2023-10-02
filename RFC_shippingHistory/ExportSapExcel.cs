using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace RFC_shippingHistory
{
    internal partial class Form1
    {
        /// <summary>
        /// 將dtResult匯出成Excel，並存入ShippingHistory資料庫中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconExport_Click(object sender, EventArgs e)
        {
            var ResultOrderedByShipperNo = from t in listSystemSelect orderby t.ShipperNo select t;
            foreach (ShippingInfo s in ResultOrderedByShipperNo)
            {
                DataRow dr = dtResult.NewRow();
                dr["銷項交貨"] = s.ShipperNo;
                dr["物料"] = s.PartNo;
                dr["客戶料號"] = s.CPartNo;
                dr["銷售文件號碼"] = s.SD;
                dr["銷售文件日期"] = s.SD_date;
                dr["客戶地址"] = s.CustomerAddressCode;
                dr["收貨方"] = s.CustomerCode;
                dr["實際發貨日期"] = s.ShipDate;
                dr["庫存數量"] = s.BatchAmount;
                dr["實際出貨數量"] = s.Quantity;
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
        /// 將 DataSet 匯出成 Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private string ExportDataSetToExcel(DataSet ds, string strPath)
        {
            lib.Control.ShowLog(tbLog, $"開始匯出Sap Excel!\r\n");
            lib.Control.ShowLog(tbLog, $"----------------------------------------------------------------------------------------------------\r\n");

            int inHeaderLength = 3, inColumn = 0, inRow = 0;
            System.Reflection.Missing Default = System.Reflection.Missing.Value;

            // 建立Excel工作簿
            strPath += @"\ShippingHistory-" + Guid.NewGuid().ToString("D") + ".xlsx";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(1);

            foreach (DataTable dt in ds.Tables)
            {
                string charTotalCol = ConvertToColumnName(dt.Columns.Count);

                // 建立Excel工作表
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add(Default, excelWorkBook.Sheets[excelWorkBook.Sheets.Count], 1, Default);
                excelWorkSheet.Name = dt.TableName; // 工作表名稱

                // 印出行名稱
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    excelWorkSheet.Cells[inHeaderLength + 1, i + 1] = dt.Columns[i].ColumnName.ToUpper();
                }

                // 印出列
                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    lib.Control.ShowPgbar(pgBar, dt.Rows.Count, m);
                    for (int n = 0; n < dt.Columns.Count; n++)
                    {
                        inColumn = n + 1;
                        inRow = inHeaderLength + 2 + m;
                        excelWorkSheet.Cells[inRow, inColumn] = dt.Rows[m][n].ToString();
                    }
                    // 若RFC沒匹配成功，以紅色背景顯示
                    if (dt.Rows[m]["收貨方"].ToString() == "" || dt.Rows[m]["批次"].ToString() == "")
                    {
                        Excel.Range Rang = excelWorkSheet.get_Range($"B{inRow}", $"{charTotalCol}{inRow}");
                        Rang.Interior.Color = ColorTranslator.FromHtml("#EFCFE3");
                    }

                    // 若「物料號碼」的值與上筆一樣就合併儲存格
                    if (m > 0 && dt.Rows[m]["銷項交貨"].ToString() == dt.Rows[m - 1]["銷項交貨"].ToString())
                    {
                        excelApp.DisplayAlerts = false;
                        excelWorkSheet.get_Range("A" + inRow.ToString(), "A" + (inRow - 1).ToString()).Merge();
                    }
                }
                excelApp.DisplayAlerts = true;

                // 印出標題
                Excel.Range cellRang = excelWorkSheet.get_Range("A1", $"{charTotalCol}3");
                cellRang.Merge(false);
                cellRang.Interior.Color = Color.White;
                cellRang.Font.Color = Color.Gray;
                cellRang.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // xlHAlignLeft 置左、xlHAlignRight 置右
                cellRang.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;  // xlVAlignTop 置上、xlVAlignBottom 置下
                cellRang.Font.Size = 26;
                excelWorkSheet.Cells[1, 1] = "Shipping History (PLEX to SAP)";

                // 為行名稱增加樣式
                cellRang = excelWorkSheet.get_Range("A4", $"{charTotalCol}4");
                cellRang.Font.Bold = true;
                cellRang.Font.Color = ColorTranslator.ToOle(Color.White);
                cellRang.Interior.Color = ColorTranslator.FromHtml("#465775");
                //excelWorkSheet.get_Range("F4").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //// Formate price column
                //excelWorkSheet.get_Range("F5").EntireColumn.NumberFormat = "0.00";
                // Auto fit columns
                excelWorkSheet.Columns.AutoFit();
            }

            // 刪除第一個工作表
            excelApp.DisplayAlerts = false;
            Excel.Worksheet lastWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets[1];
            lastWorkSheet.Delete();
            excelApp.DisplayAlerts = true;

            // 設定預設工作表
            (excelWorkBook.Sheets[1] as Excel._Worksheet).Activate();

            excelWorkBook.SaveAs(strPath, Default, Default, Default, false, Default, Excel.XlSaveAsAccessMode.xlNoChange, Default, Default, Default, Default, Default);
            excelWorkBook.Close();
            excelApp.Quit();

            MessageBox.Show($"成功匯出Sap Excel至【{strPath}】!", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return strPath;
        }
    }
}
