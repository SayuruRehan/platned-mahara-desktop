using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatnedTestMatic.CustomClasses
{


    public class ApiExecution
    {
        private static readonly HttpClient client = new HttpClient();
        private string url          = "";
        private string method       = "";
        private string headers      = "";
        private string requestBody  = "";
        private string token        = "";

        public class ApiResponse
        {
            public string ResponseBody { get; set; }
            public int StatusCode { get; set; }
        }

        public async Task<ApiResponse> Get(string postUrl, string postHeader, string postRequestBody, string postToken)
        {
            try
            {
                url         = postUrl;
                method      = "GET";
                headers     = postHeader;
                requestBody = postRequestBody;
                token       = postToken;                

                ApiResponse apiResponse = await SendRequest(url, method, headers, requestBody, token);
                return apiResponse;
            }
            catch (Exception ex)
            {
                Logger.Log($"Authentication failed: {ex.Message}", "Error");
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
                Logger.Log($"Authentication failed: {ex.Message}", "Error");
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

            Logger.Log("Handling additional headers...");
            if (!string.IsNullOrEmpty(headers))
            {
                foreach (var header in headers.Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    var headerKeyValue = header.Split(':');
                    if (headerKeyValue.Length == 2)
                    {
                        request.Headers.Add(headerKeyValue[0].Trim(), headerKeyValue[1].Trim());
                    }
                }
            }
            Logger.Log("Handling additional headers completed.");

            Logger.Log("Handling request body for POST, PUT, PATCH methods...");
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            }
            Logger.Log("Handling request body for POST, PUT, PATCH methods completed.");

            Logger.Log("Sending the request and get the response...");
            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Logger.Log("Sending the request and get the response completed.");
            Logger.Log($"Response body: {responseBody}");
            Logger.Log($"Response status: {(int)response.StatusCode}");

            return new ApiResponse
            {
                ResponseBody = responseBody,
                StatusCode = (int)response.StatusCode
            }; 
        }
    }


}
