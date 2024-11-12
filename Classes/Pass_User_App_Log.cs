using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.Classes
{
    public class Pass_User_App_Log    {        
        public string CompanyID { get; set; }
        public string UserID { get; set; }
        public int LogLineNumber { get; set; }
        public DateTime LogDate { get; set; }
        public string LogType { get; set; }
        public string LogDescription { get; set; }
        public string PLATNEDRemark { get; set; }
    }
}
