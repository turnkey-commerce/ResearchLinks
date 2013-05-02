using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ResearchLinks.SpecTests.Helpers
{
    public class StepHelpers
    {
        public static List<KeyValuePair<string, string>> SetPostData<T>(T testClass)
        {
            var postData = new List<KeyValuePair<string, string>>();
            foreach (PropertyInfo property in testClass.GetType().GetProperties())
            {
                if (property.GetValue(testClass, null) != null)
                {
                    postData.Add(new KeyValuePair<string, string>(property.Name.ToString(), property.GetValue(testClass, null).ToString()));
                }
            }
            return postData;
        }

        public static HttpClient SetupHttpClient(string userName, string password)
        {
            var client = new HttpClient();
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                var buffer = Encoding.ASCII.GetBytes(userName + ":" + password);
                var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
                client.DefaultRequestHeaders.Authorization = authHeader;
            }
            return client;
        }

    }
}
