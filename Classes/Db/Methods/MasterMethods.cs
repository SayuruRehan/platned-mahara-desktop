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
                    LicenseConsumed = dr["LICENSE_CONSUMED"] != DBNull.Value ? Convert.ToInt16(dr["LICENSE_CONSUMED"]) : 0,
                    CompanyType = dr["COMPANY_TYPE"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    //ModifiedDate = Convert.ToDateTime(dr["MODIFIED_DATE"]),
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
            DataRow dr = (DataRow)objExecute.Executes("spGetPassCompany", ReturnType.DataRow, param, CommandType.StoredProcedure);
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
                Execute.AddParameter("@COMPANY_TYPE",objPass_Company.CompanyType),
                Execute.AddParameter("@CREATED_BY",objPass_Company.CreatedBy),
                Execute.AddParameter("@ROWSTATE",objPass_Company.RowState),
            };
            objExecute.Executes("spSavePassCompany", param, CommandType.StoredProcedure);
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
            DataRow dr = (DataRow)objExecute.Executes("spGetPassCompanyContact", ReturnType.DataRow, param, CommandType.StoredProcedure);
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
                Execute.AddParameter("@COMPANY_ID", objPass_Users_Company.CompanyID),
                Execute.AddParameter("@USER_ID", objPass_Users_Company.UserID),
            };

            // Execute the stored procedure and get a DataTable
            DataTable dt = (DataTable)objExecute.Executes("spGetUserPerCompany", ReturnType.DataTable, param, CommandType.StoredProcedure);

            // Ensure the DataTable is not null and contains rows
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0]; // Get the first row

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
        public bool EditUserPassword(Pass_Users_Company objPass_Users_Company)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Users_Company.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Users_Company.UserID),
                Execute.AddParameter("@PASSWORD",objPass_Users_Company.Password),
                Execute.AddParameter("@USER_EMAIL",objPass_Users_Company.UserEmail),
            };
            objExecute.Executes("spEditUserPassword", ReturnType.DataTable, param, CommandType.StoredProcedure);
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

        #region Mahara-85 - Pass Access Control Methods

        public List<Pass_Access_Control> GetAccessRecords()
        {
            List<Pass_Access_Control> pass_Access_Control = new List<Pass_Access_Control>();
            objExecute = new Execute();
            DataTable dt = (DataTable)objExecute.Executes("spGetAccessData", ReturnType.DataTable, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Access_Control.Add(new Pass_Access_Control
                {
                    AppFunction = dr["APP_FUNCTION"].ToString(),
                    AppFunctionDescription = dr["APP_FUNCTION_DESC"].ToString(),
                    UserRole = dr["USER_ROLE"].ToString(),
                    ReadAllowed = dr["READ_ALLOWED"].ToString(),
                    CreateAllowed = dr["CREATE_ALLOWED"].ToString(),
                    UpdateAllowed = dr["UPDATE_ALLOWED"].ToString(),
                    DeleteAllowed = dr["DELETE_ALLOWED"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                });
            }
            return pass_Access_Control;
        }

        public Pass_Access_Control GetAccessPerFunction(Pass_Access_Control objPass_Access_Control)
        {
            Pass_Access_Control pass_Access_Control = null;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@APP_FUNCTION",objPass_Access_Control.AppFunction),
                Execute.AddParameter("@USER_ROLE",objPass_Access_Control.UserRole),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetAccessPerFunction", ReturnType.DataTable, param, CommandType.StoredProcedure);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                pass_Access_Control = new Pass_Access_Control
                {
                    AppFunction = dr["APP_FUNCTION"].ToString(),
                    AppFunctionDescription = dr["APP_FUNCTION_DESC"].ToString(),
                    UserRole = dr["USER_ROLE"].ToString(),
                    ReadAllowed = dr["READ_ALLOWED"].ToString(),
                    CreateAllowed = dr["CREATE_ALLOWED"].ToString(),
                    UpdateAllowed = dr["UPDATE_ALLOWED"].ToString(),
                    DeleteAllowed = dr["DELETE_ALLOWED"].ToString(),
                    CreatedBy = dr["CREATED_BY"] != DBNull.Value ? dr["CREATED_BY"].ToString() : "",
                    CreatedDate = dr["CREATED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["CREATED_DATE"]) : null,
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                };
            }
            return pass_Access_Control;

        }

        public bool SavePassAccessControl(Pass_Access_Control objPass_Access_Control)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@APP_FUNCTION",objPass_Access_Control.AppFunction),
                Execute.AddParameter("@APP_FUNCTION_DESC",objPass_Access_Control.AppFunctionDescription),
                Execute.AddParameter("@USER_ROLE",objPass_Access_Control.UserRole),
                Execute.AddParameter("@READ_ALLOWED",objPass_Access_Control.ReadAllowed),
                Execute.AddParameter("@CREATE_ALLOWED",objPass_Access_Control.CreateAllowed),
                Execute.AddParameter("@UPDATE_ALLOWED",objPass_Access_Control.UpdateAllowed),
                Execute.AddParameter("@DELETE_ALLOWED",objPass_Access_Control.DeleteAllowed),
                Execute.AddParameter("@CREATED_BY",objPass_Access_Control.CreatedBy),
            };
            objExecute.Executes("spSavePassAccessControl", param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditPassAccessControl(Pass_Access_Control objPass_Access_Control)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@APP_FUNCTION",objPass_Access_Control.AppFunction),
                Execute.AddParameter("@APP_FUNCTION_DESC",objPass_Access_Control.AppFunctionDescription),
                Execute.AddParameter("@USER_ROLE",objPass_Access_Control.UserRole),
                Execute.AddParameter("@READ_ALLOWED",objPass_Access_Control.ReadAllowed),
                Execute.AddParameter("@CREATE_ALLOWED",objPass_Access_Control.CreateAllowed),
                Execute.AddParameter("@UPDATE_ALLOWED",objPass_Access_Control.UpdateAllowed),
                Execute.AddParameter("@DELETE_ALLOWED",objPass_Access_Control.DeleteAllowed),
                Execute.AddParameter("@MODIFIED_BY",objPass_Access_Control.ModifiedBy),
            };
            objExecute.Executes("spEditPassAccessControl", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeletePassAccessControl(Pass_Access_Control objPass_Access_Control)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@APP_FUNCTION",objPass_Access_Control.AppFunction),
                Execute.AddParameter("@USER_ROLE",objPass_Access_Control.UserRole),
            };
            objExecute.Executes("spDeletePassAccessControl", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        #endregion

        #region Mahara-90 Pass Collection Methods
        [Obsolete]
        public List<Pass_Json_Collection> GetPassJsonCollections()
        {
            List<Pass_Json_Collection> pass_Json_Collection = new List<Pass_Json_Collection>();
            objExecute = new Execute();
            DataTable dt = (DataTable)objExecute.Executes("spGetJsonCollections", ReturnType.DataTable, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Json_Collection.Add(new Pass_Json_Collection
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    CollectionID = dr["COLLECTION_ID"].ToString(),
                    CollectionName = dr["COLLECTION_NAME"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Json_Collection;
        }

        public List<Pass_Json_Collection> GetPassJsonCollectionPerUser(Pass_Json_Collection objPass_Json_Collection)
        {
            List<Pass_Json_Collection> pass_Json_Collections = new List<Pass_Json_Collection>();
            objExecute = new Execute();
            param = new SqlParameter[]
            {
               Execute.AddParameter("@COMPANY_ID",objPass_Json_Collection.CompanyID),
               Execute.AddParameter("@USER_ID",objPass_Json_Collection.UserID),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetJsonCollectionPerUser", ReturnType.DataTable, param, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Json_Collections.Add(new Pass_Json_Collection
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    CollectionID = dr["COLLECTION_ID"].ToString(),
                    CollectionName = dr["COLLECTION_NAME"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Json_Collections;
        }

        [Obsolete]
        public bool SavePassJsonCollection(Pass_Json_Collection objPass_Json_Collection)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_Collection.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_Collection.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_Collection.CollectionID),
                Execute.AddParameter("@COLLECTION_NAME",objPass_Json_Collection.CollectionName),
                Execute.AddParameter("@CREATED_BY",objPass_Json_Collection.CreatedBy),
            };
            objExecute.Executes("spSaveJsonCollection", param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditPassJsonCollection(Pass_Json_Collection objPass_Json_Collection)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_Collection.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_Collection.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_Collection.CollectionID),
                Execute.AddParameter("@COLLECTION_NAME",objPass_Json_Collection.CollectionName),
                Execute.AddParameter("@MODIFIED_BY",objPass_Json_Collection.ModifiedBy),
            };
            objExecute.Executes("spEditJsonCollection", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeletePassJsonCollection(Pass_Json_Collection objPass_Json_Collection)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_Collection.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_Collection.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_Collection.CollectionID),
                Execute.AddParameter("@MODIFIED_BY",objPass_Json_Collection.ModifiedBy),
            };
            objExecute.Executes("spDeleteJsonCollection", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        public bool SharePassShareJsonCollection(Pass_Json_Collection objPass_Json_Collection)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_Collection.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_Collection.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_Collection.CollectionID),
                Execute.AddParameter("@MODIFIED_BY",objPass_Json_Collection.ModifiedBy),
            };
            objExecute.Executes("spShareJsonCollection", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }
        #endregion

        #region Mahara-90 Pass JSON File Methods

        public List<Pass_Json_File> GetPassJsonFiles()
        {
            List<Pass_Json_File> pass_Json_File = new List<Pass_Json_File>();
            objExecute = new Execute();
            DataTable dt = (DataTable)objExecute.Executes("spGetJsonFiles", ReturnType.DataTable, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Json_File.Add(new Pass_Json_File
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    CollectionID = dr["COLLECTION_ID"].ToString(),
                    FileID = dr["FILE_ID"].ToString(),
                    FileName = dr["FILE_NAME"].ToString(),
                    FileContent = dr["FILE_CONTENT"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Json_File;
        }

        public List<Pass_Json_File> GetPassJsonFilePerUserPerCollection(Pass_Json_File objPass_Json_File)
        {
            List<Pass_Json_File> pass_Json_Files = new List<Pass_Json_File>();
            objExecute = new Execute();
            param = new SqlParameter[]
            {
               Execute.AddParameter("@COMPANY_ID",objPass_Json_File.CompanyID),
               Execute.AddParameter("@USER_ID",objPass_Json_File.UserID),
               Execute.AddParameter("@COLLECTION_ID",objPass_Json_File.CollectionID),
            };
            DataTable dt = (DataTable)objExecute.Executes("spGetJsonFilePerUserPerCollection", ReturnType.DataTable, param, CommandType.StoredProcedure);
            foreach (DataRow dr in dt.Rows)
            {
                pass_Json_Files.Add(new Pass_Json_File
                {
                    CompanyID = dr["COMPANY_ID"].ToString(),
                    UserID = dr["USER_ID"].ToString(),
                    CollectionID = dr["COLLECTION_ID"].ToString(),
                    FileID = dr["FILE_ID"].ToString(),
                    FileName = dr["FILE_NAME"].ToString(),
                    FileContent = dr["FILE_CONTENT"].ToString(),
                    CreatedBy = dr["CREATED_BY"].ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CREATED_DATE"]),
                    ModifiedBy = dr["MODIFIED_BY"] != DBNull.Value ? dr["MODIFIED_BY"].ToString() : "",
                    ModifiedDate = dr["MODIFIED_DATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["MODIFIED_DATE"]) : null,
                    RowState = dr["ROWSTATE"].ToString(),
                });
            }
            return pass_Json_Files;
        }

        [Obsolete]
        public bool SavePassJsonFile(Pass_Json_File objPass_Json_File)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_File.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_File.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_File.CollectionID),
                Execute.AddParameter("@File_ID",objPass_Json_File.FileID),
                Execute.AddParameter("@FILE_NAME",objPass_Json_File.FileName),
                Execute.AddParameter("@FILE_CONTENT",objPass_Json_File.FileContent),
                Execute.AddParameter("@CREATED_BY",objPass_Json_File.CreatedBy),
            };
            objExecute.Executes("spSaveJsonFile", param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool EditPassJsonFile(Pass_Json_File objPass_Json_File)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_File.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_File.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_File.CollectionID),
                Execute.AddParameter("@File_ID",objPass_Json_File.FileID),
                Execute.AddParameter("@FILE_NAME",objPass_Json_File.FileName),
                //Execute.AddParameter("@FILE_CONTENT",objPass_Json_File.FileContent),
                Execute.AddParameter("@MODIFIED_BY",objPass_Json_File.ModifiedBy),
            };
            objExecute.Executes("spEditJsonFile", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        [Obsolete]
        public bool DeletePassJsonFile(Pass_Json_File objPass_Json_File)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_File.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_File.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_File.CollectionID),
                Execute.AddParameter("@File_ID",objPass_Json_File.FileID),
                Execute.AddParameter("@MODIFIED_BY",objPass_Json_File.ModifiedBy),
            };
            objExecute.Executes("spDeleteJsonFile", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }

        public bool SharePassShareJsonFile(Pass_Json_File objPass_Json_File)
        {
            bool res = false;
            objExecute = new Execute();
            param = new SqlParameter[]
            {
                Execute.AddParameter("@COMPANY_ID",objPass_Json_File.CompanyID),
                Execute.AddParameter("@USER_ID",objPass_Json_File.UserID),
                Execute.AddParameter("@COLLECTION_ID",objPass_Json_File.CollectionID),
                Execute.AddParameter("@File_ID",objPass_Json_File.FileID),
                Execute.AddParameter("@MODIFIED_BY",objPass_Json_File.ModifiedBy),
            };
            objExecute.Executes("spShareJsonFile", ReturnType.DataTable, param, CommandType.StoredProcedure);
            res = true;
            return res;
        }
        #endregion

    }
}
