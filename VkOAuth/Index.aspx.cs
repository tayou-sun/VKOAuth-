using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using Newtonsoft.Json;

namespace VkOAuth

{

    public partial class Index1 : Page
    {
        //Получать остальные параметры и формировать ссылки: используя их (client id, client secret и redirect uri)
        private readonly string ClientId = ConfigurationManager.AppSettings["ClientId"];
        private readonly string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private readonly string ResponseUri = ConfigurationManager.AppSettings["ResponseUri"];

        //code-behind
        protected void Page_Load(object sender, EventArgs e)
        {
            GetAuthData();
        }

        protected void AuthButton_Click(object sender, EventArgs e)
        {
             Response.Redirect("https://api.vkontakte.ru/oauth/authorize?client_id="+ 
                 ClientId + "&redirect_uri=" + 
                 ResponseUri+"&scope=offline&messages&response_type=code&display=page");

        }

      

        private void GetAuthData()
        {
            if (Request["code"] != null)
            {
                Session["code"] = Request["code"];
                GetUserInfo();
            }
        }

        private void GetUserInfo()
        {
            var request =
                WebRequest.Create(
                    "https://oauth.vk.com/access_token?client_id="+ClientId + 
                    "&client_secret="+ClientSecret + "&code=" +
                    Session["Code"] + "&redirect_uri="+ ResponseUri);

            

            //Try-catch - ловить эксепшн?
            try
            {
                var response = request.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
          

            
            var line = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

            dynamic json = JsonConvert.DeserializeObject(line);

            var userId = json.user_id;
            var accessToken = json.access_token;

            //Получаем строку (похоже: в JSON)
            //var matches = Regex.Match(line, "\"access_token\":\"([^\"]+)");
            //var accessToken = matches.Groups[1].Value;

            //matches = Regex.Match(line, "\"user_id\":(\\d+)");
            //var userId = matches.Groups[1].Value;

            //Проверить на ошибки

            Session["AccessToken"] = accessToken;

            Session["UserId"] = userId;

            var request1 =
                WebRequest.Create("https://api.vk.com/method/users.get?user_id=" +
                        Session["UserId"] + "&access_token=" + Session["AccessToken"]);
            var line1 = new StreamReader(request1.GetResponse().GetResponseStream()).ReadToEnd();
            dynamic json1 = JsonConvert.DeserializeObject(line1);


            foreach (var j in json1["response"].Children())
            {
                Session["FirstName"] = j.first_name;

                Session["LastName"] = j.last_name;   
            }



           


        }
    }
}