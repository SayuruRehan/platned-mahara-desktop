using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.Classes.Db
{
    public class Pass_Json_Collection
    {
        public string CompanyID { get; set; }
        public string UserID { get; set; }
        public string CollectionID { get; set; }
        public string CollectionName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string RowState { get; set; }
    }
}
