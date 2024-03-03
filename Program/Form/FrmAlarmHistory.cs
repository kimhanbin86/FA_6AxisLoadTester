using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;

using LibLog;

namespace Program
{
    public partial class FrmAlarmHistory : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public FrmAlarmHistory()
        {
            InitializeComponent();
        }

        private void FrmAlarmHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormAlarmHistory = null;
        }
        private void FrmAlarmHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void FrmAlarmHistory_Load(object sender, EventArgs e)
        {
            InitializeDataGridView(dgv_Data);

            AddDataGridViewColumns(dgv_Data);
        }

        private void InitializeDataGridView(DataGridView dgv)
        {
            GlobalFunction.DoubleBuffered(dgv, true);

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // 홀수 번호 행에 적용되는 기본 셀 스타일을 설정

            DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
            defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultCellStyle.Font = new Font(e_Font.Tahoma.ToString(), 11);
            dgv.DefaultCellStyle = defaultCellStyle;

            dgv.RowHeadersVisible = false;

            dgv.RowHeadersWidth = 56;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            DataGridViewRow rowTemplate = new DataGridViewRow();
            rowTemplate.Height = 28;
            dgv.RowTemplate = rowTemplate;
        }

        private void AddDataGridViewColumns(DataGridView dgv)
        {
            dgv.Columns.Clear();

            //////////////////////////////////////////////////

            #region No
            dgv.Columns.Add("No", "No");

            dgv.Columns["No"].ReadOnly = true;
            dgv.Columns["No"].SortMode = DataGridViewColumnSortMode.NotSortable;
            #endregion

            string[] columns = Enum.GetNames(typeof(e_DBAlarm_MC));
            for (int i = 0; i < columns.Length; i++)
            {
                dgv.Columns.Add(columns[i], columns[i]);

                dgv.Columns[columns[i]].ReadOnly = true;
                dgv.Columns[columns[i]].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //////////////////////////////////////////////////

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            DataGridViewCellStyle columnHeadersDefaultCellStyle = new DataGridViewCellStyle();
            columnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            columnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            columnHeadersDefaultCellStyle.ForeColor = Color.White;
            columnHeadersDefaultCellStyle.Font = new Font(e_Font.Tahoma.ToString(), 11);
            dgv.ColumnHeadersDefaultCellStyle = columnHeadersDefaultCellStyle;

            dgv.ColumnHeadersHeight = 28;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.EnableHeadersVisualStyles = false;

            //////////////////////////////////////////////////

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            HeaderText();
        }
        private void HeaderText()
        {
            //dgv_Data.Columns[e_DBAlarm_MC.StartTime.ToString()].HeaderText = "발생 시간";
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_SearchData_Click(object sender, EventArgs e)
        {
            try
            {
                string time1 = dateTimePicker1.Value.ToString("yyyyMMdd") + dateTimePicker2.Value.ToString("HHmmss");
                string time2 = dateTimePicker3.Value.ToString("yyyyMMdd") + dateTimePicker4.Value.ToString("HHmmss");

                string cmdText = string.Format("SELECT * FROM {0} WHERE {1}>='{2}' AND {3}<='{4}' ORDER BY {5} DESC", e_DBTableList.Alarm_MC,
                                                                                                                      e_DBAlarm_MC.StartTime, time1,
                                                                                                                      e_DBAlarm_MC.StartTime, time2,
                                                                                                                      e_DBAlarm_MC.StartTime
                                              );

                DataTable dt = GlobalFunction.DB.MySQL.GetDataTable(cmdText);

                if (dt.Rows.Count > 0)
                {
                    AddDataGridViewColumns(dgv_Data);
                    AddDataGridViewRows(dgv_Data, dt);
                }
                else
                {
                    AddDataGridViewColumns(dgv_Data);

                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "검색 결과가 없습니다.\r\n\r\n날짜를 확인하신 후 재시도 해주세요.", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void AddDataGridViewRows(DataGridView dgv, DataTable dt)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // 속도 관련

            string[] columns = Enum.GetNames(typeof(e_DBAlarm_MC));

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                dgv.Rows.Add();

                dgv.Rows[row].Cells["No"].Value = (row + 1).ToString();

                foreach (string col in columns)
                {
                    dgv.Rows[row].Cells[col].Value = dt.Rows[row][col].ToString();
                }
            }

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
