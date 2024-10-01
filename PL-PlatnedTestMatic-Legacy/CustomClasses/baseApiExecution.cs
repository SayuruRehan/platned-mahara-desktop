using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatnedTestMatic.CustomClasses
{


    public class ApiExecution
    {
        private static readonly HttpClient client = new HttpClient();
        private string url = "";
        private string method = "";
        private string headers = "";
        private string requestBody = null;
        private string token = "";

        public class ApiResponse
        {
            public string ResponseBody { get; set; }
            public int StatusCode { get; set; }
        }

        public async Task<ApiResponse> Get(string postUrl, string postHeader, string postRequestBody, string postToken)
        {
            try
            {
                url = postUrl;
                method = "GET";
                headers = postHeader;
                requestBody = postRequestBody;
                token = postToken;

                ApiResponse apiResponse = await SendRequest(url, method, headers, requestBody, token);
                return apiResponse;
            }
            catch (Exception ex)
            {
                Logger.Log($"GET request failed: {ex.Message}", "Error");
                return new ApiResponse
                {
                    ResponseBody = "Execution failed! Refer to application logs for more info.",
                    StatusCode = 500
                };
            }
        }

        public async Task<ApiResponse> Post(string postUrl, string postHeader, string postRequestBody, string postToken)
        {
            try
            {
                url = postUrl;
                method = "POST";
                headers = postHeader;
                requestBody = postRequestBody;
                token = postToken;

                ApiResponse apiResponse = await SendRequest(url, method, headers, requestBody, token);
                return apiResponse;
            }
            catch (Exception ex)
            {
                Logger.Log($"POST request failed: {ex.Message}", "Error");
                return new ApiResponse
                {
                    ResponseBody = "Execution failed! Refer to application logs for more info.",
                    StatusCode = 500
                };
            }
        }

        public async Task<ApiResponse> Patch(string patchUrl, string patchHeader, string patchRequestBody, string patchToken)
        {
            try
            {
                url = patchUrl;
                method = "PATCH";
                headers = patchHeader;
                requestBody = patchRequestBody;
                token = patchToken;

                ApiResponse apiResponse = await SendRequest(url, method, headers, requestBody, token);
                return apiResponse;
            }
            catch (Exception ex)
            {
                Logger.Log($"PATCH request failed: {ex.Message}", "Error");
                return new ApiResponse
                {
                    ResponseBody = "Execution failed! Refer to application logs for more info.",
                    StatusCode = 500
                };
            }
        }

        public async Task<ApiResponse> Delete(string deleteUrl, string deleteHeader, string deleteToken)
        {
            try
            {
                url = deleteUrl;
                method = "DELETE";
                headers = deleteHeader;
                token = deleteToken;

                ApiResponse apiResponse = await SendRequest(url, method, headers, null, token);
                return apiResponse;
            }
            catch (Exception ex)
            {
                Logger.Log($"DELETE request failed: {ex.Message}", "Error");
                return new ApiResponse
                {
                    ResponseBody = "Execution failed! Refer to application logs for more info.",
                    StatusCode = 500
                };
            }
        }

        private async Task<ApiResponse> SendRequest(string url, string method, string headers, string requestBody, string receivedToken)
        {
            token = receivedToken;

            Logger.Log($"Request URL: {url}");
            Logger.Log($"Request method: {method}");
            Logger.Log($"Request headers: {headers}");
            if (method != "GET")
            {
                Logger.Log($"Request body: {requestBody}");
            }
            else
            {
                Logger.Log("No request body for GET method.");
                requestBody = null;
            }

            if (string.IsNullOrEmpty(token))
            {
                Logger.Log("No saved token found. Refreshing the token...");
                frmBasicData basicDataForm = new frmBasicData();
                token = await basicDataForm.RefreshToken();
                Logger.Log("Token refreshed!");
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            Logger.Log("Validating the HTTP method...");
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentException("HTTP method cannot be null or empty", nameof(method));
            }
            Logger.Log("Validating the HTTP method completed.");

            Logger.Log("Creating the request message...");
            var request = new HttpRequestMessage(new HttpMethod(method), url);
            Logger.Log("Creating the request message completed.");

            if (!string.IsNullOrEmpty(headers))
            {
                Logger.Log("Handling additional headers...");
                foreach (var header in headers.Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    var headerKeyValue = header.Split(':');
                    if (headerKeyValue.Length == 2)
                    {
                        request.Headers.Add(headerKeyValue[0].Trim(), headerKeyValue[1].Trim());
                    }
                }
                Logger.Log("Handling additional headers completed.");
            }

            Logger.Log($"METHOD Call: {method}");
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                Logger.Log("Handling request body for POST, PUT, PATCH methods...");
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                Logger.Log("Handling request body for POST, PUT, PATCH methods completed.");
            }

            Logger.Log("Sending the request and get the response...");
            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Logger.Log("Sending the request and get the response completed.");
            Logger.Log($"Response body: {responseBody}");
            Logger.Log($"Response status: {(int)response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                if (responseBody.Contains("INVALID_VALUE_FOR_PROPERTY"))
                {
                    Logger.Log("Detected invalid value for a property. Correcting and retrying...");

                    try
                    {
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();

                            var match = Regex.Match(message, @"'([^']+)'");
                            if (match.Success)
                            {
                                string propertyName = match.Groups[1].Value;
                                Logger.Log($"Property with invalid value: {propertyName}");

                                Logger.Log($"Response Body: {responseBody}");

                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                    if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                    {
                                        var currentValue = parsedRequestBody[propertyName];
                                        Logger.Log($"Current value of '{propertyName}' is '{currentValue}'");

                                        try
                                        {
                                            parsedRequestBody[propertyName] = currentValue.ToString();
                                            Logger.Log($"Converted '{propertyName}' to string: '{parsedRequestBody[propertyName]}'");
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Log($"Error while converting '{propertyName}' to string: {ex.Message}");
                                        }

                                        var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                        Logger.Log($"Corrected Request Body: {correctedRequestBody}");

                                        Logger.Log($"Retrying the request with the corrected property '{propertyName}'...");
                                        
                                        return await SendRequest(url, method, headers, correctedRequestBody, token);
                                    }
                                    else
                                    {
                                        Logger.Log($"Property '{propertyName}' not found or request body is null.");
                                    }
                                }
                                else
                                {
                                    Logger.Log("Request body is null or empty, unable to process correction.");
                                }
                            }
                            else
                            {
                                Logger.Log("Failed to extract the property name from the error message.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error while parsing error response: {ex.Message}");
                    }
                }

                if (responseBody.Contains("UNKNOWN_CONTENT"))
                {
                    Logger.Log("Detected an unknown issue with a property. Removing and retrying...");

                    try
                    {
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();

                            var match = Regex.Match(message, @"'([^']+)'");
                            if (match.Success)
                            {
                                string propertyName = match.Groups[1].Value;
                                Logger.Log($"Property causing issue: {propertyName}");

                                Logger.Log($"Response Body: {responseBody}");

                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                    if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                    {
                                        Logger.Log($"Removing problematic property '{propertyName}' from request body.");

                                        parsedRequestBody[propertyName] = null;
                                        ((JObject)parsedRequestBody).Property(propertyName).Remove();

                                        var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                        Logger.Log($"Corrected Request Body (without '{propertyName}'): {correctedRequestBody}");

                                        Logger.Log($"Retrying the request without the problematic property '{propertyName}'...");
                                        
                                        return await SendRequest(url, method, headers, correctedRequestBody, token);
                                    }
                                    else
                                    {
                                        Logger.Log($"Property '{propertyName}' not found or request body is null.");
                                    }
                                }
                                else
                                {
                                    Logger.Log("Request body is null or empty, unable to process correction.");
                                }
                            }
                            else
                            {
                                Logger.Log("Failed to extract the property name from the error message.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error while parsing error response: {ex.Message}");
                    }
                }
            }

            return new ApiResponse
            {
                ResponseBody = responseBody,
                StatusCode = (int)response.StatusCode
            }; 
        }

    }


}
