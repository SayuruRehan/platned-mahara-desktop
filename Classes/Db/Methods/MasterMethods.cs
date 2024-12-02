using PlatnedMahara.Classes;
using PlatnedMahara.Classes.Db;
using PlatnedMahara.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.DataAccess.Methods
{
    public class MasterMethods
    {
        Execute objExecute;
        SqlParameter[] param;

        #region Pass Company Methods
        [Obsolete]
        public List<Pass_Company> GetPassCompanies()
        {
            List<Pass_Company> pass_Companies = new List<Pass_Company>();
            objExecute = new Execute();
            DataTable dt = (DataTable)objExecute.Executes("spGetPassCompanies", ReturnType.DataTable, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Companies.Add(new Pass_Company
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    CompanyName = dr["COMPANY_NAME"].ToString(),
                    CompanyAddress = dr["COMPANY_ADDRESS"].ToString(),
                    LicenseLimit = Convert.ToInt16(dr["LICENSE_LIMIT"]),
                    LicenseConsumed = Convert.ToInt16(dr["LICENSE_CONSUMED"]),
                    CompanyType = dr["COMPANY_TYPE"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"].ToString(),
                    ModifiedDate = Convert.ToDateTime(dr["MODIFIED_DATE"]),
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Companies;
        }

        [Obsolete]
        public Pass_Company GetPassCompany(Pass_Company objPass_Company)
        {
            Pass_Company pass_Company = null;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
        Execute.AddParameter("@COMPANY_ID",objPass_Company.CompanyID),
            };
            DataRow dr = (DataRow)objExecute.Executes("spGetPassCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            if (dr != null)
            {
                pass_Company = new Pass_Company
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    CompanyName = dr["COMPANY_NAME"].ToString(),
                    CompanyAddress = dr["COMPANY_ADDRESS"].ToString(),
                    LicenseLimit = Convert.ToInt16(dr["LICENSE_LIMIT"]),
                    LicenseConsumed = Convert.ToInt16(dr["LICENSE_CONSUMED"]),
                    CompanyType = dr["COMPANY_TYPE"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"].ToString(),
                    ModifiedDate = Convert.ToDateTime(dr["MODIFIED_DATE"]),
                    RowState = dr["ROWSTATE"].ToString(),
                };
            }
            return pass_Company;
        }

        [Obsolete]
        public bool SavePassCompany(Pass_Company objPass_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Company.CompanyID),
                Execute.AddParameter("@COMPANY_NAME",objPass_Company.CompanyName),
                Execute.AddParameter("@COMPANY_ADDRESS",objPass_Company.CompanyAddress),
                Execute.AddParameter("@LICENSE_LIMIT",objPass_Company.LicenseLimit),
                Execute.AddParameter("@LICENSE_CONSUMED",objPass_Company.LicenseConsumed),
                Execute.AddParameter("@COMPANY_TYPE",objPass_Company.CompanyType),
                Execute.AddParameter("@CREATED_BY",objPass_Company.CreatedBy),
                Execute.AddParameter("@ROWSTATE",objPass_Company.RowState),
            };
            objExecute.Executes("spSavePassCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditPassCompany(Pass_Company objPass_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Company.CompanyID),
                Execute.AddParameter("@COMPANY_NAME",objPass_Company.CompanyName),
                Execute.AddParameter("@COMPANY_ADDRESS",objPass_Company.CompanyAddress),
                Execute.AddParameter("@LICENSE_LIMIT",objPass_Company.LicenseLimit),
                Execute.AddParameter("@LICENSE_CONSUMED",objPass_Company.LicenseConsumed),
                Execute.AddParameter("@COMPANY_TYPE",objPass_Company.CompanyType),
                Execute.AddParameter("@MODIFIED_BY",objPass_Company.ModifiedBy),
                Execute.AddParameter("@ROWSTATE",objPass_Company.RowState),
            };
            objExecute.Executes("spEditPassCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeletePassCompany(Pass_Company objPass_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
        Execute.AddParameter("@COMPANY_ID",objPass_Company.CompanyID),
        Execute.AddParameter("@MODIFIED_BY",objPass_Company.ModifiedBy),
        Execute.AddParameter("@ROWSTATE",objPass_Company.RowState),
            };
            objExecute.Executes("spDeletePassCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }
        #endregion

        #region Pass Company Contacts Methods
        [Obsolete]
        public List<Pass_Company_Contact> GetPassCompanyContacts()
        {
            List<Pass_Company_Contact> pass_Company_Contacts = new List<Pass_Company_Contact>();
            objExecute = new Execute();
            DataTable dt = (DataTable)objExecute.Executes("spGetPassCompanyContacts", ReturnType.DataTable, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Company_Contacts.Add(new Pass_Company_Contact
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    ContactTitle = dr["COMPANY_CONTACT_TITLE"].ToString(),
                    ContactNumber = dr["COMPANY_CONTACT_NUMBER"].ToString(),
                    ContactEmail = dr["COMPANY_CONTACT_EMAIL"].ToString(),
                });

            }
            return pass_Company_Contacts;
        }

        [Obsolete]
        public Pass_Company_Contact GetPassCompanyContact(Pass_Company_Contact objPass_Company_Contact)
        {
            Pass_Company_Contact pass_Company_Contact = null;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
        Execute.AddParameter("@COMPANY_ID",objPass_Company_Contact.CompanyID),
            };
            DataRow dr = (DataRow)objExecute.Executes("spGetPassCompanyContact", ReturnType.DataTable, param, CommandType.StoredProcedure);
            if (dr != null)
            {
                pass_Company_Contact = new Pass_Company_Contact
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    ContactTitle = dr["COMPANY_CONTACT_TITLE"].ToString(),
                    ContactNumber = dr["COMPANY_CONTACT_NUMBER"].ToString(),
                    ContactEmail = dr["COMPANY_CONTACT_EMAIL"].ToString(),
                };
            }
            return pass_Company_Contact;
        }

        [Obsolete]
        public bool SavePassCompanyContact(Pass_Company_Contact objPass_Company_Contact)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Company_Contact.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Company_Contact.UserID),
                Execute.AddParameter("@COMPANY_CONTACT_TITLE",objPass_Company_Contact.ContactTitle),
                Execute.AddParameter("@COMPANY_CONTACT_NUMBER",objPass_Company_Contact.ContactNumber),
                Execute.AddParameter("@COMPANY_CONTACT_EMAIL",objPass_Company_Contact.ContactEmail),
            };
            objExecute.Executes("spSavePassCompanyContact", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditPassCompanyContact(Pass_Company_Contact objPass_Company_Contact)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Company_Contact.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Company_Contact.UserID),
                Execute.AddParameter("@COMPANY_CONTACT_TITLE",objPass_Company_Contact.ContactTitle),
                Execute.AddParameter("@COMPANY_CONTACT_NUMBER",objPass_Company_Contact.ContactNumber),
                Execute.AddParameter("@COMPANY_CONTACT_EMAIL",objPass_Company_Contact.ContactEmail),
            };
            objExecute.Executes("spEditPassCompanyContact", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeletePassCompanyContact(Pass_Company_Contact objPass_Company_Contact)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Company_Contact.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Company_Contact.UserID),
            };
            objExecute.Executes("spDeletePassCompanyContact", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }
        #endregion

        #region Pass User AppLog Methods

        [Obsolete]
        public List<Pass_User_App_Log> GetCompanyPassUserAppLogs(Pass_User_App_Log objPass_User_App_Log)
        {
            List<Pass_User_App_Log> pass_User_App_Logs = new List<Pass_User_App_Log>();
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_User_App_Log.CompanyID),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetCompanyPassUserAppLogs", ReturnType.DataTable, param, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_User_App_Logs.Add(new Pass_User_App_Log
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    LogLineNumber = Convert.ToInt32(dr["LOG_LINE_NUMBER"]),
                    LogDate = Convert.ToDateTime(dr["LOG_DATE"]),
                    LogType = dr["LOG_TYPE"].ToString(),
                    LogDescription = dr["LOG_DESCRIPTION"].ToString(),
                    PLATNEDRemark = dr["PLATNED_REMARKS"].ToString(),
                });
            }
            return pass_User_App_Logs;
        }

        [Obsolete]
        public List<Pass_User_App_Log> GetPassUserAppLogs(Pass_User_App_Log objPass_User_App_Log)
        {
            List<Pass_User_App_Log> pass_User_App_Logs = new List<Pass_User_App_Log>();
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_User_App_Log.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_User_App_Log.UserID),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetPassUserAppLogs", ReturnType.DataTable, param, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_User_App_Logs.Add(new Pass_User_App_Log
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    LogLineNumber = Convert.ToInt32(dr["LOG_LINE_NUMBER"]),
                    LogDate = Convert.ToDateTime(dr["LOG_DATE"]),
                    LogType = dr["LOG_TYPE"].ToString(),
                    LogDescription = dr["LOG_DESCRIPTION"].ToString(),
                    PLATNEDRemark = dr["PLATNED_REMARKS"].ToString(),
                });
            }
            return pass_User_App_Logs;
        }

        [Obsolete]
        public bool SavePassUserAppLogs(Pass_User_App_Log objPass_User_App_Log)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_User_App_Log.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_User_App_Log.UserID),
                Execute.AddParameter("@LOG_LINE_NUMBER",objPass_User_App_Log.LogLineNumber),
                Execute.AddParameter("@LOG_DATE",objPass_User_App_Log.LogDate),
                Execute.AddParameter("@LOG_TYPE",objPass_User_App_Log.LogType),
                Execute.AddParameter("@LOG_DESCRIPTION",objPass_User_App_Log.LogDescription),
                Execute.AddParameter("@PLATNED_REMARKS",objPass_User_App_Log.PLATNEDRemark),
            };
            objExecute.Executes("spSavePassUserAppLogs", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditPassUserAppLogs(Pass_User_App_Log objPass_User_App_Log)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_User_App_Log.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_User_App_Log.UserID),
                Execute.AddParameter("@LOG_LINE_NUMBER",objPass_User_App_Log.LogLineNumber),
                Execute.AddParameter("@LOG_DATE",objPass_User_App_Log.LogDate),
                Execute.AddParameter("@LOG_TYPE",objPass_User_App_Log.LogType),
                Execute.AddParameter("@LOG_DESCRIPTION",objPass_User_App_Log.LogDescription),
                Execute.AddParameter("@PLATNED_REMARKS",objPass_User_App_Log.PLATNEDRemark),
            };
            objExecute.Executes("spEditPassUserAppLogs", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeletePassUserAppLogs(Pass_User_App_Log objPass_User_App_Log)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
        Execute.AddParameter("@COMPANY_ID",objPass_User_App_Log.CompanyID),
        Execute.AddParameter("@USER_ID",objPass_User_App_Log.UserID),
            };
            objExecute.Executes("spDeletePassUserAppLogs", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }
        #endregion

        #region Users per Company Methods
        [Obsolete]
        public List<Pass_Users_Company> GetPassUsers()
        {
            List<Pass_Users_Company> pass_Users_Companies = new List<Pass_Users_Company>();
            objExecute = new Execute();
            DataTable dt = (DataTable)objExecute.Executes("spGetPassUsers", ReturnType.DataTable, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Users_Companies.Add(new Pass_Users_Company
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    UserName = dr["USER_NAME"].ToString(),
                    Password = dr["PASSWORD"].ToString(),
                    UserEmail = dr["USER_EMAIL"].ToString(),
                    LicenseKey = dr["LICENSE_KEY"].ToString(),
                    ValidFrom = Convert.ToDateTime(dr["VALID_FROM"]),
                    ValidTo = Convert.ToDateTime(dr["VALID_TO"]),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    UserRole = dr["USER_ROLE"].ToString(),
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Users_Companies;
        }

        [Obsolete]
        public List<Pass_Users_Company> GetUsersperCompany(Pass_Users_Company objPass_Users_Company)
        {
            List<Pass_Users_Company> pass_Users_Companies = new List<Pass_Users_Company>();
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Users_Company.CompanyID),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetUsersperCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Users_Companies.Add(new Pass_Users_Company
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    UserName = dr["USER_NAME"].ToString(),
                    Password = dr["PASSWORD"].ToString(),
                    UserEmail = dr["USER_EMAIL"].ToString(),
                    LicenseKey = dr["LICENSE_KEY"].ToString(),
                    ValidFrom = Convert.ToDateTime(dr["VALID_FROM"]),
                    ValidTo = Convert.ToDateTime(dr["VALID_TO"]),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    UserRole = dr["USER_ROLE"].ToString(),
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Users_Companies;
        }

        [Obsolete]
        public Pass_Users_Company GetUserPerCompany(Pass_Users_Company objPass_Users_Company)
        {
            Pass_Users_Company pass_Users_Company = null;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Users_Company.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Users_Company.UserID),
            };
            DataRow dr = (DataRow)objExecute.Executes("spGetUserPerCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            if (dr != null)
            {
                pass_Users_Company = new Pass_Users_Company
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    UserName = dr["USER_NAME"].ToString(),
                    Password = dr["PASSWORD"].ToString(),
                    UserEmail = dr["USER_EMAIL"].ToString(),
                    LicenseKey = dr["LICENSE_KEY"].ToString(),
                    ValidFrom = Convert.ToDateTime(dr["VALID_FROM"]),
                    ValidTo = Convert.ToDateTime(dr["VALID_TO"]),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    UserRole = dr["USER_ROLE"].ToString(),
                    RowState = dr["ROWSTATE"].ToString(),
                };
            }
            return pass_Users_Company;
        }

        [Obsolete]
        public bool SaveUsersPerCompany(Pass_Users_Company objPass_Users_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Users_Company.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Users_Company.UserID),
                Execute.AddParameter("@USER_NAME",objPass_Users_Company.UserName),
                Execute.AddParameter("@PASSWORD",objPass_Users_Company.Password),
                Execute.AddParameter("@USER_EMAIL",objPass_Users_Company.UserEmail),
                Execute.AddParameter("@LICENSE_KEY",objPass_Users_Company.LicenseKey),
                Execute.AddParameter("@VALID_FROM",objPass_Users_Company.ValidFrom),
                Execute.AddParameter("@VALID_TO",objPass_Users_Company.ValidTo),
                Execute.AddParameter("@CREATED_BY",objPass_Users_Company.CreatedBy),
                Execute.AddParameter("@USER_ROLE",objPass_Users_Company.UserRole),
                Execute.AddParameter("@ROWSTATE",objPass_Users_Company.RowState),
            };
            objExecute.Executes("spSaveUsersPerCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditUsersPerCompany(Pass_Users_Company objPass_Users_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Users_Company.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Users_Company.UserID),
                Execute.AddParameter("@USER_NAME",objPass_Users_Company.UserName),
                Execute.AddParameter("@USER_EMAIL",objPass_Users_Company.UserEmail),
                Execute.AddParameter("@VALID_FROM",objPass_Users_Company.ValidFrom),
                Execute.AddParameter("@VALID_TO",objPass_Users_Company.ValidTo),
                Execute.AddParameter("@MODIFIED_BY",GlobalData.UserId),
                Execute.AddParameter("@USER_ROLE",objPass_Users_Company.UserRole),
                Execute.AddParameter("@ROWSTATE",objPass_Users_Company.RowState),
            };
            objExecute.Executes("spEditUsersPerCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeleteUsersPerCompany(Pass_Users_Company objPass_Users_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
        Execute.AddParameter("@COMPANY_ID",objPass_Users_Company.CompanyID),
        Execute.AddParameter("@USER_ID",objPass_Users_Company.UserID),
            };
            objExecute.Executes("spDeleteUsersPerCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public List<Pass_Users_Company> GetLoginUser(Pass_Users_Company objPass_Users_Company)
        {
            List<Pass_Users_Company> pass_Users_Companies = new List<Pass_Users_Company>();
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@USER_ID",objPass_Users_Company.UserID),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetLoginUser", ReturnType.DataTable, param, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Users_Companies.Add(new Pass_Users_Company
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    UserName = dr["USER_NAME"].ToString(),
                    Password = dr["PASSWORD"].ToString(),
                    UserEmail = dr["USER_EMAIL"].ToString(),
                    LicenseKey = dr["LICENSE_KEY"].ToString(),
                    ValidFrom = Convert.ToDateTime(dr["VALID_FROM"]),
                    ValidTo = Convert.ToDateTime(dr["VALID_TO"]),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    UserRole = dr["USER_ROLE"].ToString(),
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Users_Companies;
        }


        #endregion
    }
}
