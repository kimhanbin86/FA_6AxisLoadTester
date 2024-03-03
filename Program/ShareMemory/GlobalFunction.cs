using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using LibLog;
using MySql.Data.MySqlClient;

namespace Program
{
    public static class GlobalFunction
    {
        public static bool CheckProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            return (processes.Length > 1 ? false : true);
        }

        public static string ConvertFormat(double value, string format)
        {
            string result;
            switch (format)
            {
                case "0.0": result = string.Format("{0:0.0}", value); break;
                case "0.00": result = string.Format("{0:0.00}", value); break;
                case "0.000": result = string.Format("{0:0.000}", value); break;
                case "0.0000": result = string.Format("{0:0.0000}", value); break;
                case "0.00000": result = string.Format("{0:0.00000}", value); break;
                default: result = string.Format("{0}", value); break;
            }
            return result;
        }
        public static string ConvertFormat(int value, string format)
        {
            string result;
            switch (format)
            {
                case "0.0": result = string.Format("{0:0.0}", (double)value / 10); break;
                case "0.00": result = string.Format("{0:0.00}", (double)value / 100); break;
                case "0.000": result = string.Format("{0:0.000}", (double)value / 1000); break;
                case "0.0000": result = string.Format("{0:0.0000}", (double)value / 10000); break;
                case "0.00000": result = string.Format("{0:0.00000}", (double)value / 100000); break;
                default: result = string.Format("{0}", value); break;
            }
            return result;
        }

        public static void DoubleBuffered(Control control, bool value)
        {
            Type type = control.GetType();
            PropertyInfo propertyInfo = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyInfo.SetValue(control, value);
        }

        public static string[] GetDataFormat(e_Parameter parameter)
        {
            string[] result = null;
            switch (parameter)
            {
                case e_Parameter.System:                result = GetNames(new e_DF_System());                break;
                case e_Parameter.DB:                    result = GetNames(new e_DF_DB());                    break;
                case e_Parameter.LoadCell:              result = GetNames(new e_DF_LoadCell());              break;
                case e_Parameter.Monitor:               result = GetNames(new e_DF_Monitor());               break;
                case e_Parameter.ETC:                   result = GetNames(new e_DF_ETC());                   break;
                case e_Parameter.PLC_InterfaceB_IN:     result = GetNames(new e_DF_PLC_InterfaceB_IN());     break;
                case e_Parameter.PLC_InterfaceB_OUT:    result = GetNames(new e_DF_PLC_InterfaceB_OUT());    break;
                case e_Parameter.PLC_InterfaceF_Alarm:  result = GetNames(new e_DF_PLC_InterfaceF_Alarm());  break;
                case e_Parameter.PLC_InterfaceR_IN:     result = GetNames(new e_DF_PLC_InterfaceR_IN());     break;
                case e_Parameter.PLC_InterfaceR_OUT:    result = GetNames(new e_DF_PLC_InterfaceR_OUT());    break;
                case e_Parameter.Recipe:                result = GetNames(new e_DF_Recipe());                break;
            }
            return result;
        }
        private static string[] GetNames(Enum @enum)
        {
            return Enum.GetNames(@enum.GetType());
        }

        public static string GetString(Exception ex)
        {
            return string.Format("try catch error(message=[{0}])", ex);
        }
        public static string GetString(object value)
        {
            return string.Format("{0}", value);
        }

