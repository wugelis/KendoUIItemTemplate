using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmCreateJsonWizard : Form
    {
        private static string _webApiUri;
        public static string WebApiUri
        {
            get
            {
                return _webApiUri;
            }
            set { _webApiUri = value; }
        }
        private static string _jsonColumns;
        public static string JsonColumns
        {
            get
            {
                return _jsonColumns;
            }
            set { _jsonColumns = value; }
        }
        public frmCreateJsonWizard()
        {
            InitializeComponent();

            frmCreateJsonWizard.WebApiUri = ""; //清空靜態物件，以免 Visual Studio 未關閉時，仍然保留上一次的內容.
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(txtWebApiUrl.Text);

            HttpWebResponse response = null;
            StreamReader sr = null;
            string jsonString = string.Empty;

            try
            {
                response = (HttpWebResponse)httpRequest.GetResponse();
                sr = new StreamReader(response.GetResponseStream());
                jsonString = sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    string.Format("無法取得 JSON 資料，可能 URL 不正確，或是 URL 現在無法使用！.SysInfo={0}", ex.Message), 
                    this.Text, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }
            finally
            {
                if(sr != null)
                {
                    sr.Close();
                }
            }

            JsonSerializer serializer = new JsonSerializer();

            StringReader sr2 = new StringReader(jsonString);
            JsonTextReader reader = new JsonTextReader(sr2);

            try
            {
                DataTable dtJson = new DataTable();

                var obj = (JArray)serializer.Deserialize(reader);
                Rootobject rootJsonObj = new Rootobject();
                foreach (JObject jo in obj)
                {
                    foreach (JProperty jp in jo.Properties())
                    {
                        #region 重組 JSON 物件(for Kendo UI Grid 使用)
                        rootJsonObj.columns.Add(new Column()
                        {
                            field = jp.Name,
                            title = jp.Name.Replace("_", " "),//jp.Value!=null?jp.Value.ToString(Newtonsoft.Json.Formatting.None):"",
                            width = 120
                        });
                        #endregion

                        #region 組 DatTable
                        dtJson.Columns.Add(jp.Name);
                        #endregion

                    }
                    break;
                }

                foreach (JObject jo in obj)
                {
                    DataRow dr = dtJson.NewRow();
                    foreach (JProperty jp in jo.Properties())
                    {
                        dr[jp.Name] = jp.Value != null ? jp.Value.ToString(Newtonsoft.Json.Formatting.None) : "";
                    }
                    dtJson.Rows.Add(dr);
                }

                dataGridView1.DataSource = dtJson;

                string jsonString2 = JsonConvert.SerializeObject(rootJsonObj);
                frmCreateJsonWizard.JsonColumns = jsonString2
                    .TrimStart('{')
                    .TrimEnd('}');
            }
            finally
            {
                reader.Close();
                sr2.Close();
            }
            
            btnCreateJsonColumn.Enabled = true;
        }

        private void btnCreateJsonColumn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
