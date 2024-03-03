//#define TEST

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Reflection;

using System.Windows.Media;
using System.Windows.Media.Media3D;

using LibLog;

namespace Program
{
    public partial class FrmData : Form
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

        #region Timer
        private Timer timerUpdatePLC = null;
        private void TickUpdatePLC(object sender, EventArgs e)
        {
            timerUpdatePLC?.Stop();
            try
            {
                if (tabControl1.SelectedIndex != 1) return;

                if (tabControl3.SelectedIndex < 3)
                {
                    for (int i = 0; i < GlobalPLC.F_Alarm.Count; i++)
                    {
                        dgv_PLC_F_Alarm.Rows[i].Cells[e_DGV_PLC1.Value.ToString()].Style.BackColor = (GlobalPLC.F_Alarm.Data[i] ? System.Drawing.Color.Lime : System.Drawing.Color.Gainsboro);
                        dgv_PLC_F_Alarm.Rows[i].Cells[e_DGV_PLC1.Value.ToString()].Value = GlobalPLC.F_Alarm.Data[i].ToString();
                    }

                    for (int i = 0; i < GlobalPLC.B_IN.Count; i++)
                    {
                        dgv_PLC_B_IN.Rows[i].Cells[e_DGV_PLC1.Value.ToString()].Style.BackColor = (GlobalPLC.B_IN.Data[i] ? System.Drawing.Color.Lime : System.Drawing.Color.Gainsboro);
                        dgv_PLC_B_IN.Rows[i].Cells[e_DGV_PLC1.Value.ToString()].Value = GlobalPLC.B_IN.Data[i].ToString();
                    }

                    for (int i = 0; i < GlobalPLC.B_OUT.Count; i++)
                    {
                        dgv_PLC_B_OUT.Rows[i].Cells[e_DGV_PLC1.Value.ToString()].Style.BackColor = (GlobalPLC.B_OUT.Data[i] ? System.Drawing.Color.Lime : System.Drawing.Color.Gainsboro);
                        dgv_PLC_B_OUT.Rows[i].Cells[e_DGV_PLC1.Value.ToString()].Value = GlobalPLC.B_OUT.Data[i].ToString();
                    }
                }
                else if (tabControl3.SelectedIndex >= 3)
                {
                    for (int i = 0; i < GlobalPLC.R_IN.Count; i++)
                    {
                        if (GlobalFunction.GetString(dgv_PLC_R_IN.Rows[i].Cells[e_DGV_PLC2.Value.ToString()].Value) != GlobalPLC.R_IN.Value[i].ToString())
                        {
                            dgv_PLC_R_IN.Rows[i].Cells[e_DGV_PLC2.Value.ToString()].Value = GlobalPLC.R_IN.Value[i].ToString();
                        }
                    }

                    for (int i = 0; i < GlobalPLC.R_OUT.Count; i++)
                    {
                        if (GlobalFunction.GetString(dgv_PLC_R_OUT.Rows[i].Cells[e_DGV_PLC2.Value.ToString()].Value) != GlobalPLC.R_OUT.Value[i].ToString())
                        {
                            dgv_PLC_R_OUT.Rows[i].Cells[e_DGV_PLC2.Value.ToString()].Value = GlobalPLC.R_OUT.Value[i].ToString();
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
                timerUpdatePLC?.Start();
            }
        }

        private Timer timerUpdateL = null;
        private void TickUpdateL(object sender, EventArgs e)
        {
            timerUpdateL?.Stop();
            try
            {
                lbl_Actual1.Text = GlobalValue.Test.Actual1;
                //lbl_Actual2.Text = GlobalValue.Test.Actual2;
                //lbl_Actual3.Text = GlobalValue.Test.Actual3;

                Text = GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName];
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                timerUpdateL?.Start();
            }
        }
        private Timer timerUpdateC = null;
        private void TickUpdateC(object sender, EventArgs e)
        {
            timerUpdateC?.Stop();
            try
            {
                lblCh1Value12.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._60_RAM_1_1_ACTUAL_B_LOAD] / 10);
                lblCh3Value12.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._66_RAM_1_2_ACTUAL_B_LOAD] / 10);
                lblCh5Value12.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._72_RAM_2_1_ACTUAL_B_LOAD] / 10);
                lblCh7Value12.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._78_RAM_2_2_ACTUAL_B_LOAD] / 10);

                lblInd0.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._1_RAM_1_1_HEIGHT] / 10);
                lblInd2.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._3_RAM_1_2_HEIGHT] / 10);
                lblInd4.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._5_RAM_2_1_HEIGHT] / 10);
                lblInd6.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._7_RAM_2_2_HEIGHT] / 10);

                lblInd1.Text = string.Format("{0:0.000}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._2_ORI_1_1_TURN] / 1000);
                lblInd3.Text = string.Format("{0:0.000}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._4_ORI_1_2_TURN] / 1000);
                lblInd5.Text = string.Format("{0:0.000}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._6_ORI_2_1_TURN] / 1000);
                lblInd7.Text = string.Format("{0:0.000}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._8_ORI_2_2_TURN_] / 1000);

                lblCh1Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._61_RAM_1_1_ACTUAL_B_Rx] / 100);
                lblCh1Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._62_RAM_1_1_ACTUAL_B_Ry] / 100);
                lblCh2Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._64_RAM_1_1_ACTUAL_A_Rx] / 100);
                lblCh2Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._65_RAM_1_1_ACTUAL_A_Ry] / 100);

                lblCh3Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._67_RAM_1_2_ACTUAL_B_Rx] / 100);
                lblCh3Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._68_RAM_1_2_ACTUAL_B_Ry] / 100);
                lblCh4Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._70_RAM_1_2_ACTUAL_A_Rx] / 100);
                lblCh4Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._71_RAM_1_2_ACTUAL_A_Ry] / 100);

                lblCh5Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._73_RAM_2_1_ACTUAL_B_Rx] / 100);
                lblCh5Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._74_RAM_2_1_ACTUAL_B_Ry] / 100);
                lblCh6Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._76_RAM_2_1_ACTUAL_A_Rx] / 100);
                lblCh6Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._77_RAM_2_1_ACTUAL_A_Ry] / 100);

                lblCh7Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._79_RAM_2_2_ACTUAL_B_Rx] / 100);
                lblCh7Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._80_RAM_2_2_ACTUAL_B_Ry] / 100);
                lblCh8Value13.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._82_RAM_2_2_ACTUAL_A_Rx] / 100);
                lblCh8Value14.Text = string.Format("{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._83_RAM_2_2_ACTUAL_A_Ry] / 100);

                button1.BackColor = (groupBox2.Visible ? System.Drawing.Color.Lime : System.Drawing.SystemColors.Control);
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                timerUpdateC?.Start();
            }
        }
        private Timer timerUpdateB = null;
        private void TickUpdateB(object sender, EventArgs e)
        {
            timerUpdateB?.Stop();
            try
            {
                #region MODE
                if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._17_AUTO_MODE  ] == 1 &&
                    GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._18_MANUAL_MODE] == 0
                   )
                {
                    if (lbl_Mode.Text != "자동 모드")
                    {
                        lbl_Mode.Text = "자동 모드";

                        GlobalFunction.SetPictureBoxImage(pic_Mode, GlobalValue.File.Status.Green);
                    }
                }
                else if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._17_AUTO_MODE  ] == 0 &&
                         GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._18_MANUAL_MODE] == 1
                        )
                {
                    if (lbl_Mode.Text != "수동 모드")
                    {
                        lbl_Mode.Text = "수동 모드";

                        GlobalFunction.SetPictureBoxImage(pic_Mode, GlobalValue.File.Status.Red);
                    }
                }
                else
                {
                    if (lbl_Mode.Text != "UNKNOWN")
                    {
                        lbl_Mode.Text = "UNKNOWN";

                        GlobalFunction.SetPictureBoxImage(pic_Mode, GlobalValue.File.Status.White);
                    }
                }
                #endregion

                lbl_DOOR.BackColor = (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._58_DOOR_OPEN_MONITOR] == 1 ? System.Drawing.Color.Red : System.Drawing.Color.Lime);

                if (dgv_Recipe.Rows.Count > 0)
                {
                    lblS1.Text = string.Format("{0} +-{1}", GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value),
                                                            GlobalFunction.GetString(dgv_Recipe.Rows[48 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)
                                              );
                    lblS2.Text = string.Format("{0} +-{1}", GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value),
                                                            GlobalFunction.GetString(dgv_Recipe.Rows[50 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)
                                              );
                    lblS3.Text = string.Format("{0} +-{1}", GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value),
                                                            GlobalFunction.GetString(dgv_Recipe.Rows[52 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)
                                              );
                    lblS4.Text = string.Format("{0} +-{1}", GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value),
                                                            GlobalFunction.GetString(dgv_Recipe.Rows[54 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)
                                              );

                    lblMaster0.Text = string.Format("{0:0.0}", Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));

                    lblMaster1.Text = string.Format("{0:0.00}", Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    lblMaster2.Text = string.Format("{0:0.00}", Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    lblMaster3.Text = string.Format("{0:0.00}", Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    lblMaster4.Text = string.Format("{0:0.00}", Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));

                    if (string.IsNullOrEmpty(GlobalFunction.GetString(dgv_Recipe.Rows[dgv_Recipe.CurrentCell.RowIndex].Cells[e_DF_Recipe.MIN.ToString()].Value)) ||
                        string.IsNullOrEmpty(GlobalFunction.GetString(dgv_Recipe.Rows[dgv_Recipe.CurrentCell.RowIndex].Cells[e_DF_Recipe.MAX.ToString()].Value))
                       )
                    {
                        lblRange.Text = "Range : ";
                    }
                    else
                    {
                        lblRange.Text = string.Format("Range : {0} ~ {1}", GlobalFunction.GetString(dgv_Recipe.Rows[dgv_Recipe.CurrentCell.RowIndex].Cells[e_DF_Recipe.MIN.ToString()].Value),
                                                                           GlobalFunction.GetString(dgv_Recipe.Rows[dgv_Recipe.CurrentCell.RowIndex].Cells[e_DF_Recipe.MAX.ToString()].Value)
                                                     );

                        if (!string.IsNullOrEmpty(GlobalFunction.GetString(dgv_Recipe.Rows[dgv_Recipe.CurrentCell.RowIndex].Cells[e_DF_Recipe.Unit.ToString()].Value)))
                        {
                            lblRange.Text += string.Format(" [{0}]", GlobalFunction.GetString(dgv_Recipe.Rows[dgv_Recipe.CurrentCell.RowIndex].Cells[e_DF_Recipe.Unit.ToString()].Value));
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
                timerUpdateB?.Start();
            }
        }

        private Timer timerSequence = null;
        private void TickSequence(object sender, EventArgs e)
        {
            timerSequence?.Stop();
            try
            {
                // RAM #1 TEST COMPLETED
                if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._19_RAM_1_TEST_COMPLETED] == 1)
                {
                    #region SET (RAM #1 DATA READ COMPLETED)
                    if (GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._101_RAM_1_DATA_READ_COMPLETED] != 1)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, "[PLC -> PC] RAM #1 TEST COMPLETE");

                        InitializeGraph(null, null);

                        PLC_Data_Trans_ram1_display();

                        GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._101_RAM_1_DATA_READ_COMPLETED] = 1;
                        Log.Write(MethodBase.GetCurrentMethod().Name, "[PC -> PLC] SET RAM #1 DATA READ COMPLETE");
                    }
                    #endregion
                }
                else
                {
                    #region CLEAR (RAM #1 DATA READ COMPLETED)
                    if (GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._101_RAM_1_DATA_READ_COMPLETED] == 1)
                    {
                        GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._101_RAM_1_DATA_READ_COMPLETED] = 0;
                        Log.Write(MethodBase.GetCurrentMethod().Name, "[PC -> PLC] CLEAR RAM #1 DATA READ COMPLETE");
                    }
                    #endregion
                }

                // RAM #2 TEST COMPLETED
                if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._20_RAM_2_TEST_COMPLETED] == 1)
                {
                    #region SET (RAM #2 DATA READ COMPLETED)
                    if (GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._102_RAM_2_DATA_READ_COMPLETED] != 1)
                    {
                        Log.Write(MethodBase.GetCurrentMethod().Name, "[PLC -> PC] RAM #2 TEST COMPLETE");

                        InitializeGraph(null, null);

                        PLC_Data_Trans_ram2_display();

                        GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._102_RAM_2_DATA_READ_COMPLETED] = 1;
                        Log.Write(MethodBase.GetCurrentMethod().Name, "[PC -> PLC] SET RAM #2 DATA READ COMPLETE");
                    }
                    #endregion
                }
                else
                {
                    #region CLEAR (RAM #2 DATA READ COMPLETED)
                    if (GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._102_RAM_2_DATA_READ_COMPLETED] == 1)
                    {
                        GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._102_RAM_2_DATA_READ_COMPLETED] = 0;
                        Log.Write(MethodBase.GetCurrentMethod().Name, "[PC -> PLC] CLEAR RAM #2 DATA READ COMPLETE");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                timerSequence?.Start();
            }
        }
        private void InitializeChart()
        {
            try
            {
                if (dgv_Recipe.Rows.Count > 0)
                {
                    InitializeChart(chart1,
                                    Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                    Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                   );

                    InitializeChart(chart2,
                                    Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                    Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                   );
                }
                else
                {
                    InitializeChart(chart1, 0, 0);
                    InitializeChart(chart2, 0, 0);
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void ClearTestVar(int ram)
        {
            switch (ram)
            {
                case 1:
                    GlobalValue.Test.Ram1_1Result = string.Empty;
                    GlobalValue.Test.Ram1_2Result = string.Empty;

                    GlobalValue.Test.Ram1_1LoadTestValue = 0;
                    GlobalValue.Test.Ram1_1_B_Rx = 0;
                    GlobalValue.Test.Ram1_1_B_Ry = 0;
                    GlobalValue.Test.Ram1_1_A_Rx = 0;
                    GlobalValue.Test.Ram1_1_A_Ry = 0;
                    GlobalValue.Test.Ram1_1_temp = 0;

                    GlobalValue.Test.Ram1_2LoadTestValue = 0;
                    GlobalValue.Test.Ram1_2_B_Rx = 0;
                    GlobalValue.Test.Ram1_2_B_Ry = 0;
                    GlobalValue.Test.Ram1_2_A_Rx = 0;
                    GlobalValue.Test.Ram1_2_A_Ry = 0;
                    GlobalValue.Test.Ram1_2_temp = 0;
                    break;


                case 2:
                    GlobalValue.Test.Ram2_1Result = string.Empty;
                    GlobalValue.Test.Ram2_2Result = string.Empty;

                    GlobalValue.Test.Ram2_1LoadTestValue = 0;
                    GlobalValue.Test.Ram2_1_B_Rx = 0;
                    GlobalValue.Test.Ram2_1_B_Ry = 0;
                    GlobalValue.Test.Ram2_1_A_Rx = 0;
                    GlobalValue.Test.Ram2_1_A_Ry = 0;
                    GlobalValue.Test.Ram2_1_temp = 0;

                    GlobalValue.Test.Ram2_2LoadTestValue = 0;
                    GlobalValue.Test.Ram2_2_B_Rx = 0;
                    GlobalValue.Test.Ram2_2_B_Ry = 0;
                    GlobalValue.Test.Ram2_2_A_Rx = 0;
                    GlobalValue.Test.Ram2_2_A_Ry = 0;
                    GlobalValue.Test.Ram2_2_temp = 0;
                    break;
            }

            for (int i = 0; i < isRPQSave.Length; i++)
            {
                isRPQSave[i] = true;
            }
        }
        private void PLC_Data_Trans_ram1_display()
        {
            try
            {
                // Ram2 변수 초기화
                ClearTestVar(2);

#if !TEST
                GlobalValue.Test.Ram1_1LoadTestValue = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._22_RAM_1_1_RESULT_B_LOAD] / 10;
                GlobalValue.Test.Ram1_2LoadTestValue = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._30_RAM_1_2_RESULT_B_LOAD] / 10;
#endif
                Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-1 Load=[{0:0.0}] /  1-2 Load=[{1:0.0}]", GlobalValue.Test.Ram1_1LoadTestValue, GlobalValue.Test.Ram1_2LoadTestValue));

                // 1-1만 저장
                if (GlobalValue.Test.Ram1_1LoadTestValue >  Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) &&
                    GlobalValue.Test.Ram1_2LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                   )
                {
#if !TEST
                    GlobalValue.Test.Ram1_1_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._23_RAM_1_1_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram1_1_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._24_RAM_1_1_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram1_1_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._26_RAM_1_1_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram1_1_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._27_RAM_1_1_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram1_1_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._28_RAM_1_1_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram1_1_B_Rx = CalcScale(GlobalValue.Test.Ram1_1_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram1_1_B_Ry = CalcScale(GlobalValue.Test.Ram1_1_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram1_1_A_Rx = CalcScale(GlobalValue.Test.Ram1_1_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram1_1_A_Ry = CalcScale(GlobalValue.Test.Ram1_1_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-1 A_Rx=[{0:0.00}] /  1-1 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-1 B_Rx=[{0:0.00}] /  1-1 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry));

                    #region Label 출력 (1-1)
                    // B
                    lblCh1Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_1_B_Rx);
                    lblCh1Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_1_B_Ry);
                    lblCh1Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh1Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh1Value19.Text = string.Format("{0:0.0}",  GlobalValue.Test.Ram1_1LoadTestValue);
                    lblCh1Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram1_1LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh2Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_1_A_Rx);
                    lblCh2Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_1_A_Ry);
                    lblCh2Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh2Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh2Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._13_RAM_1_1_OFFSET] / 10);
                    lblCh2Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh2Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

                    AddPointToChart(chart1, GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry);
                    AddPointToChart(chart2, GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry, GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry, "Line1");
                }
                // 1-2만 저장
                else if (GlobalValue.Test.Ram1_1LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) &&
                         GlobalValue.Test.Ram1_2LoadTestValue >  Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                        )
                {
#if !TEST
                    GlobalValue.Test.Ram1_2_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._31_RAM_1_2_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram1_2_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._32_RAM_1_2_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram1_2_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._34_RAM_1_2_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram1_2_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._35_RAM_1_2_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram1_2_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._36_RAM_1_2_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram1_2_B_Rx = CalcScale(GlobalValue.Test.Ram1_2_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram1_2_B_Ry = CalcScale(GlobalValue.Test.Ram1_2_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram1_2_A_Rx = CalcScale(GlobalValue.Test.Ram1_2_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram1_2_A_Ry = CalcScale(GlobalValue.Test.Ram1_2_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-2 A_Rx=[{0:0.00}] /  1-2 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-2 B_Rx=[{0:0.00}] /  1-2 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry));

                    #region Label 출력 (1-2)
                    // B
                    lblCh3Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_2_B_Rx);
                    lblCh3Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_2_B_Ry);
                    lblCh3Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh3Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh3Value19.Text = string.Format("{0:0.0}",  GlobalValue.Test.Ram1_2LoadTestValue);
                    lblCh3Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram1_2LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh4Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_2_A_Rx);
                    lblCh4Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram1_2_A_Ry);
                    lblCh4Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh4Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh4Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._14_RAM_1_2_OFFSET] / 10);
                    lblCh4Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh4Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

                    AddPointToChart(chart1, GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry);
                    AddPointToChart(chart2, GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry, GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry, "Line2");
                }
                // 둘 다 저장
                else if (GlobalValue.Test.Ram1_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) &&
                         GlobalValue.Test.Ram1_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                        )
                {
#if !TEST
                    GlobalValue.Test.Ram1_1_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._23_RAM_1_1_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram1_1_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._24_RAM_1_1_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram1_1_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._26_RAM_1_1_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram1_1_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._27_RAM_1_1_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram1_1_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._28_RAM_1_1_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram1_1_B_Rx = CalcScale(GlobalValue.Test.Ram1_1_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram1_1_B_Ry = CalcScale(GlobalValue.Test.Ram1_1_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram1_1_A_Rx = CalcScale(GlobalValue.Test.Ram1_1_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram1_1_A_Ry = CalcScale(GlobalValue.Test.Ram1_1_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-1 A_Rx=[{0:0.00}] /  1-1 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-1 B_Rx=[{0:0.00}] /  1-1 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry));

                    #region Label 출력 (1-1)
                    // B
                    lblCh1Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_1_B_Rx);
                    lblCh1Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_1_B_Ry);
                    lblCh1Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh1Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh1Value19.Text = string.Format("{0:0.0}", GlobalValue.Test.Ram1_1LoadTestValue);
                    lblCh1Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram1_1LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh2Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_1_A_Rx);
                    lblCh2Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_1_A_Ry);
                    lblCh2Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh2Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_1_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh2Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._13_RAM_1_1_OFFSET] / 10);
                    lblCh2Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh2Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