        public static bool LoadParameter()
        {
            bool result = true;
            for (int i = 0; i < Enum.GetNames(typeof(e_Parameter)).Length; i++)
            {
                if ((e_Parameter)i == e_Parameter.Recipe)
                {
                    //string recipeFileName = string.Format("{0}\\{1}.dat", GlobalValue.Directory.Recipe, GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName]);
                    string recipeFileName = GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName];
                    if (!LoadParameter((e_Parameter)i, recipeFileName))
                    {
                        //result = false;
                        GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_LOADING_PARAMETER_RECIPE] = true;
                        break;
                    }
                }
                else
                {
                    if (!LoadParameter((e_Parameter)i))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        public static bool LoadParameter(e_Parameter parameter, string recipeFileName = "")
        {
            bool result = false;
            try
            {
                string fileName = (parameter == e_Parameter.Recipe ? recipeFileName : string.Format("{0}\\{1}.dat", GlobalValue.Directory.Parameter, parameter));
                if (File.Exists(fileName))
                {
                    if (GlobalValue.Parameter[(int)parameter] == null)
                    {
                        GlobalValue.Parameter[(int)parameter] = new Dictionary<string, string[]>();
                    }

                    GlobalValue.Parameter[(int)parameter].Clear();

                    using (StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8))
                    {
                        string line;
                        string[] vs;
                        int num = 0;

                        while (!streamReader.EndOfStream)
                        {
                            // 한 줄 읽고
                            line = streamReader.ReadLine();
                            // step.1
                            line = line.Replace("\t", "");
                            // step.2
                            vs = line.Split(',');
                            // step.3
                            for (int i = 0; i < vs.Length; i++)
                            {
                                vs[i] = vs[i].Trim();
                            }

                            if (int.TryParse(vs[0], out num))
                            {
                                GlobalValue.Parameter[(int)parameter].Add(vs[0], vs);
                            }
                        }
                    }

                    result = (GlobalValue.Parameter[(int)parameter].Count > 0 ? true : false);
                }
                else
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("{0} 파일을 찾을 수 없습니다", fileName));
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            string log = string.Format("{0} Parameter loading {1}", parameter, (result ? "OK" : "NG"));
            if (parameter == e_Parameter.Recipe)
            {
                try   { log += string.Format(" (..\\{0})", recipeFileName.Substring(recipeFileName.IndexOf("EXE"))); }
                catch { log += string.Format(" (..\\{0})", recipeFileName); }
            }
            Log.Write(MethodBase.GetCurrentMethod().Name, log);
            return result;
        }

