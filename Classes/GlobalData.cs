using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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

        public static readonly string BaseUrlPl = "https://ifscloud-demo.platnedcloud.com";
        public static readonly string AccessTokenUrlPl = $"{BaseUrlPl}/auth/realms/platdmo/protocol/openid-connect/token";
        public static readonly string ClientIdPl = "PlatnedPass_Service";
        public static readonly string ClientSecretPl = "XSgJKPsBNb0C3Rhv4d0nF84ag4rBZcwC";
        public static readonly string ScopePl = "openid microprofile-jwt";

        // Do not change unless instance changes -- END

        //for later
        public static string? UserId { get; set; }
        public static string? UserName { get; set; }
        public static string? CompanyId { get; set; }
    }
}