#if !TEST
                    GlobalValue.Test.Ram1_2_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._31_RAM_1_2_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram1_2_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._32_RAM_1_2_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram1_2_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._34_RAM_1_2_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram1_2_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._35_RAM_1_2_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram1_2_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._36_RAM_1_2_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram1_2_B_Rx = CalcScale(GlobalValue.Test.Ram1_2_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram1_2_B_Ry = CalcScale(GlobalValue.Test.Ram1_2_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram1_2_A_Rx = CalcScale(GlobalValue.Test.Ram1_2_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram1_2_A_Ry = CalcScale(GlobalValue.Test.Ram1_2_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-2 A_Rx=[{0:0.00}] /  1-2 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("1-2 B_Rx=[{0:0.00}] /  1-2 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry));

                    #region Label 출력 (1-2)
                    // B
                    lblCh3Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_2_B_Rx);
                    lblCh3Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_2_B_Ry);
                    lblCh3Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh3Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh3Value19.Text = string.Format("{0:0.0}", GlobalValue.Test.Ram1_2LoadTestValue);
                    lblCh3Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram1_2LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh4Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_2_A_Rx);
                    lblCh4Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram1_2_A_Ry);
                    lblCh4Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh4Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram1_2_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh4Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._14_RAM_1_2_OFFSET] / 10);
                    lblCh4Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh4Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

                    AddPointToChart(chart1, GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry);
                    AddPointToChart(chart1, GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry, GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry, "Line1");

                    AddPointToChart(chart2, GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry);
                    AddPointToChart(chart2, GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry, GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry, "Line2");
                }

#if !TEST
                // Parameter - ETC - 제품 없음 하중 값
                if (GlobalValue.Test.Ram1_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram1_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram2_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram2_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                   )
#endif
                {
                    if (LoadTestDataSave(false) == false)
                    {
                        GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_DATA] = true;
                    }

                    // Parameter - ETC - RPQ 데이터
                    // 0 : 저장 안 함
                    // 1 : 저장    함
                    if (GlobalValue.Parameter[(int)e_Parameter.ETC]["2"][(int)e_DF_ETC.Value0].Trim() == "1")
                    {
                        // DOOR OPEN MONITOR == 0
                        if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._58_DOOR_OPEN_MONITOR] == 0)
                        {
                            if (GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.적용] == "1")
                            {
                                #region 2021-10-21 : 조건 수정
                                //double PP하중 = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                                //double MM하중 = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));

                                #region 2022-02-10 : 다운로드된 ++, -- 하중 값 사용

                                double PP하중 = GlobalValue.Test.PP하중;
                                double MM하중 = GlobalValue.Test.MM하중;

                                #endregion

                                double Range = Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.Range]);

                                double LoadTestValue = GlobalValue.Test.Ram1_1LoadTestValue;
                                if (LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]))
                                {
                                    if ((PP하중 <= LoadTestValue && LoadTestValue <= PP하중 + Range) ||
                                        (MM하중 - Range <= LoadTestValue && LoadTestValue <= MM하중)
                                       )
                                    {
                                        isRPQSave[(int)e_RAM.RAM_1_1] = false;
                                    }
                                }

                                LoadTestValue = GlobalValue.Test.Ram1_2LoadTestValue;
                                if (LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]))
                                {
                                    if ((PP하중 <= LoadTestValue && LoadTestValue <= PP하중 + Range) ||
                                        (MM하중 - Range <= LoadTestValue && LoadTestValue <= MM하중)
                                       )
                                    {
                                        isRPQSave[(int)e_RAM.RAM_1_2] = false;
                                    }
                                }
                                #endregion
                            }

                            if (isRPQSave[(int)e_RAM.RAM_1_1] &&
                                isRPQSave[(int)e_RAM.RAM_1_2]
                               )
                            {
                                if (LoadTestDataSave(true) == false)
                                {
                                    GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_DATA_RPQ] = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void PLC_Data_Trans_ram2_display()
        {
            try
            {
                // Ram1 변수 초기화
                ClearTestVar(1);

#if !TEST
                GlobalValue.Test.Ram2_1LoadTestValue = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._38_RAM_2_1_RESULT_B_LOAD] / 10;
                GlobalValue.Test.Ram2_2LoadTestValue = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._46_RAM_2_2_RESULT_B_LOAD] / 10;
#endif
                Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-1 Load=[{0}] /  2-2 Load=[{1}]", GlobalValue.Test.Ram2_1LoadTestValue, GlobalValue.Test.Ram2_2LoadTestValue));

                // 2-1만 저장
                if (GlobalValue.Test.Ram2_1LoadTestValue >  Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) &&
                    GlobalValue.Test.Ram2_2LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                   )
                {
#if !TEST
                    GlobalValue.Test.Ram2_1_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._39_RAM_2_1_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram2_1_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._40_RAM_2_1_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram2_1_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._42_RAM_2_1_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram2_1_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._43_RAM_2_1_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram2_1_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._44_RAM_2_1_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram2_1_B_Rx = CalcScale(GlobalValue.Test.Ram2_1_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram2_1_B_Ry = CalcScale(GlobalValue.Test.Ram2_1_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram2_1_A_Rx = CalcScale(GlobalValue.Test.Ram2_1_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram2_1_A_Ry = CalcScale(GlobalValue.Test.Ram2_1_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-1 A_Rx=[{0:0.00}] /  2-1 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-1 B_Rx=[{0:0.00}] /  2-1 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry));

                    #region Label 출력 (2-1)
                    // B
                    lblCh5Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_1_B_Rx);
                    lblCh5Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_1_B_Ry);
                    lblCh5Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh5Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh5Value19.Text = string.Format("{0:0.0}",  GlobalValue.Test.Ram2_1LoadTestValue);
                    lblCh5Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram2_1LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh6Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_1_A_Rx);
                    lblCh6Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_1_A_Ry);
                    lblCh6Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh6Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh6Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._15_RAM_2_1_OFFSET] / 10);
                    lblCh6Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh6Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

                    AddPointToChart(chart1, GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry);
                    AddPointToChart(chart2, GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry, GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry, "Line1");
                }
                // 2-2만 저장
                else if (GlobalValue.Test.Ram2_1LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) &&
                         GlobalValue.Test.Ram2_2LoadTestValue >  Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                        )
                {
#if !TEST
                    GlobalValue.Test.Ram2_2_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._47_RAM_2_2_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram2_2_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._48_RAM_2_2_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram2_2_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._50_RAM_2_2_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram2_2_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._51_RAM_2_2_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram2_2_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._52_RAM_2_2_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram2_2_B_Rx = CalcScale(GlobalValue.Test.Ram2_2_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram2_2_B_Ry = CalcScale(GlobalValue.Test.Ram2_2_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram2_2_A_Rx = CalcScale(GlobalValue.Test.Ram2_2_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram2_2_A_Ry = CalcScale(GlobalValue.Test.Ram2_2_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-2 A_Rx=[{0:0.00}] /  2-2 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-2 B_Rx=[{0:0.00}] /  2-2 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry));

                    #region Label 출력 (2-2)
                    // B
                    lblCh7Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_2_B_Rx);
                    lblCh7Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_2_B_Ry);
                    lblCh7Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh7Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh7Value19.Text = string.Format("{0:0.0}",  GlobalValue.Test.Ram2_2LoadTestValue);
                    lblCh7Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram2_2LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh8Value15.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_2_A_Rx);
                    lblCh8Value16.Text = string.Format("{0:0.00}",  GlobalValue.Test.Ram2_2_A_Ry);
                    lblCh8Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh8Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh8Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._16_RAM_2_2_OFFSET] / 10);
                    lblCh8Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh8Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

                    AddPointToChart(chart1, GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry);
                    AddPointToChart(chart2, GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry, GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry, "Line2");
                }
                // 둘 다 저장
                else if (GlobalValue.Test.Ram2_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) &&
                         GlobalValue.Test.Ram2_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                        )
                {
#if !TEST
                    GlobalValue.Test.Ram2_1_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._39_RAM_2_1_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram2_1_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._40_RAM_2_1_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram2_1_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._42_RAM_2_1_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram2_1_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._43_RAM_2_1_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram2_1_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._44_RAM_2_1_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram2_1_B_Rx = CalcScale(GlobalValue.Test.Ram2_1_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram2_1_B_Ry = CalcScale(GlobalValue.Test.Ram2_1_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram2_1_A_Rx = CalcScale(GlobalValue.Test.Ram2_1_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram2_1_A_Ry = CalcScale(GlobalValue.Test.Ram2_1_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-1 A_Rx=[{0:0.00}] /  2-1 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-1 B_Rx=[{0:0.00}] /  2-1 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry));

                    #region Label 출력 (2-1)
                    // B
                    lblCh5Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_1_B_Rx);
                    lblCh5Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_1_B_Ry);
                    lblCh5Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh5Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh5Value19.Text = string.Format("{0:0.0}", GlobalValue.Test.Ram2_1LoadTestValue);
                    lblCh5Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram2_1LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh6Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_1_A_Rx);
                    lblCh6Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_1_A_Ry);
                    lblCh6Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh6Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_1_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh6Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._15_RAM_2_1_OFFSET] / 10);
                    lblCh6Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh6Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

