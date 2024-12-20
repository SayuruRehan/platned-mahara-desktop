using DocumentFormat.OpenXml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlatnedMahara.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;

namespace PlatnedMahara.Classes
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

        public async Task<ApiResponse> ValidationRec(string valUrl, string valHeader, string valRequestBody, string valToken, string valMethod, string valRec = "ValidationRec")
        {
            try
            {
                url = valUrl;
                method = valMethod;
                headers = valHeader;
                requestBody = valRequestBody;
                token = valToken;

                ApiResponse apiResponse = await SendRequestValRec(url, method, headers, requestBody, token);
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

        public async Task<ApiResponse> SendMultipartRequest(string valUrl, string valHeader, string valRequestBody, string valToken, string valMethod, string valRec = "SendMultipartRequest", string entitySet = "", string entitySetParam = "", string entitySetArray = "")
        {
            try
            {
                url = valUrl;
                method = valMethod;
                headers = valHeader;
                requestBody = valRequestBody;
                token = valToken;

                ApiResponse apiResponse = await SendMultipartRequest(url, method, headers, requestBody, token, entitySet, entitySetParam, entitySetArray);
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

        int retryThriceSr = 0;
        string propertyNameSrTemp = "";

        private async Task<ApiResponse> SendRequest(string url, string method, string headers, string requestBody, string receivedToken)
        {
            token = receivedToken;
            string requestBodyForValidation = requestBody;
            // Mahara-105 - Preserving requestBody for GET calls when errors found - START
            string requestBodyForGet = requestBody;
            // Mahara-105 - END

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
                PageConfig basicDataForm = new PageConfig();
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
                if ((responseBody.Contains("INVALID_VALUE_FOR_PROPERTY") || responseBody.Contains("ODP_DESERIALIZATION_ERROR")) && retryThriceSr < 3 )
                {
                    retryThriceSr += 1;

                    Logger.Log("Detected invalid value for a property. Correcting and retrying...");

                    try
                    {
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();

                            //var match = Regex.Match(message, @"'([^']+)'");
                            var match = Regex.Match(message, @"'([^']+)'");
                            string propertyName = match.Groups[1].Value;
                            if (!match.Success)
                            {
                                //match = Regex.Match(message, @"property: '([^']+)'|property: (\w+)");
                                match = Regex.Match(message, @"property:\s*([^ ]+)");

                                propertyName = match.Groups[1].Value;
                                if (match.Success && propertyName == "")
                                {
                                    propertyName = match.Groups[0].Value;
                                    if (match.Success && propertyName == "")
                                    {
                                        propertyName = match.Groups[2].Value;
                                        if (match.Success && propertyName == "")
                                        {
                                            propertyName = match.Groups[3].Value;
                                        }
                                    }
                                }
                                if (!match.Success)
                                {
                                    match = Regex.Match(message, @"property '([^']+)'|property: (\w+)");
                                    propertyName = match.Groups[1].Value;
                                    if (match.Success && propertyName == "")
                                    {
                                        propertyName = match.Groups[0].Value;
                                        if (match.Success && propertyName == "")
                                        {
                                            propertyName = match.Groups[2].Value;
                                            if (match.Success && propertyName == "")
                                            {
                                                propertyName = match.Groups[3].Value;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (propertyName == "")
                                {
                                    propertyName = match.Groups[0].Value;
                                    if (propertyName == "")
                                    {
                                        propertyName = match.Groups[2].Value;
                                        if (propertyName == "")
                                        {
                                            propertyName = match.Groups[3].Value;
                                        }
                                    }
                                }
                            }
                            //var match = Regex.Match(message, @"property: '([^']+)'|property: (\w+)");
                            //var match = Regex.Match(message, @"property '([^']+)'|property: (\w+)");

                            if (match.Success)
                            {

                                Logger.Log($"Property with invalid value: {propertyName}");

                                Logger.Log($"Response Body: {responseBody}");

                                // Mahara-105 - Setting body for corrections of GET calls - START
                                if (method == "GET")
                                {                                    
                                    requestBody = requestBodyForGet;
                                    Logger.Log($"Error in GET URL or Body found. Setting body for corrections: {requestBody}");
                                }
                                // Mahara-105 - END

                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                    if (propertyNameSrTemp != propertyName)
                                    {
                                        propertyNameSrTemp = propertyName;
                                        retryThriceSr = 0;
                                    }

                                    if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                    {
                                        var currentValue = parsedRequestBody[propertyName];
                                        Logger.Log($"Current value of '{propertyName}' is '{currentValue}'");

                                        if (currentValue is bool)
                                        {
                                            parsedRequestBody[propertyName] = currentValue.ToString().ToUpper();
                                            Logger.Log($"Converted boolean '{propertyName}' to string: '{parsedRequestBody[propertyName]}'");
                                        }
                                        else if (DateTime.TryParse(currentValue.ToString(), out DateTime parsedDate))
                                        {
                                            //parsedRequestBody[propertyName] = parsedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                                            if (retryThriceSr != 3)
                                            {
                                                parsedRequestBody[propertyName] = parsedDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                                Logger.Log($"Date format yyyy-MM-ddTHH:mm:ssZ");
                                            }
                                            else
                                            {
                                                parsedRequestBody[propertyName] = parsedDate.ToString("yyyy-MM-dd");
                                                Logger.Log($"Date format yyyy-MM-dd");
                                                retryThriceSr = 0;
                                            }

                                            Logger.Log($"Converted '{propertyName}' to ISO 8601 date format: {parsedRequestBody[propertyName]}");
                                        }
                                        else if (int.TryParse(currentValue.ToString(), out int numericValue))
                                        {
                                            parsedRequestBody[propertyName] = numericValue.ToString();
                                            Logger.Log($"Converted string '{propertyName}' to integer value: '{parsedRequestBody[propertyName]}'");
                                        }
                                        else
                                        {
                                            try
                                            {
                                                parsedRequestBody[propertyName] = currentValue.ToString();
                                                Logger.Log($"Converted '{propertyName}' to string: '{parsedRequestBody[propertyName]}'");
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.Log($"Error while converting '{propertyName}' to string: {ex.Message}");
                                            }
                                        }

                                        var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                        Logger.Log($"Corrected Request Body: {correctedRequestBody}");

                                        Logger.Log($"Retrying the request with the corrected property '{propertyName}'...");

                                        // Mahara-105 - Reconstructing the URL for GET issues - START
                                        if(method == "GET")
                                        {
                                            try
                                            {
                                                url = UrlReconstructor.ReconstructUrl(url, requestBody, true);
                                                Logger.Log("GET - Constructed URL: " + url);
                                            }
                                            catch (Exception e)
                                            {
                                                Logger.Log("No parameters found in the URL. Skipping URL Construction.");
                                            }
                                        }                                        
                                        // Mahara-105 - END

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
                            if (!match.Success)
                            {
                                match = Regex.Match(message, @"'([^']+)'|:\s*([^ ]+)");
                            }

                            if (match.Success)
                            {
                                string propertyName = match.Groups[1].Value;
                                if (propertyName == "")
                                {
                                    propertyName = match.Groups[2].Value;
                                }
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

                if (responseBody.Contains("DATABASE_ERROR") && responseBody.Contains("may not be specified for new objects"))
                {
                    Logger.Log("Detected a database error related to a property. Removing and retrying...");

                    retryThrice = 0;

                    try
                    {
                        // Extract only the JSON part of the response body
                        var jsonStartIndex = responseBody.IndexOf("{");
                        var jsonEndIndex = responseBody.LastIndexOf("}") + 1;

                        if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                        {
                            string jsonContent = responseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
                            dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                            if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                            {
                                string message = errorResponse.error.details[0]?.message?.ToString();
                                var match = Regex.Match(message, @"Field \[([^]]+)\]");

                                if (match.Success)
                                {
                                    string propertyName = match.Groups[1].Value;
                                    Logger.Log($"Property causing database issue: {propertyName}");
                                    Logger.Log($"Response Body: {responseBody}");

                                    if (!string.IsNullOrEmpty(requestBody))
                                    {
                                        dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                        if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                        {
                                            Logger.Log($"Removing problematic property '{propertyName}' from request body.");

                                            parsedRequestBody[propertyName] = null; // Optional, but could be left in place
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
                                    Logger.Log("Failed to extract the property name from the database error message.");
                                }
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

        int firstIterationForValRec = 0;
        int firstIterationForValRecDbError = 1;
        private async Task<ApiResponse> SendRequestValRec(string url, string method, string headers, string requestBody, string receivedToken)
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
                PageConfig basicDataForm = new PageConfig();
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

            var response = new System.Net.Http.HttpResponseMessage();
            var responseBody = "";

            if (firstIterationForValRec == 0)
            {
                firstIterationForValRec += 1;
                firstIterationForValRecDbError = 0;
            }
            else
            {
                Logger.Log("Sending the request and get the response...");
                response = await client.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
                Logger.Log("Sending the request and get the response completed.");
                Logger.Log($"Response body: {responseBody}");
                Logger.Log($"Response status: {(int)response.StatusCode}");
            }


            if (!response.IsSuccessStatusCode || (response.IsSuccessStatusCode && firstIterationForValRec == 1))
            {


                if (responseBody.Contains("INVALID_VALUE_FOR_PROPERTY") || responseBody.Contains("ODP_DESERIALIZATION_ERROR"))
                {
                    Logger.Log("Detected invalid value for a property. Correcting and retrying...");

                    try
                    {
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();

                            var match = Regex.Match(message, @"'([^']+)'");
                            string propertyName = match.Groups[1].Value;
                            if (!match.Success)
                            {
                                match = Regex.Match(message, @"property:\s*([^ ]+)");

                                propertyName = match.Groups[1].Value;
                                if (match.Success && propertyName == "")
                                {
                                    propertyName = match.Groups[0].Value;
                                    if (match.Success && propertyName == "")
                                    {
                                        propertyName = match.Groups[2].Value;
                                        if (match.Success && propertyName == "")
                                        {
                                            propertyName = match.Groups[3].Value;
                                        }
                                    }
                                }
                                if (!match.Success)
                                {
                                    match = Regex.Match(message, @"property '([^']+)'|property: (\w+)");
                                    propertyName = match.Groups[1].Value;
                                    if (match.Success && propertyName == "")
                                    {
                                        propertyName = match.Groups[0].Value;
                                        if (match.Success && propertyName == "")
                                        {
                                            propertyName = match.Groups[2].Value;
                                            if (match.Success && propertyName == "")
                                            {
                                                propertyName = match.Groups[3].Value;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (propertyName == "")
                                {
                                    propertyName = match.Groups[0].Value;
                                    if (propertyName == "")
                                    {
                                        propertyName = match.Groups[2].Value;
                                        if (propertyName == "")
                                        {
                                            propertyName = match.Groups[3].Value;
                                        }
                                    }
                                }
                            }

                            if (match.Success)
                            {

                                Logger.Log($"Property with invalid value: {propertyName}");
                                Logger.Log($"Response Body: {responseBody}");

                                // Deserialize the request body to access ValidationRec
                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);
                                    if (parsedRequestBody != null && parsedRequestBody.ValidationRec != null)
                                    {
                                        dynamic validationRec = parsedRequestBody.ValidationRec;

                                        // Check if the property exists in ValidationRec
                                        if (validationRec[propertyName] != null)
                                        {
                                            var currentValue = validationRec[propertyName];
                                            Logger.Log($"Current value of '{propertyName}' is '{currentValue}'");

                                            // Handle the corrections based on the value type
                                            if (currentValue is bool)
                                            {
                                                validationRec[propertyName] = currentValue.ToString().ToUpper();
                                                Logger.Log($"Converted boolean '{propertyName}' to string: '{validationRec[propertyName]}'");
                                            }
                                            else if (DateTime.TryParse(currentValue.ToString(), out DateTime parsedDate))
                                            {
                                                validationRec[propertyName] = parsedDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                                Logger.Log($"Converted '{propertyName}' to ISO 8601 date format: '{validationRec[propertyName]}'");
                                            }
                                            else if (int.TryParse(currentValue.ToString(), out int numericValue))
                                            {
                                                if (currentValue is string)
                                                {
                                                    validationRec[propertyName] = numericValue;
                                                    Logger.Log($"Converted string '{propertyName}' to integer value: '{validationRec[propertyName]}'");
                                                }
                                                else
                                                {
                                                    // If it's already a numeric value, convert it to a string
                                                    validationRec[propertyName] = numericValue.ToString();
                                                    Logger.Log($"Converted numeric '{propertyName}' to string: '{validationRec[propertyName]}'");
                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    validationRec[propertyName] = currentValue.ToString();
                                                    Logger.Log($"Converted '{propertyName}' to string: '{validationRec[propertyName]}'");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Logger.Log($"Error while converting '{propertyName}' to string: {ex.Message}");
                                                }
                                            }

                                            // Construct the new request body with updated ValidationRec
                                            var correctedRequestBody = new JObject
                                            {
                                                ["ValidationRec"] = validationRec
                                            };
                                            string serializedRequestBody = JsonConvert.SerializeObject(correctedRequestBody);
                                            Logger.Log($"Corrected Request Body: {serializedRequestBody}");
                                            //requestBody = serializedRequestBody;

                                            Logger.Log($"Retrying the request with the corrected property '{propertyName}'...");
                                            return await SendRequestValRec(url, method, headers, serializedRequestBody, token);
                                        }
                                        else
                                        {
                                            Logger.Log($"Property '{propertyName}' not found in ValidationRec.");
                                        }
                                    }
                                    else
                                    {
                                        Logger.Log("Request body is null or does not contain ValidationRec.");
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
                            if (!match.Success)
                            {
                                match = Regex.Match(message, @"'([^']+)'|:\s*([^ ]+)");
                            }

                            if (match.Success)
                            {
                                string propertyName = match.Groups[1].Value;
                                if (string.IsNullOrEmpty(propertyName))
                                {
                                    propertyName = match.Groups[2].Value;
                                }
                                Logger.Log($"Property causing issue: {propertyName}");

                                Logger.Log($"Response Body: {responseBody}");

                                // Deserialize the request body to access ValidationRec
                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                    if (parsedRequestBody != null && parsedRequestBody.ValidationRec != null)
                                    {
                                        dynamic validationRec = parsedRequestBody.ValidationRec;

                                        // Check if the property exists in ValidationRec
                                        if (validationRec[propertyName] != null)
                                        {
                                            Logger.Log($"Removing problematic property '{propertyName}' from ValidationRec.");

                                            // Remove the problematic property
                                            ((JObject)validationRec).Property(propertyName).Remove();

                                            // Construct the new request body with updated ValidationRec
                                            var correctedRequestBody = new JObject
                                            {
                                                ["ValidationRec"] = validationRec
                                            };
                                            string serializedRequestBody = JsonConvert.SerializeObject(correctedRequestBody);
                                            Logger.Log($"Corrected Request Body (without '{propertyName}'): {serializedRequestBody}");

                                            //requestBody = serializedRequestBody;

                                            Logger.Log($"Retrying the request without the problematic property '{propertyName}'...");
                                            return await SendRequestValRec(url, method, headers, serializedRequestBody, token);
                                        }
                                        else
                                        {
                                            Logger.Log($"Property '{propertyName}' not found in ValidationRec.");
                                        }
                                    }
                                    else
                                    {
                                        Logger.Log("Request body is null or does not contain ValidationRec.");
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

                if (firstIterationForValRecDbError == 0 || responseBody.Contains("DATABASE_ERROR"))
                {
                    if (responseBody.Contains("DATABASE_ERROR") && responseBody.Contains("CASE not found"))
                    {
                        Logger.Log("Detected 'CASE not found' database error. Constructing request body with 'ValidationRec'...");

                        try
                        {
                            // Parse the original request body
                            dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                            if (parsedRequestBody != null)
                            {
                                var validationRec = new JObject();

                                // Loop through all properties of parsedRequestBody
                                foreach (var property in (IDictionary<string, JToken>)parsedRequestBody)
                                {
                                    string propertyName = property.Key;
                                    var currentValue = property.Value;
                                    Logger.Log($"Current value of '{propertyName}' is {currentValue}");

                                    validationRec[propertyName] = currentValue;
                                }

                                // Add ValidationRec to the existing request body
                                parsedRequestBody.ValidationRec = validationRec;

                                // Construct the new request body
                                string correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                Logger.Log($"Constructed Request Body with ValidationRec: {correctedRequestBody}");

                                // Retry the request with the corrected request body
                                return await SendRequestValRec(url, method, headers, correctedRequestBody, token);
                            }
                            else
                            {
                                Logger.Log("Parsed request body is null. Unable to proceed with corrections.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Error while constructing 'ValidationRec' for database error: {ex.Message}");
                        }
                    }
                    else
                    {
                        firstIterationForValRecDbError += 1;
                        Logger.Log("Detected database error. Correcting by constructing 'ValidationRec' dynamically from the request body...");
                        //requestBody = requestBodyForValidation;
                        try
                        {
                            // Parse the original request body
                            dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                            if (parsedRequestBody != null)
                            {
                                // Initialize the ValidationRec JObject
                                var validationRec = new JObject();

                                // Loop through all properties of parsedRequestBody
                                foreach (var property in (IDictionary<string, JToken>)parsedRequestBody)
                                {
                                    string propertyName = property.Key;
                                    var currentValue = property.Value;
                                    Logger.Log($"Current value of '{propertyName}' is '{currentValue}'");

                                    validationRec[propertyName] = currentValue;
                                }

                                // Create the request body with the dynamically built 'ValidationRec'
                                var correctedRequestBody = new JObject
                                {
                                    ["ValidationRec"] = validationRec
                                };

                                string serializedRequestBody = JsonConvert.SerializeObject(correctedRequestBody);
                                Logger.Log($"Constructed Request Body: {serializedRequestBody}");

                                // Retry the request with the corrected request body
                                return await SendRequestValRec(url, method, headers, serializedRequestBody, token);
                            }
                            else
                            {
                                Logger.Log("Parsed request body is null. Unable to proceed with corrections.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Error while constructing 'ValidationRec' from request body: {ex.Message}");
                        }
                    }
                }
            }


            return new ApiResponse
            {
                ResponseBody = responseBody,
                StatusCode = (int)response.StatusCode
            };
        }

        int retryThrice = 0;
        string propertyNameTemp = "";

        private async Task<ApiResponse> SendMultipartRequest(string url, string method, string headers, string requestBody, string receivedToken, string entitySet, string entitySetParam, string entitySetArray)
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
                PageConfig basicDataForm = new PageConfig();
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

            // Extract the value of 'OrderNo'
            JObject jsonObject = JObject.Parse(requestBody);            
            string? entitySetParamValue = jsonObject[$"{entitySetParam}"]?.ToString();
            Logger.Log($"Building request body for the {entitySetParam}: {entitySetParamValue}");

            // Get all property names and Join them as a single string separated by commas
            JObject json = JObject.Parse(requestBody);
            List<string> propertyNames = json.Properties().Select(p => p.Name).ToList();
            string propertyNamesString = string.Join(",", propertyNames);

            // Generate boundaries for the batch and changeset
            string mainBoundary = "35a3b075-f782-4f72-8ef5-3fe4ab5e14bc";
            string changesetBoundary = "a5a8b006-a690-4a64-9bc1-4fcd9ea55f78";

            // Construct the multipart body for the batch request
            var multipartBody = new StringBuilder();

            // Start the outer boundary
            multipartBody.AppendLine($"--{mainBoundary}");
            multipartBody.AppendLine($"Content-Type: multipart/mixed;boundary={changesetBoundary}");
            multipartBody.AppendLine();

            // Add the first part (CreateChangeRequest - Init: true)
            multipartBody.AppendLine($"--{changesetBoundary}");
            multipartBody.AppendLine("Content-Type: application/http");
            multipartBody.AppendLine("Content-Transfer-Encoding:binary");
            multipartBody.AppendLine("Content-Id: 1");
            multipartBody.AppendLine();
            multipartBody.AppendLine("POST CreateChangeRequest HTTP/1.1");
            multipartBody.AppendLine("Accept:application/json;odata.metadata=full;IEEE754Compatible=true");
            multipartBody.AppendLine("Content-Type: application/json;IEEE754Compatible=true");
            multipartBody.AppendLine();
            multipartBody.AppendLine("{\"Init\":true}");
            multipartBody.AppendLine();

            // Add the second part (CustomerOrderSet)
            multipartBody.AppendLine($"--{changesetBoundary}");
            multipartBody.AppendLine("Content-Type: application/http");
            multipartBody.AppendLine("Content-Transfer-Encoding:binary");
            multipartBody.AppendLine("Content-Id: 2");
            multipartBody.AppendLine();
            multipartBody.AppendLine($"POST {entitySet}({entitySetParam}='{entitySetParamValue}')/{entitySetArray}?select-fields={propertyNamesString} HTTP/1.1"); // Use your full endpoint here
            multipartBody.AppendLine("Accept:application/json;odata.metadata=full;IEEE754Compatible=true");
            multipartBody.AppendLine("Content-Type: application/json;IEEE754Compatible=true");
            multipartBody.AppendLine("X-IFS-Accept-Warnings: true");
            multipartBody.AppendLine();
            multipartBody.AppendLine(requestBody); // Your JSON request body for CustomerOrderSet
            multipartBody.AppendLine();

            // Add the third part (CreateChangeRequest - Init: false)
            multipartBody.AppendLine($"--{changesetBoundary}");
            multipartBody.AppendLine("Content-Type: application/http");
            multipartBody.AppendLine("Content-Transfer-Encoding:binary");
            multipartBody.AppendLine("Content-Id: 3");
            multipartBody.AppendLine();
            multipartBody.AppendLine("POST CreateChangeRequest HTTP/1.1");
            multipartBody.AppendLine("Accept:application/json;odata.metadata=full;IEEE754Compatible=true");
            multipartBody.AppendLine("Content-Type: application/json;IEEE754Compatible=true");
            multipartBody.AppendLine();
            multipartBody.AppendLine("{\"Init\":false}");
            multipartBody.AppendLine();

            // End of changeset
            multipartBody.AppendLine($"--{changesetBoundary}--");
            multipartBody.AppendLine();

            // End the batch
            multipartBody.AppendLine($"--{mainBoundary}--");

            // Create the content with the proper Content-Type
            var content = new StringContent(multipartBody.ToString(), Encoding.UTF8, "multipart/mixed");
            content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", mainBoundary));

            Logger.Log($"Multipart body: {multipartBody.ToString()}");

            // Send the batch request
            var batchRequest = new HttpRequestMessage(HttpMethod.Post, $"{url}")
            {
                Content = content
            };

            var response = await client.SendAsync(batchRequest);
            var responseBody = await response.Content.ReadAsStringAsync();

            Logger.Log($"Response body: {responseBody}");
            Logger.Log($"Response status: {(int)response.StatusCode}");



            if ((responseBody.Contains("INVALID_VALUE_FOR_PROPERTY") || responseBody.Contains("ODP_DESERIALIZATION_ERROR")) && retryThrice < 3)
            {
                Logger.Log("Detected invalid value for a property. Correcting and retrying...");

                retryThrice += 1;

                Logger.Log("Retry count out of three: " + retryThrice);

                try
                {
                    // Extract only the JSON part of the response body
                    var jsonStartIndex = responseBody.IndexOf("{");
                    var jsonEndIndex = responseBody.LastIndexOf("}") + 1;

                    if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                    {
                        string jsonContent = responseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();
                            var match = Regex.Match(message, @"'([^']+)'");
                            string propertyName = "";

                            if (match.Success)
                            {
                                propertyName = match.Groups[1].Value;
                            }
                            else
                            {
                                match = Regex.Match(message, @"property:\s*([^ ]+)");
                                if (match.Success)
                                {
                                    propertyName = match.Groups[1].Value;
                                }
                            }

                            if (string.IsNullOrEmpty(propertyName))
                            {
                                Logger.Log("Failed to extract the property name from the error message.");
                                //break; // Exit if property name could not be determined
                            }

                            Logger.Log($"Property with invalid value: {propertyName}");
                            Logger.Log($"Response Body: {responseBody}");

                            if (!string.IsNullOrEmpty(requestBody))
                            {
                                dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                if (propertyNameTemp != propertyName)
                                {
                                    propertyNameTemp = propertyName;
                                    retryThrice = 0;
                                }

                                if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                {
                                    var currentValue = parsedRequestBody[propertyName];
                                    Logger.Log($"Current value of '{propertyName}' is {currentValue}");

                                    // Check if currentValue is a JToken and its underlying type is Boolean
                                    if (currentValue is JValue jValue && jValue.Type == JTokenType.Boolean && retryThrice == 3)
                                    {
                                        bool boolValue = (bool)jValue.Value;
                                        Logger.Log($"Boolean property '{propertyName}' remains unchanged: {boolValue}");
                                    }
                                    // Check if it's a string that looks like a boolean (e.g. "True" or "False")
                                    else if (currentValue is JValue jStringValue && jStringValue.Type == JTokenType.String &&
                                             (jStringValue.Value.ToString() == "True" || jStringValue.Value.ToString() == "False"))
                                    {
                                        bool boolValue = bool.Parse(jStringValue.Value.ToString());
                                        parsedRequestBody[propertyName] = boolValue;  // Assign it back as a real boolean
                                        Logger.Log($"Converted string '{propertyName}' to boolean: {parsedRequestBody[propertyName]}");
                                    }
                                    else if (currentValue is JValue jStringValueStr && jStringValueStr.Type == JTokenType.Boolean &&
                                             (jStringValueStr.Value.ToString() == "True" || jStringValueStr.Value.ToString() == "False"))
                                    {
                                        bool boolValue = bool.Parse(jStringValueStr.Value.ToString());
                                        parsedRequestBody[propertyName] = boolValue.ToString().ToUpper(); ;
                                        Logger.Log($"Converted string '{propertyName}' to boolean: {parsedRequestBody[propertyName]}");
                                    }
                                    else if (currentValue is bool)
                                    {
                                        parsedRequestBody[propertyName] = currentValue.ToString().ToUpper();
                                        Logger.Log($"Converted boolean '{propertyName}' to string: {parsedRequestBody[propertyName]}");
                                    }
                                    else if (DateTime.TryParse(currentValue.ToString(), out DateTime parsedDate))
                                    {
                                        if (retryThrice != 3)
                                        {
                                            parsedRequestBody[propertyName] = parsedDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                            Logger.Log($"Date format yyyy-MM-ddTHH:mm:ssZ");
                                        }
                                        else
                                        {
                                            parsedRequestBody[propertyName] = parsedDate.ToString("yyyy-MM-dd");
                                            Logger.Log($"Date format yyyy-MM-dd");
                                        }

                                        Logger.Log($"Converted '{propertyName}' to ISO 8601 date format: {parsedRequestBody[propertyName]}");
                                    }
                                    else if (int.TryParse(currentValue.ToString(), out int numericValue))
                                    {
                                        if (currentValue is string)
                                        {
                                            parsedRequestBody[propertyName] = numericValue;
                                            Logger.Log($"Converted string '{propertyName}' to integer value: {parsedRequestBody[propertyName]}");
                                        }
                                        else
                                        {
                                            parsedRequestBody[propertyName] = numericValue.ToString();
                                            Logger.Log($"Converted numeric '{propertyName}' to string: {parsedRequestBody[propertyName]}");
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            /*if (currentValue.Contains("True") || currentValue.Contains("False"))
                                            {
                                                parsedRequestBody[propertyName] = currentValue.ToString().ToUpper();
                                            }
                                            else */
                                            if (currentValue == true || currentValue == false)
                                            {
                                                parsedRequestBody[propertyName] = currentValue.ToString().ToUpper();
                                            }
                                            else
                                            {
                                                parsedRequestBody[propertyName] = currentValue.ToString();
                                            }

                                            Logger.Log($"Converted '{propertyName}' to string: '{parsedRequestBody[propertyName]}'");
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Log($"Error while converting '{propertyName}' to string: {ex.Message}");
                                        }
                                    }

                                    var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                    Logger.Log($"Corrected Request Body: {correctedRequestBody}");

                                    Logger.Log($"Retrying the request with the corrected property '{propertyName}'...");
                                    return await SendMultipartRequest(url, method, headers, correctedRequestBody, token, entitySet, entitySetParam, entitySetArray);
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
                    }
                    else
                    {
                        Logger.Log("Failed to extract JSON from response body.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while parsing error response: {ex.Message}");
                }
            }

            if (retryThrice == 3)
            {
                retryThrice += 1;

                try
                {
                    // Extract only the JSON part of the response body
                    var jsonStartIndex = responseBody.IndexOf("{");
                    var jsonEndIndex = responseBody.LastIndexOf("}") + 1;

                    if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                    {
                        string jsonContent = responseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();
                            var match = Regex.Match(message, @"'([^']+)'");
                            string propertyName = "";

                            if (match.Success)
                            {
                                propertyName = match.Groups[1].Value;
                            }
                            else
                            {
                                match = Regex.Match(message, @"property:\s*([^ ]+)");
                                if (match.Success)
                                {
                                    propertyName = match.Groups[1].Value;
                                }
                            }

                            if (string.IsNullOrEmpty(propertyName))
                            {
                                Logger.Log("Failed to extract the property name from the error message.");
                                //break; // Exit if property name could not be determined
                            }

                            Logger.Log($"Property with invalid value: {propertyName}");
                            Logger.Log($"Response Body: {responseBody}");

                            if (!string.IsNullOrEmpty(requestBody))
                            {
                                dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                {
                                    var currentValue = parsedRequestBody[propertyName];
                                    Logger.Log($"Current value of '{propertyName}' is {currentValue}");

                                    if (int.TryParse(currentValue.ToString(), out int numericValue))
                                    {
                                        // Check if the current value was initially a string
                                        if (currentValue is string)
                                        {
                                            // Convert the integer value to double and format to two decimal points
                                            parsedRequestBody[propertyName] = string.Format("{0:F2}", Convert.ToDouble(numericValue));
                                            Logger.Log($"Converted string '{propertyName}' to double value with two decimal points: '{parsedRequestBody[propertyName]}'");
                                        }
                                        else
                                        {
                                            // If it's already a numeric value, round and format it with two decimal points
                                            parsedRequestBody[propertyName] = Math.Round(Convert.ToDouble(numericValue), 2).ToString("F2");
                                            Logger.Log($"Converted numeric '{propertyName}' to string value with two decimal points: '{parsedRequestBody[propertyName]}'");
                                        }
                                    }

                                    var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                    Logger.Log($"Corrected Request Body: {parsedRequestBody}");

                                    Logger.Log($"Retrying the request with the corrected property '{propertyName}'...");
                                    return await SendMultipartRequest(url, method, headers, parsedRequestBody, token, entitySet, entitySetParam, entitySetArray);
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
                    }
                    else
                    {
                        Logger.Log("Failed to extract JSON from response body.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while parsing error response: {ex.Message}");
                }

            }


            if (responseBody.Contains("UNKNOWN_CONTENT") || retryThrice == 4)
            {
                Logger.Log("Detected an unknown issue with a property. Removing and retrying...");

                retryThrice = 0;

                try
                {

                    // Extract only the JSON part of the response body
                    var jsonStartIndex = responseBody.IndexOf("{");
                    var jsonEndIndex = responseBody.LastIndexOf("}") + 1;

                    if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                    {
                        string jsonContent = responseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();

                            var match = Regex.Match(message, @"'([^']+)'");
                            if (!match.Success)
                            {
                                match = Regex.Match(message, @"'([^']+)'|:\s*([^ ]+)");
                            }

                            if (match.Success)
                            {
                                string propertyName = match.Groups[1].Value;
                                if (propertyName == "")
                                {
                                    propertyName = match.Groups[2].Value;
                                }
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

                                        return await SendMultipartRequest(url, method, headers, correctedRequestBody, token, entitySet, entitySetParam, entitySetArray);
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
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while parsing error response: {ex.Message}");
                }
            }

            if (responseBody.Contains("DATABASE_ERROR") && responseBody.Contains("may not be specified for new objects"))
            {
                Logger.Log("Detected a database error related to a property. Removing and retrying...");

                retryThrice = 0;

                try
                {
                    // Extract only the JSON part of the response body
                    var jsonStartIndex = responseBody.IndexOf("{");
                    var jsonEndIndex = responseBody.LastIndexOf("}") + 1;

                    if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                    {
                        string jsonContent = responseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();
                            var match = Regex.Match(message, @"Field \[([^]]+)\]");

                            if (match.Success)
                            {
                                string propertyName = match.Groups[1].Value;
                                Logger.Log($"Property causing database issue: {propertyName}");
                                Logger.Log($"Response Body: {responseBody}");

                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                    if (parsedRequestBody != null && parsedRequestBody[propertyName] != null)
                                    {
                                        Logger.Log($"Removing problematic property '{propertyName}' from request body.");

                                        parsedRequestBody[propertyName] = null; // Optional, but could be left in place
                                        ((JObject)parsedRequestBody).Property(propertyName).Remove();

                                        var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                        Logger.Log($"Corrected Request Body (without '{propertyName}'): {correctedRequestBody}");

                                        Logger.Log($"Retrying the request without the problematic property '{propertyName}'...");

                                        return await SendMultipartRequest(url, method, headers, correctedRequestBody, token, entitySet, entitySetParam, entitySetArray);
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
                                Logger.Log("Failed to extract the property name from the database error message.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while parsing error response: {ex.Message}");
                }
            }

            if (responseBody.Contains("DATABASE_ERROR") && responseBody.Contains("FndBoolean.NOTEXIST"))
            {
                Logger.Log("Detected a database error related to a property. Converting and retrying...");

                retryThrice = 0;

                try
                {
                    // Extract only the JSON part of the response body
                    var jsonStartIndex = responseBody.IndexOf("{");
                    var jsonEndIndex = responseBody.LastIndexOf("}") + 1;

                    if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                    {
                        string jsonContent = responseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
                        dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                        if (errorResponse?.error?.details != null && errorResponse.error.details.Count > 0)
                        {
                            string message = errorResponse.error.details[0]?.message?.ToString();
                            var match = Regex.Match(message, "\"(True|False)\"");

                            if (match.Success)
                            {
                                string booleanValue = match.Groups[1].Value;
                                Logger.Log($"Detected boolean issue: {booleanValue}");
                                Logger.Log($"Response Body: {responseBody}");

                                if (!string.IsNullOrEmpty(requestBody))
                                {
                                    dynamic parsedRequestBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                                    if (parsedRequestBody != null)
                                    {
                                        Logger.Log($"Processing request body for boolean corrections.");

                                        foreach (var property in parsedRequestBody)
                                        {
                                            string propertyName = property.Path;
                                            string propertyValue = property.First.ToString();

                                            // Check for 'True' or 'False' string values (case-sensitive)
                                            if (propertyValue == "False" || propertyValue == "True")
                                            {
                                                bool boolValue = bool.Parse(propertyValue.ToLower());
                                                parsedRequestBody[propertyName] = boolValue; //.ToString().ToLower(); // Convert boolean to lowercase true/false
                                                Logger.Log($"Converted property '{propertyName}' from {propertyValue} to {parsedRequestBody[propertyName]}");
                                            }
                                            else
                                            {
                                                Logger.Log($"No special conversion needed for '{propertyName}' with value '{propertyValue}'");
                                            }
                                        }

                                        var correctedRequestBody = JsonConvert.SerializeObject(parsedRequestBody);
                                        Logger.Log($"Corrected Request Body: {correctedRequestBody}");

                                        Logger.Log($"Retrying the request with corrected boolean properties...");

                                        return await SendMultipartRequest(url, method, headers, correctedRequestBody, token, entitySet, entitySetParam, entitySetArray);
                                    }
                                    else
                                    {
                                        Logger.Log($"Parsed request body is null.");
                                    }
                                }
                                else
                                {
                                    Logger.Log("Request body is null or empty, unable to process correction.");
                                }
                            }
                            else
                            {
                                Logger.Log("Failed to extract the boolean value from the database error message.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while parsing error response: {ex.Message}");
                }
            }


            retryThrice = 0;

            return new ApiResponse
            {
                ResponseBody = responseBody,
                StatusCode = (int)response.StatusCode
            };
        }









    }



}
