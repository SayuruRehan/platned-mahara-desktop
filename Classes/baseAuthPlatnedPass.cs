using DocumentFormat.OpenXml.Drawing.Charts;
using Irony.Parsing;
using Microsoft.UI.Xaml.Controls;
using PlatnedMahara.Classes.Db;
using PlatnedMahara.DataAccess.Methods;
using PlatnedMahara.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Media.Protection.PlayReady;
using static PlatnedMahara.Classes.ApiExecution;

namespace PlatnedMahara.Classes
{
    public class AuthPlatnedPass
    {
        private static string token = "";
        private static bool validLogin = false;
        private static bool recordsaved = false;
        private static string accessTokenUrlPl = GlobalData.AccessTokenUrlPl;
        private static string clientIdPl = GlobalData.ClientIdPl;
        private static string clientSecretPl = GlobalData.ClientSecretPl;
        private static string scopePl = GlobalData.ScopePl;
        private static string userId = GlobalData.UserId;
        private static string companyId = GlobalData.CompanyId;

        private static readonly HttpClient client = new HttpClient();
        MasterMethods masterMethods;

        public static async Task<string> GetAccessTokenPlatndPass(string accessTokenUrl, string clientId, string clientSecret, string scope)
        {
            Logger.Log("Encoding the client ID and secret started...");
            var clientCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            Logger.Log("Encoding the client ID and secret completed.");

            Logger.Log("Creating the request message started...");
            var request = new HttpRequestMessage(HttpMethod.Post, accessTokenUrl);
            Logger.Log($"Created request: {request}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientCredentials);
            Logger.Log("Creating the request message completed.");

            Logger.Log("Preparing the content (form-urlencoded) started...");
            var content = new StringContent($"grant_type=client_credentials&scope={scope}", Encoding.UTF8, "application/x-www-form-urlencoded");
            Logger.Log($"Created content: {content}");
            request.Content = content;
            Logger.Log("Preparing the content (form-urlencoded) completed.");

            Logger.Log("Send the request started...");
            var response = await client.SendAsync(request);
            Logger.Log($"Received response: {response}");
            response.EnsureSuccessStatusCode();
            Logger.Log("Send the request completed.");

            Logger.Log("Parsing the response and extracting the access token started...");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
            Logger.Log("Parsing the response and extracting the access token completed.");

            return tokenResponse.access_token;
        }

        public static async Task<bool> validateLogin(string userId, string password)
        {
            try
            {
                var jsonBody = $@"
                                        {{
                                            ""Objstate"": ""IfsApp.PassUsersHandling.PassUsersPerCompanyState'Active'"",
                                            ""UserId"": ""{userId}"",
                                            ""Password"": ""{password}""
                                        }}";

                var jsonDoc = JsonDocument.Parse(jsonBody);
                var Objstate = jsonDoc.RootElement.GetProperty("Objstate").GetString();
                var UserId = jsonDoc.RootElement.GetProperty("UserId").GetString();
                var Password = jsonDoc.RootElement.GetProperty("Password").GetString();

                var baseUrl = $"{GlobalData.BaseUrlPl}/main/ifsapplications/projection/v1/PassUsersHandling.svc/UsersSet";

                var filter = $"Objstate eq {Objstate} and UserId eq '{UserId}' and Password eq '{Password}'";

                var uriBuilder = new UriBuilder(baseUrl)
                {
                    //Query = $"$filter={System.Uri.EscapeDataString(filter)}"
                    Query = $"$filter={filter}"
                };

                token = await GetAccessTokenPlatndPass(accessTokenUrlPl, clientIdPl, clientSecretPl, scopePl);


                string method = "GET";
                string url = uriBuilder.ToString();
                string headers = "";
                string requestBody = jsonBody;

                ApiExecution api = new ApiExecution();
                ApiExecution.ApiResponse apiResponse = null;

                Logger.Log("GET - Request body: " + requestBody);
                apiResponse = await api.Get(url, headers, requestBody, token);


                Logger.Log($"Response StatusCode={apiResponse.StatusCode}, ResponseBody={apiResponse.ResponseBody}");

                if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 201))
                {
                    var content = apiResponse?.ResponseBody;

                    // Parse the API response content
                    var apiResponseParsed = JsonDocument.Parse(content);

                    // Check if the response contains the Password
                    if (apiResponseParsed.RootElement.TryGetProperty("value", out JsonElement valueElement))
                    {
                        foreach (var item in valueElement.EnumerateArray())
                        {
                            var responsePassword = item.GetProperty("Password").GetString();
                            var responseCompanyId = item.GetProperty("CompanyId").GetString();
                            var responseUserName = item.GetProperty("UserName").GetString();

                            // Check if the password matches the one from the request
                            if (responsePassword == Password)
                            {
                                Logger.Log("Password validated with Platned Pass");
                                validLogin = true;

                                GlobalData.UserId = userId;
                                GlobalData.UserName = responseUserName;
                                GlobalData.CompanyId = responseCompanyId;

                                return validLogin;
                            }
                            else
                            {
                                Logger.Log("Password key does not match.");
                                validLogin = false;

                                return validLogin;
                            }
                        }
                    }
                    else
                    {
                        Logger.Log("Password not found in the response.");
                        validLogin = false;

                        return validLogin;
                    }
                }
                else
                {
                    Logger.Log($"Error: {apiResponse.StatusCode}");
                    validLogin = false;

                    return validLogin;
                }

                return validLogin;

            }
            catch (Exception ex)
            {
                validLogin = false;
                return validLogin;
            }
        }


