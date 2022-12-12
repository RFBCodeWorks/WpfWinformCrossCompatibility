
namespace RobotSoftwareTool.UserForms.Shared
{
    partial class MultiFileSelector
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
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Accept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select which files you wish to copy into the destination";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 75);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(322, 289);
            this.checkedListBox1.TabIndex = 1;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Cancel.Location = new System.Drawing.Point(177, 370);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_Accept
            // 
            this.Btn_Accept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Accept.Location = new System.Drawing.Point(258, 370);
            this.Btn_Accept.Name = "Btn_Accept";
            this.Btn_Accept.Size = new System.Drawing.Size(75, 23);
            this.Btn_Accept.TabIndex = 3;
            this.Btn_Accept.Text = "Accept";
            this.Btn_Accept.UseVisualStyleBackColor = true;
            this.Btn_Accept.Click += new System.EventHandler(this.Btn_Accept_Click);
            // 
            // MultiFileSelector
            // 
            this.AcceptButton = this.Btn_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Btn_Cancel;
            this.ClientSize = new System.Drawing.Size(346, 406);
            this.ControlBox = false;
            this.Controls.Add(this.Btn_Accept);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiFileSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MultiFileSelector";
            this.Load += new System.EventHandler(this.MultiFileSelector_Load);
            this.Shown += new System.EventHandler(this.OnShownEvent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Accept;
    }
}