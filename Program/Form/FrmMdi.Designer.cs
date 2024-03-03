
namespace Program
{
    partial class FrmMdi
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMdi));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.종료ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.화면ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.제품데이터ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.데이터검색ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alarmHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.축값모니터링ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_ClearLog = new System.Windows.Forms.Button();
            this.listViewLog1 = new LibLog.ListViewLog();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.recipe변환ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.화면ToolStripMenuItem,
            this.toolToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.종료ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.파일ToolStripMenuItem.Text = "파일 (&F)";
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.종료ToolStripMenuItem.ShowShortcutKeys = false;
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.종료ToolStripMenuItem.Text = "종료";
            this.종료ToolStripMenuItem.Click += new System.EventHandler(this.종료ToolStripMenuItem_Click);
            // 
            // 화면ToolStripMenuItem
            // 
            this.화면ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.제품데이터ToolStripMenuItem,
            this.데이터검색ToolStripMenuItem,
            this.alarmHistoryToolStripMenuItem,
            this.축값모니터링ToolStripMenuItem,
            this.recipe변환ToolStripMenuItem});
            this.화면ToolStripMenuItem.Name = "화면ToolStripMenuItem";
            this.화면ToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.화면ToolStripMenuItem.Text = "화면 (&W)";
            // 
            // 제품데이터ToolStripMenuItem
            // 
            this.제품데이터ToolStripMenuItem.Name = "제품데이터ToolStripMenuItem";
            this.제품데이터ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.제품데이터ToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.제품데이터ToolStripMenuItem.Text = "제품 데이터";
            this.제품데이터ToolStripMenuItem.Click += new System.EventHandler(this.제품데이터ToolStripMenuItem_Click);
            // 
            // 데이터검색ToolStripMenuItem
            // 
            this.데이터검색ToolStripMenuItem.Name = "데이터검색ToolStripMenuItem";
            this.데이터검색ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.데이터검색ToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.데이터검색ToolStripMenuItem.Text = "데이터 검색";
            this.데이터검색ToolStripMenuItem.Click += new System.EventHandler(this.데이터검색ToolStripMenuItem_Click);
            // 
            // alarmHistoryToolStripMenuItem
            // 
            this.alarmHistoryToolStripMenuItem.Name = "alarmHistoryToolStripMenuItem";
            this.alarmHistoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.alarmHistoryToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.alarmHistoryToolStripMenuItem.Text = "Alarm History";
            this.alarmHistoryToolStripMenuItem.Click += new System.EventHandler(this.alarmHistoryToolStripMenuItem_Click);
            // 
            // 축값모니터링ToolStripMenuItem
            // 
            this.축값모니터링ToolStripMenuItem.Name = "축값모니터링ToolStripMenuItem";
            this.축값모니터링ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.축값모니터링ToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.축값모니터링ToolStripMenuItem.Text = "6축 값 모니터링";
            this.축값모니터링ToolStripMenuItem.Click += new System.EventHandler(this.축값모니터링ToolStripMenuItem_Click);
            // 
            // toolToolStripMenuItem
            // 
            this.toolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem,
            this.parameterToolStripMenuItem});
            this.toolToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 9F);
            this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            this.toolToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.toolToolStripMenuItem.Text = "Tool";
            this.toolToolStripMenuItem.Visible = false;
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.logToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.logToolStripMenuItem.Text = "Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // parameterToolStripMenuItem
            // 
            this.parameterToolStripMenuItem.Name = "parameterToolStripMenuItem";
            this.parameterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.parameterToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.parameterToolStripMenuItem.Text = "Parameter";
            this.parameterToolStripMenuItem.Click += new System.EventHandler(this.parameterToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 694);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 35);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(100, 33);
            this.toolStripStatusLabel1.Text = "PLC";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 33);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AutoSize = false;
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(100, 33);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_ClearLog);
            this.panel1.Controls.Add(this.listViewLog1);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 494);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 200);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            // 
            // btn_ClearLog
            // 
            this.btn_ClearLog.Location = new System.Drawing.Point(3, 172);
            this.btn_ClearLog.Name = "btn_ClearLog";
            this.btn_ClearLog.Size = new System.Drawing.Size(50, 25);
            this.btn_ClearLog.TabIndex = 2;
            this.btn_ClearLog.Text = "Clear";
            this.btn_ClearLog.UseVisualStyleBackColor = true;
            this.btn_ClearLog.Visible = false;
            this.btn_ClearLog.Click += new System.EventHandler(this.btn_ClearLog_Click);
            // 
            // listViewLog1
            // 
            this.listViewLog1.BackColor = System.Drawing.Color.Black;
            this.listViewLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLog1.Font = new System.Drawing.Font("나눔고딕코딩", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listViewLog1.ForeColor = System.Drawing.Color.Yellow;
            this.listViewLog1.FullRowSelect = true;
            this.listViewLog1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewLog1.HideSelection = false;
            this.listViewLog1.Location = new System.Drawing.Point(0, 5);
            this.listViewLog1.MultiSelect = false;
            this.listViewLog1.Name = "listViewLog1";
            this.listViewLog1.Size = new System.Drawing.Size(1008, 195);
            this.listViewLog1.TabIndex = 1;
            this.listViewLog1.TabStop = false;
            this.listViewLog1.UseCompatibleStateImageBehavior = false;
            this.listViewLog1.View = System.Windows.Forms.View.Details;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Highlight;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1008, 5);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            this.splitter1.DoubleClick += new System.EventHandler(this.splitter1_DoubleClick);
            this.splitter1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitter1_MouseUp);
            // 
            // recipe변환ToolStripMenuItem
            // 
            this.recipe변환ToolStripMenuItem.Name = "recipe변환ToolStripMenuItem";
            this.recipe변환ToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.recipe변환ToolStripMenuItem.Text = "Recipe 변환";
            this.recipe변환ToolStripMenuItem.Click += new System.EventHandler(this.recipe변환ToolStripMenuItem_Click);
            // 
            // FrmMdi
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMdi";
            this.Text = "FrmMdi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMdi_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMdi_FormClosed);
            this.Load += new System.EventHandler(this.FrmMdi_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private LibLog.ListViewLog listViewLog1;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 종료ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 화면ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 제품데이터ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 데이터검색ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button btn_ClearLog;
        private System.Windows.Forms.ToolStripMenuItem alarmHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 축값모니터링ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recipe변환ToolStripMenuItem;
    }
}

