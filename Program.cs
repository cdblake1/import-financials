using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;

namespace pull_financial_data
{
    public class Program
    {
        /*Example of Full Call 
        https://www.quandl.com/api/v3/datatables/WIKI/PRICES?
        date=1999-11-18&
        ticker=A&
        api_key=J2nwxx24_AKyktqyQ3FC*/
        public static readonly HttpClient client = new HttpClient();
        public const string API_URL = "https://api.iextrading.com/1.0/stock/JBL/stats?";
        // public const string API_KEY = "J2nwxx24_AKyktqyQ3FC";
        public static Dictionary<string, string> parameters = new Dictionary<string, string>();
        public static void Main(string[] args)

        {
            // BuildWebHost(args).Run();
            // parameters.Add("date", "2018-3-27");
            parameters.Add("symbols", "JBL");
            parameters.Add("types", "stats");
            // parameters.Add("api_key", API_KEY);
            var apiCall = buildAPICall(parameters);
            processRepositories("https://api.iextrading.com/1.0/stock/JBL/stats").Wait();

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static string buildAPICall(Dictionary<string, string> parameters)
        {
            var parametersString = "";
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                parametersString += parameter.Key + "=" + parameter.Value
                    + (parameter.Key.Equals("api_call") ? "" : "&");
            }

            return API_URL + parametersString;
        }

        public static string configureCurrentDate()
        {
            var date = DateTime.Today.AddDays(-365).ToString();
            var configuredDate = date.Substring(0, date.IndexOf(' '));
            var tempDateArr = configuredDate.Split("/");

            return tempDateArr[2] + "-" + tempDateArr[0] + "-" + tempDateArr[1];
        }

        public static async Task processRepositories(string API_CALL)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/JSON"));

            var streamTask = client.GetStreamAsync(API_CALL);       
            Dictionary<string, string> financials = JsonObject; 
        }
    }

    public class CompanyFinancialData
    {
        public string sharesOutstanding;
        public int price;
    }
}