#if !TEST
                    GlobalValue.Test.Ram2_2_B_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._47_RAM_2_2_RESULT_B_Rx] / 100;
                    GlobalValue.Test.Ram2_2_B_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._48_RAM_2_2_RESULT_B_Ry] / 100;
                    GlobalValue.Test.Ram2_2_A_Rx = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._50_RAM_2_2_RESULT_A_Rx] / 100;
                    GlobalValue.Test.Ram2_2_A_Ry = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._51_RAM_2_2_RESULT_A_Ry] / 100;
                    GlobalValue.Test.Ram2_2_temp = (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._52_RAM_2_2_TEMPERATURE];

                    if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.USE].Trim() == "1")
                    {
                        GlobalValue.Test.Ram2_2_B_Rx = CalcScale(GlobalValue.Test.Ram2_2_B_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Lx])
                                                                );
                        GlobalValue.Test.Ram2_2_B_Ry = CalcScale(GlobalValue.Test.Ram2_2_B_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ly])
                                                                );
                        GlobalValue.Test.Ram2_2_A_Rx = CalcScale(GlobalValue.Test.Ram2_2_A_Rx,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Ux])
                                                                );
                        GlobalValue.Test.Ram2_2_A_Ry = CalcScale(GlobalValue.Test.Ram2_2_A_Ry,
                                                                 Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)),
                                                                 Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Scale_Uy])
                                                                );
                    }
