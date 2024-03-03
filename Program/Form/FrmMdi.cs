using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Reflection;

using LibLog;

namespace Program
{
    public partial class FrmMdi : Form
    {
        #region Thread
        private System.Threading.Thread threadAlarm = null;
        private bool isThreadAlarm = false;
        private void ProcessAlarm()
        {
            bool[] prevF_Alarm = new bool[GlobalPLC.F_Alarm.Count];
            bool[] prevPCAlarm = new bool[GlobalValue.PCAlarm.Length];

            string cmdText = string.Empty;

            while (isThreadAlarm)
            {
                try
                {
                    // PLC
                    for (int i = 0; i < GlobalPLC.F_Alarm.Count; i++)
                    {
                        if (prevF_Alarm[i] != GlobalPLC.F_Alarm.Data[i])
                        {
                            prevF_Alarm[i] = GlobalPLC.F_Alarm.Data[i];

                            if (prevF_Alarm[i])
                            {
                                //ShowAlarm(GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm][(i + 1).ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Address],
                                //          GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm][(i + 1).ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Description]
                                //         );

                                cmdText = string.Format("INSERT INTO {0}({1},{2},{3}) VALUES('{4}','{5}','{6}')", e_DBTableList.Alarm_MC,
                                                                                                                  e_DBAlarm_MC.StartTime,
                                                                                                                  e_DBAlarm_MC.Code,
                                                                                                                  e_DBAlarm_MC.Name,
                                                                                                                  DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                                                                                  GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm][(i + 1).ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Address],
                                                                                                                  GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm][(i + 1).ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Description]
                                                       );

                                GlobalFunction.DB.MySQL.Query(cmdText);
                            }
                            else
                            {
                                cmdText = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3} IS NULL AND {4}='{5}'", e_DBTableList.Alarm_MC,
                                                                                                                    e_DBAlarm_MC.EndTime, DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                                                                                    e_DBAlarm_MC.EndTime,
                                                                                                                    e_DBAlarm_MC.Code, GlobalValue.Parameter[(int)e_Parameter.PLC_InterfaceF_Alarm][(i + 1).ToString()][(int)e_DF_PLC_InterfaceF_Alarm.Address]
                                                       );

                                GlobalFunction.DB.MySQL.Query(cmdText);
                            }
                        }
                    }