        public static bool UpdateParameterRecipe(System.Windows.Forms.DataGridView dgv)
        {
            bool result = false;
            #region 백업
            Dictionary<string, string[]> backup = new Dictionary<string, string[]>();
            foreach (string key in GlobalValue.Parameter[(int)e_Parameter.Recipe].Keys)
            {
                backup.Add(key, GlobalValue.Parameter[(int)e_Parameter.Recipe][key]);
            }
            #endregion
            try
            {
                GlobalValue.Parameter[(int)e_Parameter.Recipe].Clear();

                for (int row = 0; row < dgv.Rows.Count; row++)
                {
                    string[] vs = new string[dgv.Columns.Count];

                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        vs[col] = GetString(dgv.Rows[row].Cells[col].Value);
                    }

                    GlobalValue.Parameter[(int)e_Parameter.Recipe].Add(vs[0], vs);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            if (result == false)
            {
                #region 복구
                GlobalValue.Parameter[(int)e_Parameter.Recipe].Clear();
                foreach (string key in backup.Keys)
                {
                    GlobalValue.Parameter[(int)e_Parameter.Recipe].Add(key, backup[key]);
                }
                #endregion
            }
            return result;
        }

        public static bool SaveParameter(e_Parameter parameter, string recipeFileName = "", bool isBackup = true)
        {
            bool result = false;
            string logSrc = string.Empty;
            try
            {
                //string path = (parameter == e_Parameter.Recipe ? GlobalValue.Directory.Recipe : GlobalValue.Directory.Parameter);
                //string name = (parameter == e_Parameter.Recipe ? GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName] : parameter.ToString());
                //string src = string.Format("{0}\\{1}.dat", path, name);
                string src = (parameter == e_Parameter.Recipe ? GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName] : string.Format("{0}\\{1}.dat", GlobalValue.Directory.Parameter, parameter));

                if (BackupParameter(parameter, src, isBackup))
                {
                    if (parameter == e_Parameter.Recipe)
                    {
                        if (recipeFileName != "")
                        {
                            src = recipeFileName;
                        }
                    }

                    if (File.Exists(src))
                    {
                        File.Delete(src);
                    }

                    logSrc = src;

                    using (StreamWriter streamWriter = new StreamWriter(new FileStream(src, FileMode.Append), Encoding.UTF8))
                    {
                        streamWriter.WriteLine("==================================================");
                        streamWriter.WriteLine(string.Format("{0} ({1})", parameter, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        streamWriter.WriteLine("==================================================");

                        string[] vs = GetDataFormat(parameter);

                        string line = string.Empty;
                        for (int i = 0; i < vs.Length; i++)
                        {
                            line += vs[i];
                            if (i < vs.Length - 1)
                            {
                                line += ",\t";
                            }
                        }
                        streamWriter.WriteLine(line);

                        line = string.Empty;
                        for (int i = 1; i <= GlobalValue.Parameter[(int)parameter].Count; i++)
                        {
                            for (int j = 0; j < vs.Length; j++)
                            {
                                line += GlobalValue.Parameter[(int)parameter][i.ToString()][j];
                                if (j < vs.Length - 1)
                                {
                                    line += ",\t";
                                }
                            }
                            line += Environment.NewLine;
                        }
                        streamWriter.Write(line);
                    }

                    result = true;
                }
                //else
                //{
                //    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Parameter Backup NG ({0})", parameter));
                //}
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            string log = string.Format("{0} Parameter save {1}", parameter, (result ? "OK" : "NG"));
            if (result)
            {
                try   { log += string.Format(" (..\\{0})", logSrc.Substring(logSrc.IndexOf("EXE"))); }
                catch { log += string.Format(" (..\\{0})", logSrc); }
            }
            Log.Write(MethodBase.GetCurrentMethod().Name, log);
            return result;
        }
        private static bool BackupParameter(e_Parameter parameter, string src, bool isBackup)
        {
            if (isBackup == false) return true;
            bool result = false;
            string logDst = string.Empty;
            try
            {
                if (File.Exists(src))
                {
                    string path = (parameter == e_Parameter.Recipe ? GlobalValue.Directory.bakRecipe : GlobalValue.Directory.bakParameter);
                    string name = Path.GetFileNameWithoutExtension(src);
                    DateTime now = DateTime.Now;
                    string dst = string.Format("{0}\\{1}_{2}_{3}.bak", path, name, now.ToString("yyyyMMdd"), now.ToString("HHmmss"));
                    logDst = dst;

                    if (!Directory.Exists(path))
                         Directory.CreateDirectory(path);

                    File.SetAttributes(src, FileAttributes.Normal);
                    File.Copy(src, dst, true);

                    result = true;
                }
                else
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("{0} 파일을 찾을 수 없습니다", src));
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
            }
            string log = string.Format("{0} Parameter backup {1}", parameter, (result ? "OK" : "NG"));
            if (result)
            {
                try   { log += string.Format(" (..\\{0} -> ..\\{1})", src.Substring(src.IndexOf("EXE")), logDst.Substring(logDst.IndexOf("EXE"))); }
                catch { log += string.Format(" (..\\{0} -> ..\\{1})", src,                               logDst); }
            }
            Log.Write(MethodBase.GetCurrentMethod().Name, log);
            //return result;
            return true; // 백업 여부 상관없이 진행될 수 있도록 true 리턴
        }

        public static void SetPictureBoxImage(PictureBox pictureBox, string fileName)
        {
            if (pictureBox.Image != null)
            {
                ClearPictureBoxImage(pictureBox);
            }

            pictureBox.Image = Image.FromFile(fileName);
        }
        private static void ClearPictureBoxImage(PictureBox pictureBox)
        {
            pictureBox.Image.Dispose();
            pictureBox.Image = null;
        }

        public static void SetProgressBar(int min, int max)
        {
            if (GlobalValue.Form.FormMdi != null)
            {
                GlobalValue.Form.FormMdi.toolStripProgressBar1.Minimum = min;
                GlobalValue.Form.FormMdi.toolStripProgressBar1.Maximum = max;

                GlobalValue.Form.FormMdi.toolStripProgressBar1.Value = GlobalValue.Form.FormMdi.toolStripProgressBar1.Minimum;
            }
        }
        public static void SetProgressBarValue(int value)
        {
            if (GlobalValue.Form.FormMdi != null)
            {
                GlobalValue.Form.FormMdi.toolStripProgressBar1.Value = value;

                //if ((value % ((GlobalValue.Form.FormMdi.toolStripProgressBar1.Maximum - GlobalValue.Form.FormMdi.toolStripProgressBar1.Minimum + 1) / 10)) == 0)
                //{
                //    Application.DoEvents();
                //}
            }
        }

        public static DialogResult MessageBox(string call, string text, MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            Log.Write(call, string.Format("MessageBox=[{0}]", text.Replace("\r\n", " ")));

            DialogResult result = System.Windows.Forms.MessageBox.Show(text, "DAECO", buttons, icon);
            if (buttons != MessageBoxButtons.OK)
            {
                Log.Write(call, string.Format("[{0}]=[{1}]", buttons, result));
            }

            return result;
        }

        public static class DataGridView
        {
            public static void SetProperties(System.Windows.Forms.DataGridView dgv)
            {
                DoubleBuffered(dgv, true);

                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToResizeRows = false;

                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // 홀수 번호 행에 적용되는 기본 셀 스타일을 설정

                DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
                defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                defaultCellStyle.Font = new Font(e_Font.맑은_고딕.ToString(), 14);
                if (dgv.Name == "dgv_Recipe" ||
                    dgv.Name == "dgv_PLC_R_IN" ||
                    dgv.Name == "dgv_PLC_R_OUT"
                   )
                {
                    defaultCellStyle.Font = new Font(e_Font.맑은_고딕.ToString(), 13);
                }
                dgv.DefaultCellStyle = defaultCellStyle;

                dgv.RowHeadersVisible = false;

                dgv.RowHeadersWidth = 56;
                dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                DataGridViewRow rowTemplate = new DataGridViewRow();
                rowTemplate.Height = 28;
                dgv.RowTemplate = rowTemplate;
            }

            public static void AddColumns(System.Windows.Forms.DataGridView dgv, string[] columns)
            {
                dgv.Columns.Clear();

                for (int i = 0; i < columns.Length; i++)
                {
                    dgv.Columns.Add(columns[i], columns[i]);

                    dgv.Columns[i].ReadOnly = (columns[i] == e_DF_Recipe.Data.ToString() ? false : true);
                    dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                if (dgv.Name == "dgv_Recipe")
                {
                    //dgv.Columns[(int)e_DF_Recipe.Unit   ].Visible = false;
                    dgv.Columns[(int)e_DF_Recipe.Address].Visible = false;
                    dgv.Columns[(int)e_DF_Recipe.Count  ].Visible = false;
                    dgv.Columns[(int)e_DF_Recipe.Format ].Visible = false;
                    dgv.Columns[(int)e_DF_Recipe.C      ].Visible = false;
                }

                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cellStyle.BackColor = Color.DimGray;
                cellStyle.ForeColor = Color.White;
                cellStyle.Font = new Font(e_Font.Tahoma.ToString(), 14);
                dgv.ColumnHeadersDefaultCellStyle = cellStyle;

                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                dgv.ColumnHeadersHeight = 28;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgv.EnableHeadersVisualStyles = false;
            }

            public static void AddRows(System.Windows.Forms.DataGridView dgv, e_Parameter parameter)
            {
                dgv.Rows.Clear();

                try
                {
                    #region GlobalValue.Parameter 기준 데이터 출력
                    if (GlobalValue.Parameter[(int)parameter] != null)
                    {
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                        switch (parameter)
                        {
                            case e_Parameter.PLC_InterfaceF_Alarm:
                            case e_Parameter.PLC_InterfaceB_IN:
                            case e_Parameter.PLC_InterfaceB_OUT:
                                for (int i = 1; i <= GlobalValue.Parameter[(int)parameter].Count; i++)
                                {
                                    dgv.Rows.Add(GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceF_Alarm.No],
                                                 string.Empty,
                                                 GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Address],
                                                 GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Description]
                                                );
                                }
                                break;


                            case e_Parameter.PLC_InterfaceR_IN:
                            case e_Parameter.PLC_InterfaceR_OUT:
                                for (int i = 1; i <= GlobalValue.Parameter[(int)parameter].Count; i++)
                                {
                                    dgv.Rows.Add(GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceR_IN.No],
                                                 string.Empty,
                                                 GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceR_IN.Address],
                                                 GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceR_IN.Count],
                                                 GlobalValue.Parameter[(int)parameter][i.ToString()][(int)e_DF_PLC_InterfaceR_IN.Description]
                                                );
                                }
                                break;


                            default:
                                for (int i = 0; i < GlobalValue.Parameter[(int)parameter].Count; i++)
                                {
                                    dgv.Rows.Add(GlobalValue.Parameter[(int)parameter][(i + 1).ToString()]);
                                }
                                break;
                        }

                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    }
                    #endregion

