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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.iconSearch = new System.Windows.Forms.PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.iconSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl請選擇檔案
            // 
            this.lbl請選擇檔案.AutoSize = true;
            this.lbl請選擇檔案.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl請選擇檔案.Location = new System.Drawing.Point(383, 138);
            this.lbl請選擇檔案.Name = "lbl請選擇檔案";
            this.lbl請選擇檔案.Size = new System.Drawing.Size(112, 25);
            this.lbl請選擇檔案.TabIndex = 1;
            this.lbl請選擇檔案.Text = "從檔案匯入";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(132, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "從資料夾匯入";
            // 
            // pgBar
            // 
            this.pgBar.ForeColor = System.Drawing.Color.Lime;
            this.pgBar.Location = new System.Drawing.Point(59, 722);
            this.pgBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(717, 22);
            this.pgBar.TabIndex = 12;
            // 
            // tbLog
            // 
            this.tbLog.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbLog.Location = new System.Drawing.Point(59, 202);
            this.tbLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(716, 494);
            this.tbLog.TabIndex = 13;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTime.Location = new System.Drawing.Point(246, 35);
            this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 22);
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
            this.lblDate.Location = new System.Drawing.Point(107, 35);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 22);
            this.lblDate.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(664, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 25);
            this.label2.TabIndex = 19;
            this.label2.Text = "建立出貨單";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1734, 765);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 25);
            this.label4.TabIndex = 21;
            this.label4.Text = "匯出成 Excel";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(679, 776);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 25);
            this.label5.TabIndex = 24;
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Location = new System.Drawing.Point(825, 202);
            this.dgvResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersWidth = 51;
            this.dgvResult.RowTemplate.Height = 24;
            this.dgvResult.Size = new System.Drawing.Size(1033, 542);
            this.dgvResult.TabIndex = 25;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPage.Location = new System.Drawing.Point(1290, 824);
            this.lblPage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(140, 25);
            this.lblPage.TabIndex = 26;
            this.lblPage.Text = "第幾筆/共幾筆";
            // 
            // iconEdit
            // 
            this.iconEdit.Image = global::RFC_shippingHistory.Properties.Resources.edit;
            this.iconEdit.Location = new System.Drawing.Point(1328, 759);
            this.iconEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconEdit.Name = "iconEdit";
            this.iconEdit.Size = new System.Drawing.Size(72, 49);
            this.iconEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconEdit.TabIndex = 31;
            this.iconEdit.TabStop = false;
            this.iconEdit.Click += new System.EventHandler(this.iconEdit_Click);
            // 
            // iconPrevious
            // 
            this.iconPrevious.Image = global::RFC_shippingHistory.Properties.Resources.left_arrow__1_;
            this.iconPrevious.Location = new System.Drawing.Point(1228, 764);
            this.iconPrevious.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconPrevious.Name = "iconPrevious";
            this.iconPrevious.Size = new System.Drawing.Size(48, 38);
            this.iconPrevious.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPrevious.TabIndex = 30;
            this.iconPrevious.TabStop = false;
            this.iconPrevious.Click += new System.EventHandler(this.iconPrevous_Click);
            // 
            // iconLast
            // 
            this.iconLast.Image = global::RFC_shippingHistory.Properties.Resources.fast_forward;
            this.iconLast.Location = new System.Drawing.Point(1513, 765);
            this.iconLast.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconLast.Name = "iconLast";
            this.iconLast.Size = new System.Drawing.Size(55, 38);
            this.iconLast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconLast.TabIndex = 29;
            this.iconLast.TabStop = false;
            this.iconLast.Click += new System.EventHandler(this.iconLast_Click);
            // 
            // iconNext
            // 
            this.iconNext.Image = global::RFC_shippingHistory.Properties.Resources.right_arrow__1_;
            this.iconNext.Location = new System.Drawing.Point(1439, 764);
            this.iconNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconNext.Name = "iconNext";
            this.iconNext.Size = new System.Drawing.Size(47, 38);
            this.iconNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconNext.TabIndex = 28;
            this.iconNext.TabStop = false;
            this.iconNext.Click += new System.EventHandler(this.iconNext_Click);
            // 
            // icobFirst
            // 
            this.icobFirst.Image = global::RFC_shippingHistory.Properties.Resources.rewind;
            this.icobFirst.Location = new System.Drawing.Point(1159, 765);
            this.icobFirst.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.icobFirst.Name = "icobFirst";
            this.icobFirst.Size = new System.Drawing.Size(61, 38);
            this.icobFirst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icobFirst.TabIndex = 27;
            this.icobFirst.TabStop = false;
            this.icobFirst.Click += new System.EventHandler(this.icobFirst_Click);
            // 
            // iconClear
            // 
            this.iconClear.Image = global::RFC_shippingHistory.Properties.Resources.clean;
            this.iconClear.Location = new System.Drawing.Point(699, 765);
            this.iconClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconClear.Name = "iconClear";
            this.iconClear.Size = new System.Drawing.Size(98, 63);
            this.iconClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconClear.TabIndex = 23;
            this.iconClear.TabStop = false;
            this.iconClear.Click += new System.EventHandler(this.iconClear_Click);
            // 
            // iconExport
            // 
            this.iconExport.Image = global::RFC_shippingHistory.Properties.Resources.export;
            this.iconExport.Location = new System.Drawing.Point(1670, 752);
            this.iconExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconExport.Name = "iconExport";
            this.iconExport.Size = new System.Drawing.Size(57, 63);
            this.iconExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconExport.TabIndex = 22;
            this.iconExport.TabStop = false;
            this.iconExport.Click += new System.EventHandler(this.iconExport_Click);
            // 
            // iconWrite2SAP
            // 
            this.iconWrite2SAP.Image = global::RFC_shippingHistory.Properties.Resources.sap;
            this.iconWrite2SAP.Location = new System.Drawing.Point(573, 119);
            this.iconWrite2SAP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconWrite2SAP.Name = "iconWrite2SAP";
            this.iconWrite2SAP.Size = new System.Drawing.Size(83, 71);
            this.iconWrite2SAP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconWrite2SAP.TabIndex = 18;
            this.iconWrite2SAP.TabStop = false;
            this.iconWrite2SAP.Click += new System.EventHandler(this.iconWrite2SAP_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RFC_shippingHistory.Properties.Resources.time;
            this.pictureBox1.Location = new System.Drawing.Point(56, 26);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // iconFolder
            // 
            this.iconFolder.Image = global::RFC_shippingHistory.Properties.Resources.open_folder;
            this.iconFolder.Location = new System.Drawing.Point(59, 124);
            this.iconFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconFolder.Name = "iconFolder";
            this.iconFolder.Size = new System.Drawing.Size(67, 52);
            this.iconFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconFolder.TabIndex = 5;
            this.iconFolder.TabStop = false;
            this.iconFolder.Click += new System.EventHandler(this.iconFolder_Click);
            // 
            // iconFile
            // 
            this.iconFile.Image = global::RFC_shippingHistory.Properties.Resources.excel;
            this.iconFile.Location = new System.Drawing.Point(300, 127);
            this.iconFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconFile.Name = "iconFile";
            this.iconFile.Size = new System.Drawing.Size(77, 49);
            this.iconFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconFile.TabIndex = 4;
            this.iconFile.TabStop = false;
            this.iconFile.Click += new System.EventHandler(this.iconFile_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1105, 132);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 33);
            this.comboBox1.TabIndex = 33;
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(1503, 130);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(264, 33);
            this.comboBox2.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(1394, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 25);
            this.label6.TabIndex = 35;
            this.label6.Text = "客戶地址";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(997, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 25);
            this.label7.TabIndex = 36;
            this.label7.Text = "客戶料號";
            // 
            // iconSearch
            // 
            this.iconSearch.Image = global::RFC_shippingHistory.Properties.Resources.loupe;
            this.iconSearch.Location = new System.Drawing.Point(1803, 127);
            this.iconSearch.Margin = new System.Windows.Forms.Padding(4);
            this.iconSearch.Name = "iconSearch";
            this.iconSearch.Size = new System.Drawing.Size(55, 49);
            this.iconSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconSearch.TabIndex = 37;
            this.iconSearch.TabStop = false;
            this.iconSearch.Click += new System.EventHandler(this.iconSearch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1924, 872);
            this.Controls.Add(this.iconSearch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
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
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iconWrite2SAP);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.pgBar);
            this.Controls.Add(this.iconFolder);
            this.Controls.Add(this.iconFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl請選擇檔案);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            ((System.ComponentModel.ISupportInitialize)(this.iconSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl請選擇檔案;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox iconFile;
        private System.Windows.Forms.PictureBox iconFolder;
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
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox iconSearch;
    }
}

