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
    public partial class FrmConverter : Form
    {
        public FrmConverter()
        {
            InitializeComponent();
        }

        private void FrmConverter_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalValue.Form.FormConverter = null;
        }
        private void FrmConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void FrmConverter_Load(object sender, EventArgs e)
        {
            Location = new Point(25, 75);
        }

        private Dictionary<string, string[]> prevRecipe = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> currRecipe = new Dictionary<string, string[]>();

        /// <summary>
        /// 파일 선택
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.InitialDirectory = @"C:\DAECO\2. Machine";
                    dlg.Filter = "LOD|*.lod|ALL|*.*";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        lbl_SelectedFileName.Text = dlg.FileName;

                        lbl_ResultFileName.Text = string.Empty;
                    }
                    else
                    {
                        lbl_SelectedFileName.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }

        /// <summary>
        /// 변환
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbl_SelectedFileName.Text))
            {
                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, "Recipe 파일 선택 후 재시도 해주세요", MessageBoxButtons.OK);
                return;
            }

            try
            {
                if (GetPrevRecipe(lbl_SelectedFileName.Text))
                {
                    if (SetCurrRecipe())
                    {
                        lbl_ResultFileName.Text = SaveRecipe();
                    }
                    else
                    {
                    }
                }
                else
                {
                }

                GlobalFunction.MessageBox(MethodBase.GetCurrentMethod().Name, string.Format("Recipe 변환 {0}", (lbl_ResultFileName.Text != string.Empty ? "OK" : "NG")), MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
        }
        private bool GetPrevRecipe(string fileName)
        {
            bool result = false;
            try
            {
                if (File.Exists(fileName))
                {
                    prevRecipe.Clear();

                    using (StreamReader streamReader = new StreamReader(fileName, Encoding.Default))
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
                                prevRecipe.Add(vs[0], vs);
                            }
                        }
                    }

                    result = (prevRecipe.Count > 0 ? true : false);
                }
                else
                {
                    Log.Write(MethodBase.GetCurrentMethod().Name, string.Format("{0} 파일을 찾을 수 없습니다", fileName));
                }
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }
        private bool SetCurrRecipe()
        {
            bool result = false;
            try
            {
                #region step.1
                currRecipe.Clear();

                foreach (string key in GlobalValue.Parameter[(int)e_Parameter.Recipe].Keys)
                {
                    currRecipe.Add(key, GlobalValue.Parameter[(int)e_Parameter.Recipe][key]);
                }
                #endregion

                #region step.2
                foreach (string key in prevRecipe.Keys)
                {
                    switch (key)
                    {
                        case "1": currRecipe["1"][(int)e_DF_Recipe.Data] = prevRecipe[key][2]; break;
                        case "2": currRecipe["2"][(int)e_DF_Recipe.Data] = prevRecipe[key][2]; break;
                        case "3": currRecipe["3"][(int)e_DF_Recipe.Data] = prevRecipe[key][2]; break;
                        case "6": currRecipe["12"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["12"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "7": currRecipe["13"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["13"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "8": currRecipe["7"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["7"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "9": currRecipe["8"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["8"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "10": currRecipe["9"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["9"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "11": currRecipe["10"][(int)e_DF_Recipe.Data] = prevRecipe[key][2]; break;
                        case "12": currRecipe["15"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["15"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "13": currRecipe["16"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["16"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "15": currRecipe["14"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["14"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "16": currRecipe["5"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["5"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "18": currRecipe["40"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["40"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "19": currRecipe["41"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["41"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "20": currRecipe["42"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["42"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "21": currRecipe["43"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["43"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "25": currRecipe["93"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["93"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "26": currRecipe["94"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["94"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "30": currRecipe["34"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["34"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "31": currRecipe["35"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["35"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "33": currRecipe["37"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["37"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "34": currRecipe["38"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["38"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "42": currRecipe["6"][(int)e_DF_Recipe.Data] = prevRecipe[key][2]; break;
                        case "43": currRecipe["26"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["26"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "46": currRecipe["96"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["96"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "47": currRecipe["97"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["97"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "48": currRecipe["98"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["98"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "49": currRecipe["99"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["99"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "50": currRecipe["100"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["100"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "51": currRecipe["101"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["101"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "52": currRecipe["102"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["102"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "53": currRecipe["103"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["103"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "54": currRecipe["104"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["104"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "55": currRecipe["105"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["105"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "56": currRecipe["106"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["106"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "57": currRecipe["107"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["107"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "58": currRecipe["108"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["108"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "59": currRecipe["109"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["109"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "60": currRecipe["110"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["110"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "61": currRecipe["111"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["111"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "62": currRecipe["112"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["112"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "63": currRecipe["113"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["113"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "64": currRecipe["114"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["114"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "65": currRecipe["115"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["115"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "66": currRecipe["116"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToInt32(prevRecipe[key][2]), currRecipe["116"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "67": currRecipe["117"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["117"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "68": currRecipe["118"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["118"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "69": currRecipe["119"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["119"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "70": currRecipe["120"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["120"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "71": currRecipe["121"][(int)e_DF_Recipe.Data] = prevRecipe[key][2]; break;
                        case "79": currRecipe["18"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["18"][(int)e_DF_Recipe.Format]).ToString(); break;
                        case "80": currRecipe["19"][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(Convert.ToDouble(prevRecipe[key][2]), currRecipe["19"][(int)e_DF_Recipe.Format]).ToString(); break;
                    }
                }
                #endregion

                #region step.3
                foreach (string key in currRecipe.Keys)
                {
                    switch (key)
                    {
                        case "5": // [하중시험] - 시험 지연 시간
                            currRecipe[key][(int)e_DF_Recipe.Data] = "500";
                            break;
                        case "7": // [하중시험] - 오리엔테이션 토오크%
                            currRecipe[key][(int)e_DF_Recipe.Data] = "30";
                            break;
                        case "8": // [하중시험] - 오리엔테이션 속도
                            currRecipe[key][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(5.0, currRecipe[key][(int)e_DF_Recipe.Format]).ToString();
                            break;


                        case "20": // [마스터 샘플] - Ux
                        case "21": // [마스터 샘플] - Uy
                        case "22": // [마스터 샘플] - Lx
                        case "23": // [마스터 샘플] - Ly
                            currRecipe[key][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(0.0, currRecipe[key][(int)e_DF_Recipe.Format]).ToString();
                            break;


                        case "25": // 오리엔테이션 시작권수
                            currRecipe[key][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(0.6, currRecipe[key][(int)e_DF_Recipe.Format]).ToString();
                            break;


                        case "27": // 역회전 상승
                        case "28": // 정지권 확인선택
                        case "29": // 정지권
                            currRecipe[key][(int)e_DF_Recipe.Data] = currRecipe[key][(int)e_DF_Recipe.MIN];
                            break;
                        case "30": // 범위 +-
                            currRecipe[key][(int)e_DF_Recipe.Data] = "0.05";
                            break;


                        case "31": // 상측 시트판 두께
                            currRecipe[key][(int)e_DF_Recipe.Data] = "79.0";
                            break;
                        case "32": // 하측 시트판 두께
                            currRecipe[key][(int)e_DF_Recipe.Data] = "44.0";
                            break;


                        case "40": // 램1-1 위치 옵셋
                        case "41": // 램1-2 위치 옵셋
                        case "42": // 램2-1 위치 옵셋
                        case "43": // 램2-2 위치 옵셋
                            currRecipe[key][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(0.0, currRecipe[key][(int)e_DF_Recipe.Format]).ToString();
                            break;


                        case "46": // [6축 데이타] - 6축시험선택
                            currRecipe[key][(int)e_DF_Recipe.Data] = currRecipe[key][(int)e_DF_Recipe.MAX];
                            break;
                        case "47": // [6축 데이타] - Ux
                        case "48": // [6축 데이타] - +-
                        case "49": // [6축 데이타] - Uy
                        case "50": // [6축 데이타] - +-
                        case "51": // [6축 데이타] - Lx
                        case "52": // [6축 데이타] - +-
                        case "53": // [6축 데이타] - Ly
                        case "54": // [6축 데이타] - +-
                            currRecipe[key][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(0.0, currRecipe[key][(int)e_DF_Recipe.Format]).ToString();
                            break;


                        case "57": // [6축 옵셋] - 1-1 Ux 옵셋
                        case "58": // [6축 옵셋] - 1-1 Uy 옵셋
                        case "59": // [6축 옵셋] - 1-1 Lx 옵셋
                        case "60": // [6축 옵셋] - 1-1 Ly 옵셋
                        case "61": // [6축 옵셋] - 1-2 Ux 옵셋
                        case "62": // [6축 옵셋] - 1-2 Uy 옵셋
                        case "63": // [6축 옵셋] - 1-2 Lx 옵셋
                        case "64": // [6축 옵셋] - 1-2 Ly 옵셋
                        case "65": // [6축 옵셋] - 2-1 Ux 옵셋
                        case "66": // [6축 옵셋] - 2-1 Uy 옵셋
                        case "67": // [6축 옵셋] - 2-1 Lx 옵셋
                        case "68": // [6축 옵셋] - 2-1 Ly 옵셋
                        case "69": // [6축 옵셋] - 2-2 Ux 옵셋
                        case "70": // [6축 옵셋] - 2-2 Uy 옵셋
                        case "71": // [6축 옵셋] - 2-2 Lx 옵셋
                        case "72": // [6축 옵셋] - 2-2 Ly 옵셋
                            currRecipe[key][(int)e_DF_Recipe.Data] = GlobalFunction.ConvertFormat(0.0, currRecipe[key][(int)e_DF_Recipe.Format]).ToString();
                            break;
                    }
                }
                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            return result;
        }
        private string SaveRecipe()
        {
            bool result = false;
            string path = string.Empty;
            try
            {
                path = string.Format("{0}\\{1}", GlobalValue.Directory.Recipe, currRecipe["2"][(int)e_DF_Recipe.Data]); // 품명

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                path += string.Format("\\{0}.dat", currRecipe["1"][(int)e_DF_Recipe.Data]); // 품번

                if (File.Exists(path))
                    File.Delete(path);

                using (StreamWriter streamWriter = new StreamWriter(new FileStream(path, FileMode.Append), Encoding.UTF8))
                {
                    streamWriter.WriteLine("==================================================");
                    streamWriter.WriteLine(string.Format("Recipe ({0})", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    streamWriter.WriteLine("==================================================");

                    string[] vs = GlobalFunction.GetDataFormat(e_Parameter.Recipe);

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
                    for (int i = 1; i <= currRecipe.Count; i++)
                    {
                        for (int j = 0; j < vs.Length; j++)
                        {
                            line += currRecipe[i.ToString()][j];
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
            catch (Exception ex)
            {
                Log.Write(MethodBase.GetCurrentMethod().Name, GlobalFunction.GetString(ex));
            }
            string log = string.Format("{0} save {1}", e_Parameter.Recipe, (result ? "OK" : "NG"));
            if (result)
            {
                log += string.Format(" (..\\{0})", path);
            }
            Log.Write(MethodBase.GetCurrentMethod().Name, log);
            return (result ? path : string.Empty);
        }
    }
}
