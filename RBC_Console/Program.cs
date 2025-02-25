using HtmlAgilityPack;
using Newtonsoft.Json;

using RBC_Console;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;


CallAPI callAPI = new CallAPI();
var p = await callAPI.GetRBCWeathManagmentAdvisor(new System.Net.Http.HttpClient(), "https://www.rbcwealthmanagement.com/en-ca/wp-admin/admin-ajax.php", "Toronto");
// Console.WriteLine(p);

var oMyclass = JsonConvert.DeserializeObject<AdvisorResponseViewModel>(p);
// Console.WriteLine(oMyclass);

string plainText = callAPI.ExtractTextFromHtml(oMyclass.data.html);
// Console.WriteLine(plainText);

List<RBCDataViewModel> rBCDataViewModel = new List<RBCDataViewModel>();
var htmlDocument = new HtmlDocument();
htmlDocument.LoadHtml(oMyclass.data.html);
string xpathQuery = "//div[contains(@class, 'rbc-expandable-trigger rbc-expandable-arrow-wrap')]";
var texts = htmlDocument.
                DocumentNode
                .SelectNodes(xpathQuery)
                .Select(x => x.InnerText)
                .ToList();
var textsDummy = htmlDocument.
                DocumentNode
                .SelectNodes(xpathQuery)
                .Select(x => x.ChildNodes)
                .ToList();
int cnt = 0;
foreach(HtmlNodeCollection item in textsDummy)
{
    if (cnt > 0)
    {
        string titleval = item[1].InnerHtml;
        var title = callAPI.ExtractTextFromHtml(titleval);
        string[] TitleAddress = title.Split("\n");

        string TitleResult = TitleAddress[0].ToString().Trim();

        string addressval = TitleAddress[4].ToString().Trim() + TitleAddress[5].ToString().Trim();
        var address = callAPI.ExtractTextFromHtml(addressval);

        var showmoreButton = item[3];
        var branchid = callAPI.ExtractBranchId(showmoreButton.OuterHtml);
        rBCDataViewModel.Add(new RBCDataViewModel() { Title = TitleResult, Address = address, BranchId = branchid });
    }
    cnt++;
}

foreach (var val in rBCDataViewModel)
{
    val.Emaployees = new List<EmployeeViewModel>();
    var p1 = await callAPI.GetRBCWeathManagmentEmployeeList(new System.Net.Http.HttpClient(), "https://www.rbcwealthmanagement.com/en-ca/wp-admin/admin-ajax.php", val.BranchId);
    var EmpList = JsonConvert.DeserializeObject<AdvisorResponseViewModel>(p1);

    string xpathQuery2 = "//div[contains(@class, 'rbc-caption rbc-caption-full rbc-caption-align-top')]";
    var htmlDocument2 = new HtmlDocument();
    htmlDocument2.LoadHtml(EmpList.data.html);

    var textsDummy2 = htmlDocument2.
                                        DocumentNode
                                        .SelectNodes(xpathQuery2)
                                        .Select(x => x.ChildNodes)
                                        .ToList();
    int cnt2 = 0;
    foreach (HtmlNodeCollection item in textsDummy2)
    {
        if (cnt2 > 0)
        {
            var Empname = item[3].SelectNodes("//h3[contains(@class, 'h6 header-alt mb-0')]")[cnt2-1].InnerHtml;
            var Name = callAPI.ExtractTextFromHtml(Empname.ToString());
            string Phone = "";
            string Email = "";
            string URL = "";
            var LiAnchors = item[3].SelectNodes("//li").ToList();
            int Cnt4 = 0;
            var Result = item[3].OuterHtml;
            HtmlTag _tag;
            HtmlParser _parsehtml = new HtmlParser(Result);
            while (_parsehtml.ParseNext("a", out _tag))
            {
                if (Cnt4 == 0)
                {
                    _tag.Attributes.TryGetValue("href", out Phone);
                }
                else if (Cnt4 == 1)
                {
                    _tag.Attributes.TryGetValue("href", out Email);
                }
                else if (Cnt4 == 2)
                {
                    _tag.Attributes.TryGetValue("href", out URL);
                }
                Cnt4++;
            }            
            val.Emaployees.Add(new EmployeeViewModel() { Name = Name, Phone = Phone, Email = Email, URL = URL });
        }
        cnt2++;
    }

    // Console.WriteLine(val.BranchId + " <==> " + val.Title + " <==> " + val.Address + " <==> " + string.Join(", ", val.Emaployees.Select(r => r.Name)) + "\n");
    // Console.WriteLine(string.Join(", ", val.Emaployees.Select(r => r.Name)) + string.Join(", ", val.Emaployees.Select(r => r.URL)) +  "\n");

    foreach (var emp in val.Emaployees)
    {
        var employeedetails = await callAPI.GetRBCWeathManagmentEmployeeBranch(new System.Net.Http.HttpClient(), emp.URL);
        string xpathcontact = "//div[contains(@class, 'contact-info')]";
        var htmlDocumentcontact = new HtmlDocument();
        htmlDocumentcontact.LoadHtml(employeedetails.ToString());

        var textsNodeContact = htmlDocumentcontact.
                                            DocumentNode
                                            .SelectNodes(xpathcontact)
                                            .Select(x => x.ChildNodes)
                                            .ToList();

        foreach (HtmlNodeCollection itemNode in textsNodeContact)
        {
            var Empnme = itemNode[5];
            var Empname = Empnme.SelectNodes("//p");
            var Name = callAPI.ExtractTextFromHtml(Empname[0].InnerHtml.ToString());
        }           
    }

}





//HtmlTag tag;
//HtmlParser parse = new HtmlParser(oMyclass.data.html);
//while (parse.ParseNext("h3", out tag))
//{
//    // See if this anchor links to us
//    string value;
//    if (tag.Attributes.TryGetValue("innerText", out value))
//    {
//        // value contains URL referenced by this link
//    }
//}