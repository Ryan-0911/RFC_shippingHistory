using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFC_shippingHistory
{
    internal partial class Form1
    {
        ///// <summary>
        ///// 從資料夾匯入 Plex Excel
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void iconFolder_Click(object sender, EventArgs e)
        //{
        //    ClearList();


        //    ImportFolder();

        //    // Step 1: 取得 CustomerCode
        //    getCustomerCode();

        //    // Step 2: 取得某客戶某物料的批次庫存狀況
        //    dealWithBatch();

        //    // 以客戶物料、客戶地址將listAllOptions分成多頁 (一頁等於一個DataTable)
        //    LoadListDtOneView();

        //    // 將進度條歸零
        //    lib.Control.ShowPgbar(pgBar, 0, 0);
        //}

        /// <summary>
        /// 從檔案匯入Plex Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconFile_Click(object sender, EventArgs e)
        {
            ClearList();


            bool CallSap = ImportFile();

            if (CallSap)
            {
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
            };
        }

        /// <summary>
        /// 匯入Plex Excel檔案並將之存入ShippingHistory資料庫
        /// </summary>
        protected bool ImportFile()
        {
            OpenFileDialog dialog = new OpenFileDialog(); //建立檔案選擇視窗
            dialog.Title = "選擇要匯入的excel檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "xls Files(*.xls; *.xlsx; *.csv)| *.xls; *.xlsx; *.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult importPlex = MessageBox.Show($"確定要將 [{dialog.FileName}] 匯入嗎?", "匯入確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (importPlex == DialogResult.OK)
                {
                    ExcelProcessAll(dialog.FileName, "Plex");
                    return true;
                }
                return false;
            }
            return false;
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
    }
}
