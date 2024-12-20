﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.Classes.Db
{
    public class Pass_Access_Control
    {
        public string AppFunction { get; set; }
        public string AppFunctionDescription { get; set; }
        public string UserRole { get; set; }
        public string ReadAllowed { get; set; }
        public string CreateAllowed { get; set; }
        public string UpdateAllowed { get; set; }
        public string DeleteAllowed { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
