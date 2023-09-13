namespace RFC_shippingHistory
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbl請選擇檔案 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pgBar = new System.Windows.Forms.ProgressBar();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDate = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iconDB = new System.Windows.Forms.PictureBox();
            this.iconFolder = new System.Windows.Forms.PictureBox();
            this.iconFile = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFile)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl請選擇檔案
            // 
            this.lbl請選擇檔案.AutoSize = true;
            this.lbl請選擇檔案.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl請選擇檔案.Location = new System.Drawing.Point(109, 116);
            this.lbl請選擇檔案.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl請選擇檔案.Name = "lbl請選擇檔案";
            this.lbl請選擇檔案.Size = new System.Drawing.Size(73, 20);
            this.lbl請選擇檔案.TabIndex = 1;
            this.lbl請選擇檔案.Text = "選擇檔案";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(318, 116);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "選擇資料夾";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(40, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(249, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "請點選圖示選擇存入資料庫的來源";
            // 
            // pgBar
            // 
            this.pgBar.ForeColor = System.Drawing.Color.Lime;
            this.pgBar.Location = new System.Drawing.Point(44, 441);
            this.pgBar.Margin = new System.Windows.Forms.Padding(2);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(752, 18);
            this.pgBar.TabIndex = 12;
            // 
            // tbLog
            // 
            this.tbLog.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbLog.Location = new System.Drawing.Point(40, 160);
            this.tbLog.Margin = new System.Windows.Forms.Padding(2);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(756, 259);
            this.tbLog.TabIndex = 13;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTime.Location = new System.Drawing.Point(691, 39);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 17);
            this.lblTime.TabIndex = 14;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDate.Location = new System.Drawing.Point(662, 17);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 17);
            this.lblDate.TabIndex = 15;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RFC_shippingHistory.Properties.Resources.time;
            this.pictureBox1.Location = new System.Drawing.Point(646, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // iconDB
            // 
            this.iconDB.Image = global::RFC_shippingHistory.Properties.Resources.database;
            this.iconDB.Location = new System.Drawing.Point(750, 476);
            this.iconDB.Margin = new System.Windows.Forms.Padding(2);
            this.iconDB.Name = "iconDB";
            this.iconDB.Size = new System.Drawing.Size(58, 39);
            this.iconDB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconDB.TabIndex = 6;
            this.iconDB.TabStop = false;
            // 
            // iconFolder
            // 
            this.iconFolder.Image = global::RFC_shippingHistory.Properties.Resources.open_folder;
            this.iconFolder.Location = new System.Drawing.Point(44, 104);
            this.iconFolder.Margin = new System.Windows.Forms.Padding(2);
            this.iconFolder.Name = "iconFolder";
            this.iconFolder.Size = new System.Drawing.Size(50, 42);
            this.iconFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconFolder.TabIndex = 5;
            this.iconFolder.TabStop = false;
            this.iconFolder.Click += new System.EventHandler(this.iconFolder_Click);
            // 
            // iconFile
            // 
            this.iconFile.Image = global::RFC_shippingHistory.Properties.Resources.excel;
            this.iconFile.Location = new System.Drawing.Point(245, 107);
            this.iconFile.Margin = new System.Windows.Forms.Padding(2);
            this.iconFile.Name = "iconFile";
            this.iconFile.Size = new System.Drawing.Size(58, 39);
            this.iconFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconFile.TabIndex = 4;
            this.iconFile.TabStop = false;
            this.iconFile.Click += new System.EventHandler(this.iconFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 544);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.pgBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.iconDB);
            this.Controls.Add(this.iconFolder);
            this.Controls.Add(this.iconFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl請選擇檔案);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "出貨歷史";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl請選擇檔案;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox iconFile;
        private System.Windows.Forms.PictureBox iconFolder;
        private System.Windows.Forms.PictureBox iconDB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgBar;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

