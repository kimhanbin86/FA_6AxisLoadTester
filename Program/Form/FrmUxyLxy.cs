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
    public partial class FrmUxyLxy : Form
    {
        private string disp품번 = string.Empty;

        private string specUx1 = string.Empty;
        private string specUx2 = string.Empty;
        private string specUy1 = string.Empty;
        private string specUy2 = string.Empty;
        private string specLx1 = string.Empty;
        private string specLx2 = string.Empty;
        private string specLy1 = string.Empty;
        private string specLy2 = string.Empty;

        private Timer timer1 = null;
        private void Tick1(object sender, EventArgs e)
        {
            timer1?.Stop();
            try
            {
                if (disp품번 != GlobalValue.Test.Actual1)
                {
                    disp품번 = GlobalValue.Test.Actual1;

                    lbl_품번.Text = disp품번;

                    if (GlobalValue.Form.FormData != null)
                    {
                        if (GlobalValue.Form.FormData.dgv_Recipe.Rows.Count > 0)
                        {
                            specUx1 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[47 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specUx2 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[48 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specUy1 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[49 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specUy2 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[50 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specLx1 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[51 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specLx2 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[52 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specLy1 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[53 - 1].Cells[(int)e_DF_Recipe.Data].Value);
                            specLy2 = string.Format("{0:0.00}", GlobalValue.Form.FormData.dgv_Recipe.Rows[54 - 1].Cells[(int)e_DF_Recipe.Data].Value);

                            lbl_Spec_Ux1.Text = specUx1;
                            lbl_Spec_Ux2.Text = specUx2;
                            lbl_Spec_Uy1.Text = specUy1;
                            lbl_Spec_Uy2.Text = specUy2;
                            lbl_Spec_Lx1.Text = specLx1;
                            lbl_Spec_Lx2.Text = specLx2;
                            lbl_Spec_Ly1.Text = specLy1;
                            lbl_Spec_Ly2.Text = specLy2;

                            InitializeChart(chart_Ux, specUx1, specUx2);
                            InitializeChart(chart_Uy, specUy1, specUy2);
                            InitializeChart(chart_Lx, specLx1, specLx2);
                            InitializeChart(chart_Ly, specLy1, specLy2);

                            AddPointToChart();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                timer1?.Start();
            }
        }

        private Timer timer2 = null;
        private void Tick2(object sender, EventArgs e)
        {
            timer2?.Stop();
            try
            {
                btn_Up_Ux.Enabled =
                btn_Up_Uy.Enabled =
                btn_Up_Lx.Enabled =
                btn_Up_Ly.Enabled =

                btn_Down_Ux.Enabled =
                btn_Down_Uy.Enabled =
                btn_Down_Lx.Enabled =
                btn_Down_Ly.Enabled = !GlobalValue.Form.FormData.isOffsetDownload;
            }
            finally
            {
                timer2?.Start();
            }
        }

        private System.Threading.Thread thread = null;
        private bool isThread = false;
        private void Process()
        {
            while (isThread)
            {
                try
                {
                    if (isInitializeChart)
                    {
                        if (isAddPointToChart)
                        {
                            isAddPointToChart = false;

                            double yValue = 0.0;
                            int yCount = 0;
                            for (int i = 0; i < chart_Ux.Series.Count; i++)
                            {
                                for (int j = 0; j < chart_Ux.Series[i].Points.Count; j++)
                                {
                                    if (chart_Ux.Series[i].Points[j].MarkerSize > 0)
                                    {
                                        yValue += chart_Ux.Series[i].Points[j].YValues[0];
                                        yCount++;
                                    }
                                }
                            }
                            //lbl_Avg_Ux.Text = (yValue / yCount).ToString();
                            //lbl_Avg_Ux.Text = string.Format("{0:0.0}", yValue / yCount);
                            SetLabelText(lbl_Avg_Ux, string.Format("{0:0.0}", yValue / yCount));

                            yValue = 0.0;
                            yCount = 0;
                            for (int i = 0; i < chart_Uy.Series.Count; i++)
                            {
                                for (int j = 0; j < chart_Uy.Series[i].Points.Count; j++)
                                {
                                    if (chart_Uy.Series[i].Points[j].MarkerSize > 0)
                                    {
                                        yValue += chart_Uy.Series[i].Points[j].YValues[0];
                                        yCount++;
                                    }
                                }
                            }
                            //lbl_Avg_Uy.Text = (yValue / yCount).ToString();
                            //lbl_Avg_Uy.Text = string.Format("{0:0.0}", yValue / yCount);
                            SetLabelText(lbl_Avg_Uy, string.Format("{0:0.0}", yValue / yCount));

                            yValue = 0.0;
                            yCount = 0;
                            for (int i = 0; i < chart_Lx.Series.Count; i++)
                            {
                                for (int j = 0; j < chart_Lx.Series[i].Points.Count; j++)
                                {
                                    if (chart_Lx.Series[i].Points[j].MarkerSize > 0)
                                    {
                                        yValue += chart_Lx.Series[i].Points[j].YValues[0];
                                        yCount++;
                                    }
                                }
                            }
                            //lbl_Avg_Lx.Text = (yValue / yCount).ToString();
                            //lbl_Avg_Lx.Text = string.Format("{0:0.0}", yValue / yCount);
                            SetLabelText(lbl_Avg_Lx, string.Format("{0:0.0}", yValue / yCount));

                            yValue = 0.0;
                            yCount = 0;
                            for (int i = 0; i < chart_Ly.Series.Count; i++)
                            {
                                for (int j = 0; j < chart_Ly.Series[i].Points.Count; j++)
                                {
                                    if (chart_Ly.Series[i].Points[j].MarkerSize > 0)
                                    {
                                        yValue += chart_Ly.Series[i].Points[j].YValues[0];
                                        yCount++;
                                    }
                                }
                            }
                            //lbl_Avg_Ly.Text = (yValue / yCount).ToString();
                            //lbl_Avg_Ly.Text = string.Format("{0:0.0}", yValue / yCount);
                            SetLabelText(lbl_Avg_Ly, string.Format("{0:0.0}", yValue / yCount));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(100);
            }
        }
        private void SetLabelText(Label lbl, string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate() { SetLabelText(lbl, text); }));
                }
                else
                {
                    lbl.Text = text;
                }
            }
            catch { }
        }

        private void InitializeChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string spec1, string spec2)
        {
            isInitializeChart = false;

            try
            {
                chart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(spec1) - Convert.ToDouble(spec2) - 2;
                chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(spec1) + Convert.ToDouble(spec2) + 2;

                if (chart.ChartAreas[0].AxisY.Interval != 1)
                {
                    chart.ChartAreas[0].AxisY.Interval = 1;
                }

                //chart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd\r\nHH:mm:ss";
                chart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm";
                chart.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
                chart.ChartAreas[0].AxisX.Interval = 20;

                if (chart.Legends.Count > 0)
                {
                    chart.Legends.Clear();
                }

                if (chart.Series.Count > 0)
                {
                    chart.Series.Clear();

                    chart.Series.Add("No.1_Point");
                    chart.Series.Add("No.2_Point");
                    chart.Series.Add("No.3_Point");
                    chart.Series.Add("No.4_Point");

                    int idx = 0;
                    for (; idx < chart.Series.Count; idx++)
                    {
                        chart.Series[idx].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

                        chart.Series[idx].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                        chart.Series[idx].MarkerSize = 6;

                        chart.Series[idx].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

                        switch (idx)
                        {
                            case 0: chart.Series[idx].Color = Color.Blue; break;
                            case 1: chart.Series[idx].Color = Color.Red; break;
                            case 2: chart.Series[idx].Color = Color.DeepSkyBlue; break;
                            case 3: chart.Series[idx].Color = Color.Lime; break;
                        }
                    }
                }

                chart.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.Auto;
            }
            catch { }
        }

        private bool isInitializeChart = false;
        private void AddPointToChart()
        {
            try
            {
                string time1 = dateTimePicker1.Value.ToString("yyyyMMdd") + dateTimePicker2.Value.ToString("HHmmss");

                string cmdText = string.Format("SELECT * FROM {0} WHERE {1}='{2}' AND {3}>='{4}' ORDER BY {5} ASC", e_DBTableList.Result_MC,
                                                                                                                    e_DBResult_MC.Number, disp품번,
                                                                                                                    e_DBResult_MC.Time, time1,
                                                                                                                    e_DBResult_MC.Time
                                              );

                DataTable dt = GlobalFunction.DB.MySQL.GetDataTable(cmdText);

                if (dt.Rows.Count > 0)
                {
                    string time = string.Empty;
                    double value = 0;

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        time = dt.Rows[row][(int)e_DBResult_MC.Time].ToString();

                        for (int col = (int)e_DBResult_MC.No_1_Ux; col <= (int)e_DBResult_MC.No_4_Ly; col++)
                        {
                            value = Convert.ToDouble(dt.Rows[row][col].ToString());

                            switch ((e_DBResult_MC)col)
                            {
                                case e_DBResult_MC.No_1_Ux: AddPointToChart(0, 0, time, value); break;
                                case e_DBResult_MC.No_1_Uy: AddPointToChart(1, 0, time, value); break;
                                case e_DBResult_MC.No_1_Lx: AddPointToChart(2, 0, time, value); break;
                                case e_DBResult_MC.No_1_Ly: AddPointToChart(3, 0, time, value); break;

                                case e_DBResult_MC.No_2_Ux: AddPointToChart(0, 1, time, value); break;
                                case e_DBResult_MC.No_2_Uy: AddPointToChart(1, 1, time, value); break;
                                case e_DBResult_MC.No_2_Lx: AddPointToChart(2, 1, time, value); break;
                                case e_DBResult_MC.No_2_Ly: AddPointToChart(3, 1, time, value); break;

                                case e_DBResult_MC.No_3_Ux: AddPointToChart(0, 2, time, value); break;
                                case e_DBResult_MC.No_3_Uy: AddPointToChart(1, 2, time, value); break;
                                case e_DBResult_MC.No_3_Lx: AddPointToChart(2, 2, time, value); break;
                                case e_DBResult_MC.No_3_Ly: AddPointToChart(3, 2, time, value); break;

                                case e_DBResult_MC.No_4_Ux: AddPointToChart(0, 3, time, value); break;
                                case e_DBResult_MC.No_4_Uy: AddPointToChart(1, 3, time, value); break;
                                case e_DBResult_MC.No_4_Lx: AddPointToChart(2, 3, time, value); break;
                                case e_DBResult_MC.No_4_Ly: AddPointToChart(3, 3, time, value); break;
                            }
                        }
                    }
                }
                else
                {
                    lbl_Avg_Ux.Text = "0.0";
                    lbl_Avg_Uy.Text = "0.0";
                    lbl_Avg_Lx.Text = "0.0";
                    lbl_Avg_Ly.Text = "0.0";
                }

                isInitializeChart = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private bool isAddPointToChart = false;
        public void AddPointToChart(int chart, int series, string xValue, double yValue)
        {
            xValue = xValue.Insert(12, ":");
            xValue = xValue.Insert(10, ":");
            xValue = xValue.Insert( 8, " ");
            xValue = xValue.Insert( 6, "-");
            xValue = xValue.Insert( 4, "-");

            DateTime dt = DateTime.Parse(xValue);

            var time = dt.ToOADate();

            if (chart == 0)
            {
                chart_Ux.Series[series].Points.AddXY(time, yValue);
            }
            else if (chart == 1)
            {
                chart_Uy.Series[series].Points.AddXY(time, yValue);
            }
            else if (chart == 2)
            {
                chart_Lx.Series[series].Points.AddXY(time, yValue);
            }
            else if (chart == 3)
            {
                chart_Ly.Series[series].Points.AddXY(time, yValue);
            }

            if (yValue == 0)
            {
                if (chart == 0)
                {
                    chart_Ux.Series[series].Points[chart_Ux.Series[series].Points.Count - 1].MarkerSize = 0;
                }
                else if (chart == 1)
                {
                    chart_Uy.Series[series].Points[chart_Uy.Series[series].Points.Count - 1].MarkerSize = 0;
                }
                else if (chart == 2)
                {
                    chart_Lx.Series[series].Points[chart_Lx.Series[series].Points.Count - 1].MarkerSize = 0;
                }
                else if (chart == 3)
                {
                    chart_Ly.Series[series].Points[chart_Ly.Series[series].Points.Count - 1].MarkerSize = 0;
                }
            }

            isAddPointToChart = true;
        }

        public FrmUxyLxy()
        {
            InitializeComponent();
        }

        private void FrmUxyLxy_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormUxyLxy = null;
        }
        private void FrmUxyLxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1 != null)
            {
                if (timer1.Enabled)
                    timer1.Stop();
                timer1.Dispose();
                timer1 = null;
            }

            isThread = false;

            if (timer2 != null)
            {
                if (timer2.Enabled)
                    timer2.Stop();
                timer2.Dispose();
                timer2 = null;
            }
        }
        private void FrmUxyLxy_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(0);
            dateTimePicker2.Value = DateTime.Parse("00:00:00");

            timer1 = new Timer();
            timer1.Tick += new EventHandler(Tick1);
            timer1.Interval = 1000;
            timer1.Start();

            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 1)
            {
                Location = new Point(screens[1].WorkingArea.X, screens[1].WorkingArea.Y);
                Size = new Size(screens[1].WorkingArea.Width, screens[1].WorkingArea.Height);
            }
            else
            {
                Location = new Point(Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.Monitor]["1"][(int)e_DF_Monitor.Location_X]), Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.Monitor]["1"][(int)e_DF_Monitor.Location_Y]));
                Size = new Size(Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.Monitor]["1"][(int)e_DF_Monitor.Size_X]), Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.Monitor]["1"][(int)e_DF_Monitor.Size_Y]));
            }

            thread = new System.Threading.Thread(Process);
            thread.IsBackground = true;
            isThread = true;
            thread.Start();

            timer2 = new Timer();
            timer2.Tick += new EventHandler(Tick2);
            timer2.Interval = 10;
            timer2.Start();
        }

        private void chart_Paint(object sender, PaintEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart chart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;

            try
            {
                float minX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Minimum);
                float maxX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Maximum);

                float cenY1 = 0;
                float cenY2 = 0;

                switch (chart.Name)
                {
                    case "chart_Ux":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Ux1.Text) + Convert.ToDouble(lbl_Spec_Ux2.Text));
                        cenY2 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Ux1.Text) - Convert.ToDouble(lbl_Spec_Ux2.Text));
                        break;


                    case "chart_Uy":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Uy1.Text) + Convert.ToDouble(lbl_Spec_Uy2.Text));
                        cenY2 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Uy1.Text) - Convert.ToDouble(lbl_Spec_Uy2.Text));
                        break;


                    case "chart_Lx":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Lx1.Text) + Convert.ToDouble(lbl_Spec_Lx2.Text));
                        cenY2 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Lx1.Text) - Convert.ToDouble(lbl_Spec_Lx2.Text));
                        break;


                    case "chart_Ly":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Ly1.Text) + Convert.ToDouble(lbl_Spec_Ly2.Text));
                        cenY2 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Ly1.Text) - Convert.ToDouble(lbl_Spec_Ly2.Text));
                        break;
                }

                Pen pen = new Pen(Color.Red, 3);

                PointF pt1 = new PointF(minX, cenY1);
                PointF pt2 = new PointF(maxX, cenY1);
                e.Graphics.DrawLine(pen, pt1, pt2);

                pt1 = new PointF(minX, cenY2);
                pt2 = new PointF(maxX, cenY2);
                e.Graphics.DrawLine(pen, pt1, pt2);

                switch (chart.Name)
                {
                    case "chart_Ux":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Ux1.Text));
                        break;


                    case "chart_Uy":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Uy1.Text));
                        break;


                    case "chart_Lx":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Lx1.Text));
                        break;


                    case "chart_Ly":
                        cenY1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(lbl_Spec_Ly1.Text));
                        break;
                }

                Pen pen2 = new Pen(Color.Blue, 3);

                PointF pt12 = new PointF(minX, cenY1);
                PointF pt22 = new PointF(maxX, cenY1);
                e.Graphics.DrawLine(pen2, pt12, pt22);
            }
            catch { }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            InitializeChart(chart_Ux, specUx1, specUx2);
            InitializeChart(chart_Uy, specUy1, specUy2);
            InitializeChart(chart_Lx, specLx1, specLx2);
            InitializeChart(chart_Ly, specLy1, specLy2);

            AddPointToChart();
        }

        public enum e_RAM
        {
            RAM_1_1,
            RAM_1_2,
            RAM_2_1,
            RAM_2_2
        }

        private void btn_Up_Click(object sender, EventArgs e)
        {
            //btn_Up_Ux
            //btn_Up_Uy
            //btn_Up_Lx
            //btn_Up_Ly
            try
            {
                Button btnC = sender as Button;
                string btnS = btnC.Name.Substring(7);
                //MessageBox.Show(btnS);

                #region check condition
                if (GlobalValue.Form.FormData == null ||
                    GlobalValue.Form.FormData.dgv_Recipe.Rows.Count == 0
                   )
                {
                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "제품 데이터를 확인할 수 없습니다", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion

                double[] offset = new double[Enum.GetNames(typeof(e_RAM)).Length];

                #region read offset from recipe
                switch (btnS)
                {
                    case "Ux":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;


                    case "Uy":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;


                    case "Lx":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;


                    case "Ly":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;
                }
                #endregion

                for (int i = 0; i < offset.Length; i++)
                {
                    offset[i] = offset[i] + 1;
                }

                #region write offset to recipe
                switch (btnS)
                {
                    case "Ux":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;


                    case "Uy":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;


                    case "Lx":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;


                    case "Ly":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;
                }
                #endregion

                GlobalValue.Form.FormData.isOffsetDownload = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void btn_Down_Click(object sender, EventArgs e)
        {
            //btn_Down_Ux
            //btn_Down_Uy
            //btn_Down_Lx
            //btn_Down_Ly
            try
            {
                Button btnC = sender as Button;
                string btnS = btnC.Name.Substring(9);
                //MessageBox.Show(btnS);

                #region check condition
                if (GlobalValue.Form.FormData == null ||
                    GlobalValue.Form.FormData.dgv_Recipe.Rows.Count == 0
                   )
                {
                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "제품 데이터를 확인할 수 없습니다", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion

                double[] offset = new double[Enum.GetNames(typeof(e_RAM)).Length];

                #region read offset from recipe
                switch (btnS)
                {
                    case "Ux":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;


                    case "Uy":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;


                    case "Lx":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;


                    case "Ly":
                        offset[(int)e_RAM.RAM_1_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_1_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_1] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        offset[(int)e_RAM.RAM_2_2] = Convert.ToDouble(GlobalFunction.GetString(GlobalValue.Form.FormData.dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                        break;
                }
                #endregion

                for (int i = 0; i < offset.Length; i++)
                {
                    offset[i] = offset[i] - 1;
                }

                #region write offset to recipe
                switch (btnS)
                {
                    case "Ux":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;


                    case "Uy":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;


                    case "Lx":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;


                    case "Ly":
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_1_2];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_1];
                        GlobalValue.Form.FormData.dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = offset[(int)e_RAM.RAM_2_2];
                        break;
                }
                #endregion

                GlobalValue.Form.FormData.isOffsetDownload = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
    }
}
