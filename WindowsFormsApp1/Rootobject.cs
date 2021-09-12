using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 
    /// </summary>
    public class Rootobject
    {
        private List<Column> _columns;
        public List<Column> columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new List<Column>();
                }
                return _columns;
            }
            set { _columns = value; }
        }
    }

    public class Column
    {
        public string field { get; set; }
        public string title { get; set; }
        public int width { get; set; }
    }
}