                    // PC
                    for (int i = 0; i < GlobalValue.PCAlarm.Length; i++)
                    {
                        if (prevPCAlarm[i] != GlobalValue.PCAlarm[i])
                        {
                            prevPCAlarm[i] = GlobalValue.PCAlarm[i];

                            if (prevPCAlarm[i])
                            {
                                ShowAlarm(string.Format("PC{0}", i),
                                          ((e_PCAlarm)i).ToString().Replace("_", " ")
                                         );

                                cmdText = string.Format("INSERT INTO {0}({1},{2},{3}) VALUES('{4}','{5}','{6}')", e_DBTableList.Alarm_MC,
                                                                                                                  e_DBAlarm_MC.StartTime,
                                                                                                                  e_DBAlarm_MC.Code,
                                                                                                                  e_DBAlarm_MC.Name,
                                                                                                                  DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                                                                                  string.Format("PC{0}", i),
                                                                                                                  ((e_PCAlarm)i).ToString().Replace("_", " ")
                                                       );

                                GlobalFunction.DB.MySQL.Query(cmdText);
                            }
                            else
                            {
                                cmdText = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3} IS NULL AND {4}='{5}'", e_DBTableList.Alarm_MC,
                                                                                                                    e_DBAlarm_MC.EndTime, DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                                                                                    e_DBAlarm_MC.EndTime,
                                                                                                                    e_DBAlarm_MC.Code, string.Format("PC{0}", i)
                                                       );

                                GlobalFunction.DB.MySQL.Query(cmdText);
                            }
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
        public void ShowAlarm(string alarmCode, string alarmName)
        {
            try
            {
                Log.Write("Alarm", string.Format("Code=[{0}] /  Name=[{1}]", alarmCode, alarmName));

                if (GlobalValue.Form.FormAlarm == null)
                {
                    GlobalValue.Form.FormAlarm = new FrmAlarm(alarmCode, alarmName);

                    //GlobalValue.Form.FormAlarm.AlarmCode = alarmCode;
                    //GlobalValue.Form.FormAlarm.AlarmName = alarmName;

                    Invoke(new MethodInvoker(GlobalValue.Form.FormAlarm.Show));
                }
                else
                {
                    GlobalValue.Form.FormAlarm.AlarmCode = alarmCode;
                    GlobalValue.Form.FormAlarm.AlarmName = alarmName;

                    //Invoke(new MethodInvoker(GlobalValue.Form.FormAlarm.BringToFront));
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        private System.Threading.Thread threadAlive = null;
        private bool isThreadAlive = false;
        private void ProcessAlive()
        {
            while (isThreadAlive)
            {
                try
                {
                    if (GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._31_ALIVE] == 0)
                    {
                        GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._31_ALIVE] = 1;
                    }
                    else
                    {
                        GlobalPLC.R_OUT.Value[(int)e_PLC_R_OUT._31_ALIVE] = 0;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private System.Threading.Thread threadPLC = null;
        private bool isThreadPLC = false;
        private void ProcessPLC()
        {
            const short mask = 0x0001;
            short value;

            short[] tmpValue = new short[GlobalPLC.R_IN.Value.Length]; // R_IN 값 전부 순간적으로 0이 되는 이슈가 발견되어 버퍼 적용

            #region if (GlobalValue.Form.FormData != null)
            string szDevice;
            short[] shortArray;
            byte[] byteArray;

            int row;
            #endregion

            while (isThreadPLC)
            {
                try
                {
                    if (GlobalPLC.Status != 0)
                    {
                        GlobalPLC.MX.Close();

                        GlobalPLC.MX.ActLogicalStationNumber = Convert.ToInt32(GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.PLC_No]);

                        GlobalPLC.Status = GlobalPLC.MX.Open();
                    }
                    else
                    {
                        #region F_Alarm
                        GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(GlobalPLC.F_Alarm.StartAddress,
                                                                         GlobalPLC.F_Alarm.Value.Length,
                                                                     out GlobalPLC.F_Alarm.Value[0]
                                                                        );

                        if (GlobalPLC.Status != 0) continue;

                        for (int word = 0; word < GlobalPLC.F_Alarm.Value.Length; word++)
                        {
                            for (int bit = 0; bit < 16; bit++)
                            {
                                GlobalPLC.F_Alarm.Data[word * 16 + bit] = (((GlobalPLC.F_Alarm.Value[word] >> bit) & mask) == mask ? true : false);
                            }
                        }
                        #endregion
                        #region B_IN
                        GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(GlobalPLC.B_IN.StartAddress,
                                                                         GlobalPLC.B_IN.Value.Length,
                                                                     out GlobalPLC.B_IN.Value[0]
                                                                        );

                        if (GlobalPLC.Status != 0) continue;

                        for (int word = 0; word < GlobalPLC.B_IN.Value.Length; word++)
                        {
                            for (int bit = 0; bit < 16; bit++)
                            {
                                GlobalPLC.B_IN.Data[word * 16 + bit] = (((GlobalPLC.B_IN.Value[word] >> bit) & mask) == mask ? true : false);
                            }
                        }
                        #endregion
                        #region B_OUT
                        for (int word = 0; word < GlobalPLC.B_OUT.Value.Length; word++)
                        {
                            value = 0;
                            for (int bit = 0; bit < 16; bit++)
                            {
                                value += (short)((GlobalPLC.B_OUT.Data[word * 16 + bit] ? 1 : 0) * Math.Pow(2, bit));
                            }
                            GlobalPLC.B_OUT.Value[word] = value;
                        }

                        GlobalPLC.Status = GlobalPLC.MX.WriteDeviceBlock2(GlobalPLC.B_OUT.StartAddress,
                                                                          GlobalPLC.B_OUT.Value.Length,
                                                                      ref GlobalPLC.B_OUT.Value[0]
                                                                         );

                        if (GlobalPLC.Status != 0) continue;
                        #endregion
                        #region R_IN
                        // TODO : 추후 정리할 것

                        //GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(GlobalPLC.R_IN.StartAddress,
                        //                                                 GlobalPLC.R_IN.Value.Length,
                        //                                             out GlobalPLC.R_IN.Value[0]
                        //                                                );

                        //if (GlobalPLC.Status != 0) continue;

                        //Array.Clear(tmpValue, 0, GlobalPLC.R_IN.Value.Length);

                        GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(GlobalPLC.R_IN.StartAddress,
                                                                         GlobalPLC.R_IN.Value.Length,
                                                                     out tmpValue[0]
                                                                        );

                        if (GlobalPLC.Status != 0)
                        {
                            //Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("GlobalPLC.Status=[{0}]", GlobalPLC.Status));
                            GlobalValue.PCAlarm[(int)e_PCAlarm.ALARM_PLC_STATUS_R_IN] = true;
                            continue;
                        }

                        // 버퍼 값 중에 0이 아닌 값이 발견되면 업데이트
                        if (Array.Exists(tmpValue, x => x != 0))
                        {
                            Array.Copy(tmpValue, GlobalPLC.R_IN.Value, GlobalPLC.R_IN.Value.Length);
                        }
                        #endregion
                        #region R_OUT
                        GlobalPLC.Status = GlobalPLC.MX.WriteDeviceBlock2(GlobalPLC.R_OUT.StartAddress,
                                                                          GlobalPLC.R_OUT.Value.Length,
                                                                      ref GlobalPLC.R_OUT.Value[0]
                                                                         );

                        if (GlobalPLC.Status != 0) continue;
                        #endregion

                        if (GlobalValue.Form.FormData != null)
                        {
                            DataGridView dgv = GlobalValue.Form.FormData.dgv_Recipe;

                            if (dgv.Rows.Count == 0) continue;

                            #region Actual1
                            row = 1;

                            szDevice = GlobalFunction.GetString(dgv.Rows[row - 1].Cells[e_DF_Recipe.Address.ToString()].Value);
                            shortArray = new short[Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[row - 1].Cells[e_DF_Recipe.Count.ToString()].Value))];
                            GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(szDevice, shortArray.Length, out shortArray[0]);

                            if (GlobalPLC.Status != 0) continue;

                            byteArray = new byte[shortArray.Length * 2];
                            for (int j = 0; j < shortArray.Length; j++)
                            {
                                byteArray[j * 2 + 0] = BitConverter.GetBytes(shortArray[j])[0];
                                byteArray[j * 2 + 1] = BitConverter.GetBytes(shortArray[j])[1];
                            }

                            GlobalValue.Test.Actual1 = Encoding.Default.GetString(byteArray).Trim((char)0x00);
                            #endregion
#if false
                            #region Actual2
                            row = 2;

                            szDevice = GlobalFunction.GetString(dgv.Rows[row - 1].Cells[e_DF_Recipe.Address.ToString()].Value);
                            shortArray = new short[Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[row - 1].Cells[e_DF_Recipe.Count.ToString()].Value))];
                            GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(szDevice, shortArray.Length, out shortArray[0]);

                            if (GlobalPLC.Status != 0) continue;

                            byteArray = new byte[shortArray.Length * 2];
                            for (int j = 0; j < shortArray.Length; j++)
                            {
                                byteArray[j * 2 + 0] = BitConverter.GetBytes(shortArray[j])[0];
                                byteArray[j * 2 + 1] = BitConverter.GetBytes(shortArray[j])[1];
                            }

                            GlobalValue.Test.Actual2 = Encoding.Default.GetString(byteArray).Trim((char)0x00);
                            #endregion
                            #region Actual3
                            row = 3;

                            szDevice = GlobalFunction.GetString(dgv.Rows[row - 1].Cells[e_DF_Recipe.Address.ToString()].Value);
                            shortArray = new short[Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[row - 1].Cells[e_DF_Recipe.Count.ToString()].Value))];
                            GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(szDevice, shortArray.Length, out shortArray[0]);

                            if (GlobalPLC.Status != 0) continue;

                            byteArray = new byte[shortArray.Length * 2];
                            for (int j = 0; j < shortArray.Length; j++)
                            {
                                byteArray[j * 2 + 0] = BitConverter.GetBytes(shortArray[j])[0];
                                byteArray[j * 2 + 1] = BitConverter.GetBytes(shortArray[j])[1];
                            }

                            GlobalValue.Test.Actual3 = Encoding.Default.GetString(byteArray).Trim((char)0x00);
                            #endregion
#endif
                            #region Download 등
                            int intData;
                            string stringData;

                            if (GlobalValue.Form.FormData.isDownload ||
                                GlobalValue.Form.FormData.isUpload
                               )
                            {
                                SetProgressBar(0, dgv.Rows.Count - 1);

                                for (int i = 0; i < dgv.Rows.Count; i++)
                                {
                                    if (GlobalPLC.Status != 0) break;

                                    try
                                    {
                                        szDevice = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Address.ToString()].Value);

                                        switch (GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Value))
                                        {
                                            case "B":
                                                if (GlobalValue.Form.FormData.isDownload)
                                                {
                                                    intData = ((DataGridViewComboBoxCell)dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()]).Items.IndexOf(GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value));

                                                    GlobalPLC.Status = GlobalPLC.MX.SetDevice(szDevice, intData);
                                                }
                                                else if (GlobalValue.Form.FormData.isUpload)
                                                {
                                                    GlobalPLC.Status = GlobalPLC.MX.GetDevice(szDevice, out intData);

                                                    if (intData == 0)
                                                    {
                                                        //dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.MIN.ToString()].Value);
                                                        SetDataGridViewCellValue(dgv, i, e_DF_Recipe.Data.ToString(), GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.MIN.ToString()].Value));
                                                    }
                                                    else if (intData == 1)
                                                    {
                                                        //dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.MAX.ToString()].Value);
                                                        SetDataGridViewCellValue(dgv, i, e_DF_Recipe.Data.ToString(), GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.MAX.ToString()].Value));
                                                    }
                                                    else
                                                    {
                                                        SetDataGridViewCellValue(dgv, i, e_DF_Recipe.Data.ToString(), string.Empty);
                                                    }
                                                }
                                                break;


                                            case "C":
                                                if (GlobalValue.Form.FormData.isDownload)
                                                {
                                                    stringData = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value);

                                                    if (!string.IsNullOrEmpty(stringData))
                                                    {
                                                        if ((stringData.Length % 2) > 0)
                                                        {
                                                            byteArray = new byte[stringData.Length + 1];

                                                            Array.Copy(Encoding.Default.GetBytes(stringData), byteArray, stringData.Length);
                                                        }
                                                        else
                                                        {
                                                            byteArray = Encoding.Default.GetBytes(stringData);
                                                        }

                                                        shortArray = new short[Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Count.ToString()].Value))];

                                                        for (int j = 0; j < byteArray.Length / 2; j++)
                                                        {
                                                            shortArray[j] = BitConverter.ToInt16(byteArray, 2 * j);
                                                        }

                                                        GlobalPLC.Status = GlobalPLC.MX.WriteDeviceBlock2(szDevice, shortArray.Length, ref shortArray[0]);
                                                    }
                                                }
                                                else if (GlobalValue.Form.FormData.isUpload)
                                                {
                                                    shortArray = new short[Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Count.ToString()].Value))];

                                                    GlobalPLC.Status = GlobalPLC.MX.ReadDeviceBlock2(szDevice, shortArray.Length, out shortArray[0]);

                                                    byteArray = new byte[shortArray.Length * 2];

                                                    for (int j = 0; j < shortArray.Length; j++)
                                                    {
                                                        byteArray[2 * j + 0] = BitConverter.GetBytes(shortArray[j])[0];
                                                        byteArray[2 * j + 1] = BitConverter.GetBytes(shortArray[j])[1];
                                                    }

                                                    //dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value = Encoding.Default.GetString(byteArray).Trim((char)0x00);
                                                    SetDataGridViewCellValue(dgv, i, e_DF_Recipe.Data.ToString(), Encoding.Default.GetString(byteArray).Trim((char)0x00));
                                                }
                                                break;


                                            case "N":
                                                if (GlobalValue.Form.FormData.isDownload)
                                                {
                                                    stringData = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value);

                                                    if (stringData.IndexOf(".") >= 0)
                                                    {
                                                        short temp = (short)(double.Parse(stringData) * Math.Pow(10, stringData.Length - stringData.IndexOf(".") - 1));

                                                        GlobalPLC.Status = GlobalPLC.MX.SetDevice(szDevice, temp);
                                                    }
                                                    else
                                                    {
                                                        GlobalPLC.Status = GlobalPLC.MX.SetDevice(szDevice, Convert.ToInt16(stringData));
                                                    }
                                                }
                                                else if (GlobalValue.Form.FormData.isUpload)
                                                {
                                                    GlobalPLC.Status = GlobalPLC.MX.GetDevice(szDevice, out intData);

                                                    // int 데이터를 short형으로 변환 필요
                                                    //dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value = GlobalFunction.ConvertFormat((short)intData, GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Format.ToString()].Value));
                                                    SetDataGridViewCellValue(dgv, i, e_DF_Recipe.Data.ToString(), GlobalFunction.ConvertFormat((short)intData, GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Format.ToString()].Value)));
                                                }
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                                    }

                                    SetProgressBarValue(i);
                                }

                                if (GlobalValue.Form.FormData.isDownload)
                                    GlobalValue.Form.FormData.isDownload = false;

                                if (GlobalValue.Form.FormData.isUpload)
                                    GlobalValue.Form.FormData.isUpload = false;
                            }
                            else if (GlobalValue.Form.FormData.isOffsetDownload)
                            {
                                int idxS = 40 - 1;
                                int idxE = 78 - 1; // 2021-10-19 : 하중 옵셋까지 전송

                                SetProgressBar(idxS, idxE);

                                for (int i = idxS; i <= idxE; i++)
                                {
                                    if (GlobalPLC.Status != 0) break;

                                    try
                                    {
                                        szDevice = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Address.ToString()].Value);

                                        switch (GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Type.ToString()].Value))
                                        {
                                            case "B":
                                                intData = ((DataGridViewComboBoxCell)dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()]).Items.IndexOf(GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value));

                                                GlobalPLC.Status = GlobalPLC.MX.SetDevice(szDevice, intData);
                                                break;


                                            case "C":
                                                stringData = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value);

                                                if (!string.IsNullOrEmpty(stringData))
                                                {
                                                    if ((stringData.Length % 2) > 0)
                                                    {
                                                        byteArray = new byte[stringData.Length + 1];

                                                        Array.Copy(Encoding.Default.GetBytes(stringData), byteArray, stringData.Length);
                                                    }
                                                    else
                                                    {
                                                        byteArray = Encoding.Default.GetBytes(stringData);
                                                    }

                                                    shortArray = new short[Convert.ToInt32(GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Count.ToString()].Value))];

                                                    for (int j = 0; j < byteArray.Length / 2; j++)
                                                    {
                                                        shortArray[j] = BitConverter.ToInt16(byteArray, 2 * j);
                                                    }

                                                    GlobalPLC.Status = GlobalPLC.MX.WriteDeviceBlock2(szDevice, shortArray.Length, ref shortArray[0]);
                                                }
                                                break;


                                            case "N":
                                                stringData = GlobalFunction.GetString(dgv.Rows[i].Cells[e_DF_Recipe.Data.ToString()].Value);

                                                if (stringData.IndexOf(".") >= 0)
                                                {
                                                    short temp = (short)(double.Parse(stringData) * Math.Pow(10, stringData.Length - stringData.IndexOf(".") - 1));

                                                    GlobalPLC.Status = GlobalPLC.MX.SetDevice(szDevice, temp);
                                                }
                                                else
                                                {
                                                    GlobalPLC.Status = GlobalPLC.MX.SetDevice(szDevice, Convert.ToInt16(stringData));
                                                }
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
                                    }

                                    SetProgressBarValue(i);
                                }

                                GlobalValue.Form.FormData.isOffsetDownload = false;
                            }
                            #endregion

                            #region 2022-02-10 : 다운로드된 ++, -- 하중 값 사용

                            szDevice = "R3081";
                            GlobalPLC.Status = GlobalPLC.MX.GetDevice(szDevice, out intData);
                            GlobalValue.Test.PP하중 = Convert.ToDouble(GlobalFunction.ConvertFormat((short)intData, "0.0"));

                            szDevice = "R3085";
                            GlobalPLC.Status = GlobalPLC.MX.GetDevice(szDevice, out intData);
                            GlobalValue.Test.MM하중 = Convert.ToDouble(GlobalFunction.ConvertFormat((short)intData, "0.0"));

                            //Log.Write("chk", string.Format("PP하중=[{0}] /  MM하중=[{1}]", GlobalValue.Test.PP하중, GlobalValue.Test.MM하중));

                            #endregion
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
        private void SetProgressBar(int min, int max)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate () { SetProgressBar(min, max); }));
                }
                else
                {
                    toolStripProgressBar1.Minimum = min;
                    toolStripProgressBar1.Maximum = max;

                    toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
                }
            }
            catch { }
        }
        private void SetProgressBarValue(int value)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate () { SetProgressBarValue(value); }));
                }
                else
                {
                    toolStripProgressBar1.Value = value;
                }
            }
            catch { }
        }
        private void SetDataGridViewCellValue(DataGridView dgv, int row, string column, string value)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate () { SetDataGridViewCellValue(dgv, row, column, value); }));
                }
                else
                {
                    dgv.Rows[row].Cells[column].Value = value;
                }
            }
            catch { }
        }

        private void StartThread()
        {
            threadAlarm = new System.Threading.Thread(ProcessAlarm);
            threadAlarm.IsBackground = true;
            isThreadAlarm = true;
            threadAlarm.Start();

            threadAlive = new System.Threading.Thread(ProcessAlive);
            threadAlive.IsBackground = true;
            isThreadAlive = true;
            threadAlive.Start();

            threadPLC = new System.Threading.Thread(ProcessPLC);
            threadPLC.IsBackground = true;
            isThreadPLC = true;
            threadPLC.Start();
        }
        private void StopThread()
        {
            if (isThreadAlarm)
                isThreadAlarm = false;

            if (isThreadAlive)
                isThreadAlive = false;

            if (isThreadPLC)
                isThreadPLC = false;
        }
        #endregion

        #region Timer
        private Timer timerUpdate = null;
        private void TickUpdate(object sender, EventArgs e)
        {
            timerUpdate?.Stop();
            try
            {
                Text = GlobalValue.Parameter[(int)e_Parameter.System]["1"][(int)e_DF_System.Name];

                toolStripStatusLabel1.BackColor = (GlobalPLC.Status == 0 ? Color.Lime : Color.Red);
                toolStripStatusLabel2.Text = string.Format("{0:0.00} %", (double)toolStripProgressBar1.Value / toolStripProgressBar1.Maximum * 100);
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            finally
            {
                timerUpdate?.Start();
            }
        }

        private void StartTimer()
        {
            timerUpdate = new Timer();
            timerUpdate.Tick += new EventHandler(TickUpdate);
            timerUpdate.Interval = 100;
            timerUpdate.Start();
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
        }
        #endregion

        public FrmMdi()
        {
            InitializeComponent();
        }

        private void FrmMdi_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Dispose();
        }
        private void FrmMdi_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel == false)
            {
                StopTimer();

                StopThread();

                GlobalDevice.Stop();
            }
        }
        private void FrmMdi_Load(object sender, EventArgs e)
        {
            if (GlobalFunction.CheckProcess(Process.GetCurrentProcess().ProcessName))
            {
                GlobalValue.Form.FormMdi = this;

                //logToolStripMenuItem_Click(null, null);

                Log.MsgEvent += new Log.MsgEventHandler(LogMsgEvent);

                if (GlobalFunction.LoadParameter())
                {
                    if (GlobalDevice.Start())
                    {
                        StartThread();

                        StartTimer();

                        제품데이터ToolStripMenuItem_Click(null, null);

                        if (GlobalValue.Parameter[(int)e_Parameter.ETC]["3"][(int)e_DF_ETC.Value0].Trim() == "1")
                        {
                            축값모니터링ToolStripMenuItem_Click(null, null);
                        }
                    }
                    else
                    {
                        ShowAlarm(string.Format("PC{0}", (int)e_PCAlarm.ERROR_START_DEVICE),
                                  e_PCAlarm.ERROR_START_DEVICE.ToString().Replace("_", " ")
                                 );
                    }
                }
                else
                {
                    ShowAlarm(string.Format("PC{0}", (int)e_PCAlarm.ERROR_LOADING_PARAMETER),
                              e_PCAlarm.ERROR_LOADING_PARAMETER.ToString().Replace("_", " ")
                             );
                }
            }
            else
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "프로그램이 이미 실행 중입니다", MessageBoxButtons.OK);

                종료ToolStripMenuItem_Click(null, null);
            }
        }

        private void LogMsgEvent(Log.Msg msg)
        {
            listViewLog1.AddListViewItem(msg);
        }

        private void splitter1_DoubleClick(object sender, EventArgs e)
        {
            panel1.Height = 200;
        }
        private void splitter1_MouseUp(object sender, MouseEventArgs e)
        {
            panel1.Height = panel1.Height - e.Location.Y;
        }

        private void CloseMdiChildren()
        {
            foreach (Form form in MdiChildren)
            {
                form.Close();
            }
        }

        #region MenuStrip
        public void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 제품데이터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalValue.Form.FormData != null)
            {
                return;
            }

            CloseMdiChildren();

            GlobalValue.Form.FormData = new FrmData();
            GlobalValue.Form.FormData.MdiParent = this;
            GlobalValue.Form.FormData.WindowState = FormWindowState.Maximized;
            GlobalValue.Form.FormData.Show();
        }

        private void 데이터검색ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalValue.Form.FormSearchData == null)
            {
                GlobalValue.Form.FormSearchData = new FrmSearchData();

                GlobalValue.Form.FormSearchData.Show();
            }
            else
            {
                GlobalValue.Form.FormSearchData.BringToFront();
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logToolStripMenuItem.Checked = !logToolStripMenuItem.Checked;

            panel1.Visible = logToolStripMenuItem.Checked;

            if (panel1.Visible)
            {
                splitter1_DoubleClick(null, null);
            }
        }

        private void parameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalValue.Form.FormParameter == null)
            {
                GlobalValue.Form.FormParameter = new FrmParameter();

                GlobalValue.Form.FormParameter.ShowDialog();
            }
        }
        #endregion

        private void btn_ClearLog_Click(object sender, EventArgs e)
        {
            listViewLog1.Items.Clear();
        }

        private void alarmHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalValue.Form.FormAlarmHistory == null)
            {
                GlobalValue.Form.FormAlarmHistory = new FrmAlarmHistory();

                GlobalValue.Form.FormAlarmHistory.Show();
            }
            else
            {
                GlobalValue.Form.FormAlarmHistory.BringToFront();
            }
        }

        private void 축값모니터링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalValue.Form.FormUxyLxy == null)
            {
                GlobalValue.Form.FormUxyLxy = new FrmUxyLxy();

                GlobalValue.Form.FormUxyLxy.Show();
            }
            else
            {
                GlobalValue.Form.FormUxyLxy.BringToFront();
            }
        }

        private void recipe변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalValue.Form.FormConverter == null)
            {
                GlobalValue.Form.FormConverter = new FrmConverter();

                GlobalValue.Form.FormConverter.Show();
            }
            else
            {
                GlobalValue.Form.FormConverter.BringToFront();
            }
        }
    }
}
