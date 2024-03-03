using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class FrmAlarm : Form
    {
        public string AlarmCode = string.Empty;
        public string AlarmName = string.Empty;

        #region Timer
        private Timer timerUpdate = null;
        private void TickUpdate(object sender, EventArgs e)
        {
            timerUpdate?.Stop();
            try
            {
                if (lbl_AlarmCode.Text != AlarmCode) lbl_AlarmCode.Text = AlarmCode;
                if (lbl_AlarmName.Text != AlarmName) lbl_AlarmName.Text = AlarmName;
            }
            finally
            {
                timerUpdate?.Start();
            }
        }

        private Timer timerBringToFront = null;
        private void TickBringToFront(object sender, EventArgs e)
        {
            timerBringToFront?.Stop();
            try
            {
                BringToFront();
            }
            finally
            {
                timerBringToFront?.Start();
            }
        }

        private void StartTimer()
        {
            timerUpdate = new Timer();
            timerUpdate.Tick += new EventHandler(TickUpdate);
            timerUpdate.Interval = 100;
            timerUpdate.Start();

            timerBringToFront = new Timer();
            timerBringToFront.Tick += new EventHandler(TickBringToFront);
            timerBringToFront.Interval = 1000;
            timerBringToFront.Start();
        }
        private void StopTimer()
        {
            if (timerUpdate != null)
            {
                if (timerUpdate.Enabled)
                    timerUpdate.Stop();
                timerUpdate.Dispose();
                timerUpdate = null;
            }

            if (timerBringToFront != null)
            {
                if (timerBringToFront.Enabled)
                    timerBringToFront.Stop();
                timerBringToFront.Dispose();
                timerBringToFront = null;
            }
        }
        #endregion

        public FrmAlarm()
        {
            InitializeComponent();
        }
        public FrmAlarm(string alarmCode, string alarmName)
        {
            InitializeComponent();

            AlarmCode = alarmCode;
            AlarmName = alarmName;
        }

        private void FrmAlarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormAlarm = null;
        }
        private void FrmAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTimer();

            Array.Clear(GlobalValue.PCAlarm, 0, GlobalValue.PCAlarm.Length);
        }
        private void FrmAlarm_Load(object sender, EventArgs e)
        {
            lbl_AlarmCode.Text = AlarmCode;
            lbl_AlarmName.Text = AlarmName;

            StartTimer();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
