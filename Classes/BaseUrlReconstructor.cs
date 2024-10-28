using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PlatnedMahara.Classes
{
    public class UrlReconstructor
    {
        public static string ReconstructUrl(string url, string requestBody)
        {
            // Extract the parameters part from the URL
            string pattern = @"\.svc/[^/]+?\((.*?)\)";
            Match match = Regex.Match(url, pattern);

            try
            {
                if (!match.Success)
                {
                    Logger.Log("No parameters found in the URL");
                    return url;  // Return original URL if no parameters are found
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Error processing URL: {e.Message}");
                return url;
            }

            // Get the parameters part (everything between the parentheses)
            string paramsPart = match.Groups[1].Value;

            // Split the parameters into key-value pairs, preserving quote style and null values
            Dictionary<string, (string Value, bool PreserveQuotes, bool IsNull)> urlParams = new Dictionary<string, (string, bool, bool)>();
            string[] keyValuePairs = paramsPart.Split(',');

            foreach (string keyValuePair in keyValuePairs)
            {
                string[] keyValue = keyValuePair.Split('=');
                string key = keyValue[0].Trim();

                // Preserve the exact value format
                string value = keyValue[1].Trim();
                bool isNull = value.Equals("null", StringComparison.OrdinalIgnoreCase);

                // Determine if both the first and last characters are single quotes
                bool preserveQuotes = value.StartsWith("'") && value.EndsWith("'");

                // If it's not null and preserveQuotes is true, retain the value including quotes
                if (isNull)
                {
                    value = null;
                }
                else if (preserveQuotes)
                {
                    // Remove the surrounding quotes but keep the value for comparison
                    value = value.Substring(1, value.Length - 2);
                }

                urlParams[key] = (value, preserveQuotes, isNull);
            }

            // Parse the request body JSON
            JObject requestBodyJson = JObject.Parse(requestBody);

            foreach (var paramKey in urlParams.Keys)
            {
                // Only update if original value is not null
                if (!urlParams[paramKey].IsNull && requestBodyJson.ContainsKey(paramKey))
                {
                    string newValue = requestBodyJson[paramKey].ToString();

                    // Check if the original value has embedded quotes and format accordingly
                    string originalValue = urlParams[paramKey].Value;
                    if (originalValue.Contains("'"))
                    {
                        // Find the last single quote position and replace the value between quotes
                        int lastQuoteIndex = originalValue.LastIndexOf("'");
                        int firstQuoteIndex = originalValue.IndexOf("'");

                        if (firstQuoteIndex != lastQuoteIndex) // Ensure there are multiple quotes
                        {
                            // Replace the content inside the quotes with the new value
                            urlParams[paramKey] = (originalValue.Substring(0, firstQuoteIndex + 1) + newValue + "'", urlParams[paramKey].PreserveQuotes, false);
                        }

                    }
                    else
                    {
                        urlParams[paramKey] = (newValue, urlParams[paramKey].PreserveQuotes, false);
                    }

                    // Check if the original value is in datetime format
                    if (urlParams[paramKey].Value != null && DateTime.TryParse(urlParams[paramKey].Value, out DateTime originalDate))
                    {
                        // If it's a valid DateTime, format it to the desired string format
                        newValue = DateTime.Parse(newValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                        urlParams[paramKey] = (newValue, urlParams[paramKey].PreserveQuotes, false);
                    }
                }
            }

            // Rebuild the parameters string without extra spaces after commas
            List<string> newParamsList = new List<string>();
            foreach (var param in urlParams)
            {
                string formattedValue;

                // Keep original quoting style
                if (param.Value.IsNull)
                {
                    formattedValue = "null";
                }
                else if (param.Value.PreserveQuotes)
                {
                    formattedValue = $"'{param.Value.Value}'";
                }
                else
                {
                    formattedValue = param.Value.Value;
                }

                newParamsList.Add($"{param.Key}={formattedValue}");
            }
            string newParamsPart = string.Join(",", newParamsList);  // Join without extra spaces after commas

            // Construct the new URL by replacing only the parameter part, leaving any subsequent parentheses unchanged
            string newUrl = Regex.Replace(url, pattern, $"{match.Groups[0].Value.Split('(')[0]}({newParamsPart})");

            return newUrl;
        }
    }
}
