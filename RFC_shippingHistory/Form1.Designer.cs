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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.lblPage = new System.Windows.Forms.Label();
            this.iconEdit = new System.Windows.Forms.PictureBox();
            this.iconPrevious = new System.Windows.Forms.PictureBox();
            this.iconLast = new System.Windows.Forms.PictureBox();
            this.iconNext = new System.Windows.Forms.PictureBox();
            this.icobFirst = new System.Windows.Forms.PictureBox();
            this.iconClear = new System.Windows.Forms.PictureBox();
            this.iconExport = new System.Windows.Forms.PictureBox();
            this.iconWrite2SAP = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iconFolder = new System.Windows.Forms.PictureBox();
            this.iconFile = new System.Windows.Forms.PictureBox();
            this.dgvEdit = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconLast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icobFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWrite2SAP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl請選擇檔案
            // 
            this.lbl請選擇檔案.AutoSize = true;
            this.lbl請選擇檔案.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl請選擇檔案.Location = new System.Drawing.Point(307, 116);
            this.lbl請選擇檔案.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl請選擇檔案.Name = "lbl請選擇檔案";
            this.lbl請選擇檔案.Size = new System.Drawing.Size(89, 20);
            this.lbl請選擇檔案.TabIndex = 1;
            this.lbl請選擇檔案.Text = "從檔案匯入";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(98, 116);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "從資料夾匯入";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.IndianRed;
            this.label3.Location = new System.Drawing.Point(40, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "請點選圖示執行功能";
            // 
            // pgBar
            // 
            this.pgBar.ForeColor = System.Drawing.Color.Lime;
            this.pgBar.Location = new System.Drawing.Point(44, 578);
            this.pgBar.Margin = new System.Windows.Forms.Padding(2);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(538, 18);
            this.pgBar.TabIndex = 12;
            // 
            // tbLog
            // 
            this.tbLog.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbLog.Location = new System.Drawing.Point(44, 162);
            this.tbLog.Margin = new System.Windows.Forms.Padding(2);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(538, 396);
            this.tbLog.TabIndex = 13;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTime.Location = new System.Drawing.Point(1288, 43);
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
            this.lblDate.Location = new System.Drawing.Point(1182, 43);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 17);
            this.lblDate.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(687, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "建立出貨單";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1295, 116);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "匯出成 Excel";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(509, 621);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 20);
            this.label5.TabIndex = 24;
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Location = new System.Drawing.Point(619, 162);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowTemplate.Height = 24;
            this.dgvResult.Size = new System.Drawing.Size(775, 434);
            this.dgvResult.TabIndex = 25;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPage.Location = new System.Drawing.Point(1282, 621);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(112, 20);
            this.lblPage.TabIndex = 26;
            this.lblPage.Text = "第幾筆/共幾筆";
            // 
            // iconEdit
            // 
            this.iconEdit.Image = global::RFC_shippingHistory.Properties.Resources.edit;
            this.iconEdit.Location = new System.Drawing.Point(996, 607);
            this.iconEdit.Name = "iconEdit";
            this.iconEdit.Size = new System.Drawing.Size(54, 39);
            this.iconEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconEdit.TabIndex = 31;
            this.iconEdit.TabStop = false;
            this.iconEdit.Click += new System.EventHandler(this.iconEdit_Click);
            // 
            // iconPrevious
            // 
            this.iconPrevious.Image = global::RFC_shippingHistory.Properties.Resources.left_arrow__1_;
            this.iconPrevious.Location = new System.Drawing.Point(921, 611);
            this.iconPrevious.Name = "iconPrevious";
            this.iconPrevious.Size = new System.Drawing.Size(36, 30);
            this.iconPrevious.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPrevious.TabIndex = 30;
            this.iconPrevious.TabStop = false;
            this.iconPrevious.Click += new System.EventHandler(this.iconPrevous_Click);
            // 
            // iconLast
            // 
            this.iconLast.Image = global::RFC_shippingHistory.Properties.Resources.fast_forward;
            this.iconLast.Location = new System.Drawing.Point(1135, 612);
            this.iconLast.Name = "iconLast";
            this.iconLast.Size = new System.Drawing.Size(41, 30);
            this.iconLast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconLast.TabIndex = 29;
            this.iconLast.TabStop = false;
            this.iconLast.Click += new System.EventHandler(this.iconLast_Click);
            // 
            // iconNext
            // 
            this.iconNext.Image = global::RFC_shippingHistory.Properties.Resources.right_arrow__1_;
            this.iconNext.Location = new System.Drawing.Point(1079, 611);
            this.iconNext.Name = "iconNext";
            this.iconNext.Size = new System.Drawing.Size(35, 30);
            this.iconNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconNext.TabIndex = 28;
            this.iconNext.TabStop = false;
            this.iconNext.Click += new System.EventHandler(this.iconNext_Click);
            // 
            // icobFirst
            // 
            this.icobFirst.Image = global::RFC_shippingHistory.Properties.Resources.rewind;
            this.icobFirst.Location = new System.Drawing.Point(869, 612);
            this.icobFirst.Name = "icobFirst";
            this.icobFirst.Size = new System.Drawing.Size(46, 30);
            this.icobFirst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icobFirst.TabIndex = 27;
            this.icobFirst.TabStop = false;
            this.icobFirst.Click += new System.EventHandler(this.icobFirst_Click);
            // 
            // iconClear
            // 
            this.iconClear.Image = global::RFC_shippingHistory.Properties.Resources.clean;
            this.iconClear.Location = new System.Drawing.Point(524, 612);
            this.iconClear.Name = "iconClear";
            this.iconClear.Size = new System.Drawing.Size(58, 40);
            this.iconClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconClear.TabIndex = 23;
            this.iconClear.TabStop = false;
            this.iconClear.Click += new System.EventHandler(this.iconClear_Click);
            // 
            // iconExport
            // 
            this.iconExport.Image = global::RFC_shippingHistory.Properties.Resources.export;
            this.iconExport.Location = new System.Drawing.Point(1246, 99);
            this.iconExport.Name = "iconExport";
            this.iconExport.Size = new System.Drawing.Size(42, 57);
            this.iconExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconExport.TabIndex = 22;
            this.iconExport.TabStop = false;
            this.iconExport.Click += new System.EventHandler(this.iconExport_Click);
            // 
            // iconWrite2SAP
            // 
            this.iconWrite2SAP.Image = global::RFC_shippingHistory.Properties.Resources.sap;
            this.iconWrite2SAP.Location = new System.Drawing.Point(619, 100);
            this.iconWrite2SAP.Name = "iconWrite2SAP";
            this.iconWrite2SAP.Size = new System.Drawing.Size(63, 56);
            this.iconWrite2SAP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconWrite2SAP.TabIndex = 18;
            this.iconWrite2SAP.TabStop = false;
            this.iconWrite2SAP.Click += new System.EventHandler(this.iconWrite2SAP_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RFC_shippingHistory.Properties.Resources.time;
            this.pictureBox1.Location = new System.Drawing.Point(1146, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // iconFolder
            // 
            this.iconFolder.Image = global::RFC_shippingHistory.Properties.Resources.open_folder;
            this.iconFolder.Location = new System.Drawing.Point(44, 107);
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
            // dgvEdit
            // 
            this.dgvEdit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEdit.Location = new System.Drawing.Point(132, 294);
            this.dgvEdit.Name = "dgvEdit";
            this.dgvEdit.ReadOnly = true;
            this.dgvEdit.RowTemplate.Height = 24;
            this.dgvEdit.Size = new System.Drawing.Size(441, 264);
            this.dgvEdit.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1473, 698);
            this.Controls.Add(this.iconEdit);
            this.Controls.Add(this.iconPrevious);
            this.Controls.Add(this.iconLast);
            this.Controls.Add(this.iconNext);
            this.Controls.Add(this.icobFirst);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.iconClear);
            this.Controls.Add(this.iconExport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvEdit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iconWrite2SAP);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.pgBar);
            this.Controls.Add(this.label3);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconLast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icobFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWrite2SAP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl請選擇檔案;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox iconFile;
        private System.Windows.Forms.PictureBox iconFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pgBar;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox iconWrite2SAP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox iconExport;
        private System.Windows.Forms.PictureBox iconClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.PictureBox icobFirst;
        private System.Windows.Forms.PictureBox iconNext;
        private System.Windows.Forms.PictureBox iconLast;
        private System.Windows.Forms.PictureBox iconPrevious;
        private System.Windows.Forms.PictureBox iconEdit;
        private System.Windows.Forms.DataGridView dgvEdit;
    }
}

