using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.Classes
{
    public class Pass_Company
    {
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public int LicenseLimit { get; set; }
        public int LicenseConsumed { get; set; }
        public string CompanyType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string RowState { get; set; }
    }
}


