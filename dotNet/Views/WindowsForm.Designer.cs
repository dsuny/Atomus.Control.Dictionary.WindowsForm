namespace Atomus.Control.Dictionary
{
    partial class WindowsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TextBox_Search = new System.Windows.Forms.TextBox();
            this.CheckBox_SearchAll = new System.Windows.Forms.CheckBox();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Controls.Add(this.TextBox_Search, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CheckBox_SearchAll, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(325, 34);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // TextBox_Search
            // 
            this.TextBox_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Search.Location = new System.Drawing.Point(3, 4);
            this.TextBox_Search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox_Search.Name = "TextBox_Search";
            this.TextBox_Search.Size = new System.Drawing.Size(290, 25);
            this.TextBox_Search.TabIndex = 1;
            this.TextBox_Search.TextChanged += new System.EventHandler(this.Text_Search_TextChanged);
            this.TextBox_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_Search_KeyDown);
            // 
            // CheckBox_SearchAll
            // 
            this.CheckBox_SearchAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBox_SearchAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckBox_SearchAll.Location = new System.Drawing.Point(299, 4);
            this.CheckBox_SearchAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CheckBox_SearchAll.Name = "CheckBox_SearchAll";
            this.CheckBox_SearchAll.Size = new System.Drawing.Size(23, 26);
            this.CheckBox_SearchAll.TabIndex = 1;
            this.CheckBox_SearchAll.UseVisualStyleBackColor = true;
            this.CheckBox_SearchAll.CheckedChanged += new System.EventHandler(this.CheckBox_SearchAll_CheckedChanged);
            // 
            // DataGridView1
            // 
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView1.Location = new System.Drawing.Point(0, 34);
            this.DataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.RowTemplate.Height = 23;
            this.DataGridView1.Size = new System.Drawing.Size(325, 292);
            this.DataGridView1.TabIndex = 0;
            this.DataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellDoubleClick);
            this.DataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataGridView1_KeyDown);
            this.DataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseDown);
            this.DataGridView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseMove);
            // 
            // WindowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 326);
            this.Controls.Add(this.DataGridView1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WindowsForm";
            this.Deactivate += new System.EventHandler(this.WindowsForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowsForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.WindowsForm_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.WindowsForm_VisibleChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WindowsForm_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView DataGridView1;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.CheckBox CheckBox_SearchAll;
    }
}