#endif
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-2 A_Rx=[{0:0.00}] /  2-2 A_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry));
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("2-2 B_Rx=[{0:0.00}] /  2-2 B_Ry=[{1:0.00}]", GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry));

                    #region Label 출력 (2-2)
                    // B
                    lblCh7Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_2_B_Rx);
                    lblCh7Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_2_B_Ry);
                    lblCh7Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_B_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[22 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh7Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_B_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[23 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    lblCh7Value19.Text = string.Format("{0:0.0}", GlobalValue.Test.Ram2_2LoadTestValue);
                    lblCh7Value20.Text = string.Format("{0:0.0}", -GlobalValue.Test.Ram2_2LoadTestValue + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                        + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[18 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );

                    // A
                    lblCh8Value15.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_2_A_Rx);
                    lblCh8Value16.Text = string.Format("{0:0.00}", GlobalValue.Test.Ram2_2_A_Ry);
                    lblCh8Value17.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_A_Rx + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[20 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh8Value18.Text = string.Format("{0:0.00}", -GlobalValue.Test.Ram2_2_A_Ry + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                                                                 + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[21 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                                                      );
                    lblCh8Value19.Text = string.Format("{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._16_RAM_2_2_OFFSET] / 10);
                    lblCh8Value20.Text = string.Format("{0:0.0}", Convert.ToDouble(lblCh8Value19.Text) - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[16 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                    #endregion

                    AddPointToChart(chart1, GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry);
                    AddPointToChart(chart1, GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry, GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry, "Line1");

                    AddPointToChart(chart2, GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry);
                    AddPointToChart(chart2, GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry);

                    AddPointToChart3D(GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry, GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry, "Line2");
                }

#if !TEST
                // Parameter - ETC - 제품 없음 하중 값
                if (GlobalValue.Test.Ram1_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram1_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram2_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram2_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                   )
#endif
                {
                    if (LoadTestDataSave(false) == false)
                    {
                        GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_DATA] = true;
                    }

                    // Parameter - ETC - RPQ 데이터
                    // 0 : 저장 안 함
                    // 1 : 저장    함
                    if (GlobalValue.Parameter[(int)e_Parameter.ETC]["2"][(int)e_DF_ETC.Value0].Trim() == "1")
                    {
                        // DOOR OPEN MONITOR == 0
                        if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._58_DOOR_OPEN_MONITOR] == 0)
                        {
                            if (GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.적용] == "1")
                            {
                                #region 2021-10-21 : 조건 수정
                                //double PP하중 = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                                //double MM하중 = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));

                                #region 2022-02-10 : 다운로드된 ++, -- 하중 값 사용

                                double PP하중 = GlobalValue.Test.PP하중;
                                double MM하중 = GlobalValue.Test.MM하중;

                                #endregion

                                double Range = Convert.ToDouble(GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.Range]);

                                double LoadTestValue = GlobalValue.Test.Ram2_1LoadTestValue;
                                if (LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]))
                                {
                                    if ((PP하중 <= LoadTestValue && LoadTestValue <= PP하중 + Range) ||
                                        (MM하중 - Range <= LoadTestValue && LoadTestValue <= MM하중)
                                       )
                                    {
                                        isRPQSave[(int)e_RAM.RAM_2_1] = false;
                                    }
                                }

                                LoadTestValue = GlobalValue.Test.Ram2_2LoadTestValue;
                                if (LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]))
                                {
                                    if ((PP하중 <= LoadTestValue && LoadTestValue <= PP하중 + Range) ||
                                        (MM하중 - Range <= LoadTestValue && LoadTestValue <= MM하중)
                                       )
                                    {
                                        isRPQSave[(int)e_RAM.RAM_2_2] = false;
                                    }
                                }
                                #endregion
                            }

                            if (isRPQSave[(int)e_RAM.RAM_2_1] &&
                                isRPQSave[(int)e_RAM.RAM_2_2]
                               )
                            {
                                if (LoadTestDataSave(true) == false)
                                {
                                    GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_DATA_RPQ] = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private bool LoadTestDataSave(bool isRPQ)
        {
            bool result = false;
            try
            {
                // if 필요 없지만 그냥 유지함
                if (GlobalValue.Test.Ram1_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram1_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram2_1LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) ||
                    GlobalValue.Test.Ram2_2LoadTestValue > Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0])
                   )
                {
                    #region SET OK/NG/-
                    switch (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._21_RAM_1_1_TEST_RESULT])
                    {
                        case 1:   GlobalValue.Test.Ram1_1Result = "OK";  break;
                        case 2:   GlobalValue.Test.Ram1_1Result = "NG";  break;
                        default:  GlobalValue.Test.Ram1_1Result = "-";   break;
                    }

                    switch (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._29_RAM_1_2_TEST_RESULT])
                    {
                        case 1:   GlobalValue.Test.Ram1_2Result = "OK";  break;
                        case 2:   GlobalValue.Test.Ram1_2Result = "NG";  break;
                        default:  GlobalValue.Test.Ram1_2Result = "-";   break;
                    }

                    switch (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._37_RAM_2_1_TEST_RESULT])
                    {
                        case 1:   GlobalValue.Test.Ram2_1Result = "OK";  break;
                        case 2:   GlobalValue.Test.Ram2_1Result = "NG";  break;
                        default:  GlobalValue.Test.Ram2_1Result = "-";   break;
                    }

                    switch (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._45_RAM_2_2_TEST_RESULT])
                    {
                        case 1:   GlobalValue.Test.Ram2_2Result = "OK";  break;
                        case 2:   GlobalValue.Test.Ram2_2Result = "NG";  break;
                        default:  GlobalValue.Test.Ram2_2Result = "-";   break;
                    }
                    #endregion

                    #region SET Var
                    if (GlobalValue.Test.Ram1_1LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) || isRPQSave[(int)e_RAM.RAM_1_1] == false)
                    {
                        GlobalValue.Test.Ram1_1LoadTestValue = 0;
                        GlobalValue.Test.Ram1_1_B_Rx = 0;
                        GlobalValue.Test.Ram1_1_B_Ry = 0;
                        GlobalValue.Test.Ram1_1_A_Rx = 0;
                        GlobalValue.Test.Ram1_1_A_Ry = 0;
                        GlobalValue.Test.Ram1_1Result = "-";
                    }

                    if (GlobalValue.Test.Ram1_2LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) || isRPQSave[(int)e_RAM.RAM_1_2] == false)
                    {
                        GlobalValue.Test.Ram1_2LoadTestValue = 0;
                        GlobalValue.Test.Ram1_2_B_Rx = 0;
                        GlobalValue.Test.Ram1_2_B_Ry = 0;
                        GlobalValue.Test.Ram1_2_A_Rx = 0;
                        GlobalValue.Test.Ram1_2_A_Ry = 0;
                        GlobalValue.Test.Ram1_2Result = "-";
                    }

                    if (GlobalValue.Test.Ram2_1LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) || isRPQSave[(int)e_RAM.RAM_2_1] == false)
                    {
                        GlobalValue.Test.Ram2_1LoadTestValue = 0;
                        GlobalValue.Test.Ram2_1_B_Rx = 0;
                        GlobalValue.Test.Ram2_1_B_Ry = 0;
                        GlobalValue.Test.Ram2_1_A_Rx = 0;
                        GlobalValue.Test.Ram2_1_A_Ry = 0;
                        GlobalValue.Test.Ram2_1Result = "-";
                    }

                    if (GlobalValue.Test.Ram2_2LoadTestValue <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]) || isRPQSave[(int)e_RAM.RAM_2_2] == false)
                    {
                        GlobalValue.Test.Ram2_2LoadTestValue = 0;
                        GlobalValue.Test.Ram2_2_B_Rx = 0;
                        GlobalValue.Test.Ram2_2_B_Ry = 0;
                        GlobalValue.Test.Ram2_2_A_Rx = 0;
                        GlobalValue.Test.Ram2_2_A_Ry = 0;
                        GlobalValue.Test.Ram2_2Result = "-";
                    }
                    #endregion

                    if (isRPQ)
                    {
                        #region 폴더 체크 및 생성
                        string folder = @"C:\RPQ";
                        if (Directory.Exists(folder) == false)
                            Directory.CreateDirectory(folder);
                        #endregion

                        string fileName = string.Format("{0}\\RPQDATA.txt", folder);

                        using (StreamWriter streamWriter = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                        {
                            string value = string.Empty;

                            if (GlobalValue.Parameter[(int)e_Parameter.ETC]["2"][(int)e_DF_ETC.Value2].Trim() == "1")
                            {
                                value = DateTime.Now.ToString("#yyyy-MM-dd HH:mm:ss#");
                                value += string.Format(",{0}", GlobalValue.Test.Actual1);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._85_시험_높이] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._86_PP_하중값] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._87_P__하중값] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._88_M__하중값] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._89_MM_하중값] / 10);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram1_1LoadTestValue, GlobalValue.Test.Ram1_2LoadTestValue, GlobalValue.Test.Ram2_1LoadTestValue, GlobalValue.Test.Ram2_2LoadTestValue);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._90_A_Rx_M_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._91_A_Rx_P_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._92_A_Ry_M_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._93_A_Ry_P_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._94_B_Rx_M_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._95_B_Rx_P_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._96_B_Ry_M_오차값] / 100);
                                value += string.Format(",{0:0.00}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._97_B_Ry_P_오차값] / 100);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry, GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry, GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry, GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry, GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram1_1Result, GlobalValue.Test.Ram1_2Result, GlobalValue.Test.Ram2_1Result, GlobalValue.Test.Ram2_2Result);
                                value += ",,,,,";
                                value += string.Format(",{0}", POP_Open());
                            }
                            else if (GlobalValue.Parameter[(int)e_Parameter.ETC]["2"][(int)e_DF_ETC.Value2].Trim() == "0")
                            {
                                value = DateTime.Now.ToString("#yyyy-MM-dd HH:mm:ss#");
                                value += string.Format(",{0}", GlobalValue.Test.Actual1);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._85_시험_높이] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._86_PP_하중값] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._87_P__하중값] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._88_M__하중값] / 10);
                                value += string.Format(",{0:0.0}", (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._89_MM_하중값] / 10);
                                value += string.Format(",{0},{1},{2},{3}", GlobalValue.Test.Ram1_1LoadTestValue, GlobalValue.Test.Ram1_2LoadTestValue, GlobalValue.Test.Ram2_1LoadTestValue, GlobalValue.Test.Ram2_2LoadTestValue);
                                value += string.Format(",{0}", POP_Open());
                            }

                            streamWriter.WriteLine(value);
                        }

                        result = true;
                    }
                    else
                    {
                        string cmdText = string.Format("INSERT INTO {0}(", e_DBTableList.Result_MC);

                        string[] columns = Enum.GetNames(typeof(e_DBResult_MC));
                        for (int i = 0; i < columns.Length; i++)
                        {
                            cmdText += string.Format("{0}", (e_DBResult_MC)i);
                            if (i < columns.Length - 1)
                            {
                                cmdText += ",";
                            }
                        }
                        cmdText += ") VALUES";

                        string now = DateTime.Now.ToString("yyyyMMddHHmmss");

                        string var = string.Format("('{0}','{1}','{2:0.0}','{3:0.0}','{4:0.0}','{5:0.0}','{6:0.0}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}')",
                                                   now,
                                                   GlobalValue.Test.Actual1,
                                                   (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._85_시험_높이] / 10,
                                                   (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._86_PP_하중값] / 10,
                                                   (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._87_P__하중값] / 10,
                                                   (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._88_M__하중값] / 10,
                                                   (double)GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._89_MM_하중값] / 10,
                                                   GlobalValue.Test.Ram1_1LoadTestValue, GlobalValue.Test.Ram1_2LoadTestValue, GlobalValue.Test.Ram2_1LoadTestValue, GlobalValue.Test.Ram2_2LoadTestValue,
                                                   GlobalValue.Test.Ram1_1_A_Rx, GlobalValue.Test.Ram1_1_A_Ry, GlobalValue.Test.Ram1_1_B_Rx, GlobalValue.Test.Ram1_1_B_Ry,
                                                   GlobalValue.Test.Ram1_2_A_Rx, GlobalValue.Test.Ram1_2_A_Ry, GlobalValue.Test.Ram1_2_B_Rx, GlobalValue.Test.Ram1_2_B_Ry,
                                                   GlobalValue.Test.Ram2_1_A_Rx, GlobalValue.Test.Ram2_1_A_Ry, GlobalValue.Test.Ram2_1_B_Rx, GlobalValue.Test.Ram2_1_B_Ry,
                                                   GlobalValue.Test.Ram2_2_A_Rx, GlobalValue.Test.Ram2_2_A_Ry, GlobalValue.Test.Ram2_2_B_Rx, GlobalValue.Test.Ram2_2_B_Ry,
                                                   GlobalValue.Test.Ram1_1Result, GlobalValue.Test.Ram1_2Result, GlobalValue.Test.Ram2_1Result, GlobalValue.Test.Ram2_2Result,
                                                   GlobalValue.Test.Ram1_1_temp, GlobalValue.Test.Ram1_2_temp, GlobalValue.Test.Ram2_1_temp, GlobalValue.Test.Ram2_2_temp
                                                  );

                        //result = GlobalFunction.DB.MSSQL.Query(cmdText + var);
                        result = GlobalFunction.DB.MySQL.Query(cmdText + var);

                        if (result)
                        {
                            var = var.Replace("(", "");
                            var = var.Replace(")", "");
                            var = var.Replace("'", "");

                            AddDataGridViewRow(dgv_Data, var);

                            if (GlobalValue.Form.FormUxyLxy != null)
                            {
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(0, 0, now, GlobalValue.Test.Ram1_1_A_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(1, 0, now, GlobalValue.Test.Ram1_1_A_Ry);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(2, 0, now, GlobalValue.Test.Ram1_1_B_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(3, 0, now, GlobalValue.Test.Ram1_1_B_Ry);

                                GlobalValue.Form.FormUxyLxy.AddPointToChart(0, 1, now, GlobalValue.Test.Ram1_2_A_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(1, 1, now, GlobalValue.Test.Ram1_2_A_Ry);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(2, 1, now, GlobalValue.Test.Ram1_2_B_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(3, 1, now, GlobalValue.Test.Ram1_2_B_Ry);

                                GlobalValue.Form.FormUxyLxy.AddPointToChart(0, 2, now, GlobalValue.Test.Ram2_1_A_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(1, 2, now, GlobalValue.Test.Ram2_1_A_Ry);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(2, 2, now, GlobalValue.Test.Ram2_1_B_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(3, 2, now, GlobalValue.Test.Ram2_1_B_Ry);

                                GlobalValue.Form.FormUxyLxy.AddPointToChart(0, 3, now, GlobalValue.Test.Ram2_2_A_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(1, 3, now, GlobalValue.Test.Ram2_2_A_Ry);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(2, 3, now, GlobalValue.Test.Ram2_2_B_Rx);
                                GlobalValue.Form.FormUxyLxy.AddPointToChart(3, 3, now, GlobalValue.Test.Ram2_2_B_Ry);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("{0} Data save {1}", (isRPQ ? "RPQ" : GlobalValue.Parameter[(int)e_Parameter.DB]["1"][(int)e_DF_DB.Type]), (result ? "OK" : "NG")));
            return result;
        }

        public enum e_RAM
        {
            RAM_1_1,
            RAM_1_2,
            RAM_2_1,
            RAM_2_2
        }
        private bool[] isRPQSave = new bool[Enum.GetNames(typeof(e_RAM)).Length];

        private string POP_Open()
        {
            string result = ",,";
            try
            {
                string fileName = "C:\\RPQ\\POP.txt";
                if (File.Exists(fileName))
                {
                    using (StreamReader streamReader = new StreamReader(fileName))
                    {
                        result = streamReader.ReadLine();
                    }
                }
                else
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, "POP.txt 파일을 찾을 수 없습니다");
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private double CalcScale(double curr, double spec, double scale)
        {
            double result = curr;
            try
            {
                result = (curr- spec) * scale + spec;
                if (GlobalValue.Parameter[(int)e_Parameter.LoadCell]["1"][(int)e_DF_LoadCell.Logging].Trim() == "1")
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("{0} -> {1}", curr, result));
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        public bool edgeOffsetDownload = false;
        public bool edgeDownload = false;
        public bool edgeUpload = false;
        private Timer timerPLC = null;
        private void TickPLC(object sender, EventArgs e)
        {
            timerPLC?.Stop();
            try
            {
                if (edgeOffsetDownload != isOffsetDownload)
                {
                    edgeOffsetDownload = isOffsetDownload;

                    if (edgeOffsetDownload == false)
                    {
                        if (GlobalValue.Form.FormMdi.toolStripProgressBar1.Value < GlobalValue.Form.FormMdi.toolStripProgressBar1.Maximum)
                        {
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_DOWNLOAD_PARAMETER_OFFSET] = true;
                        }
                        else
                        {
                            if (SaveDownloadFile(true) == false)
                            {
                                GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER_RECIPE_DOWNLOAD] = true;
                            }
                        }
                    }
                }

                if (edgeDownload != isDownload)
                {
                    edgeDownload = isDownload;

                    if (edgeDownload == false)
                    {
                        if (GlobalValue.Form.FormMdi.toolStripProgressBar1.Value < GlobalValue.Form.FormMdi.toolStripProgressBar1.Maximum)
                        {
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_DOWNLOAD_PARAMETER] = true;
                        }
                        else
                        {
                            if (SaveDownloadFile(false) == false)
                            {
                                GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER_RECIPE_DOWNLOAD] = true;
                            }
                        }
                    }
                }

                if (edgeUpload != isUpload)
                {
                    edgeUpload = isUpload;

                    if (edgeUpload == false)
                    {
                        if (GlobalValue.Form.FormMdi.toolStripProgressBar1.Value < GlobalValue.Form.FormMdi.toolStripProgressBar1.Maximum)
                        {
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_UPLOAD_PARAMETER] = true;
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
                timerPLC?.Start();
            }
        }
        private bool SaveDownloadFile(bool isOffset)
        {
            bool result = false;
            try
            {
                if (GlobalFunction.UpdateParameterRecipe(dgv_Recipe))
                {
                    if (Directory.Exists(GlobalValue.Directory.Download) == false)
                        Directory.CreateDirectory(GlobalValue.Directory.Download);

                    string 품번 = GlobalFunction.GetString(dgv_Recipe.Rows[1 - 1].Cells[e_DF_Recipe.Data.ToString()].Value);
                    DateTime now = DateTime.Now;

                    string recipeFileName = string.Empty;
                    if (isOffset)
                    {
                        recipeFileName = string.Format("{0}\\{1}_{2}_{3}_Offset.dat", GlobalValue.Directory.Download, 품번, now.ToString("yyyyMMdd"), now.ToString("HHmmss"));
                    }
                    else
                    {
                        recipeFileName = string.Format("{0}\\{1}_{2}_{3}.dat", GlobalValue.Directory.Download, 품번, now.ToString("yyyyMMdd"), now.ToString("HHmmss"));
                    }

                    if (GlobalFunction.SaveParameter(e_Parameter.Recipe, recipeFileName, false))
                    {
#region 다운로드 내용 기록
                        Dictionary<string, string[]> temp = new Dictionary<string, string[]>();
                        foreach (string key in GlobalValue.Parameter[(int)e_Parameter.Recipe].Keys)
                        {
                            temp.Add(key, GlobalValue.Parameter[(int)e_Parameter.Recipe][key]);
                        }
#endregion

                        if (GlobalFunction.LoadParameter(e_Parameter.Recipe, GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName]))
                        {
                            GlobalFunction.DataGridView.AddRows(dgv_Recipe, e_Parameter.Recipe);

                            int idx = 0;

                            for (int row = 0; row < dgv_Recipe.Rows.Count; row++)
                            {
                                if (temp[(row + 1).ToString()][(int)e_DF_Recipe.Data] != GlobalFunction.GetString(dgv_Recipe.Rows[row].Cells[(int)e_DF_Recipe.Data].Value))
                                {
                                    //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("No.{0} /  DATA BEFORE DOWNLOAD=[{1}] /  DATA AFTER DOWNLOAD=[{2}]", row + 1, GlobalFunction.GetString(dgv_Recipe.Rows[row].Cells[(int)e_DF_Recipe.Data].Value), temp[(row + 1).ToString()][(int)e_DF_Recipe.Data]));
                                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("No.{0} {1} : [{2}] -> [{3}]", row + 1, temp[(row + 1).ToString()][(int)e_DF_Recipe.Name], GlobalFunction.GetString(dgv_Recipe.Rows[row].Cells[(int)e_DF_Recipe.Data].Value), temp[(row + 1).ToString()][(int)e_DF_Recipe.Data]));

                                    dgv_Recipe.Rows[row].Cells[(int)e_DF_Recipe.Data].Value = temp[(row + 1).ToString()][(int)e_DF_Recipe.Data];

                                    //dgv_Recipe.FirstDisplayedCell = dgv_Recipe.Rows[row].Cells[e_DF_Recipe.No.ToString()];

                                    if (idx == 0)
                                    {
                                        idx = row;
                                    }
                                }
                            }

                            dgv_Recipe.FirstDisplayedCell = dgv_Recipe.Rows[idx].Cells[e_DF_Recipe.No.ToString()];

                            //btn_Chart3D_Init_Click(null, null);

                            result = true;
                        }
                        else
                        {
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_LOADING_PARAMETER_RECIPE] = true;
                        }
                    }
                    else
                    {
                        GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER_RECIPE] = true;
                    }
                }
                else
                {
                    GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_UPDATE_PARAMETER_RECIPE] = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private void UpdateMinMax()
        {
            try
            {
                if (dgv_Recipe.Rows.Count > 0)
                {
                    chart3.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)) - 10;
                    chart3.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)) + 10;

                    dateTimePicker1.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    dateTimePicker2.Value = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));
                    txt_LOTNO.Text = "LOTNO ALL";
                    Update_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void StartTimer()
        {
            timerUpdatePLC = new Timer();
            timerUpdatePLC.Tick += new EventHandler(TickUpdatePLC);
            timerUpdatePLC.Interval = 100;
            timerUpdatePLC.Start();

            timerUpdateL = new Timer();
            timerUpdateL.Tick += new EventHandler(TickUpdateL);
            timerUpdateL.Interval = 100;
            timerUpdateL.Start();

            timerUpdateC = new Timer();
            timerUpdateC.Tick += new EventHandler(TickUpdateC);
            timerUpdateC.Interval = 100;
            timerUpdateC.Start();

            timerUpdateB = new Timer();
            timerUpdateB.Tick += new EventHandler(TickUpdateB);
            timerUpdateB.Interval = 100;
            timerUpdateB.Start();

            timerSequence = new Timer();
            timerSequence.Tick += new EventHandler(TickSequence);
            timerSequence.Interval = 100;
            timerSequence.Start();

            timerPLC = new Timer();
            timerPLC.Tick += new EventHandler(TickPLC);
            timerPLC.Interval = 100;
            timerPLC.Start();
        }
        private void StopTimer()
        {
            if (timerUpdatePLC != null)
            {
                if (timerUpdatePLC.Enabled)
                    timerUpdatePLC.Stop();
                timerUpdatePLC.Dispose();
                timerUpdatePLC = null;
            }

            if (timerUpdateL != null)
            {
                if (timerUpdateL.Enabled)
                    timerUpdateL.Stop();
                timerUpdateL.Dispose();
                timerUpdateL = null;
            }

            if (timerUpdateC != null)
            {
                if (timerUpdateC.Enabled)
                    timerUpdateC.Stop();
                timerUpdateC.Dispose();
                timerUpdateC = null;
            }

            if (timerUpdateB != null)
            {
                if (timerUpdateB.Enabled)
                    timerUpdateB.Stop();
                timerUpdateB.Dispose();
                timerUpdateB = null;
            }

            if (timerSequence != null)
            {
                if (timerSequence.Enabled)
                    timerSequence.Stop();
                timerSequence.Dispose();
                timerSequence = null;
            }

            if (timerPLC != null)
            {
                if (timerPLC.Enabled)
                    timerPLC.Stop();
                timerPLC.Dispose();
                timerPLC = null;
            }
        }
#endregion

        public FrmData()
        {
            InitializeComponent();
        }

        private void FrmData_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormData = null;
        }
        private void FrmData_FormClosing(object sender, FormClosingEventArgs e)
        {
            //bool check = false;
            //for (int i = 0; i < dgv_Recipe.Rows.Count; i++)
            //{
            //    if (dgv_Recipe.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Style.BackColor == System.Drawing.Color.PowderBlue)
            //    {
            //        check = true;
            //        break;
            //    }
            //}

            //if (check)
            {
                //if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "파라미터 변경 사항이 발견되었습니다.\r\n\r\n저장하지 않고 종료하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "머신 프로그램을 종료하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            if (e.Cancel == false)
            {
                StopTimer();

                // 3D-Graph 탭 마우스 휠 이벤트 해제
                chart1.MouseWheel -= new MouseEventHandler(Chart1_MouseWheel);
                chart2.MouseWheel -= new MouseEventHandler(Chart2_MouseWheel);
            }
        }
        private void FrmData_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();          //          DataGridView 초기화 (Recipe, B_IN, B_OUT, F_Alarm, R_IN, R_OUT)
            InitializeDataGridView(dgv_Data);  // Graph 탭 DataGridView 초기화 (Data)

            // 3D-Graph 탭 Chart 초기화
            // 2021-07-29 : 아래 button7_Click(null, null); 추가하므로 주석 처리
            // 2021-07-30 : 이름 변경 button7_Click -> InitializeGraph
            //InitializeChart(chart1, 0, 0);
            //InitializeChart(chart2, 0, 0);

            // 3D-Graph 탭 마우스 휠 이벤트 연결
            chart1.MouseWheel += new MouseEventHandler(Chart1_MouseWheel);
            chart2.MouseWheel += new MouseEventHandler(Chart2_MouseWheel);

            // Graph 탭 시간 설정
            dateTimePicker1.Value = DateTime.Now.AddDays(0);
            // dateTimePicker1 시간(yyyyMMdd000000)부터 현재까지 데이터 쿼리 및 출력
            dateTimePicker2.Value = DateTime.Now.AddHours(-1);
            Update_Click(null, null);

            StartTimer();

            ClearLabelText();

            groupBox2.Visible = true; // Offset View

            btn_OffsetReset_Click(null, null);

            btn_Chart3D_Init_Internal.Visible = false;
            btn_Chart3D_DrawLine.Visible = false;
            btn_Chart3D_RemoveLine.Visible = false;

            bool visible = false;

            btn_Chart3D_Init.Visible = visible;
            btn_Chart3D_Default.Visible = visible;
            btn_Chart3D_XY.Visible = visible;
            btn_Chart3D_XZ.Visible = visible;
            btn_Chart3D_YZ.Visible = visible;

            //btn_Chart3D_Init_Click(null, null);

            InitializeGraph(null, null);

            //////////////////////////////////////////////////
            //////////////////////////////////////////////////

#if TEST
            btn_Test11.Visible = true;
            btn_Test12.Visible = true;
            btn_Test1.Visible = true;
            btn_Test21.Visible = true;
            btn_Test22.Visible = true;
            btn_Test2.Visible = true;
            button2.Visible = true; // C

            btn_OffsetReset.Visible = false;
#else
            btn_Test11.Visible = false;
            btn_Test12.Visible = false;
            btn_Test1.Visible = false;
            btn_Test21.Visible = false;
            btn_Test22.Visible = false;
            btn_Test2.Visible = false;
            button2.Visible = false; // C
#endif
        }

        private void ClearLabelText()
        {
            Label label;

            foreach (Control control in groupBox1.Controls)
            {
                if (control is Label)
                {
                    label = control as Label;

                    if (label.Name.ToLower().IndexOf("label") < 0)
                    {
                        label.Text = string.Empty;
                    }
                }
            }

            foreach (Control control in groupBox2.Controls)
            {
                if (control is Label)
                {
                    label = control as Label;

                    if (label.Name.ToLower().IndexOf("label") < 0)
                    {
                        label.Text = string.Empty;
                    }
                }
            }
        }

        private void InitializeDataGridView()
        {
#region SetProperties
            GlobalFunction.DataGridView.SetProperties(dgv_Recipe);

            GlobalFunction.DataGridView.SetProperties(dgv_PLC_F_Alarm);
            GlobalFunction.DataGridView.SetProperties(dgv_PLC_B_IN);
            GlobalFunction.DataGridView.SetProperties(dgv_PLC_B_OUT);

            GlobalFunction.DataGridView.SetProperties(dgv_PLC_R_IN);
            GlobalFunction.DataGridView.SetProperties(dgv_PLC_R_OUT);
#endregion

#region AddColumns
            GlobalFunction.DataGridView.AddColumns(dgv_Recipe, Enum.GetNames(typeof(e_DF_Recipe)));

            GlobalFunction.DataGridView.AddColumns(dgv_PLC_F_Alarm, Enum.GetNames(typeof(e_DGV_PLC1)));
            GlobalFunction.DataGridView.AddColumns(dgv_PLC_B_IN, Enum.GetNames(typeof(e_DGV_PLC1)));
            GlobalFunction.DataGridView.AddColumns(dgv_PLC_B_OUT, Enum.GetNames(typeof(e_DGV_PLC1)));

            GlobalFunction.DataGridView.AddColumns(dgv_PLC_R_IN, Enum.GetNames(typeof(e_DGV_PLC2)));
            GlobalFunction.DataGridView.AddColumns(dgv_PLC_R_OUT, Enum.GetNames(typeof(e_DGV_PLC2)));
#endregion

#region AddRows
            GlobalFunction.DataGridView.AddRows(dgv_Recipe, e_Parameter.Recipe);

            GlobalFunction.DataGridView.AddRows(dgv_PLC_F_Alarm, e_Parameter.PLC_InterfaceF_Alarm);
            GlobalFunction.DataGridView.AddRows(dgv_PLC_B_IN, e_Parameter.PLC_InterfaceB_IN);
            GlobalFunction.DataGridView.AddRows(dgv_PLC_B_OUT, e_Parameter.PLC_InterfaceB_OUT);

            GlobalFunction.DataGridView.AddRows(dgv_PLC_R_IN, e_Parameter.PLC_InterfaceR_IN);
            GlobalFunction.DataGridView.AddRows(dgv_PLC_R_OUT, e_Parameter.PLC_InterfaceR_OUT);
#endregion
        }
        private void InitializeDataGridView(DataGridView dgv)
        {
            GlobalFunction.DoubleBuffered(dgv, true);

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray; // 홀수 번호 행에 적용되는 기본 셀 스타일을 설정

            DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
            defaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultCellStyle.Font = new System.Drawing.Font(e_Font.Tahoma.ToString(), 11);
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
            columnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DimGray;
            columnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            columnHeadersDefaultCellStyle.Font = new System.Drawing.Font(e_Font.Tahoma.ToString(), 11);
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
            try
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // 속도 관련

                string[] columns = Enum.GetNames(typeof(e_DBResult_MC));
                string time = string.Empty;

                bool isGetBackColorLoad = false;
                if (txt_LOTNO.Text == GlobalFunction.GetString(dgv_Recipe.Rows[1 - 1].Cells[e_DF_Recipe.Data.ToString()].Value))
                {
                    isGetBackColorLoad = true;
                }

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

                        if (col == e_DBResult_MC.No_1_LoadValue.ToString() ||
                            col == e_DBResult_MC.No_2_LoadValue.ToString() ||
                            col == e_DBResult_MC.No_3_LoadValue.ToString() ||
                            col == e_DBResult_MC.No_4_LoadValue.ToString()
                           )
                        {
                            dgv.Rows[row].Cells[col].Style.BackColor = System.Drawing.Color.Yellow;

                            if (isGetBackColorLoad)
                            {
                                switch (GetBackColorLoad(Convert.ToDouble(GlobalFunction.GetString(dgv.Rows[row].Cells[col].Value))))
                                {
                                    case 1: dgv.Rows[row].Cells[col].Style.BackColor = System.Drawing.Color.Lime;  break;
                                    case 2: dgv.Rows[row].Cells[col].Style.BackColor = System.Drawing.Color.Red;   break;
                                }
                            }
                        }
                    }
                }

                dgv_Data.FirstDisplayedCell = dgv_Data.Rows[dgv.Rows.Count - 1].Cells["No"];

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void AddDataGridViewRow(DataGridView dgv, string data)
        {
            int count = 300; // default
            try { count = Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["1"][(int)e_DF_ETC.Value0]); }
            catch { }
            if (count > 0)
            {
                if (dgv.Rows.Count >= count)
                {
                    dgv.Rows.Clear();
                }
            }

            try
            {
                string[] columns = data.Split(',');
                string time = string.Empty;

                dgv.Rows.Add();

                dgv.Rows[dgv.Rows.Count - 1].Cells[0].Value = dgv.Rows.Count.ToString();

                for (int i = 1; i < dgv_Data.Columns.Count; i++)
                {
                    if (i == 1) // Time
                    {
                        time = columns[i - 1];
                        time = time.Insert(13 - 1, ":");
                        time = time.Insert(11 - 1, ":");
                        time = time.Insert(9 - 1, " /  ");
                        time = time.Insert(7 - 1, "-");
                        time = time.Insert(5 - 1, "-");

                        dgv.Rows[dgv.Rows.Count - 1].Cells[i].Value = time;
                    }
                    else
                    {
                        dgv.Rows[dgv.Rows.Count - 1].Cells[i].Value = columns[i - 1];
                    }

                    if (i == (int)e_DBResult_MC.No_1_LoadValue + 1 ||
                        i == (int)e_DBResult_MC.No_2_LoadValue + 1 ||
                        i == (int)e_DBResult_MC.No_3_LoadValue + 1 ||
                        i == (int)e_DBResult_MC.No_4_LoadValue + 1
                       )
                    {
                        dgv.Rows[dgv.Rows.Count - 1].Cells[i].Style.BackColor = System.Drawing.Color.Yellow;

                        switch (GetBackColorLoad(Convert.ToDouble(GlobalFunction.GetString(dgv.Rows[dgv.Rows.Count - 1].Cells[i].Value))))
                        {
                            case 1: dgv.Rows[dgv.Rows.Count - 1].Cells[i].Style.BackColor = System.Drawing.Color.Lime;  break;
                            case 2: dgv.Rows[dgv.Rows.Count - 1].Cells[i].Style.BackColor = System.Drawing.Color.Red;   break;
                        }
                    }
                }

                dgv_Data.FirstDisplayedCell = dgv_Data.Rows[dgv.Rows.Count - 1].Cells["No"];

                double value = 0;

                for (int col = (int)e_DBResult_MC.No_1_LoadValue; col <= (int)e_DBResult_MC.No_4_LoadValue; col++)
                {
                    time = time.Replace(" ", "");
                    time = time.Replace("/", "\r\n");

                    try { value = Convert.ToDouble(columns[col]); }
                    catch { value = 0; }

                    AddPointToChartData(chart3, col - (int)e_DBResult_MC.No_1_LoadValue, time, value);
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private int GetBackColorLoad(double value)
        {
            int result = 0; // NONE
            try
            {
                double PP하중 = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                double MM하중 = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));

                if (MM하중 <= value && value <= PP하중)
                {
                    result = 1; // OK
                }
                else if (value < MM하중 ||
                         value > PP하중
                        )
                {
                    result = 2; // NG
                }

                if (value <= Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.ETC]["4"][(int)e_DF_ETC.Value0]))
                {
                    result = 0; // NONE
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }

        private void DataGridView_CellValueChanged_Recipe(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            try
            {
                if (e.ColumnIndex == (int)e_DF_Recipe.Data)
                {
                    if (e.RowIndex >= 0 &&
                        e.RowIndex < dgv.Rows.Count
                       )
                    {
                        switch (GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Type.ToString()].Value))
                        {
                            case "C":
                                string C = GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value);
                                int byteCount = Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Count.ToString()].Value)) * 2;

                                if (C.Length > byteCount)
                                {
                                    dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value = C.Substring(0, byteCount);
                                }
                                break;


                            case "N":
                                double MIN = double.NaN;
                                double MAX = double.NaN;
                                double N = double.NaN;

                                if (double.TryParse(GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.MIN.ToString()].Value), out MIN) &&
                                    double.TryParse(GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.MAX.ToString()].Value), out MAX) &&
                                    double.TryParse(GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value), out N)
                                   )
                                {
                                    if       (N < MIN) N = MIN;
                                    else if  (N > MAX) N = MAX;

                                    dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(N, GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Format.ToString()].Value));
                                }
                                else
                                {
                                    dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value = string.Empty;
                                    //dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.MIN.ToString()].Value);
                                }
                                break;
                        }

                        if ((e.RowIndex % 2) != 0)
                        {
                            dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Style.BackColor = System.Drawing.Color.PaleGoldenrod;
                        }
                        else
                        {
                            dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Style.BackColor = System.Drawing.Color.LightYellow;
                        }

                        if (GlobalFunction.GetString(dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Value) != GlobalValue.Parameter[(int)e_Parameter.Recipe][(e.RowIndex + 1).ToString()][(int)e_DF_Recipe.Data])
                        {
                            dgv.Rows[e.RowIndex].Cells[e_DF_Recipe.Data.ToString()].Style.BackColor = System.Drawing.Color.PowderBlue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void DataGridView_CellMouseUp_PLC_B_OUT(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            try
            {
                if (e.ColumnIndex == (int)e_DGV_PLC1.Value)
                {
                    if (e.RowIndex >= 0 &&
                        e.RowIndex < dgv.Rows.Count
                       )
                    {
                        GlobalPLC.B_OUT.Data[e.RowIndex] = !GlobalPLC.B_OUT.Data[e.RowIndex];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

#region Chart (3D-Graph)
        private void InitializeChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, double refX, double refY)
        {
#region property
            chart.ChartAreas[0].AxisX.Minimum = -MINIMAX;
            chart.ChartAreas[0].AxisX.Maximum =  MINIMAX;

            chart.ChartAreas[0].AxisY.Minimum = -MINIMAX;
            chart.ChartAreas[0].AxisY.Maximum =  MINIMAX;

            chart.ChartAreas[0].AxisX.Interval = 20;
            chart.ChartAreas[0].AxisY.Interval = 20;

#region Zoom
            // ScrollBar.Enabled, ScaleView.Zoomable 기본값이 true이기 때문에 주석 처리

            //chart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            //chart.ChartAreas[0].AxisY.ScrollBar.Enabled = true;

            //chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart.ChartAreas[0].AxisY.ScaleView.ZoomReset();

            zoomFrames1.Clear();
            zoomFrames2.Clear();
#endregion

            chart.Series[0].IsValueShownAsLabel = true;
            chart.Series[0].Label = "(#VALX, #VALY)";
            chart.Series[0].LabelBackColor = System.Drawing.Color.Blue;
            chart.Series[0].LabelForeColor = System.Drawing.Color.Yellow;

            if (chart.Legends.Count > 0)
            {
                chart.Legends.Clear();
            }

            if (chart.Series[0].ChartType != System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point)
            {
                chart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
#endregion

#region 포인트 초기화 후 중점 및 축별 리미트 포인트 추가
            chart.Series[0].Points.Clear();

            chart.Series[0].Points.AddXY(0, 0);

            chart.Series[0].Points.AddXY(0, chart.ChartAreas[0].AxisY.Maximum);
            chart.Series[0].Points.AddXY(0, chart.ChartAreas[0].AxisY.Minimum);

            chart.Series[0].Points.AddXY(chart.ChartAreas[0].AxisX.Minimum, 0);
            chart.Series[0].Points.AddXY(chart.ChartAreas[0].AxisX.Maximum, 0);

            for (int i = 0; i < chart.Series[0].Points.Count; i++)
            {
                chart.Series[0].Points[i].MarkerSize = 0;

                chart.Series[0].Points[i].Color = (i == 0 ? System.Drawing.Color.Lime : System.Drawing.Color.Red);

                chart.Series[0].Points[i].LabelBackColor = System.Drawing.Color.White;
                chart.Series[0].Points[i].LabelForeColor = System.Drawing.Color.White;
            }
#endregion

#region spec 포인트 추가 후 안 보이게끔 마커 사이즈 축소
            chart.Series[0].Points.AddXY(refX, refY);

            chart.Series[0].Points[chart.Series[0].Points.Count - 1].MarkerSize = 0;
#endregion
        }

        private void AddPointToChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, double xValue, double yValue)
        {
            chart.Series[0].Points.AddXY(xValue, yValue);
        }

        private void Chart_Paint(object sender, PaintEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart chart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;

            System.Windows.Forms.DataVisualization.Charting.Axis xAxis = chart.ChartAreas[0].AxisX;
            System.Windows.Forms.DataVisualization.Charting.Axis yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Blue, 2);

#region 원 그리기
                float topX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Minimum);
                float topY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.ChartAreas[0].AxisY.Maximum);
                float botX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Maximum);
                float botY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.ChartAreas[0].AxisY.Minimum);

                System.Drawing.RectangleF rectangleF = System.Drawing.RectangleF.FromLTRB(topX, topY, botX, botY);

                e.Graphics.DrawEllipse(pen, rectangleF);
