using CavokatorAPI.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace CavokatorAPI.Controllers
{
    public class NotamController : ApiController
    {
        [HttpPost]
        [Route("Notam/FetchAidap")]
        public string FetchAidap(FetchAidapModel data)
        {
            string responseFromServer = string.Empty;
            try
            {
                byte[] bytes;
                using (StreamReader sr2 = new StreamReader(WebConfigurationManager.AppSettings["ClientCertificatePath"]))
                {
                    using (var memstream = new MemoryStream())
                    {
                        sr2.BaseStream.CopyTo(memstream);
                        bytes = memstream.ToArray();
                    }
                }

                X509Certificate2 clientCertificate = new X509Certificate2(bytes, WebConfigurationManager.AppSettings["ClientCertificatePassword"]);

                var arr = new List<KeyValuePair<string,string>>();
                //{
                //     new KeyValuePair<string, string>("uid", "mortega"),
                //     new KeyValuePair<string, string>("password", "Aidap.201805"),
                //     new KeyValuePair<string, string>("active", "Y"),
                //     new KeyValuePair<string, string>("type", "I"),
                //     new KeyValuePair<string, string>("location_id", "EGLL")
                // };
                string postData = "uid=mortega&password=Aidap.201805&active=Y&type=I&location_id=EGLL";
                postData = "";
                foreach (PropertyInfo prop in data.GetType().GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    var proValue = prop.GetValue(data, null).ToString();
                    if(proValue != null)
                    {
                        //var kvp = new KeyValuePair<string, string>(prop.Name, prop.GetValue(data, null).ToString());
                        //arr.Add(kvp);
                        postData += $"{prop.Name}={proValue}&";
                    }
                }
                if(postData.Length > 1)
                    postData = postData.Substring(0,postData.Length - 1);

                var formContent = new FormUrlEncodedContent(arr);


                // RESTSHARP
                var client = new RestClient(WebConfigurationManager.AppSettings["ServerBaseURL"]+"/aidap/XmlNotamServlet");
                client.ClientCertificates = new X509CertificateCollection() { clientCertificate };
              //  string postData = "uid=mortega&password=Aidap.201805&active=Y&type=I&location_id=EGLL";

                // Json to post.
                //string jsonToSend = JsonHelper.ToJson(Json);
                //var jsonToSend = new JavaScriptSerializer().Serialize(data);
                


                var request = new RestRequest(Method.POST);
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                //request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
               // request.RequestFormat = DataFormat.Json;

                var res = client.Execute(request);
                responseFromServer = res.Content;

            }
            catch (Exception ex)
            {

            }

            return responseFromServer;

        }
    }
}
