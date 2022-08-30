namespace mycalltruck.Admin
{
    partial class FrmPopbillRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPopbillRegister));
            this.LayoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.WebContainer = new System.Windows.Forms.Panel();
            this.Web = new System.Windows.Forms.WebBrowser();
            this.LayoutRoot.SuspendLayout();
            this.WebContainer.SuspendLayout();
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
            this.LayoutRoot.Size = new System.Drawing.Size(1184, 611);
            this.LayoutRoot.TabIndex = 0;
            // 
            // WebContainer
            // 
            this.WebContainer.Controls.Add(this.Web);
            this.WebContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebContainer.Location = new System.Drawing.Point(3, 3);
            this.WebContainer.Name = "WebContainer";
            this.WebContainer.Size = new System.Drawing.Size(1178, 605);
            this.WebContainer.TabIndex = 0;
            // 
            // Web
            // 
            this.Web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Web.Location = new System.Drawing.Point(0, 0);
            this.Web.MinimumSize = new System.Drawing.Size(20, 20);
            this.Web.Name = "Web";
            this.Web.Size = new System.Drawing.Size(1178, 605);
            this.Web.TabIndex = 0;
            // 
            // FrmPopbillRegister
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1184, 611);
            this.Controls.Add(this.LayoutRoot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPopbillRegister";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "전자세금관리";
            this.Load += new System.EventHandler(this.FrmPopbill_Load);
            this.LayoutRoot.ResumeLayout(false);
            this.WebContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutRoot;
        private System.Windows.Forms.Panel WebContainer;
        private System.Windows.Forms.WebBrowser Web;
    }
}