#endregion

#region 가로선 그리기
                float minX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Minimum);
                float maxX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Maximum);
                float cenY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(0);

                System.Drawing.PointF pt1 = new System.Drawing.PointF(minX, cenY);
                System.Drawing.PointF pt2 = new System.Drawing.PointF(maxX, cenY);

                e.Graphics.DrawLine(pen, pt1, pt2);
#endregion

#region 세로선 그리기
                float cenX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(0);
                float maxY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.ChartAreas[0].AxisY.Maximum);
                float minY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.ChartAreas[0].AxisY.Minimum);

                pt1 = new System.Drawing.PointF(cenX, maxY);
                pt2 = new System.Drawing.PointF(cenX, minY);

                e.Graphics.DrawLine(pen, pt1, pt2);
#endregion

                if (chart.Series[0].Points.Count > 5)
                {
                    if (dgv_Recipe.Rows.Count > 0)
                    {
                        if (chart.Name == "chart1")
                        {
                            // Upper
                            botX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.Series[0].Points[5].XValue     + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[48 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                            topX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.Series[0].Points[5].XValue     - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[48 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                            topY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.Series[0].Points[5].YValues[0] + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[50 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                            botY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.Series[0].Points[5].YValues[0] - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[50 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                        }
                        else
                        {
                            // Lower
                            botX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.Series[0].Points[5].XValue     + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[52 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                            topX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.Series[0].Points[5].XValue     - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[52 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                            topY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.Series[0].Points[5].YValues[0] + Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[54 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                            botY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.Series[0].Points[5].YValues[0] - Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[54 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                        }
                    }
                    else
                    {
                        botX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.Series[0].Points[5].XValue     + 4);
                        topX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.Series[0].Points[5].XValue     - 4);
                        topY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.Series[0].Points[5].YValues[0] + 4);
                        botY = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(chart.Series[0].Points[5].YValues[0] - 4);
                    }

                    rectangleF = System.Drawing.RectangleF.FromLTRB(topX, topY, botX, botY);

                    e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red, 2), topX, topY, Math.Abs(botX - topX), Math.Abs(topY - botY));
                }

                if (chart.Series[0].Points.Count > 6)
                {
                    for (int i = 6; i < chart.Series[0].Points.Count; i++)
                    {
                        chart.Series[0].Points[i].MarkerSize = 5;

                        chart.Series[0].Points[i].Color = System.Drawing.Color.Red;

                        chart.Series[0].Points[i].LabelBackColor = System.Drawing.Color.Black;
                        chart.Series[0].Points[i].LabelForeColor = System.Drawing.Color.Yellow;
                    }
                }
            }
            catch (Exception ex)
            {
                if (GlobalFunction.GetString(ex).Contains("OverflowException"))
                {
                    if (chart.Name == "chart1")
                    {
                        ZoomFrame frame = zoomFrames1.Pop();

                        xAxis.ScaleView.Zoom(frame.XStart, frame.XEnd);
                        yAxis.ScaleView.Zoom(frame.YStart, frame.YEnd);
                    }
                    else
                    {
                        ZoomFrame frame = zoomFrames2.Pop();

                        xAxis.ScaleView.Zoom(frame.XStart, frame.XEnd);
                        yAxis.ScaleView.Zoom(frame.YStart, frame.YEnd);
                    }
                }
                else
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }
            }
        }

