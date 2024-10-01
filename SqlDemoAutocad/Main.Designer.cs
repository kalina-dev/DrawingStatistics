namespace AutoCAD.SQL.Plugin
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
            btnLoadLines = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            btnDrawLines = new Button();
            lblInfo = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnLoadLines
            // 
            btnLoadLines.BackColor = Color.FromArgb(192, 255, 192);
            btnLoadLines.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoadLines.Location = new Point(29, 23);
            btnLoadLines.Margin = new Padding(4, 4, 4, 4);
            btnLoadLines.Name = "btnLoadLines";
            btnLoadLines.Size = new Size(200, 62);
            btnLoadLines.TabIndex = 3;
            btnLoadLines.Text = "Load Lines to DB";
            btnLoadLines.UseVisualStyleBackColor = false;
            btnLoadLines.Click += BtnLoadLines_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnLoadLines);
            groupBox1.Location = new Point(16, 15);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(247, 156);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Create";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnDrawLines);
            groupBox2.Location = new Point(287, 15);
            groupBox2.Margin = new Padding(4, 4, 4, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 4, 4, 4);
            groupBox2.Size = new Size(244, 156);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Retrieve";
            // 
            // btnDrawLines
            // 
            btnDrawLines.BackColor = Color.FromArgb(255, 128, 0);
            btnDrawLines.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDrawLines.Location = new Point(20, 23);
            btnDrawLines.Margin = new Padding(4, 4, 4, 4);
            btnDrawLines.Name = "btnDrawLines";
            btnDrawLines.Size = new Size(200, 62);
            btnDrawLines.TabIndex = 3;
            btnDrawLines.Text = "Retrieve Lines from DB";
            btnDrawLines.UseVisualStyleBackColor = false;
            btnDrawLines.Click += BtnDrawLines_Click;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(41, 478);
            lblInfo.Margin = new Padding(4, 0, 4, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(103, 16);
            lblInfo.TabIndex = 12;
            lblInfo.Text = "Message here...";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(577, 519);
            Controls.Add(lblInfo);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            Name = "Main";
            Text = "Main Form";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnLoadLines;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDrawLines;
        private System.Windows.Forms.Label lblInfo;
    }
}