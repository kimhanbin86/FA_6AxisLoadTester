using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LibLog;

namespace Program
{
    public partial class FrmHidden : Form
    {
        private string PP하중 = string.Empty; // ++하중
        private string MM하중 = string.Empty; // --하중

        private bool before적용 = false;
        private string beforeRange = string.Empty;

        public FrmHidden()
        {
            InitializeComponent();
        }
        public FrmHidden(string PP하중, string MM하중)
        {
            InitializeComponent();

            this.PP하중 = PP하중;
            this.MM하중 = MM하중;
        }

        private void FrmHidden_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormHidden = null;
        }
        private void FrmHidden_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timerUpdate != null)
            {
                if (timerUpdate.Enabled)
                    timerUpdate.Stop();
                timerUpdate.Dispose();
                timerUpdate = null;
            }
        }
        private void FrmHidden_Load(object sender, EventArgs e)
        {
            before적용 = (GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.적용] == "1" ? true : false);
            beforeRange = GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.Range];

            Initialize();

            timerUpdate = new Timer();
            timerUpdate.Tick += new EventHandler(TickUpdate);
            timerUpdate.Interval = 100;
            timerUpdate.Start();
        }

        private void Initialize()
        {
            chk_적용.Checked = before적용;
            txt_Range.Text = beforeRange;

            //txt_PP하중.Text = PP하중;
            //txt_MM하중.Text = MM하중;

            // 2022-02-10 : 다운로드된 ++, -- 하중 값 사용
            txt_PP하중.Text = GlobalValue.Test.PP하중.ToString();
            txt_MM하중.Text = GlobalValue.Test.MM하중.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool current적용 = chk_적용.Checked;
            string currentRange = txt_Range.Text.Trim();

            double result;
            if (double.TryParse(currentRange, out result))
            {
                if (before적용 != current적용 ||
                    beforeRange != currentRange
                   )
                {
                    GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.적용] = (current적용 ? "1" : "0");
                    Log.Write(Text, string.Format("적용=[{0} -> {1}]", before적용, current적용));

                    GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.Range] = currentRange;
                    Log.Write(Text, string.Format("Range=[{0} -> {1}]", beforeRange, currentRange));

                    if (GlobalFunction.SaveParameter(e_Parameter.System) == false)
                    {
                        GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER] = true;
                    }
                }
            }
            else
            {
                GlobalFunction.MessageBox(Text, "숫자만 입력해 주세요", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Close();
        }

        #region 2022-02-10 : 다운로드된 ++, -- 하중 값 사용

        private Timer timerUpdate = null;
        private void TickUpdate(object sender, EventArgs e)
        {
            timerUpdate?.Stop();
            try
            {
                txt_PP하중.Text = GlobalValue.Test.PP하중.ToString();
                txt_MM하중.Text = GlobalValue.Test.MM하중.ToString();
            }
            catch { }
            finally
            {
                timerUpdate?.Start();
            }
        }

        #endregion
    }
}
