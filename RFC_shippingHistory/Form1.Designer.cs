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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbl請選擇檔案 = new System.Windows.Forms.Label();
            this.pgBar = new System.Windows.Forms.ProgressBar();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDate = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvOne = new System.Windows.Forms.DataGridView();
            this.lblPage = new System.Windows.Forms.Label();
            this.comboSearch = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.pictureBoxError = new System.Windows.Forms.PictureBox();
            this.iconSearch = new System.Windows.Forms.PictureBox();
            this.iconEdit = new System.Windows.Forms.PictureBox();
            this.iconPrevious = new System.Windows.Forms.PictureBox();
            this.iconLast = new System.Windows.Forms.PictureBox();
            this.iconNext = new System.Windows.Forms.PictureBox();
            this.iconFirst = new System.Windows.Forms.PictureBox();
            this.iconClear = new System.Windows.Forms.PictureBox();
            this.iconExport = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iconFile = new System.Windows.Forms.PictureBox();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconLast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFile)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl請選擇檔案
            // 
            this.lbl請選擇檔案.AutoSize = true;
            this.lbl請選擇檔案.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl請選擇檔案.Location = new System.Drawing.Point(106, 113);
            this.lbl請選擇檔案.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl請選擇檔案.Name = "lbl請選擇檔案";
            this.lbl請選擇檔案.Size = new System.Drawing.Size(114, 20);
            this.lbl請選擇檔案.TabIndex = 1;
            this.lbl請選擇檔案.Text = "匯入Plex Excel";
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
            this.lblTime.Location = new System.Drawing.Point(191, 28);
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
            this.lblDate.Location = new System.Drawing.Point(80, 28);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 17);
            this.lblDate.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1279, 620);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "匯出Sap Excel";
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
            // dgvOne
            // 
            this.dgvOne.AllowUserToAddRows = false;
            this.dgvOne.AllowUserToDeleteRows = false;
            this.dgvOne.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvOne.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvOne.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgvOne.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOne.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOne.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOne.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOne.EnableHeadersVisualStyles = false;
            this.dgvOne.Location = new System.Drawing.Point(619, 162);
            this.dgvOne.Name = "dgvOne";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOne.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOne.RowHeadersWidth = 51;
            this.dgvOne.RowTemplate.Height = 24;
            this.dgvOne.Size = new System.Drawing.Size(775, 434);
            this.dgvOne.TabIndex = 25;
            this.dgvOne.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvOne_CellBeginEdit);
            this.dgvOne.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvOne_CellFormatting);
            this.dgvOne.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvOne_CellPainting);
            this.dgvOne.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvOne_CellValidating);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPage.Location = new System.Drawing.Point(928, 658);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(112, 20);
            this.lblPage.TabIndex = 26;
            this.lblPage.Text = "第幾筆/共幾筆";
            // 
            // comboSearch
            // 
            this.comboSearch.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.comboSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSearch.Font = new System.Drawing.Font("微軟正黑體 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboSearch.FormattingEnabled = true;
            this.comboSearch.Location = new System.Drawing.Point(991, 105);
            this.comboSearch.Margin = new System.Windows.Forms.Padding(2);
            this.comboSearch.Name = "comboSearch";
            this.comboSearch.Size = new System.Drawing.Size(340, 28);
            this.comboSearch.TabIndex = 33;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblFilter.Location = new System.Drawing.Point(987, 74);
            this.lblFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(230, 20);
            this.lblFilter.TabIndex = 36;
            this.lblFilter.Text = "Plex出貨單-客戶代號/客戶料號";
            // 
            // pictureBoxError
            // 
            this.pictureBoxError.Image = global::RFC_shippingHistory.Properties.Resources.exclamation;
            this.pictureBoxError.Location = new System.Drawing.Point(619, 105);
            this.pictureBoxError.Name = "pictureBoxError";
            this.pictureBoxError.Size = new System.Drawing.Size(44, 34);
            this.pictureBoxError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxError.TabIndex = 42;
            this.pictureBoxError.TabStop = false;
            // 
            // iconSearch
            // 
            this.iconSearch.Image = global::RFC_shippingHistory.Properties.Resources.find;
            this.iconSearch.Location = new System.Drawing.Point(1352, 102);
            this.iconSearch.Name = "iconSearch";
            this.iconSearch.Size = new System.Drawing.Size(42, 39);
            this.iconSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconSearch.TabIndex = 37;
            this.iconSearch.TabStop = false;
            this.iconSearch.Click += new System.EventHandler(this.iconSearch_Click);
            // 
            // iconEdit
            // 
            this.iconEdit.Image = global::RFC_shippingHistory.Properties.Resources.edit;
            this.iconEdit.Location = new System.Drawing.Point(956, 606);
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
            this.iconPrevious.Location = new System.Drawing.Point(881, 610);
            this.iconPrevious.Name = "iconPrevious";
            this.iconPrevious.Size = new System.Drawing.Size(36, 30);
            this.iconPrevious.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPrevious.TabIndex = 30;
            this.iconPrevious.TabStop = false;
            this.iconPrevious.Click += new System.EventHandler(this.iconPrevious_Click);
            // 
            // iconLast
            // 
            this.iconLast.Image = global::RFC_shippingHistory.Properties.Resources.fast_forward;
            this.iconLast.Location = new System.Drawing.Point(1095, 611);
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
            this.iconNext.Location = new System.Drawing.Point(1039, 610);
            this.iconNext.Name = "iconNext";
            this.iconNext.Size = new System.Drawing.Size(35, 30);
            this.iconNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconNext.TabIndex = 28;
            this.iconNext.TabStop = false;
            this.iconNext.Click += new System.EventHandler(this.iconNext_Click);
            // 
            // iconFirst
            // 
            this.iconFirst.Image = global::RFC_shippingHistory.Properties.Resources.rewind;
            this.iconFirst.Location = new System.Drawing.Point(829, 611);
            this.iconFirst.Name = "iconFirst";
            this.iconFirst.Size = new System.Drawing.Size(46, 30);
            this.iconFirst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconFirst.TabIndex = 27;
            this.iconFirst.TabStop = false;
            this.iconFirst.Click += new System.EventHandler(this.iconFirst_Click);
            // 
            // iconClear
            // 
            this.iconClear.Image = global::RFC_shippingHistory.Properties.Resources.clear_format;
            this.iconClear.Location = new System.Drawing.Point(526, 608);
            this.iconClear.Name = "iconClear";
            this.iconClear.Size = new System.Drawing.Size(67, 33);
            this.iconClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconClear.TabIndex = 23;
            this.iconClear.TabStop = false;
            this.iconClear.Click += new System.EventHandler(this.iconClear_Click);
            // 
            // iconExport
            // 
            this.iconExport.Image = global::RFC_shippingHistory.Properties.Resources.export__1_;
            this.iconExport.Location = new System.Drawing.Point(1214, 606);
            this.iconExport.Name = "iconExport";
            this.iconExport.Size = new System.Drawing.Size(43, 39);
            this.iconExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconExport.TabIndex = 22;
            this.iconExport.TabStop = false;
            this.iconExport.Click += new System.EventHandler(this.iconExport_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RFC_shippingHistory.Properties.Resources.time;
            this.pictureBox1.Location = new System.Drawing.Point(42, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // iconFile
            // 
            this.iconFile.Image = global::RFC_shippingHistory.Properties.Resources.excel;
            this.iconFile.Location = new System.Drawing.Point(44, 105);
            this.iconFile.Margin = new System.Windows.Forms.Padding(2);
            this.iconFile.Name = "iconFile";
            this.iconFile.Size = new System.Drawing.Size(58, 39);
            this.iconFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconFile.TabIndex = 4;
            this.iconFile.TabStop = false;
            this.iconFile.Tag = "匯入Plex Excel";
            this.iconFile.Click += new System.EventHandler(this.iconFile_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblError.Location = new System.Drawing.Point(684, 108);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(73, 20);
            this.lblError.TabIndex = 43;
            this.lblError.Text = "錯誤原因";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1463, 714);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.pictureBoxError);
            this.Controls.Add(this.iconSearch);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.comboSearch);
            this.Controls.Add(this.iconEdit);
            this.Controls.Add(this.iconPrevious);
            this.Controls.Add(this.iconLast);
            this.Controls.Add(this.iconNext);
            this.Controls.Add(this.iconFirst);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.dgvOne);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.iconClear);
            this.Controls.Add(this.iconExport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.pgBar);
            this.Controls.Add(this.iconFile);
            this.Controls.Add(this.lbl請選擇檔案);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "出貨歷史";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconLast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl請選擇檔案;
        private System.Windows.Forms.PictureBox iconFile;
        private System.Windows.Forms.ProgressBar pgBar;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox iconExport;
        private System.Windows.Forms.PictureBox iconClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvOne;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.PictureBox iconFirst;
        private System.Windows.Forms.PictureBox iconNext;
        private System.Windows.Forms.PictureBox iconLast;
        private System.Windows.Forms.PictureBox iconPrevious;
        private System.Windows.Forms.PictureBox iconEdit;
        private System.Windows.Forms.ComboBox comboSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.PictureBox iconSearch;
        private System.Windows.Forms.PictureBox pictureBoxError;
        private System.Windows.Forms.Label lblError;
    }
}

