using InsuranceApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace InsuranceApp.Controllers
{
    public class UserController : Controller
    {
        public async Task<ActionResult> GetUserByID(string id, string user, string psw)
        {
            Client UserInfoSelected = new Client();
            List<Client> UserInfo = new List<Client>();
            List<string> restrictions = new List<string>();

            using (var client = new HttpClient())
            {

                //Clearing the headers
                client.DefaultRequestHeaders.Clear();

                //Validated
                restrictions.Add("admin");
                restrictions.Add("users");
                var validatedOk = ValidateCredentias(client, user, psw, restrictions);

                if (validatedOk)
                {
                    //Sending request to find web api REST service resource using HttpClient
                    HttpResponseMessage ResUser = await client.GetAsync("http://www.mocky.io/v2/5808862710000087232b75ac");

                    //Checking the response is successful or not which is sent using HttpClient
                    if (ResUser.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var UserResponse = ResUser.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the list
                        var RootInfoUser = JsonConvert.DeserializeObject<RootObject>(UserResponse);

                        UserInfoSelected = UserInfo.Find(x => x.Id == id) ?? new Client();
                        UserInfo = RootInfoUser.clients.FindAll(x => x.Id == UserInfoSelected.Id);
                    }
                    //returning the employee list to view
                    return View("ClientView", UserInfo);
                }
                else
                {
                    return new HttpUnauthorizedResult();
                }
            }
        }
        public async Task<ActionResult> GetUserByName(string name, string user, string psw)
        {
            Client UserInfoSelected = new Client();
            List<Client> UserInfo = new List<Client>();
            List<string> restrictions = new List<string>();

            using (var client = new HttpClient())
            {

                //Clearing the headers
                client.DefaultRequestHeaders.Clear();

                //Validated
                restrictions.Add("admin");
                restrictions.Add("users");
                var validatedOk = ValidateCredentias(client, user, psw, restrictions);

                if (validatedOk)
                {
                    //Sending request to find web api REST service resource using HttpClient
                    HttpResponseMessage ResUser = await client.GetAsync("http://www.mocky.io/v2/5808862710000087232b75ac");

                    //Checking the response is successful or not which is sent using HttpClient
                    if (ResUser.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var UserResponse = ResUser.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the list
                        var RootInfoUser = JsonConvert.DeserializeObject<RootObject>(UserResponse);

                        UserInfoSelected = UserInfo.Find(x => x.Name == name) ?? new Client();
                        UserInfo.Add(UserInfoSelected);
                    }
                    //returning the employee list to view
                    return View("ClientView", UserInfo);
                }
                else
                {
                    return new HttpUnauthorizedResult();
                }
            }
        }

        public async Task<ActionResult> GetUserByPoliceNumber(string policyNumber, string user, string psw)
        {
            List<Policy> PoliciesInfo = new List<Policy>();
            Policy PoliciesInfoSelected = new Policy();
            List<Client> UserInfo = new List<Client>();
            List<string> restrictions = new List<string>();

            using (var client = new HttpClient())
            {

                //Clearing the headers
                client.DefaultRequestHeaders.Clear();

                //Validated
                restrictions.Add("admin");
                var validatedOk = ValidateCredentias(client, user, psw, restrictions);

                if (validatedOk)
                {
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage ResPolicies = await client.GetAsync("http://www.mocky.io/v2/580891a4100000e8242b75c5");
                    HttpResponseMessage ResUser = await client.GetAsync("http://www.mocky.io/v2/5808862710000087232b75ac");

                    //Checking the response is successful or not which is sent using HttpClient
                    if (ResPolicies.IsSuccessStatusCode && ResUser.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var PoliciesResponse = ResPolicies.Content.ReadAsStringAsync().Result;
                        var UserResponse = ResUser.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the list
                        var RootInfoPolicies = JsonConvert.DeserializeObject<RootObject>(PoliciesResponse);
                        var RootInfoUser = JsonConvert.DeserializeObject<RootObject>(UserResponse);

                        PoliciesInfoSelected = RootInfoPolicies.policies.Find(x => x.Id == policyNumber) ?? new Policy();
                        UserInfo = RootInfoUser.clients.FindAll(x => x.Id == (PoliciesInfoSelected.ClientId));
                    }
                    //returning the employee list to view
                    return View("ClientView", UserInfo);
                }
                else
                {
                    return new HttpUnauthorizedResult();
                }
            }
        }

        private bool ValidateCredentias(HttpClient client, string user, string psw, List<string> restriction)
        {

            //Basic Authentication Simulation
            byte[] byteArray = Encoding.ASCII.GetBytes(user + ":" + psw);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            string decodedAuthenticationToken = Encoding.UTF8.GetString(
                Convert.FromBase64String(client.DefaultRequestHeaders.Authorization.Parameter));

            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string credentials = encoding.GetString(Convert.FromBase64String(client.DefaultRequestHeaders.Authorization.Parameter));

            string[] credentialsArray = credentials.Split(':');
            string username = credentialsArray[0];
            string password = credentialsArray[1];

            //Sending request to find web api REST service resource using HttpClient
            HttpResponseMessage ResUser = client.GetAsync("http://www.mocky.io/v2/5808862710000087232b75ac").Result;
            var RootInfoUser = JsonConvert.DeserializeObject<RootObject>(ResUser.Content.ReadAsStringAsync().Result);

            //Comparing username and psw to validate
            var UserInfo = RootInfoUser.clients.Find(x => x.Email == username && x.Id == password);
            if (UserInfo != null)
            {
                if (restriction.Exists(x => x.EndsWith(UserInfo.Role)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}