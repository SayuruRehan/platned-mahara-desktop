using CommunityToolkit.WinUI.UI.Controls;
using PlatnedMahara.Classes.Db;
using PlatnedMahara.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace PlatnedMahara.Classes
{
    public static class AccessControl
    {
        #region Mahara-85 - RBAC validations

        private static string userRole = GlobalData.UserRole;

        static AccessControl()
        {
            userRole = GlobalData.UserRole;
        }

        public static bool IsGranted(string accessFunction, string action)
        {
            if(userRole.Trim() == "Super Admin")
            {
                return true;
            }

            switch (action.Trim().ToUpper())
            {
                case "C":
                    return Create(accessFunction);

                case "R":
                    return Read(accessFunction);

                case "U":
                    return Update(accessFunction);

                case "D":
                    return Delete(accessFunction);

                default:
                    return false;
            }
        }

        #region RBAC - Read

        private static bool Read(string accessFunction)
        {
            if (accessFunction != "")
            {
                Pass_Access_Control pass_Access_det = new Pass_Access_Control
                {
                    AppFunction = accessFunction,
                    UserRole = GlobalData.UserRole.Trim()
                };

                Pass_Access_Control pass_Access = new Pass_Access_Control();
                pass_Access = new AuthPlatnedPass().GetAccess_Per_Function(pass_Access_det);

                if (pass_Access != null)
                {
                    return Convert.ToBoolean(pass_Access.ReadAllowed);
                }
            }
            return false;
        }

        #endregion

        #region RBAC - Create

        private static bool Create(string accessFunction)
        {
            if (accessFunction != "")
            {
                Pass_Access_Control pass_Access_det = new Pass_Access_Control
                {
                    AppFunction = accessFunction,
                    UserRole = GlobalData.UserRole.Trim()
                };

                Pass_Access_Control pass_Access = new Pass_Access_Control();
                pass_Access = new AuthPlatnedPass().GetAccess_Per_Function(pass_Access_det);

                if (pass_Access != null)
                {
                    return Convert.ToBoolean(pass_Access.CreateAllowed);
                }
            }
            return false;
        }

        #endregion

        #region RBAC - Update

        private static bool Update(string accessFunction)
        {
            if (accessFunction != "")
            {
                Pass_Access_Control pass_Access_det = new Pass_Access_Control
                {
                    AppFunction = accessFunction,
                    UserRole = GlobalData.UserRole.Trim()
                };

                Pass_Access_Control pass_Access = new Pass_Access_Control();
                pass_Access = new AuthPlatnedPass().GetAccess_Per_Function(pass_Access_det);

                if (pass_Access != null)
                {
                    return Convert.ToBoolean(pass_Access.UpdateAllowed);
                }
            }
            return false;
        }

        #endregion

        #region RBAC - Delete

        private static bool Delete(string accessFunction)
        {
            if (accessFunction != "")
            {
                Pass_Access_Control pass_Access_det = new Pass_Access_Control
                {
                    AppFunction = accessFunction,
                    UserRole = GlobalData.UserRole.Trim()
                };

                Pass_Access_Control pass_Access = new Pass_Access_Control();
                pass_Access = new AuthPlatnedPass().GetAccess_Per_Function(pass_Access_det);

                if (pass_Access != null)
                {
                    return Convert.ToBoolean(pass_Access.DeleteAllowed);
                }
            }
            return false;
        }

        #endregion


        #endregion
    }
}
