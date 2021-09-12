using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using System.Windows.Forms;

namespace MyItemTemplateVSIX1
{
    /// <summary>
    /// 
    /// </summary>
    public class MyItemWizard : IWizard
    {
        private DTE _dte;
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            
        }

        public void ProjectFinishedGenerating(Project project)
        {
            
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            
        }

        public void RunFinished()
        {
            
        }

        /// <summary>
        /// (RunStart事件)開始產生新的專案.
        /// </summary>
        /// <param name="automationObject">目前執行的 Visual Studio 的執行個體.</param>
        /// <param name="replacementsDictionary">專案的所有文字檔案內容</param>
        /// <param name="runKind">指定會定義範本精靈可建立之不同範本的常數</param>
        /// <param name="customParams"></param>
        public void RunStarted(
            object automationObject, 
            Dictionary<string, string> replacementsDictionary, 
            WizardRunKind runKind, 
            object[] customParams)
        {
            _dte = (DTE)automationObject;

            frmCreateJsonWizard createJsonForm = new frmCreateJsonWizard();
            DialogResult result = createJsonForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                string webApiUrl = frmCreateJsonWizard.WebApiUri;
                string jsonColumnString = frmCreateJsonWizard.JsonColumns;

                replacementsDictionary.Add("$apiHost$", webApiUrl);
                replacementsDictionary.Add("$jsonColumns$", jsonColumnString);
            }
            else
            {
                throw new WizardCancelledException("使用者取消操作！");
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
