using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Reflection;

using LibLog;

namespace Program
{
    public partial class FrmSearchData : Form
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

        public FrmSearchData()
        {
            InitializeComponent();
        }

        private void FrmSearchData_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormSearchData = null;
        }
        private void FrmSearchData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void FrmSearchData_Load(object sender, EventArgs e)
        {
            InitializeDataGridView(dgv_Data);

            AddDataGridViewColumns(dgv_Data);

            // 기본 내림차순으로 SET -> 오름차순으로 SET
            cb_SortMethod.SelectedIndex = 1;
            //cb_SortMethod.SelectedIndex = 0;

            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Parse("00:00:00");
            dateTimePicker3.Value = DateTime.Now.AddDays(0);
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

            string[] columns = Enum.GetNames(typeof(e_DBResult_MC));
            for (int i = 0; i < columns.Length; i++)
            {
                dgv.Columns.Add(columns[i], columns[i]);

                dgv.Columns[columns[i]].ReadOnly = true;
                dgv.Columns[columns[i]].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (i >= (int)e_DBResult_MC.No_1_Temp)
                {
                    dgv.Columns[columns[i]].Visible = false;
                }
            }

            dgv.Columns[e_DBResult_MC.Time.ToString()].Frozen = true;

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
            dgv_Data.Columns[e_DBResult_MC.Position.ToString()].HeaderText += " [mm]";

            dgv_Data.Columns[e_DBResult_MC.PPLoad.ToString()].HeaderText = "++Load [kg]";
            dgv_Data.Columns[e_DBResult_MC. PLoad.ToString()].HeaderText = "+ Load [kg]";
            dgv_Data.Columns[e_DBResult_MC. MLoad.ToString()].HeaderText = "- Load [kg]";
            dgv_Data.Columns[e_DBResult_MC.MMLoad.ToString()].HeaderText = "--Load [kg]";

            dgv_Data.Columns[e_DBResult_MC.No_1_LoadValue.ToString()].HeaderText = "No.1 [kg]";
            dgv_Data.Columns[e_DBResult_MC.No_2_LoadValue.ToString()].HeaderText = "No.2 [kg]";
            dgv_Data.Columns[e_DBResult_MC.No_3_LoadValue.ToString()].HeaderText = "No.3 [kg]";
            dgv_Data.Columns[e_DBResult_MC.No_4_LoadValue.ToString()].HeaderText = "No.4 [kg]";

            dgv_Data.Columns[e_DBResult_MC.No_1_Ux.ToString()].HeaderText = "1 Ux [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_1_Uy.ToString()].HeaderText = "1 Uy [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_1_Lx.ToString()].HeaderText = "1 Lx [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_1_Ly.ToString()].HeaderText = "1 Ly [mm]";

            dgv_Data.Columns[e_DBResult_MC.No_2_Ux.ToString()].HeaderText = "2 Ux [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_2_Uy.ToString()].HeaderText = "2 Uy [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_2_Lx.ToString()].HeaderText = "2 Lx [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_2_Ly.ToString()].HeaderText = "2 Ly [mm]";

            dgv_Data.Columns[e_DBResult_MC.No_3_Ux.ToString()].HeaderText = "3 Ux [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_3_Uy.ToString()].HeaderText = "3 Uy [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_3_Lx.ToString()].HeaderText = "3 Lx [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_3_Ly.ToString()].HeaderText = "3 Ly [mm]";

            dgv_Data.Columns[e_DBResult_MC.No_4_Ux.ToString()].HeaderText = "4 Ux [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_4_Uy.ToString()].HeaderText = "4 Uy [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_4_Lx.ToString()].HeaderText = "4 Lx [mm]";
            dgv_Data.Columns[e_DBResult_MC.No_4_Ly.ToString()].HeaderText = "4 Ly [mm]";

            dgv_Data.Columns[e_DBResult_MC.No_1_Result.ToString()].HeaderText = "1 Result";
            dgv_Data.Columns[e_DBResult_MC.No_2_Result.ToString()].HeaderText = "2 Result";
            dgv_Data.Columns[e_DBResult_MC.No_3_Result.ToString()].HeaderText = "3 Result";
            dgv_Data.Columns[e_DBResult_MC.No_4_Result.ToString()].HeaderText = "4 Result";

            dgv_Data.Columns[e_DBResult_MC.No_1_Temp.ToString()].HeaderText = "1 temp [deg]";
            dgv_Data.Columns[e_DBResult_MC.No_2_Temp.ToString()].HeaderText = "2 temp [deg]";
            dgv_Data.Columns[e_DBResult_MC.No_3_Temp.ToString()].HeaderText = "3 temp [deg]";
            dgv_Data.Columns[e_DBResult_MC.No_4_Temp.ToString()].HeaderText = "4 temp [deg]";
        }

        private void AddDataGridViewRows(DataGridView dgv, DataTable dt)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // 속도 관련

            string[] columns = Enum.GetNames(typeof(e_DBResult_MC));
            string time = string.Empty;

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                dgv.Rows.Add();

                dgv.Rows[row].Cells["No"].Value = (row + 1).ToString();

                foreach (string col in columns)
                {
                    if (col == e_DBResult_MC.Time.ToString())
                    {
                        time = dt.Rows[row][col].ToString();
                        time = time.Insert(13 - 1, ":");
                        time = time.Insert(11 - 1, ":");
                        time = time.Insert( 9 - 1, " /  ");
                        time = time.Insert( 7 - 1, "-");
                        time = time.Insert( 5 - 1, "-");

                        dgv.Rows[row].Cells[col].Value = time;
                    }
                    else
                    {
                        dgv.Rows[row].Cells[col].Value = dt.Rows[row][col].ToString();
                    }
                }
            }

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btn_LastData_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;
                if (int.TryParse(txt_LastData.Text, out cnt) == false)
                {
                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "Count 확인 후 재시도 해주세요", MessageBoxButtons.OK);
                    return;
                }

                string cmdText = string.Format("SELECT * FROM {0} ORDER BY {1} DESC LIMIT {2}", e_DBTableList.Result_MC,
                                                                                                e_DBResult_MC.Time,
                                                                                                cnt
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

                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "검색 결과가 없습니다", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void btn_SearchData_Click(object sender, EventArgs e)
        {
            try
            {
                string time1 = dateTimePicker1.Value.ToString("yyyyMMdd") + dateTimePicker2.Value.ToString("HHmmss");
                string time2 = dateTimePicker3.Value.ToString("yyyyMMdd") + dateTimePicker4.Value.ToString("HHmmss");

                string cmdText = string.Format("SELECT * FROM {0} WHERE {1}>='{2}' AND {3}<='{4}' ", e_DBTableList.Result_MC,
                                                                                                     e_DBResult_MC.Time, time1,
                                                                                                     e_DBResult_MC.Time, time2
                                              );

                if (txt_LOTNO.Text.Trim() == string.Empty)
                {
                    txt_LOTNO.Text = "LOTNO ALL";
                }

                if (txt_LOTNO.Text != "LOTNO ALL")
                {
                    cmdText += string.Format("AND {0}='{1}' ", e_DBResult_MC.Number, txt_LOTNO.Text);
                }

                cmdText += string.Format("ORDER BY {0} {1}", e_DBResult_MC.Time, (cb_SortMethod.SelectedIndex == 0 ? "DESC" : "ASC"));

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
        private void btn_ExportCSV_Click(object sender, EventArgs e)
        {
            if (dgv_Data.Rows.Count == 0)
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "검색 후 재시도 해주세요", MessageBoxButtons.OK);
                return;
            }

            try
            {
                // CSV 폴더가 없으면 만들고
                if (Directory.Exists(GlobalValue.Directory.CSV) == false)
                    Directory.CreateDirectory(GlobalValue.Directory.CSV);

                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.InitialDirectory = GlobalValue.Directory.CSV;
                    dlg.DefaultExt = "txt";
                    dlg.Filter = "TEXT|*.txt|ALL|*.*";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // 동일 이름의 파일이 있으면 지우고
                        if (File.Exists(dlg.FileName))
                        {
                            File.SetAttributes(dlg.FileName, FileAttributes.Normal);
                            File.Delete(dlg.FileName);
                        }

                        using (StreamWriter streamWriter = new StreamWriter(new FileStream(dlg.FileName, FileMode.Append)))
                        {
                            string value = string.Empty;

                            #region 헤더 작성
                            for (int col = 0; col < dgv_Data.Columns.Count; col++)
                            {
                                if (col < (int)e_DBResult_MC.No_1_Temp + 1)
                                {
                                    value += string.Format("_{0}", dgv_Data.Columns[col].HeaderText);
                                }
                                if (col < dgv_Data.Columns.Count - 1)
                                {
                                    value += "\t";
                                }
                            }
                            streamWriter.WriteLine(value);
                            #endregion

                            #region 데이터 작성
                            for (int row = 0; row < dgv_Data.Rows.Count; row++)
                            {
                                value = string.Empty;

                                for (int col = 0; col < dgv_Data.Columns.Count; col++)
                                {
                                    if (col < (int)e_DBResult_MC.No_1_Temp + 1)
                                    {
                                        value += string.Format("{0}", dgv_Data.Rows[row].Cells[col].Value);
                                    }
                                    if (col < dgv_Data.Columns.Count - 1)
                                    {
                                        value += "\t";
                                    }
                                }
                                streamWriter.WriteLine(value);
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
