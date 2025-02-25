using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RBC_Console
{
    public class CallAPI
    {
        public async Task<string> GetRBCWeathManagmentAdvisor(HttpClient httpClient,string url, string SearchValue)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action", "rbcwm_get_advisors_branches"),
                new KeyValuePair<string, string>("nonce", "c2a115e8b2"),
                new KeyValuePair<string, string>("location_string", SearchValue),
                new KeyValuePair<string, string>("data_source", "ca")
            });

            using HttpResponseMessage response = await httpClient.PostAsync(url, formContent);
            bool ResultCode = response.EnsureSuccessStatusCode().IsSuccessStatusCode;
            if (ResultCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                return "Error";
            }
        }
        public async Task<string> GetRBCWeathManagmentEmployeeList(HttpClient httpClient, string url, string branchId)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action", "rbcwm_get_advisors_by_branch"),
                new KeyValuePair<string, string>("nonce", "c2a115e8b2"),
                new KeyValuePair<string, string>("branch_id", branchId),
                new KeyValuePair<string, string>("data_source", "ca")
            });

            using HttpResponseMessage response = await httpClient.PostAsync(url, formContent);
            bool ResultCode = response.EnsureSuccessStatusCode().IsSuccessStatusCode;
            if (ResultCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                return "Error";
            }
        }
        public async Task<string> GetRBCWeathManagmentEmployeeBranch(HttpClient httpClient, string url)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(url);
            bool ResultCode = response.EnsureSuccessStatusCode().IsSuccessStatusCode;
            if (ResultCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                return "Error";
            }
        }

        public string ExtractTextFromHtml(string html)
        {
            if (html == null)
            {
                return "";
            }
            string plainText = Regex.Replace(html, "<[^>]+?>", " ");
            plainText = System.Net.WebUtility.HtmlDecode(plainText).Trim();

            return plainText;
        }

        public string ExtractBranchId(string Html)
        {
            string Result = "";
            HtmlTag tag;
            HtmlParser parse = new HtmlParser(Html);
            while (parse.ParseNext("button", out tag))
            {
                // See if this anchor links to us
                string value;
                if (tag.Attributes.TryGetValue("data-branch_id", out value))
                {
                    // value contains URL referenced by this link
                }
                Result = value;
            }
            return Result;
        }

    }
}
