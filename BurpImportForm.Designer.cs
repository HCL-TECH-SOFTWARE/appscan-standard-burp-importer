/******************************************************************
* Licensed Materials - Property of HCL
* (c) Copyright HCL Technologies Ltd. 2015, 2016.
* Note to U.S. Government Users Restricted Rights.
******************************************************************/
namespace BurpTrafficImporter
{
    partial class BurpImportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BurpImportForm));
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxSourceFilePath = new System.Windows.Forms.TextBox();
            this.checkBoxUseFirstAsSTP = new AppScan.Gui.UserControls.CheckBoxEx();
            this.checkedListBoxAdditionalDomains = new System.Windows.Forms.CheckedListBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelAdditionalDomains = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Location = new System.Drawing.Point(417, 9);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxSourceFilePath
            // 
            this.textBoxSourceFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceFilePath.Location = new System.Drawing.Point(12, 12);
            this.textBoxSourceFilePath.Name = "textBoxSourceFilePath";
            this.textBoxSourceFilePath.Size = new System.Drawing.Size(399, 20);
            this.textBoxSourceFilePath.TabIndex = 0;
            this.textBoxSourceFilePath.TextChanged += new System.EventHandler(this.textBoxSourceFilePath_TextChanged);
            // 
            // checkBoxUseFirstAsSTP
            // 
            this.checkBoxUseFirstAsSTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.checkBoxUseFirstAsSTP.CheckBoxControl.AutoSize = true;
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Location = new System.Drawing.Point(1, 1);
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Margin = new System.Windows.Forms.Padding(1);
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Name = "checkBox1";
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Size = new System.Drawing.Size(15, 15);
            this.checkBoxUseFirstAsSTP.CheckBoxControl.TabIndex = 0;
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Tag = "null";
            this.checkBoxUseFirstAsSTP.CheckBoxControl.UseVisualStyleBackColor = true;
            this.checkBoxUseFirstAsSTP.CheckBoxControl.Checked = true;
            // 
            // 
            // 
            this.checkBoxUseFirstAsSTP.LabelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxUseFirstAsSTP.LabelControl.Location = new System.Drawing.Point(20, 0);
            this.checkBoxUseFirstAsSTP.LabelControl.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.checkBoxUseFirstAsSTP.LabelControl.Name = "label1";
            this.checkBoxUseFirstAsSTP.LabelControl.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.checkBoxUseFirstAsSTP.LabelControl.Size = new System.Drawing.Size(457, 20);
            this.checkBoxUseFirstAsSTP.LabelControl.TabIndex = 1;
            this.checkBoxUseFirstAsSTP.LabelControl.Text = "Use first request as Starting Point URL";
            this.checkBoxUseFirstAsSTP.Location = new System.Drawing.Point(12, 43);
            this.checkBoxUseFirstAsSTP.Name = "checkBoxUseFirstAsSTP";
            this.checkBoxUseFirstAsSTP.Size = new System.Drawing.Size(480, 22);
            this.checkBoxUseFirstAsSTP.TabIndex = 2;
            // 
            // checkedListBoxAdditionalDomains
            // 
            this.checkedListBoxAdditionalDomains.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxAdditionalDomains.FormattingEnabled = true;
            this.checkedListBoxAdditionalDomains.Location = new System.Drawing.Point(13, 87);
            this.checkedListBoxAdditionalDomains.Name = "checkedListBoxAdditionalDomains";
            this.checkedListBoxAdditionalDomains.Size = new System.Drawing.Size(398, 79);
            this.checkedListBoxAdditionalDomains.TabIndex = 3;
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonImport.Location = new System.Drawing.Point(417, 143);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 5;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(417, 114);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelAdditionalDomains
            // 
            this.labelAdditionalDomains.AutoSize = true;
            this.labelAdditionalDomains.Location = new System.Drawing.Point(12, 68);
            this.labelAdditionalDomains.Name = "labelAdditionalDomains";
            this.labelAdditionalDomains.Size = new System.Drawing.Size(214, 13);
            this.labelAdditionalDomains.TabIndex = 6;
            this.labelAdditionalDomains.Text = "Select domains to add to Additional Domains";
            // 
            // BurpImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 170);
            this.Controls.Add(this.labelAdditionalDomains);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.checkedListBoxAdditionalDomains);
            this.Controls.Add(this.checkBoxUseFirstAsSTP);
            this.Controls.Add(this.textBoxSourceFilePath);
            this.Controls.Add(this.buttonBrowse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BurpImportForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Burp Traffic Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxSourceFilePath;
        private AppScan.Gui.UserControls.CheckBoxEx checkBoxUseFirstAsSTP;
        private System.Windows.Forms.CheckedListBox checkedListBoxAdditionalDomains;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelAdditionalDomains;
    }
}