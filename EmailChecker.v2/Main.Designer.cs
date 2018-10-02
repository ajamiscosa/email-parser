namespace EmailChecker.v2
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.txtUsername = new SwingWERX.Controls.InputBox(this.components);
            this.txtPassword = new SwingWERX.Controls.InputBox(this.components);
            this.txtPath = new SwingWERX.Controls.InputBox(this.components);
            this.btnProcess = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(12, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.MaximumSize = new System.Drawing.Size(313, 2);
            this.label1.MinimumSize = new System.Drawing.Size(313, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 2);
            this.label1.TabIndex = 3;
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(292, 96);
            this.btnChooseFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(33, 32);
            this.btnChooseFile.TabIndex = 5;
            this.btnChooseFile.Text = "...";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.OpenExcelFile);
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12.5F);
            this.txtUsername.Location = new System.Drawing.Point(12, 12);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.NullText = "LinkedIn Username";
            this.txtUsername.Size = new System.Drawing.Size(312, 30);
            this.txtUsername.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12.5F);
            this.txtPassword.Location = new System.Drawing.Point(12, 47);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.NullText = "LinkedIn Password";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(312, 30);
            this.txtPassword.TabIndex = 8;
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Segoe UI", 12.5F);
            this.txtPath.Location = new System.Drawing.Point(12, 97);
            this.txtPath.Name = "txtPath";
            this.txtPath.NullText = "Input Excel Path";
            this.txtPath.Size = new System.Drawing.Size(274, 30);
            this.txtPath.TabIndex = 9;
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Segoe UI", 12.5F);
            this.btnProcess.Location = new System.Drawing.Point(11, 173);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(314, 32);
            this.btnProcess.TabIndex = 10;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.StartProcess);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 135);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(312, 32);
            this.progressBar.TabIndex = 11;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 216);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email Verifier";
            this.Load += new System.EventHandler(this.LoadEvent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChooseFile;
        private SwingWERX.Controls.InputBox txtUsername;
        private SwingWERX.Controls.InputBox txtPassword;
        private SwingWERX.Controls.InputBox txtPath;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