#region Zoom
        private class ZoomFrame
        {
            public double XStart { get; set; }
            public double XEnd { get; set; }
            public double YStart { get; set; }
            public double YEnd { get; set; }
        }
        private Stack<ZoomFrame> zoomFrames1 = new Stack<ZoomFrame>();
        private Stack<ZoomFrame> zoomFrames2 = new Stack<ZoomFrame>();

        private const int zoomFactor = 5;

        private void Chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart chart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;

            System.Windows.Forms.DataVisualization.Charting.Axis xAxis = chart.ChartAreas[0].AxisX;
            System.Windows.Forms.DataVisualization.Charting.Axis yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0)
                {
                    if (zoomFrames1.Count > 0)
                    {
                        ZoomFrame frame = zoomFrames1.Pop();

                        xAxis.ScaleView.Zoom(frame.XStart, frame.XEnd);
                        yAxis.ScaleView.Zoom(frame.YStart, frame.YEnd);
                    }
                    else if (zoomFrames1.Count == 0)
                    {
                        xAxis.ScaleView.ZoomReset();
                        yAxis.ScaleView.ZoomReset();
                    }
                }
                else if (e.Delta > 0)
                {
                    //if (zoomFrames1.Count < 5)
                    {
                        double xMin = xAxis.ScaleView.ViewMinimum;
                        double xMax = xAxis.ScaleView.ViewMaximum;
                        double yMin = yAxis.ScaleView.ViewMinimum;
                        double yMax = yAxis.ScaleView.ViewMaximum;

                        zoomFrames1.Push(new ZoomFrame() { XStart = xMin, XEnd = xMax, YStart = yMin, YEnd = yMax });

                        long posXStart  = (long)(xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / zoomFactor);
                        long posXEnd    = (long)(xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / zoomFactor);
                        long posYStart  = (long)(yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / zoomFactor);
                        long posYEnd    = (long)(yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / zoomFactor);

                        xAxis.ScaleView.Zoom(posXStart, posXEnd);
                        yAxis.ScaleView.Zoom(posYStart, posYEnd);
                    }
                }
            }
            catch { }
        }
        private void Chart2_MouseWheel(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart chart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;

            System.Windows.Forms.DataVisualization.Charting.Axis xAxis = chart.ChartAreas[0].AxisX;
            System.Windows.Forms.DataVisualization.Charting.Axis yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0)
                {
                    if (zoomFrames2.Count > 0)
                    {
                        ZoomFrame frame = zoomFrames2.Pop();

                        xAxis.ScaleView.Zoom(frame.XStart, frame.XEnd);
                        yAxis.ScaleView.Zoom(frame.YStart, frame.YEnd);
                    }
                    else if (zoomFrames2.Count == 0)
                    {
                        xAxis.ScaleView.ZoomReset();
                        yAxis.ScaleView.ZoomReset();
                    }
                }
                else if (e.Delta > 0)
                {
                    //if (zoomFrames2.Count < 5)
                    {
                        double xMin = xAxis.ScaleView.ViewMinimum;
                        double xMax = xAxis.ScaleView.ViewMaximum;
                        double yMin = yAxis.ScaleView.ViewMinimum;
                        double yMax = yAxis.ScaleView.ViewMaximum;

                        zoomFrames2.Push(new ZoomFrame() { XStart = xMin, XEnd = xMax, YStart = yMin, YEnd = yMax });

                        long posXStart  = (long)(xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / zoomFactor);
                        long posXEnd    = (long)(xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / zoomFactor);
                        long posYStart  = (long)(yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / zoomFactor);
                        long posYEnd    = (long)(yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / zoomFactor);

                        xAxis.ScaleView.Zoom(posXStart, posXEnd);
                        yAxis.ScaleView.Zoom(posYStart, posYEnd);
                    }
                }
            }
            catch { }
        }