                    #region 튜닝
                    switch (parameter)
                    {
                        case e_Parameter.Recipe:
                            if (dgv.Rows.Count > 0)
                            {
                                SetProgressBar(0, dgv.Rows.Count - 1);

                                for (int i = 0; i < dgv.Rows.Count; i++)
                                {
                                    if (GetString(dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Value) == "B")
                                    {
                                        DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();

                                        comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

                                        comboBoxCell.Items.Add(GlobalValue.Parameter[(int)parameter][(i + 1).ToString()][(int)e_DF_Recipe.MIN]);
                                        comboBoxCell.Items.Add(GlobalValue.Parameter[(int)parameter][(i + 1).ToString()][(int)e_DF_Recipe.MAX]);

                                        comboBoxCell.Value = GlobalValue.Parameter[(int)parameter][(i + 1).ToString()][(int)e_DF_Recipe.Data];

                                        dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()] = comboBoxCell;
                                    }

                                    dgv.Rows[i].Cells[e_DF_Recipe.No.ToString()].Style.BackColor = Color.DimGray;
                                    dgv.Rows[i].Cells[e_DF_Recipe.No.ToString()].Style.ForeColor = Color.White;

                                    dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Style.BackColor = Color.DimGray;
                                    dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Style.ForeColor = Color.White;

                                    if ((i % 2) != 0)
                                    {
                                        dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Style.BackColor = Color.PaleGoldenrod;
                                    }
                                    else
                                    {
                                        dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Style.BackColor = Color.LightYellow;
                                    }

                                    if (GetString(dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Value) != "B" &&
                                        GetString(dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Value) != "C" &&
                                        GetString(dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Value) != "N"
                                       )
                                    {
                                        dgv.Rows[i].ReadOnly = true;
                                    }

                                    SetProgressBarValue(i);
                                }
                            }
                            break;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                }
            }
        }

        public static class DB
        {
            public static class MSSQL
            {
                private static string GetConnectionString()
                {
                    string result = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Connection Timeout={4}", GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Server],
                                                                                                                    GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Database],
                                                                                                                    GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Uid],
                                                                                                                    GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Pwd],
                                                                                                                    GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Timeout]
                                                 );
                    //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Connection String=[{0}]", result));
                    return result;
                }

                public static DataTable GetDataTable(string cmdText)
                {
                    DataTable result = new DataTable();
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();

                                //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query=[{0}]", cmdText));

                                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmdText, connection))
                                {
                                    dataAdapter.Fill(result);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    //Log.Write(MethodBase.GetCurrentMethod().Name, (result.Rows.Count > 0 ? "Query OK" : "Query NG"));
                    if (result.Rows.Count == 0)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query NG=[{0}]", cmdText));
                    }
                    return result;
                }

                public static bool Query(string cmdText)
                {
                    bool result = false;
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();

                                //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query=[{0}]", cmdText));

                                using (SqlCommand command = new SqlCommand(cmdText, connection))
                                {
                                    result = (command.ExecuteNonQuery() > 0 ? true : false);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    //Log.Write(MethodBase.GetCurrentMethod().Name, (result ? "Query OK" : "Query NG"));
                    if (result == false)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query NG=[{0}]", cmdText));
                    }
                    return result;
                }
            }

            public static class MySQL
            {
                private static string GetConnectionString()
                {
                    //string result = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Server],
                    //                                                                                  GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Port],
                    //                                                                                  GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Database],
                    //                                                                                  GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Uid],
                    //                                                                                  GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Pwd]
                    //                             );
                    string result = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};Connection Timeout={5}", GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Server],
                                                                                                                             GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Port],
                                                                                                                             GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Database],
                                                                                                                             GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Uid],
                                                                                                                             GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Pwd],
                                                                                                                             GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Timeout]
                                                 );
                    //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Connection String=[{0}]", result));
                    return result;
                }

                public static DataTable GetDataTable(string cmdText)
                {
                    DataTable result = new DataTable();
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();

                                //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query=[{0}]", cmdText));

                                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmdText, connection))
                                {
                                    dataAdapter.Fill(result);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    //Log.Write(MethodBase.GetCurrentMethod().Name, (result.Rows.Count > 0 ? "Query OK" : "Query NG"));
                    if (result.Rows.Count == 0)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query NG=[{0}]", cmdText));
                    }
                    return result;
                }

                public static bool Query(string cmdText)
                {
                    bool result = false;
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();

                                //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query=[{0}]", cmdText));

                                using (MySqlCommand command = new MySqlCommand(cmdText, connection))
                                {
                                    result = (command.ExecuteNonQuery() > 0 ? true : false);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, GetString(ex));
                    }
                    //Log.Write(MethodBase.GetCurrentMethod().Name, (result ? "Query OK" : "Query NG"));
                    if (result == false)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("Query NG=[{0}]", cmdText));
                    }
                    return result;
                }
            }
        }
    }

    public static class GlobalDevice
    {
        public static bool Start()
        {
            bool result = true;
            for (int i = 0; i < Enum.GetNames(typeof(e_Device)).Length; i++)
            {
                if (!Start((e_Device)i))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        public static bool Start(e_Device device)
        {
            bool result = false;
            try
            {
                Stop(device);

                switch (device)
                {
                    case e_Device.PLC:
                        GlobalPLC.MX = new ActUtlTypeLib.ActUtlType();

                        int count;

                        #region B_IN
                        GlobalPLC.B_IN.StartAddress = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceB_IN]["1"][(int)e_DF_PLC_InterfaceB_IN.Address];
                        count = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceB_IN].Count;
                        GlobalPLC.B_IN.Count = count;
                        GlobalPLC.B_IN.Value = new short[count / 16];
                        GlobalPLC.B_IN.Data = new bool[count];
                        GlobalPLC.B_IN.Description = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            GlobalPLC.B_IN.Description[i] = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceB_IN][(i + 1).ToString()][(int)e_DF_PLC_InterfaceB_IN.Description];
                        }
                        #endregion
                        #region B_OUT
                        GlobalPLC.B_OUT.StartAddress = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceB_OUT]["1"][(int)e_DF_PLC_InterfaceB_OUT.Address];
                        count = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceB_OUT].Count;
                        GlobalPLC.B_OUT.Count = count;
                        GlobalPLC.B_OUT.Value = new short[count / 16];
                        GlobalPLC.B_OUT.Data = new bool[count];
                        GlobalPLC.B_OUT.Description = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            GlobalPLC.B_OUT.Description[i] = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceB_OUT][(i + 1).ToString()][(int)e_DF_PLC_InterfaceB_OUT.Description];
                        }
                        #endregion
                        #region F_Alarm
                        GlobalPLC.F_Alarm.StartAddress = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm]["1"][(int)e_DF_PLC_InterfaceF_Alarm.Address];
                        count = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm].Count;
                        GlobalPLC.F_Alarm.Count = count;
                        GlobalPLC.F_Alarm.Value = new short[count / 16];
                        GlobalPLC.F_Alarm.Data = new bool[count];
                        GlobalPLC.F_Alarm.Description = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            GlobalPLC.F_Alarm.Description[i] = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm][(i + 1).ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Description];
                        }
                        #endregion
                        #region R_IN
                        GlobalPLC.R_IN.StartAddress = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceR_IN]["1"][(int)e_DF_PLC_InterfaceR_IN.Address];
                        count = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceR_IN].Count;
                        GlobalPLC.R_IN.Count = count;
                        GlobalPLC.R_IN.Value = new short[count];
                        GlobalPLC.R_IN.Description = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            GlobalPLC.R_IN.Description[i] = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceR_IN][(i + 1).ToString()][(int)e_DF_PLC_InterfaceR_IN.Description];
                        }
                        #endregion
                        #region R_OUT
                        GlobalPLC.R_OUT.StartAddress = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceR_OUT]["1"][(int)e_DF_PLC_InterfaceR_OUT.Address];
                        count = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceR_OUT].Count;
                        GlobalPLC.R_OUT.Count = count;
                        GlobalPLC.R_OUT.Value = new short[count];
                        GlobalPLC.R_OUT.Description = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            GlobalPLC.R_OUT.Description[i] = GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceR_OUT][(i + 1).ToString()][(int)e_DF_PLC_InterfaceR_OUT.Description];
                        }
                        #endregion
                        break;
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        public static void Stop()
        {
            for (int i = 0; i < Enum.GetNames(typeof(e_Device)).Length; i++)
            {
                Stop((e_Device)i);
            }
        }
        public static void Stop(e_Device device)
        {
            switch (device)
            {
                case e_Device.PLC:
                    if (GlobalPLC.MX != null)
                    {
                        GlobalPLC.MX.Close();
                        // TODO
                        //GlobalPLC.MX = null;
                    }
                    break;
            }
        }
    }
}
