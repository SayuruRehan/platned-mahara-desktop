using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PL_PlatnedTestMatic.Classes
{
    public class UrlReconstructor
    {
        public static string ReconstructUrl(string url, string requestBody)
        {
            // Extract the parameters part from the URL
            string pattern = @"\((.*?)\)";
            Match match = Regex.Match(url, pattern);

            try
            {
                if (!match.Success)
                {
                    Logger.Log("No parameters found in the URL");
                }
            }
            catch (Exception e) {
                Logger.Log("No parameters found in the URL");
            }
            

            // Get the parameters part (everything between the parentheses)
            string paramsPart = match.Groups[1].Value;

            // Split the parameters into key-value pairs
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            string[] keyValuePairs = paramsPart.Split(',');

            foreach (string keyValuePair in keyValuePairs)
            {
                string[] keyValue = keyValuePair.Split('=');
                string key = keyValue[0].Trim();
                string value = keyValue[1].Trim('\'');
                urlParams[key] = value;
            }

            // Parse the request body JSON
            JObject requestBodyJson = JObject.Parse(requestBody);

            // Reconstruct the parameters part using values from the request body
            foreach (var paramKey in urlParams.Keys)
            {
                if (requestBodyJson.ContainsKey(paramKey))
                {
                    urlParams[paramKey] = requestBodyJson[paramKey].ToString();
                }
            }

            // Rebuild the parameters string
            List<string> newParamsList = new List<string>();
            foreach (var param in urlParams)
            {
                newParamsList.Add($"{param.Key}='{param.Value}'");
            }
            string newParamsPart = string.Join(", ", newParamsList);

            // Replace the old parameters part with the new one
            string newUrl = Regex.Replace(url, pattern, $"({newParamsPart})");

            return newUrl;
        }
    }
}