#endregion

        private double MINIMAX = 80;
        private double HEIGHT = 200;

        private void btn_Chart3D_Init_Internal_Click(object sender, EventArgs e)
        {
            Chart3D.InitScene();
        }
        private void InitializeChart3D(object sender, EventArgs e)
        {
            return;

            btn_Chart3D_Init_Internal_Click(null, null);

            Point3D startPoint = new Point3D(-MINIMAX, -MINIMAX, 0);
            int count = 8;
            Chart3D.AddAxis(startPoint, count, (int)MINIMAX / count * 2, count, (int)MINIMAX / count * 2, 4, (int)HEIGHT / 4);

#region Circle
            Point3D downCircle = new Point3D(0, 0, 0);
            Point3D upCircle = new Point3D(0, 0, HEIGHT);

            Chart3D.AddXYCircle(downCircle, MINIMAX, 1, Brushes.Blue);
            Chart3D.AddXYCircle(upCircle, MINIMAX, 1, Brushes.Blue);
#endregion

            //Center Line
            Chart3D.AddLine(downCircle, upCircle, Brushes.Lime, 1);

#region Square
            Point3D downS_P1 = new Point3D(-5, 40, 0);
            Point3D downS_P2 = new Point3D( 5, 40, 0);
            Point3D downS_P3 = new Point3D( 5, 45, 0);
            Point3D downS_P4 = new Point3D(-5, 45, 0);

            Point3D upS_P1 = new Point3D(-5,  0, HEIGHT);
            Point3D upS_P2 = new Point3D( 5,  0, HEIGHT);
            Point3D upS_P3 = new Point3D( 5, -5, HEIGHT);
            Point3D upS_P4 = new Point3D(-5, -5, HEIGHT);

            if (dgv_Recipe.Rows.Count > 0)
            {
                double Lx = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[51 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                double Dx = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[52 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                double Ly = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[53 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                double Dy = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[54 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                downS_P1 = new Point3D(Lx - Dx, Ly - Dy, 0);
                downS_P2 = new Point3D(Lx + Dx, Ly - Dy, 0);
                downS_P3 = new Point3D(Lx + Dx, Ly + Dy, 0);
                downS_P4 = new Point3D(Lx - Dx, Ly + Dy, 0);

         double Ux = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[47 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                Dx = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[48 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
         double Uy = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[49 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                Dy = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[50 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));
                upS_P1 = new Point3D(Ux - Dx, Uy - Dy, HEIGHT);
                upS_P2 = new Point3D(Ux + Dx, Uy - Dy, HEIGHT);
                upS_P3 = new Point3D(Ux + Dx, Uy + Dy, HEIGHT);
                upS_P4 = new Point3D(Ux - Dx, Uy + Dy, HEIGHT);
            }

            Chart3D.AddSquare(downS_P1, downS_P2, downS_P3, downS_P4, Brushes.Red);
            Chart3D.AddSquare(upS_P1, upS_P2, upS_P3, upS_P4, Brushes.Red);
#endregion

            Chart3D.ChangePlane(_3D.UserControl1.e_ViewPlane.Default);
        }
        private void btn_Chart3D_Default_Click(object sender, EventArgs e)
        {
            Chart3D.ChangePlane(_3D.UserControl1.e_ViewPlane.Default);
        }
        private void btn_Chart3D_XY_Click(object sender, EventArgs e)
        {
            Chart3D.ChangePlane(_3D.UserControl1.e_ViewPlane.XY);
        }
        private void btn_Chart3D_XZ_Click(object sender, EventArgs e)
        {
            Chart3D.ChangePlane(_3D.UserControl1.e_ViewPlane.XZ);
        }
        private void btn_Chart3D_YZ_Click(object sender, EventArgs e)
        {
            Chart3D.ChangePlane(_3D.UserControl1.e_ViewPlane.YZ);
        }
        private void btn_Chart3D_RemoveLine_Click(object sender, EventArgs e)
        {
            InitializeChart3D(null, null);
        }
        private void btn_Chart3D_DrawLine_Click(object sender, EventArgs e)
        {
            AddPointToChart3D(0, 43, 0, 0, "Line");
        }

        private void AddPointToChart3D(double Lx, double Ly, double Ux, double Uy, string keyName)
        {
            return;

            Point3D startPoint = new Point3D(Lx, Ly, 0);
            Point3D endPOint = new Point3D(Ux, Uy, HEIGHT);
            Chart3D.AddLine(startPoint, endPOint, Brushes.Blue, 1, keyName);
        }
        #endregion

        #region Chart (2D-Graph) /  History 탭
        private void InitializeChartData(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            #region property
            #region Minimum, Maximum
            chart.ChartAreas[0].AxisY.Minimum = 350;
            chart.ChartAreas[0].AxisY.Maximum = 500;

            if (dgv_Recipe.Rows.Count > 0)
            {
                chart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)) - 10;
                chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)) + 10;
            }

            chart.ChartAreas[0].AxisY.Interval = 5;
            #endregion

            if (chart.Legends.Count > 0)
            {
                chart.Legends.Clear();
            }

            if (chart.Series.Count > 0)
            {
                chart.Series.Clear();

                #region Point
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

                    switch (idx)
                    {
                        case 0: chart.Series[idx].Color = System.Drawing.Color.Blue; break;
                        case 1: chart.Series[idx].Color = System.Drawing.Color.Red; break;
                        case 2: chart.Series[idx].Color = System.Drawing.Color.DeepSkyBlue; break;
                        case 3: chart.Series[idx].Color = System.Drawing.Color.Lime; break;
                    }
                }
                #endregion
            }
            #endregion
        }

        private void AddPointToChartData(System.Windows.Forms.DataVisualization.Charting.Chart chart, int series, string xValue, double yValue)
        {
            var time = xValue;

            chart.Series[series + 0].Points.AddXY(time, yValue);
        }
        #endregion

        #region Button
        private void OffsetView_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = !groupBox2.Visible;
        }

        private void Offset_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = sender as Button;

                //btn_Offset11
                //btn_Offset12
                //btn_Offset21
                //btn_Offset22
                //btn_Offset0
                int idx = Convert.ToInt32(button.Name.Substring(10));

                // 0 : 기존 하중기준
                // 1 :      변위기준
                int mode = ((DataGridViewComboBoxCell)dgv_Recipe.Rows[11 - 1].Cells[e_DF_Recipe.Data.ToString()]).Items.IndexOf(GlobalFunction.GetString(dgv_Recipe.Rows[11 - 1].Cells[e_DF_Recipe.Data.ToString()].Value));

                switch (idx)
                {
                    case 0:
                        dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh2Value17.Text;
                        dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh2Value18.Text;
                        dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh1Value17.Text;
                        dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh1Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[40 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh2Value20.Text;
                                dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[40 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[40 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh1Value20.Text;
                                break;
                        }

                        dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh4Value17.Text;
                        dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh4Value18.Text;
                        dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh3Value17.Text;
                        dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh3Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[41 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh4Value20.Text;
                                dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[41 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[41 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh3Value20.Text;
                                break;
                        }

                        dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh6Value17.Text;
                        dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh6Value18.Text;
                        dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh5Value17.Text;
                        dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh5Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[42 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh6Value20.Text;
                                dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[42 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[42 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh5Value20.Text;
                                break;
                        }

                        dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh8Value17.Text;
                        dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh8Value18.Text;
                        dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh7Value17.Text;
                        dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh7Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[43 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh8Value20.Text;
                                dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[43 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[43 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh7Value20.Text;
                                break;
                        }
                        break;


                    case 11:
                        dgv_Recipe.Rows[57 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh2Value17.Text;
                        dgv_Recipe.Rows[58 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh2Value18.Text;
                        dgv_Recipe.Rows[59 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh1Value17.Text;
                        dgv_Recipe.Rows[60 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh1Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[40 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh2Value20.Text;
                                dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[40 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[40 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[75 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh1Value20.Text;
                                break;
                        }
                        break;


                    case 12:
                        dgv_Recipe.Rows[61 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh4Value17.Text;
                        dgv_Recipe.Rows[62 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh4Value18.Text;
                        dgv_Recipe.Rows[63 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh3Value17.Text;
                        dgv_Recipe.Rows[64 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh3Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[41 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh4Value20.Text;
                                dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[41 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[41 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[76 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh3Value20.Text;
                                break;
                        }
                        break;


                    case 21:
                        dgv_Recipe.Rows[65 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh6Value17.Text;
                        dgv_Recipe.Rows[66 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh6Value18.Text;
                        dgv_Recipe.Rows[67 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh5Value17.Text;
                        dgv_Recipe.Rows[68 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh5Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[42 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh6Value20.Text;
                                dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[42 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[42 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[77 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh5Value20.Text;
                                break;
                        }
                        break;


                    case 22:
                        dgv_Recipe.Rows[69 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh8Value17.Text;
                        dgv_Recipe.Rows[70 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh8Value18.Text;
                        dgv_Recipe.Rows[71 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh7Value17.Text;
                        dgv_Recipe.Rows[72 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh7Value18.Text;

                        switch (mode)
                        {
                            case 0: // 기존 하중기준
                                dgv_Recipe.Rows[43 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh8Value20.Text;
                                dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                break;
                            case 1: //      변위기준
                                dgv_Recipe.Rows[43 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat(0, GlobalFunction.GetString(dgv_Recipe.Rows[43 - 1].Cells[e_DF_Recipe.Format.ToString()].Value));
                                dgv_Recipe.Rows[78 - 1].Cells[e_DF_Recipe.Data.ToString()].Value = lblCh7Value20.Text;
                                break;
                        }
                        break;
                }

                tabControl1.SelectedIndex = 0;
                tabControl3.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
#region Properties
                    dlg.InitialDirectory = GlobalValue.Directory.Recipe;
                    dlg.Filter = "DATA|*.dat|ALL|*.*";
#endregion

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try   { Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("OpenFileDialog (..\\{0})", dlg.FileName.Substring(dlg.FileName.IndexOf("EXE")))); }
                        catch { Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("OpenFileDialog (..\\{0})", dlg.FileName)); }

                        if (GlobalFunction.LoadParameter(e_Parameter.Recipe, dlg.FileName))
                        {
                            GlobalFunction.DataGridView.AddRows(dgv_Recipe, e_Parameter.Recipe);

#region Update System Parameter (LastFileName)
                            //string fileName = Path.GetFileNameWithoutExtension(dlg.FileName);
                            string fileName = dlg.FileName;

                            if (fileName != GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName])
                            {
                                GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName] = fileName;

                                if (GlobalFunction.SaveParameter(e_Parameter.System) == false)
                                {
                                    GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER] = true;
                                }
                            }
                            #endregion

                            //btn_Chart3D_Init_Click(null, null);

                            UpdateMinMax();
                        }
                        else
                        {
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_LOADING_PARAMETER_RECIPE] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
#region 검수 (20210716)
                    string 고객사 = GlobalFunction.GetString(dgv_Recipe.Rows[3 - 1].Cells[e_DF_Recipe.Data.ToString()].Value);
                    string   품명 = GlobalFunction.GetString(dgv_Recipe.Rows[2 - 1].Cells[e_DF_Recipe.Data.ToString()].Value);

                    //if (string.IsNullOrEmpty(고객사) == false &&
                    //    string.IsNullOrEmpty(  품명) == false
                    //   )
                    if (string.IsNullOrEmpty(품명) == false) // 20210925
                    {
                        //string path = string.Format("{0}\\{1}\\{2}", GlobalValue.Directory.Recipe, 고객사, 품명);
                        string path = string.Format("{0}\\{1}", GlobalValue.Directory.Recipe, 품명); // 20210925

                        if (Directory.Exists(path) == false)
                            Directory.CreateDirectory(path);

                        dlg.InitialDirectory = path;

                        string 품번 = GlobalFunction.GetString(dgv_Recipe.Rows[1 - 1].Cells[e_DF_Recipe.Data.ToString()].Value);
                        DateTime now = DateTime.Now;

                        dlg.FileName = 품번;
                    }
#endregion

#region Properties
                    if (string.IsNullOrEmpty(dlg.FileName))
                    {
                        dlg.InitialDirectory = GlobalValue.Directory.Recipe;
                    }
                    dlg.DefaultExt = "dat";
                    dlg.Filter = "DATA|*.dat|ALL|*.*";
#endregion

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try   { Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("SaveFileDialog (..\\{0})", dlg.FileName.Substring(dlg.FileName.IndexOf("EXE")))); }
                        catch { Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("SaveFileDialog (..\\{0})", dlg.FileName)); }

                        if (GlobalFunction.UpdateParameterRecipe(dgv_Recipe))
                        {
                            if (GlobalFunction.SaveParameter(e_Parameter.Recipe, dlg.FileName))
                            {
                                GlobalFunction.DataGridView.AddRows(dgv_Recipe, e_Parameter.Recipe);

#region Update System Parameter (LastFileName)
                                //string fileName = Path.GetFileNameWithoutExtension(dlg.FileName);
                                string fileName = dlg.FileName;

                                if (fileName != GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName])
                                {
                                    GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.LastFileName] = fileName;

                                    if (GlobalFunction.SaveParameter(e_Parameter.System) == false)
                                    {
                                        GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER] = true;
                                    }
                                }
#endregion

                                //btn_Chart3D_Init_Click(null, null);
                            }
                            else
                            {
                                GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_SAVE_PARAMETER_RECIPE] = true;
                            }
                        }
                        else
                        {
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ERROR_UPDATE_PARAMETER_RECIPE] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        public bool isOffsetDownload = false;
        public bool isDownload = false;
        public bool isUpload = false;
        private bool CheckPLC()
        {
            if (isOffsetDownload ||
                isDownload ||
                isUpload
               )
            {
                return false;
            }
            return true;
        }
        private void OffsetDownload_Click(object sender, EventArgs e)
        {
            //if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._17_AUTO_MODE] == 0 &&
            //    GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._18_MANUAL_MODE] == 1
            //   )
            {
                if (CheckPLC())
                {
                    if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "오프셋 파라미터만 PLC로 전송 하시겠습니까?\r\n\r\n※\t전송 데이터는 저장하되,\r\n\t현재 파라미터는 별도 저장이 필요합니다.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        edgeOffsetDownload = isOffsetDownload = true;
                    }
                }
                else
                {
                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "현재 작업이 완료된 후에 재시도 해주세요", MessageBoxButtons.OK);
                }
            }
            //else
            //{
            //    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "수동모드에서 재시도 해주세요", MessageBoxButtons.OK);
            //}
        }
        private void Download_Click(object sender, EventArgs e)
        {
            //if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._17_AUTO_MODE] == 0 &&
            //    GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._18_MANUAL_MODE] == 1
            //   )
            {
                if (CheckPLC())
                {
                    if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "파라미터를 PLC로 전송 하시겠습니까?\r\n\r\n※\t전송 데이터는 저장하되,\r\n\t현재 파라미터는 별도 저장이 필요합니다.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        edgeDownload = isDownload = true;
                    }
                }
                else
                {
                    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "현재 작업이 완료된 후에 재시도 해주세요", MessageBoxButtons.OK);
                }
            }
            //else
            //{
            //    GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "수동모드에서 재시도 해주세요", MessageBoxButtons.OK);
            //}
        }
        private void Upload_Click(object sender, EventArgs e)
        {
            if (CheckPLC())
            {
                if (GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "파라미터를 PC로 불러오시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    edgeUpload = isUpload = true;
                }
            }
            else
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "현재 작업이 완료된 후에 재시도 해주세요", MessageBoxButtons.OK);
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            GlobalValue.Form.FormMdi.종료ToolStripMenuItem_Click(null, null);
        }

        /// <summary>
        /// 사용 안 함
        /// </summary>
        private void Today_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            try
            {
                string time1 = dateTimePicker1.Value.ToString("yyyyMMdd") + dateTimePicker2.Value.ToString("HHmmss");

                string cmdText = string.Format("SELECT * FROM {0} WHERE {1}>='{2}' ", e_DBTableList.Result_MC,
                                                                                      e_DBResult_MC.Time, time1
                                              );

                if (txt_LOTNO.Text.Trim() == string.Empty)
                {
                    txt_LOTNO.Text = "LOTNO ALL";
                }

                if (txt_LOTNO.Text != "LOTNO ALL")
                {
                    cmdText += string.Format("AND {0}='{1}' ", e_DBResult_MC.Number, txt_LOTNO.Text);
                }

                cmdText += string.Format("ORDER BY {0} ASC", e_DBResult_MC.Time);

                //DataTable dt = GlobalFunction.DB.MSSQL.GetDataTable(cmdText);
                DataTable dt = GlobalFunction.DB.MySQL.GetDataTable(cmdText);

                AddDataGridViewColumns(dgv_Data);
                InitializeChartData(chart3);

                if (dt.Rows.Count > 0)
                {
                    AddDataGridViewRows(dgv_Data, dt);

                    string time = string.Empty;
                    double value = 0;

                    for (int row = 0; row < dgv_Data.Rows.Count; row++)
                    {
                        // No 컬럼 때문에 +1
                        for (int col = (int)e_DBResult_MC.No_1_LoadValue + 1; col <= (int)e_DBResult_MC.No_4_LoadValue + 1; col++)
                        {
                            time = GlobalFunction.GetString(dgv_Data.Rows[row].Cells[e_DBResult_MC.Time.ToString()].Value);
                            time = time.Replace(" ", "");
                            time = time.Replace("/", "\r\n");

                            try { value = Convert.ToDouble(GlobalFunction.GetString(dgv_Data.Rows[row].Cells[col].Value)); }
                            catch { value = 0; }

                            AddPointToChartData(chart3, col - ((int)e_DBResult_MC.No_1_LoadValue + 1), time, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        #endregion

        #region Test
        private void Test(object sender, EventArgs e)
        {
            Button button = sender as Button;

            //btn_Test11
            //btn_Test12
            //btn_Test1
            //btn_Test21
            //btn_Test22
            //btn_Test2
            int idx = Convert.ToInt32(button.Name.Substring(8));

            switch (idx)
            {
                case 11:
                case 12:
                case 1:
                    if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._19_RAM_1_TEST_COMPLETED] == 1)
                    {
                        GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._19_RAM_1_TEST_COMPLETED] = 0;

                        btn_Test11.BackColor = System.Drawing.SystemColors.Control;
                        btn_Test12.BackColor = System.Drawing.SystemColors.Control;
                        btn_Test1.BackColor = System.Drawing.SystemColors.Control;

                        ClearTestVar(1);
                    }
                    else
                    {
                        switch (idx)
                        {
                            case 11:
                                GlobalValue.Test.Ram1_1LoadTestValue = 399.4;
                                GlobalValue.Test.Ram1_1_B_Rx = 1.80;
                                GlobalValue.Test.Ram1_1_B_Ry = 37.40;
                                GlobalValue.Test.Ram1_1_A_Rx = 0.70;
                                GlobalValue.Test.Ram1_1_A_Ry = 3.50;

                                btn_Test11.BackColor = System.Drawing.Color.Lime;
                                break;


                            case 12:
                                GlobalValue.Test.Ram1_2LoadTestValue = 399.6;
                                GlobalValue.Test.Ram1_2_B_Rx = 2.00;
                                GlobalValue.Test.Ram1_2_B_Ry = 37.60;
                                GlobalValue.Test.Ram1_2_A_Rx = 0.90;
                                GlobalValue.Test.Ram1_2_A_Ry = 3.70;

                                btn_Test12.BackColor = System.Drawing.Color.Lime;
                                break;


                            case 1:
                                GlobalValue.Test.Ram1_1LoadTestValue = 399.4;
                                GlobalValue.Test.Ram1_1_B_Rx = 1.80;
                                GlobalValue.Test.Ram1_1_B_Ry = 37.40;
                                GlobalValue.Test.Ram1_1_A_Rx = 0.70;
                                GlobalValue.Test.Ram1_1_A_Ry = 3.50;

                                GlobalValue.Test.Ram1_2LoadTestValue = 399.6;
                                GlobalValue.Test.Ram1_2_B_Rx = 2.00;
                                GlobalValue.Test.Ram1_2_B_Ry = 37.60;
                                GlobalValue.Test.Ram1_2_A_Rx = 0.90;
                                GlobalValue.Test.Ram1_2_A_Ry = 3.70;

                                btn_Test1.BackColor = System.Drawing.Color.Lime;
                                break;
                        }

                        GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._19_RAM_1_TEST_COMPLETED] = 1;
                    }
                    break;


                case 21:
                case 22:
                case 2:
                    if (GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._20_RAM_2_TEST_COMPLETED] == 1)
                    {
                        GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._20_RAM_2_TEST_COMPLETED] = 0;

                        btn_Test21.BackColor = System.Drawing.SystemColors.Control;
                        btn_Test22.BackColor = System.Drawing.SystemColors.Control;
                        btn_Test2.BackColor = System.Drawing.SystemColors.Control;

                        ClearTestVar(2);
                    }
                    else
                    {
                        switch (idx)
                        {
                            case 21:
                                GlobalValue.Test.Ram2_1LoadTestValue = 399.4;
                                GlobalValue.Test.Ram2_1_B_Rx = 1.80;
                                GlobalValue.Test.Ram2_1_B_Ry = 37.40;
                                GlobalValue.Test.Ram2_1_A_Rx = 0.70;
                                GlobalValue.Test.Ram2_1_A_Ry = 3.50;

                                btn_Test21.BackColor = System.Drawing.Color.Lime;
                                break;


                            case 22:
                                GlobalValue.Test.Ram2_2LoadTestValue = 399.6;
                                GlobalValue.Test.Ram2_2_B_Rx = 2.00;
                                GlobalValue.Test.Ram2_2_B_Ry = 37.60;
                                GlobalValue.Test.Ram2_2_A_Rx = 0.90;
                                GlobalValue.Test.Ram2_2_A_Ry = 3.70;

                                btn_Test22.BackColor = System.Drawing.Color.Lime;
                                break;


                            case 2:
                                GlobalValue.Test.Ram2_1LoadTestValue = 399.4;
                                GlobalValue.Test.Ram2_1_B_Rx = 1.80;
                                GlobalValue.Test.Ram2_1_B_Ry = 37.40;
                                GlobalValue.Test.Ram2_1_A_Rx = 0.70;
                                GlobalValue.Test.Ram2_1_A_Ry = 3.50;

                                GlobalValue.Test.Ram2_2LoadTestValue = 399.6;
                                GlobalValue.Test.Ram2_2_B_Rx = 2.00;
                                GlobalValue.Test.Ram2_2_B_Ry = 37.60;
                                GlobalValue.Test.Ram2_2_A_Rx = 0.90;
                                GlobalValue.Test.Ram2_2_A_Ry = 3.70;

                                btn_Test2.BackColor = System.Drawing.Color.Lime;
                                break;
                        }

                        GlobalPLC.R_IN.Value[(int)e_PLC_R_IN._20_RAM_2_TEST_COMPLETED] = 1;
                    }
                    break;
            }
        }
#endregion

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            return;

            if (dgv_Recipe.Rows.Count == 0)
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "파라미터 로딩 후 재시도 해주세요", MessageBoxButtons.OK);
                return;
            }

            if (GlobalValue.Form.FormHidden == null)
            {
                GlobalValue.Form.FormHidden = new FrmHidden(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value),
                                                            GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)
                                                           );

                GlobalValue.Form.FormHidden.ShowDialog();
            }
        }

        /// <summary>
        /// Graph 탭 Chart 초기화 (3D, Upper, Lower)
        /// </summary>
        private void InitializeGraph(object sender, EventArgs e)
        {
            InitializeChart();

            InitializeChart3D(null, null);
        }

        private void btn_OffsetReset_Click(object sender, EventArgs e)
        {
            lblCh2Value17.Text = "0.00";
            lblCh2Value18.Text = "0.00";
            lblCh1Value17.Text = "0.00";
            lblCh1Value18.Text = "0.00";
            lblCh2Value20.Text = "0.0";
            lblCh4Value17.Text = "0.00";
            lblCh4Value18.Text = "0.00";
            lblCh3Value17.Text = "0.00";
            lblCh3Value18.Text = "0.00";
            lblCh4Value20.Text = "0.0";
            lblCh6Value17.Text = "0.00";
            lblCh6Value18.Text = "0.00";
            lblCh5Value17.Text = "0.00";
            lblCh5Value18.Text = "0.00";
            lblCh6Value20.Text = "0.0";
            lblCh8Value17.Text = "0.00";
            lblCh8Value18.Text = "0.00";
            lblCh7Value17.Text = "0.00";
            lblCh7Value18.Text = "0.00";
            lblCh8Value20.Text = "0.0";

            lblCh1Value20.Text = "0.0";
            lblCh3Value20.Text = "0.0";
            lblCh5Value20.Text = "0.0";
            lblCh7Value20.Text = "0.0";
        }

        private void chart3_Paint(object sender, PaintEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart chart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;

            try
            {
                float minX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Minimum);
                float maxX = (float)chart.ChartAreas[0].AxisX.ValueToPixelPosition(chart.ChartAreas[0].AxisX.Maximum);

                float PP = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[34 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                float P1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[35 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                float M1 = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[37 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));
                float MM = (float)chart.ChartAreas[0].AxisY.ValueToPixelPosition(Convert.ToDouble(GlobalFunction.GetString(dgv_Recipe.Rows[38 - 1].Cells[e_DF_Recipe.Data.ToString()].Value)));

                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);

                System.Drawing.PointF pt1 = new System.Drawing.PointF(minX, PP);
                System.Drawing.PointF pt2 = new System.Drawing.PointF(maxX, PP);
                e.Graphics.DrawLine(pen, pt1, pt2);

                pt1 = new System.Drawing.PointF(minX, MM);
                pt2 = new System.Drawing.PointF(maxX, MM);
                e.Graphics.DrawLine(pen, pt1, pt2);

                pen = new System.Drawing.Pen(System.Drawing.Color.Blue, 3);

                pt1 = new System.Drawing.PointF(minX, P1);
                pt2 = new System.Drawing.PointF(maxX, P1);
                e.Graphics.DrawLine(pen, pt1, pt2);

                pt1 = new System.Drawing.PointF(minX, M1);
                pt2 = new System.Drawing.PointF(maxX, M1);
                e.Graphics.DrawLine(pen, pt1, pt2);
            }
            catch { }
        }
    }
}