        #region Pass Company Methods
        public bool CreateNewCompany(Pass_Company pass_Company)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.SavePassCompany(pass_Company);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public bool EditCompany(Pass_Company pass_Company)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.EditPassCompany(pass_Company);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public bool DeleteCompany(Pass_Company pass_Company)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.DeletePassCompany(pass_Company);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public List<Pass_Company> GetPass_Companies()
        {
            List<Pass_Company> pass_Companies = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_Companies = new List<Pass_Company>();
                pass_Companies = masterMethods.GetPassCompanies();
                return pass_Companies;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_Companies;
            }
        }
        public Pass_Company GetPass_Companies(Pass_Company p)
        {
            Pass_Company pass_Company = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_Company = new Pass_Company();
                pass_Company = masterMethods.GetPassCompany(p);
                return pass_Company;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_Company;
            }
        }
        #endregion

        #region User Management
        public bool CreateNewUser(Pass_Users_Company pass_User)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.SaveUsersPerCompany(pass_User);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public bool EditUser(Pass_Users_Company pass_User)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.EditUsersPerCompany(pass_User);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }

        public bool EditUserPassword(Pass_Users_Company pass_User)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.EditUserPassword(pass_User);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }

        public bool DeleteUser(Pass_Users_Company pass_User)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.DeleteUsersPerCompany(pass_User);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public List<Pass_Users_Company> GetPass_Users()
        {
            List<Pass_Users_Company> pass_Users = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_Users = new List<Pass_Users_Company>();
                pass_Users = masterMethods.GetPassUsers();
                return pass_Users;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_Users;
            }
        }

        public List<Pass_Users_Company> GetPass_Users_Per_Company(Pass_Users_Company p)
        {
            List<Pass_Users_Company> pass_User = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_User = new List<Pass_Users_Company>();
                pass_User = masterMethods.GetUsersperCompany(p);
                return pass_User;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_User;
            }
        }

        public List<Pass_Users_Company> GetLoginUser(Pass_Users_Company p)
        {
            List<Pass_Users_Company> pass_User = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_User = new List<Pass_Users_Company>();
                pass_User = masterMethods.GetLoginUser(p);
                return pass_User;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_User;
            }
        }

        public List<Pass_Users_Company> GetPasswordResetUser(Pass_Users_Company p)
        {
            return GetLoginUser(p);
        }

        public Pass_Users_Company GetPass_User_Per_Company(Pass_Users_Company p)
        {
            Pass_Users_Company pass_User = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_User = new Pass_Users_Company();
                pass_User = masterMethods.GetUserPerCompany(p);
                return pass_User;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_User;
            }
        }

        #endregion


        #region Mahara-58 ( Company Contact CRUD Funtions )

        //Get pass company contact list
        public List<Pass_Company_Contact> GetPass_Company_Contacts()
        {
            List<Pass_Company_Contact> pass_Company_Contacts = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_Company_Contacts = new List<Pass_Company_Contact>();
                pass_Company_Contacts = masterMethods.GetPassCompanyContacts();
                return pass_Company_Contacts;

            }
            catch (Exception e)
            {
                Logger.Log($"Error: {e.Message}");
                validLogin = false;
                return pass_Company_Contacts;
            }
        }

        public bool CreateNewPassCompanyContact(Pass_Company_Contact pass_Company_Contact)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.SavePassCompanyContact(pass_Company_Contact);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                return false;
            }
        }

        public bool DeletePassCompanyContact(Pass_Company_Contact pass_Company_Contact)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.DeletePassCompanyContact(pass_Company_Contact);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                return false;
            }
        }

        public bool EditPassCompanyContact(Pass_Company_Contact pass_Company_Contact)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.EditPassCompanyContact(pass_Company_Contact);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Mahara-85 - Access Control Functions

        public List<Pass_Access_Control> GetAccess_Records()
        {
            List<Pass_Access_Control> pass_Access_Records = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_Access_Records = new List<Pass_Access_Control>();
                pass_Access_Records = masterMethods.GetAccessRecords();
                return pass_Access_Records;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_Access_Records;
            }
        }

        public Pass_Access_Control GetAccess_Per_Function(Pass_Access_Control p)
        {
            Pass_Access_Control pass_Access = null;
            try
            {
                masterMethods = new MasterMethods();
                pass_Access = new Pass_Access_Control();
                pass_Access = masterMethods.GetAccessPerFunction(p);
                return pass_Access;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return pass_Access;
            }
        }

        public bool CreateNewAccessControl(Pass_Access_Control pass_Access_Control)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.SavePassAccessControl(pass_Access_Control);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public bool EditAccessControl(Pass_Access_Control pass_Access_Control)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.EditPassAccessControl(pass_Access_Control);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }
        public bool DeleteAccessControl(Pass_Access_Control pass_Access_Control)
        {
            try
            {
                masterMethods = new MasterMethods();
                recordsaved = masterMethods.DeletePassAccessControl(pass_Access_Control);
                return recordsaved;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                validLogin = false;
                return validLogin;
            }
        }

        #endregion

    }


}

