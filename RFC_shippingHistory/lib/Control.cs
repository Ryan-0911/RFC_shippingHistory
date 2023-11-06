using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFC_shippingHistory.lib
{
    internal class Control
    {
        delegate void logHandler(TextBox tb, string text);
        delegate void pgbarHandler(ProgressBar pgbar, int total, int current);
        delegate void comboboxItemHandler(ComboBox cb, string item);
        delegate void comboboxTextHandler(ComboBox cb, string text);
        delegate void dgvHandler(DataGridView dgv, DataTable dt);
        delegate void lblHandler(Label lbl, string error);
        delegate void picboxHandler(PictureBox picbox, bool visibleOrNot);

        /// <summary>
        /// 顯示在訊息視窗上   
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        public static void ShowLog(TextBox tb, string text)
        {
            //判斷這個TextBox物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作
                logHandler ph = new logHandler(ShowLog);
                tb.Invoke(ph, tb, text);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                //tb.Text = text + tb.Text;
                tb.AppendText(text);
            }
        }

        /// <summary>
        /// 顯示進度條
        /// </summary>
        /// <param name="pgbar"></param>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public static void ShowPgbar(ProgressBar pgbar, int total, int current)
        {
            if (pgbar.InvokeRequired)
            {
                pgbarHandler ph = new pgbarHandler(ShowPgbar);
                pgbar.Invoke(ph, pgbar, total, current);
            }
            else
            {
                pgbar.Maximum = total;
                pgbar.Value = current;
                if (pgbar.Maximum != 0 && pgbar.Value != 0)
                {
                    int percentage = (int)(((double)pgbar.Value / (double)pgbar.Maximum) * 100);
                    pgbar.Refresh();
                    pgbar.CreateGraphics().DrawString(percentage.ToString() + "%",
                                                        new Font("Arial", (float)12, FontStyle.Bold),
                                                        Brushes.Black,
                                                        new PointF(pgbar.Width / 2 - 18, pgbar.Height / 2 - 7));
                }
            }
        }

        /// <summary>
        /// 設置ComboBox選項
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        public static void AddComboBoxItem(ComboBox cb, string item)
        {
            //判斷這個ComboBox物件是否在同一個執行緒上
            if (cb.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作
                comboboxItemHandler ch = new comboboxItemHandler(AddComboBoxItem);
                cb.Invoke(ch, cb, item);
            }
            else
            {
                cb.Items.Add(item);
            }
        }

        /// <summary>
        /// 設置ComboBox文字
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        public static void SetComboBoxText(ComboBox cb, string text)
        {
            //判斷這個ComboBox物件是否在同一個執行緒上
            if (cb.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作
                comboboxTextHandler ch = new comboboxTextHandler(SetComboBoxText);
                cb.Invoke(ch, cb, text);
            }
            else
            {
                cb.Text = text;
            }
        }

        /// <summary>
        /// 將DataTable綁定至DataGridView
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        public static void BindDataTableToDataGridView(DataGridView dgv, DataTable dt)
        {
            //判斷這個ComboBox物件是否在同一個執行緒上
            if (dgv.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作
                dgvHandler dh = new dgvHandler(BindDataTableToDataGridView);
                dgv.Invoke(dh, dgv, dt);
            }
            else
            {
                dgv.DataSource = dt;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            }
        }

        /// <summary>
        /// 設置label文字
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        public static void SetLblText(Label lbl, string txt)
        {
            //判斷這個ComboBox物件是否在同一個執行緒上
            if (lbl.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作
                lblHandler lh = new lblHandler(SetLblText);
                lbl.Invoke(lh, lbl, txt);
            }
            else
            {
               lbl.Text = txt;
            }
        }


        /// <summary>
        /// 控制圖片顯示與否
        /// </summary>
        /// <param name="picbox"></param>
        /// <param name="visibleOrNot"></param>
        public static void PictureBoxVisibleOrNot(PictureBox picbox, bool visibleOrNot)
        {
            //判斷這個ComboBox物件是否在同一個執行緒上
            if (picbox.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作
                picboxHandler pbh = new picboxHandler(PictureBoxVisibleOrNot);
                picbox.Invoke(pbh, picbox, visibleOrNot);
            }
            else
            {
                picbox.Visible = visibleOrNot;
            }
        }


    }
}