using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Windows.Storage;
using System.IO;

namespace PlatnedMahara.Classes
{
    public static class GlobalData
    {
        // Internal PlatnedPass access details
        // Do not change unless instance changes -- START
        /*public static readonly string BaseUrlPl = "https://cloud.platnedcloud.com";
        public static readonly string AccessTokenUrlPl = "https://cloud.platnedcloud.com/auth/realms/platprd/protocol/openid-connect/token";
        public static readonly string ClientIdPl = "Plat_APT_Service";
        public static readonly string ClientSecretPl = "JawpIl0UXtCABshpP8TlWHFL9ghtzGue";
        public static readonly string ScopePl = "openid microprofile-jwt";*/

        // Get the app's local storage folder
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public static readonly string configFilePath = Path.Combine(localFolder.Path, "pl-application_config.xml");
        public static readonly string tempFolderPath = Path.Combine(Path.GetTempPath(), "PlatnedMahara");
        //public static readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");
        public static readonly string BaseUrlPl = "https://ifscloud-demo.platnedcloud.com";
        public static readonly string AccessTokenUrlPl = $"{BaseUrlPl}/auth/realms/platdmo/protocol/openid-connect/token";
        public static readonly string ClientIdPl = "PlatnedPass_Service";
        public static readonly string ClientSecretPl = "XSgJKPsBNb0C3Rhv4d0nF84ag4rBZcwC";
        public static readonly string ScopePl = "openid microprofile-jwt";
        // Mahara-86 - START
        public static readonly string[] AccessRoleArraySuper = {"Super Admin"};
        public static readonly string[] AccessRoleArrayUserAdmin = {"User Admin"};
        public static readonly string[] AccessRoleArrayUser = {"User"};
        // Mahara-86 - END

        // Do not change unless instance changes -- END

        private static bool _isLoggedIn = false;
        public static bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => _isLoggedIn = value;
        }
        public static string? UserId { get; set; }
        public static string? UserName { get; set; }
        public static string? CompanyId { get; set; }
        public static string? UserRole { get; set; }
        public static string? UserEmail { get; set; }
        public static string? LicenseKey { get; set; }
        public static string? UserStatus { get; set; }
    }
}
