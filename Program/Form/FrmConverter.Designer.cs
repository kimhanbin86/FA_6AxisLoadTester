
namespace Program
{
    partial class FrmConverter
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
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_SelectedFileName = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.lbl_ResultFileName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "파일 선택";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_SelectedFileName
            // 
            this.lbl_SelectedFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SelectedFileName.Location = new System.Drawing.Point(157, 40);
            this.lbl_SelectedFileName.Name = "lbl_SelectedFileName";
            this.lbl_SelectedFileName.Size = new System.Drawing.Size(500, 50);
            this.lbl_SelectedFileName.TabIndex = 1;
            this.lbl_SelectedFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(25, 137);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 50);
            this.button3.TabIndex = 3;
            this.button3.Text = "변환 및 저장";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lbl_ResultFileName
            // 
            this.lbl_ResultFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ResultFileName.Location = new System.Drawing.Point(157, 137);
            this.lbl_ResultFileName.Name = "lbl_ResultFileName";
            this.lbl_ResultFileName.Size = new System.Drawing.Size(500, 50);
            this.lbl_ResultFileName.TabIndex = 4;
            this.lbl_ResultFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmConverter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(684, 211);
            this.Controls.Add(this.lbl_ResultFileName);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.lbl_SelectedFileName);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "FrmConverter";
            this.Text = "Recipe 변환";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConverter_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmConverter_FormClosed);
            this.Load += new System.EventHandler(this.FrmConverter_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_SelectedFileName;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lbl_ResultFileName;
    }
}