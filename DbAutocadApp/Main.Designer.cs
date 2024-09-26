namespace DbAutocadDemoNemetschek
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
            this.btnLoadLines = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDrawLines = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadLines
            // 
            this.btnLoadLines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnLoadLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadLines.Location = new System.Drawing.Point(29, 23);
            this.btnLoadLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadLines.Name = "btnLoadLines";
            this.btnLoadLines.Size = new System.Drawing.Size(200, 62);
            this.btnLoadLines.TabIndex = 3;
            this.btnLoadLines.Text = "Load Lines to DB";
            this.btnLoadLines.UseVisualStyleBackColor = false;
            this.btnLoadLines.Click += new System.EventHandler(this.BtnLoadLines_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadLines);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(247, 156);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDrawLines);
            this.groupBox2.Location = new System.Drawing.Point(287, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(244, 156);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Retrieve";
            // 
            // btnDrawLines
            // 
            this.btnDrawLines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnDrawLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDrawLines.Location = new System.Drawing.Point(20, 23);
            this.btnDrawLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDrawLines.Name = "btnDrawLines";
            this.btnDrawLines.Size = new System.Drawing.Size(200, 62);
            this.btnDrawLines.TabIndex = 3;
            this.btnDrawLines.Text = "Retrieve Lines from DB";
            this.btnDrawLines.UseVisualStyleBackColor = false;
            this.btnDrawLines.Click += new System.EventHandler(this.BtnDrawLines_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(41, 478);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(103, 16);
            this.lblInfo.TabIndex = 12;
            this.lblInfo.Text = "Message here...";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 519);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Main Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnLoadLines;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDrawLines;
        private System.Windows.Forms.Label lblInfo;
    }
}