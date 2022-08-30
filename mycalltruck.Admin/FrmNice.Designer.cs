namespace mycalltruck.Admin
{
    partial class FrmNice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNice));
            this.LayoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.WebContainer = new System.Windows.Forms.Panel();
            this.Web = new System.Windows.Forms.WebBrowser();
            this.baseDataSet = new mycalltruck.Admin.DataSets.BaseDataSet();
            this.AdminInfoesTableAdapter = new mycalltruck.Admin.DataSets.BaseDataSetTableAdapters.AdminInfoesTableAdapter();
            this.LayoutRoot.SuspendLayout();
            this.WebContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutRoot
            // 
            this.LayoutRoot.ColumnCount = 1;
            this.LayoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.LayoutRoot.Controls.Add(this.WebContainer, 0, 0);
            this.LayoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutRoot.Location = new System.Drawing.Point(0, 0);
            this.LayoutRoot.Name = "LayoutRoot";
            this.LayoutRoot.RowCount = 1;
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.LayoutRoot.Size = new System.Drawing.Size(1184, 711);
            this.LayoutRoot.TabIndex = 0;
            // 
            // WebContainer
            // 
            this.WebContainer.Controls.Add(this.Web);
            this.WebContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebContainer.Location = new System.Drawing.Point(3, 3);
            this.WebContainer.Name = "WebContainer";
            this.WebContainer.Size = new System.Drawing.Size(1178, 705);
            this.WebContainer.TabIndex = 0;
            // 
            // Web
            // 
            this.Web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Web.Location = new System.Drawing.Point(0, 0);
            this.Web.MinimumSize = new System.Drawing.Size(20, 20);
            this.Web.Name = "Web";
            this.Web.Size = new System.Drawing.Size(1178, 705);
            this.Web.TabIndex = 1;
            // 
            // baseDataSet
            // 
            this.baseDataSet.DataSetName = "BaseDataSet";
            this.baseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // AdminInfoesTableAdapter
            // 
            this.AdminInfoesTableAdapter.ClearBeforeFill = true;
            // 
            // FrmNice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1184, 711);
            this.Controls.Add(this.LayoutRoot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmNice";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "전자세금관리";
            this.Load += new System.EventHandler(this.FrmPopbill_Load);
            this.LayoutRoot.ResumeLayout(false);
            this.WebContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutRoot;
        private System.Windows.Forms.Panel WebContainer;
        private DataSets.BaseDataSet baseDataSet;
        private DataSets.BaseDataSetTableAdapters.AdminInfoesTableAdapter AdminInfoesTableAdapter;
        private System.Windows.Forms.WebBrowser Web;
    }
}