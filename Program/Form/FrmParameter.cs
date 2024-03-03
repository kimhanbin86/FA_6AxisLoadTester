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
    public partial class FrmParameter : Form
    {
        public FrmParameter()
        {
            InitializeComponent();
        }

        private void FrmParameter_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormParameter = null;
        }
        private void FrmParameter_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearTabControl(tabControl);
        }
        private void FrmParameter_Load(object sender, EventArgs e)
        {
            tabControl = tabControl1;

            InitializeTabControl(tabControl);

            TabControlSelectedIndexChanged(null, null);
        }

        private TabPage[] tabPages = null;
        private DataGridView[] dataGridViews = null;
        private TabControl tabControl = null;

        private void ClearTabControl(TabControl tab)
        {
            if (dataGridViews != null)
            {
                for (int i = 0; i < dataGridViews.Length; i++)
                {
                    if (dataGridViews[i] != null)
                    {
                        dataGridViews[i].RowPostPaint -= new DataGridViewRowPostPaintEventHandler(DataGridViewRowPostPaint);

                        dataGridViews[i].Dispose();
                        dataGridViews[i] = null;
                    }
                }

                dataGridViews = null;
            }

            tab.SelectedIndexChanged -= new EventHandler(TabControlSelectedIndexChanged); // TabPage.Dispose() 시에 SelectedIndexChanged 이벤트가 발생되기 때문에 오류 방지를 위해 미리 이벤트 해제

            if (tabPages != null)
            {
                for (int i = 0; i < tabPages.Length; i++)
                {
                    if (tabPages[i] != null)
                    {
                        //tabPages[i].Controls.Clear(); // DataGridView.Dispose() 시에 자동으로 제거되기 때문에 필요 없음

                        tabPages[i].Dispose();
                        tabPages[i] = null;
                    }
                }

                tabPages = null;
            }

            tab.TabPages.Clear();
        }
        private void InitializeTabControl(TabControl tab)
        {
            string[] ArrParameter = Enum.GetNames(typeof(e_Parameter));
            int cntParameter = ArrParameter.Length;

            #region TabPage 생성 및 속성 설정 후 TabControl에 등록
            ClearTabControl(tab);

            tabPages = new TabPage[cntParameter];
            for (int i = 0; i < cntParameter; i++)
            {
                tabPages[i] = new TabPage();

                tabPages[i].Name = tabPages[i].Text = ArrParameter[i];
            }

            tab.TabPages.AddRange(tabPages);
            #endregion

            #region DataGridView 생성 및 속성 설정, 이벤트 연결
            dataGridViews = new DataGridView[cntParameter];
            for (int i = 0; i < cntParameter; i++)
            {
                dataGridViews[i] = new DataGridView();

                dataGridViews[i].Name = ArrParameter[i];
                dataGridViews[i].Dock = DockStyle.Fill;

                SetDataGridViewProperties(dataGridViews[i]);

                dataGridViews[i].RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridViewRowPostPaint);
            }
            #endregion

            #region TabPage 별로 DataGridView 등록
            for (int i = 0; i < cntParameter; i++)
            {
                tabPages[i].Controls.Add(dataGridViews[i]);
            }
            #endregion

            tab.ItemSize = new Size(0, 35); // Width 값은 자동으로 설정됨

            tab.SelectedIndexChanged += new EventHandler(TabControlSelectedIndexChanged);
        }

        private void SetDataGridViewProperties(DataGridView dgv)
        {
            GlobalFunction.DoubleBuffered(dgv, true);

            dgv.AllowUserToResizeRows = false;

            DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
            defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultCellStyle.Font = new Font(e_Font.맑은_고딕.ToString(), 11);
            dgv.DefaultCellStyle = defaultCellStyle;

            dgv.RowHeadersWidth = 56;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            DataGridViewRow rowTemplate = new DataGridViewRow();
            rowTemplate.Height = 28;
            dgv.RowTemplate = rowTemplate;
        }
        private void DataGridViewRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            Rectangle rectangle = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgv.RowHeadersWidth, e.RowBounds.Height);

            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center, // 가로
                LineAlignment = StringAlignment.Center  // 세로
            };

            e.Graphics.DrawString((e.RowIndex + 1).ToString(), new Font(e_Font.Tahoma.ToString(), 9), SystemBrushes.ControlText, rectangle, stringFormat);
        }
        private void AddDataGridViewColumns(DataGridView dgv, string[] columns)
        {
            dgv.Columns.Clear();

            for (int i = 1; i < columns.Length; i++)
            {
                dgv.Columns.Add(columns[i], columns[i]);

                dgv.Columns[i - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            DataGridViewCellStyle columnHeadersDefaultCellStyle = new DataGridViewCellStyle();
            columnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            columnHeadersDefaultCellStyle.Font = new Font(e_Font.Tahoma.ToString(), 11);
            dgv.ColumnHeadersDefaultCellStyle = columnHeadersDefaultCellStyle;

            dgv.ColumnHeadersHeight = 28;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.EnableHeadersVisualStyles = false;

            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private e_Parameter GetParameter()
        {
            return (e_Parameter)tabControl.SelectedIndex;
        }
        private void TabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_Description.Text = string.Empty;

            e_Parameter parameter = GetParameter();
            DataGridView dgv = dataGridViews[(int)parameter];

            AddDataGridViewColumns(dgv, GlobalFunction.GetDataFormat(parameter));

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            for (int row = 0; row < GlobalValue.Parameter[(int)parameter].Count; row++)
            {
                dgv.Rows.Add();

                for (int col = 1; col < dgv.Columns.Count + 1; col++)
                {
                    if (col < GlobalValue.Parameter[(int)parameter][(row + 1).ToString()].Length)
                    {
                        dgv.Rows[row].Cells[col - 1].Value = GlobalValue.Parameter[(int)parameter][(row + 1).ToString()][col];
                    }
                }
            }

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            switch (parameter)
            {
                case e_Parameter.System:
                    break;
                case e_Parameter.DB:
                    break;
                case e_Parameter.LoadCell:
                    break;
                case e_Parameter.Monitor:
                    break;
                case e_Parameter.ETC:
                    break;
                case e_Parameter.PLC_InterfaceB_IN:
                case e_Parameter.PLC_InterfaceB_OUT:
                case e_Parameter.PLC_InterfaceF_Alarm:
                case e_Parameter.PLC_InterfaceR_IN:
                case e_Parameter.PLC_InterfaceR_OUT:
                    break;
                case e_Parameter.Recipe:
                    break;
            }
        }
        private void ButtonClick(object sender, EventArgs e)
        {
            e_Parameter parameter = GetParameter();
            DataGridView dgv = dataGridViews[(int)parameter];

            Button btn = sender as Button;

            //btn_Clear
            //btn_Save
            //btn_Close
            switch (btn.Name.Substring(4))
            {
                case "Clear":
                    AddDataGridViewColumns(dgv, GlobalFunction.GetDataFormat(parameter));
                    break;


                case "Save":
                    #region 백업
                    Dictionary<string, string[]> backup = new Dictionary<string, string[]>();
                    foreach (string key in GlobalValue.Parameter[(int)parameter].Keys)
                    {
                        backup.Add(key, GlobalValue.Parameter[(int)parameter][key]);
                    }
                    #endregion

                    // Parameter 변수 초기화
                    GlobalValue.Parameter[(int)parameter].Clear();

                    #region DataGridView 내용을 Parameter 변수로 복사
                    for (int row = 0; row < dgv.Rows.Count - 1; row++)
                    {
                        string[] vs = new string[dgv.Columns.Count + 1];

                        vs[0] = (row + 1).ToString();
                        for (int col = 1; col <= dgv.Columns.Count; col++)
                        {
                            vs[col] = GlobalFunction.GetString(dgv.Rows[row].Cells[col - 1].Value);
                        }

                        GlobalValue.Parameter[(int)parameter].Add(vs[0], vs);
                    }
                    #endregion

                    if (GlobalFunction.SaveParameter(parameter))
                    {
                        GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "Save OK", MessageBoxButtons.OK);
                    }
                    else
                    {
                        GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "Save NG", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        GlobalValue.Parameter[(int)parameter].Clear();
                        foreach (string key in backup.Keys)
                        {
                            GlobalValue.Parameter[(int)parameter].Add(key, backup[key]);
                        }
                    }
                    break;


                case "Close":
                    Close();
                    break;
            }
        }
    }
}
