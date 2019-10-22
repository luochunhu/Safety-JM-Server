namespace Sys.Safety.Client.WindowHost
{
    partial class frmRpcTest
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
            this.btnRpcTest = new System.Windows.Forms.Button();
            this.btnWebApiTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRpcTest
            // 
            this.btnRpcTest.Location = new System.Drawing.Point(246, 119);
            this.btnRpcTest.Name = "btnRpcTest";
            this.btnRpcTest.Size = new System.Drawing.Size(126, 56);
            this.btnRpcTest.TabIndex = 0;
            this.btnRpcTest.Text = "Rpc测试";
            this.btnRpcTest.UseVisualStyleBackColor = true;
            this.btnRpcTest.Click += new System.EventHandler(this.btnRpcTest_Click);
            // 
            // btnWebApiTest
            // 
            this.btnWebApiTest.Location = new System.Drawing.Point(468, 119);
            this.btnWebApiTest.Name = "btnWebApiTest";
            this.btnWebApiTest.Size = new System.Drawing.Size(126, 56);
            this.btnWebApiTest.TabIndex = 1;
            this.btnWebApiTest.Text = "WebApi测试";
            this.btnWebApiTest.UseVisualStyleBackColor = true;
            this.btnWebApiTest.Click += new System.EventHandler(this.btnWebApiTest_Click);
            // 
            // frmRpcTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 425);
            this.Controls.Add(this.btnWebApiTest);
            this.Controls.Add(this.btnRpcTest);
            this.Name = "frmRpcTest";
            this.Text = "frmRpcTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRpcTest;
        private System.Windows.Forms.Button btnWebApiTest;
    }
}